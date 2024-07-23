using Entities.DTO;
using Microsoft.Extensions.Configuration;
using Models.Context;
using Models.Repositories._UnitOfWork;
using System.Data;

namespace UnitTestBase.Common
{
    public class ParametrizacionesCommon : ConfigurationCommon
    {
        public static ConfigurationSectionWebApi CreateConfiguration()
        {
            ConfigurationSectionWebApi webApiConfig = _configuration.GetSection("SectionConfigurationWebApi").Get<ConfigurationSectionWebApi>();
            return webApiConfig;
        }

        public static UnitOfWork GetUnitOfWork()
        {
            UnitOfWork work = new UnitOfWork(GetDbConnection());
            return work;
        }

        public static string ConnectContextString()
        {
            return CreateConfiguration().Repository;
        }

        public static IDbConnection GetDbConnection()
        {
            IDbConnection connection = new ConnectionFactory().GetOpenConnection(ConnectContextString(), true);
            return connection;
        }
    }
}
