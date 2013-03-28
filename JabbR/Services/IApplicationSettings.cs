﻿
namespace JabbR.Services
{
    public interface IApplicationSettings
    {
        string EncryptionKey { get; }
        string VerificationKey { get; }

        string DefaultAdminUserName { get; }
        string DefaultAdminPassword { get; }
        AuthenticationMode AuthenticationMode { get; }

        bool RequireHttps { get; }
        bool MigrateDatabase { get; }
        bool ProxyImages { get; }
        int ProxyImageMaxSizeBytes { get; }

        string ImagurClientId { get; }
        string AzureblobStorageConnectionString { get; }
        string FileSystemUploadStorageLocation { get; }
        string FileSystemUploadChatUrl { get; }

        int MaxFileUploadBytes { get; }
    }
}
