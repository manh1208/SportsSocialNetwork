using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace SportsSocialNetwork.Models.Utilities
{
    public class Setting
    {
        public static string CREDENTIAL_EMAIL = GetDataFromWebConfig("CredentialEmail");
        public static string CREDENTIAL_PASSWORDS = GetDataFromWebConfig("CredentialPasswords");

        private static string GetDataFromWebConfig(string key)
        {
            return WebConfigurationManager.AppSettings[key];
        }
    }
}