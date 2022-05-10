using MailCollector.Kit.Logger;
using MailCollector.Kit.SqlKit.Models;
using System;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace MailCollector.Kit.SqlKit
{
    public class TelegramBotSqlAdapter
    {
        private readonly SqlServerShell _sqlShell;
        private readonly ITelegramBotClient _tgBot;
        private readonly CancellationToken _cancellationToken;


        public TelegramBotSqlAdapter(SqlServerShell sqlShell, ITelegramBotClient tgBot, CancellationToken cancellationToken)
        {
            _cancellationToken = cancellationToken;
            _sqlShell = sqlShell;
            _tgBot = tgBot;
        }

        public void SaveChatId(ChatId chat)
        {
            _cancellationToken.ThrowIfCancellationRequested();

            if (chat.Identifier == null && string.IsNullOrWhiteSpace(chat.Username))
            {
                throw new ArgumentNullException("У чата оба свойства Username и Identifier были null");
            }

            var tgSub = new TelegramBotSubscriber(chat);
            _sqlShell.Insert(tgSub, TelegramBotSubscriber.TableName);
        }

        public void RemoveChatId(ChatId chat)
        {
            _cancellationToken.ThrowIfCancellationRequested();

            if (chat.Identifier == null && string.IsNullOrWhiteSpace(chat.Username))
            {
                throw new ArgumentNullException("У чата оба свойства Username и Identifier были null");
            }

            var tgSub = new TelegramBotSubscriber(chat);

            string cmd = null;
            if (tgSub.ChatId != null)
                cmd = $"WHERE {nameof(tgSub.ChatId)} = {tgSub.ChatId}";
            else
                cmd = $"WHERE {nameof(tgSub.Username)} = {tgSub.Username}";

            _sqlShell.RemoveWhere<TelegramBotSubscriber>(cmd, TelegramBotSubscriber.TableName);
        }

        public TelegramBotSubscriber[] GetAllSubscribers()
        {
            _cancellationToken.ThrowIfCancellationRequested();

            return _sqlShell.GetArrayOf<TelegramBotSubscriber>(TelegramBotSubscriber.TableName);
        }
    }
}
