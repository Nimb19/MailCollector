﻿using MailCollector.Kit.ImapKit.Models;
using MailCollector.Kit.Logger;
using MailCollector.Kit.SqlKit;
using MailCollector.Kit.SqlKit.Models;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace MailCollector.Kit.TelegramBotKit
{
    public class MailTelegramBot
    {
        private const string LogPrefix = "[MailTelegramBot] ";

        private readonly string _token;
        private readonly ILogger _logger;
        private readonly SqlServerShell _sqlServerShell;
        private static readonly ReceiverOptions _receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = { }, // receive all update types
        };

        private readonly object _botLock = new object();

        private ITelegramBotClient _bot;
        private TelegramBotSqlAdapter _tgSqlAdapter;

        public Telegram.Bot.Types.User BotInfo { get; private set; }
        public bool IsStarted { get; private set; }

        public MailTelegramBot(string token, SqlServerShell sqlServerShell, ILogger logger)
        {
            _logger = new PrefixLogger(logger, LogPrefix);
            _token = token;
            _sqlServerShell = sqlServerShell;
        }

        public void Start(CancellationToken cancelationToken)
        {
            cancelationToken.ThrowIfCancellationRequested();

            if (!IsStarted)
            {
                _logger.Warning("Была попытка старта работы бота, когда он уже включён");
                return;
            }

            _bot = new TelegramBotClient(_token);
            BotInfo = _bot.GetMeAsync().Result;

            _tgSqlAdapter = new TelegramBotSqlAdapter(_sqlServerShell, _bot, cancelationToken);

            _bot.StartReceiving(UpdateHadler, HandleErrorAsync, _receiverOptions, cancelationToken);

            IsStarted = true;
            _logger.WriteLine($"Телеграмм бот '{BotInfo.FirstName}' успешно запущен");
        }

        private async Task UpdateHadler(ITelegramBotClient botClient, Update update, CancellationToken cancelationToken)
        {
            const LogLevel logLevelToSaveUpdateInfo = LogLevel.Debug; //TODO: После отладки убери на Trace
            if (_logger.LogLevel == logLevelToSaveUpdateInfo)
            {
                var serUpdate = JsonConvert.SerializeObject(update, Formatting.Indented);
                _logger.Write(logLevelToSaveUpdateInfo, $"New update: {serUpdate}"); 
            }

            if (update.Type == UpdateType.Message)
            {
                var message = update.Message;

                if (cancelationToken.IsCancellationRequested)
                {
                    _logger.WriteLine($"Поступила команда отмены работы Telegram бота '{BotInfo.FirstName}'");
                    await SendTextMessageAsync(botClient, message.Chat, "Поступила команда отмены работы телеграмм бота. Прощайте :(");
                    IsStarted = false;
                    return;
                }

                if (string.Equals(message.Text, "/start", StringComparison.OrdinalIgnoreCase))
                {
                    if (await TrySaveChatId(message.Chat))
                    {
                        await SendTextMessageAsync(botClient, message.Chat
                            , "Приветствую! Как только будут поступать новые письма, я сразу же Вам об этом сообщу.");
                    }
                    {
                        await SendTextMessageAsync(botClient, message.Chat
                            , "Приветствую! К сожалению, я не смог подписать Вас на свою рассылку уведомлений.");
                    }
                    return;
                }
                else if (string.Equals(message.Text, "/unsubscribe", StringComparison.OrdinalIgnoreCase))
                {
                    if (await TryRemoveChatId(message.Chat))
                    {
                        await SendTextMessageAsync(botClient, message.Chat
                            , "Вы был успешно отписаны из рассылки.");
                    }
                    {
                        await SendTextMessageAsync(botClient, message.Chat
                            , "К сожалению, я не смог отписать Вас от своей рассылки уведомлений.");
                    }
                    return;
                }
                await SendTextMessageAsync(botClient, message.Chat, "Такой команды не существует.");
            }
        }

        private async Task<bool> TryRemoveChatId(ChatId chat)
        {
            try
            {
                await Task.Run(() => _tgSqlAdapter.RemoveChatId(chat));

                return true;
            }
            catch (Exception ex)
            {
                _logger.Error($"Не удалось удалить из подписчиков '{chat.Username}' (Id={chat.Identifier}): {ex}");
                return false;
            }
        }

        private async Task<bool> TrySaveChatId(ChatId chat)
        {
            try
            {
                await Task.Run(() => _tgSqlAdapter.SaveChatId(chat));

                return true;
            }
            catch (Exception ex)
            {
                _logger.Error($"Не удалось добавить подписчика '{chat.Username}' (Id={chat.Identifier}): {ex}");
                return false;
            }
        }

        public void SendMessageToAllSubsAboutNewMails(ImapMailParams[] imapMails, ImapClient imapClient)
        {
            if (imapMails.Length == 0)
            {
                _logger.Warning("Для отправки сообщений подписчикам пришло 0 писем");
                return;
            }

            string tgMessage;
            var mailFormat = "От '{0}', тема: '{1}', дата: {2}";
            if (imapMails.Length == 1)
            {
                var mail = imapMails.Single();
                tgMessage = $"На почту '{imapClient.Login}' в папку '{mail.Folder.Name}' " +
                    $"было прислано письмо {string.Format(mailFormat.ToLower(), mail.From, mail.Subject, mail.Date)}";
            }
            else if (imapMails.Length < 11)
            {
                var newTabLine = $"{Environment.NewLine}\t";
                var mails = string.Join($";{newTabLine}"
                    , imapMails.Select(x => $"От '{x.From}', тема: '{x.Subject}', дата: {x.Date}"));
                tgMessage = $"На почту '{imapClient.Login}' в папку '{imapMails.First().Folder.Name}' " +
                    $"были присланы следующие письма (порядок сохранён): {newTabLine}{mails}";
            }
            else
            {
                tgMessage = $"На почту '{imapClient.Login}' в папку '{imapMails.First().Folder.Name}' " +
                    $"было прислано более 10 писем";
            }

            SendMessageToAllSubs(tgMessage);
        }

        public void SendMessageToAllSubsAboutInitComplete(ImapClient imapClient)
        {
            string tgMessage = $"Клиент {imapClient.Login} успешно инициализирован впервые." +
                $" Его прошлые сообщения успешно сохранены.";
            SendMessageToAllSubs(tgMessage);
        }

        private void SendMessageToAllSubs(string tgMessage)
        {
            try
            {
                var subs = _tgSqlAdapter.GetAllSubscribers();
                if (subs.Length == 0)
                {
                    _logger.Warning($"Сообщение от телеграмм бота не было отправлено" +
                        $", так как не было ни одного подписчика." +
                        $" Начало сообщения: {new string(tgMessage.Take(12).ToArray())}");
                    return;
                }
                foreach (var sub in subs)
                {
                    if (sub == null || (sub.ChatId == null && sub.Username == null))
                        continue; // Просто отказоустойчивость на всякий

                    ChatId chatId = null;

                    if (sub.ChatId.HasValue)
                        chatId = new ChatId(sub.ChatId.Value);
                    else
                        chatId = new ChatId(sub.Username);

                    SendTextMessageAsync(_bot, chatId, tgMessage).Wait();
                }
                _logger.WriteLine($"Сообщения подписчикам в Telegram о новых письмах успешно высланы");
            }
            catch (Exception ex)
            {
                _logger.Error($"Не удалось разослать подписчикам о новых письмах: {ex}");
            }
        }

        public async Task SendTextMessageAsync(ITelegramBotClient tgBot, ChatId chatId, string tgMessage)
        {
            await Task.Run(() =>
            {
                lock (_botLock)
                {
                    tgBot.SendTextMessageAsync(chatId, tgMessage);
                }
            });
        }

        public async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            await Task.Run(() => _logger.Error(exception));
        }

        //public void Stop() // не нужно думать об этом. Останавливать будут токеном
        //{
        //    if (!IsStarted)
        //    {
        //        _logger.Warning("Была попытка выключения бота, когда он выключен");
        //        return;
        //    }

        //    _bot.Sto

        //    IsStarted = false;
        //    _logger.WriteLine($"Телеграмм бот '{BotInfo.FirstName}' успешно выключен");
        //}
    }
}