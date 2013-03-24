﻿using System;
using System.Configuration;

namespace JabbR.Services
{
    public class ApplicationSettings : IApplicationSettings
    {
        public string EncryptionKey
        {
            get
            {
                return ConfigurationManager.AppSettings["jabbr:encryptionKey"];
            }
        }

        public string VerificationKey
        {
            get
            {
                return ConfigurationManager.AppSettings["jabbr:verificationKey"];
            }
        }

        public string DefaultAdminUserName
        {
            get
            {
                return ConfigurationManager.AppSettings["jabbr:defaultAdminUserName"];
            }
        }

        public string DefaultAdminPassword
        {
            get
            {
                return ConfigurationManager.AppSettings["jabbr:defaultAdminPassword"];
            }
        }

        public AuthenticationMode AuthenticationMode
        {
            get
            {
                string modeValue = ConfigurationManager.AppSettings["jabbr:authenticationMode"];
                AuthenticationMode mode;
                if (Enum.TryParse<AuthenticationMode>(modeValue, out mode))
                {
                    return mode;
                }

                return AuthenticationMode.Default;
            }
        }

        public bool RequireHttps
        {
            get
            {
                string requireHttpsValue = ConfigurationManager.AppSettings["jabbr:requireHttps"];
                bool requireHttps;
                if (Boolean.TryParse(requireHttpsValue, out requireHttps))
                {
                    return requireHttps;
                }
                return false;
            }
        }

        public bool MigrateDatabase
        {
            get
            {
                string migrateDatabaseValue = ConfigurationManager.AppSettings["jabbr:migrateDatabase"];
                bool migrateDatabase;
                if (Boolean.TryParse(migrateDatabaseValue, out migrateDatabase))
                {
                    return migrateDatabase;
                }
                return false;
            }
        }

        public bool ProxyImages
        {
            get
            {
                string proxyImagesValue = ConfigurationManager.AppSettings["jabbr:proxyImages"];
                bool proxyImages;
                if (Boolean.TryParse(proxyImagesValue, out proxyImages))
                {
                    return proxyImages;
                }
                return false;
            }
        }

        public string ImagurClientId
        {
            get
            {
                return ConfigurationManager.AppSettings["jabbr:imagurClientId"];
            }
        }

        public string AzureblobStorageConnectionString
        {
            get
            {
                return ConfigurationManager.AppSettings["jabbr:azureblobStorageConnectionString"];
            }
        }

        public int MaxFileUploadBytes
        {
            get
            {
                string maxFileUploadBytesValue = ConfigurationManager.AppSettings["jabbr:maxFileUploadBytes"];
                int maxFileUploadBytes;
                if (Int32.TryParse(maxFileUploadBytesValue, out maxFileUploadBytes))
                {
                    return maxFileUploadBytes;
                }
                return 0;
            }
        }
    }
}