﻿using MailCollector.Kit.ImapKit.Models;

namespace MailCollector.Kit.ImapKit
{
    public static class SupportedImapServers
    {
        private static readonly ImapServerParams _mailRuParams =
            new ImapServerParams("imap.mail.ru", port: 0, useSsl: false);
        public static ImapServerParams MailRuParams => _mailRuParams;

        private static readonly ImapServerParams _yandexParams =
            new ImapServerParams("imap.yandex.com", port: 0, useSsl: true);
        public static ImapServerParams YandexParams => _yandexParams;

        // Можно для отчётности gmail добавить и оттестить
    }
}
