using Microsoft.Extensions.Options;
using OAuth2POC.IDP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OAuth2POC.IDP.Helpers
{
    public class SettingHelper
    {
        private static AppSettings _appSettings;

        public static void Initial(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public static ConfigMapping ConfigMapping
        {
            get
            {
                return _appSettings.ConfigMapping;
            }
        }
    }
}