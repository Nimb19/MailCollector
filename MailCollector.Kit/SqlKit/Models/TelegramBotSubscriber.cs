using Telegram.Bot.Types;

namespace MailCollector.Kit.SqlKit.Models
{
    public class TelegramBotSubscriber
    {
        public const string TableName = "TelegramBotSubscribers";

        public long? ChatId { get; set; }
        public string Username { get; set; }

        public TelegramBotSubscriber() { }

        public TelegramBotSubscriber(ChatId chat)
        {
            ChatId = chat.Identifier;
            Username = chat.Username;
        }
    }
}
