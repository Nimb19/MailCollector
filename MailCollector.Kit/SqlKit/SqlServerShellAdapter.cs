using MailCollector.Kit.ImapKit.Models;
using MailCollector.Kit.Logger;
using MailCollector.Kit.SqlKit.Models;
using MailKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace MailCollector.Kit.SqlKit
{
    /// <summary>
    ///     Адаптер ля работы с Sql оболочкой (SqlServerShell). 
    ///     Здесь вся конкретика для работы с сущностями из этого проекта.
    ///     Для каждого клиента создаётся свой адаптер для удобства, 
    ///     благо соединение с сервером в SqlServerShell потокобезопасно.
    /// </summary>
    public class SqlServerShellAdapter
    {
        public ImapClient SqlClient { get; }
        public SqlServerShell SqlShell { get; }

        private readonly List<Folder> _folders = null; 
        public IReadOnlyList<Folder> Folders => _folders;

        private readonly ILogger _logger;
        private readonly CancellationToken _cancellationToken;

        public SqlServerShellAdapter(SqlServerShell shell, ImapClient sqlClient, ILogger logger, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            _cancellationToken = cancellationToken;
            _logger = logger;
            SqlShell = shell;
            SqlClient = sqlClient;
            _folders = GetAllFolders();
        }

        /// <summary>
        ///     Взять сущности клиентов с их imap серверами. 
        /// </summary>
        public static (ImapClient Client, ImapServer Server)[] GetAllClientsWithServers(SqlServerShell shell, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var clients = shell.GetArrayOf<ImapClient>(ImapClient.TableName);
            var servers = shell.GetArrayOf<ImapServer>(ImapServer.TableName);

            var result = new List<(ImapClient, ImapServer)>();
            foreach (var client in clients)
            {
                var server = servers.First(x => x.Uid == client.ImapServerUid);
                result.Add((client, server));
            }

            return result.ToArray();
        }

        public static void UpdateClientIsWorking(SqlServerShell shell, bool? isWorkingNow, ImapClient client)
        {
            var isClientWorking = shell.ExecuteScalar($"SELECT {nameof(ImapClient.IsWorking)}" +
                $" FROM {SqlServerShell.InitialCatalog}.dbo.{ImapClient.TableName} WHERE {nameof(ImapClient.Uid)} = '{client.Uid}'");

            if (isWorkingNow?.ToString() != isClientWorking?.ToString())
            {
                shell.UpdateCell(nameof(ImapClient.IsWorking), isWorkingNow?.ToString()
                    , $"WHERE {nameof(ImapClient.Uid)} = '{client.Uid}'", ImapClient.TableName);
                client.IsWorking = isWorkingNow;
            }
        }

        /// <summary>
        ///     Взять последний индекс письма (из писем текущему клиенту).
        /// </summary>
        public int GetLastMailIndexFromFolder(Guid folderUid)
        {
            _cancellationToken.ThrowIfCancellationRequested();

            var maxFolderIndex = SqlShell.ExecuteScalar($"SELECT MAX({nameof(Mail.IndexInFolder)}) " +
                $"FROM {SqlServerShell.InitialCatalog}.dbo.{Mail.TableName} WHERE {nameof(Mail.FolderUid)} = '{folderUid}'");

            if (int.TryParse(maxFolderIndex?.ToString(), out var result))
            {
                return result;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        ///     Сохранить новое письмо.
        /// </summary>
        public void SaveMail(ImapMailParams imapMailParams)
        {
            _cancellationToken.ThrowIfCancellationRequested();

            var folder = Folders.FirstOrDefault(x => x.FullName == imapMailParams.Folder.FullName);
            if (folder == null)
            {
                folder = CreateFolder(imapMailParams.Folder);
            }
            var mail = new Mail(imapMailParams, folder.Uid);

            SqlShell.Insert(mail, Mail.TableName);
        }

        /// <summary>
        ///     Сохранить новую папку.
        /// </summary>
        public Folder CreateFolder(IMailFolder mailFolder)
        {
            _cancellationToken.ThrowIfCancellationRequested();

            var folder = new Folder(mailFolder, SqlClient.Uid);
            SqlShell.Insert(folder, Folder.TableName);
            _folders.Add(folder);

            return folder;
        }

        /// <summary>
        ///     Взять полный список папок.
        /// </summary>
        public List<Folder> GetAllFolders()
        {
            _cancellationToken.ThrowIfCancellationRequested();

            return SqlShell.GetArrayOf<Folder>(Folder.TableName, $"WHERE {nameof(Folder.ImapClientUid)} = {SqlClient.Uid}")
                .ToList();
        }
    }
}
