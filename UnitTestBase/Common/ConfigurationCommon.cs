using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace UnitTestBase.Common
{
    public class ConfigurationCommon
    {
        protected static IConfiguration _configuration;

        public static IConfiguration Configuration
        {
            get => _configuration;
            set => _configuration = value;
        }

        public static void SetConfiguration()
        {
            try
            {
                _configuration = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory().Replace("\\bin\\Debug", "").Replace("\\netcoreapp3.1", ""))
                   .AddJsonFile(@"TestSettings.json", false, false)
                   .AddEnvironmentVariables()
                   .Build();
            }
            catch (Exception)
            {
            }
        }
    }
}
