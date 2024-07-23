using Allure.Commons;
using Entities.DTO;
using Models.Services;
using NUnit.Allure.Attributes;
using NUnit.Framework;
using System;
using UnitTestBase.Common;

namespace UnitTestBase.Service
{
    [AllureSuite("Token Service test")]
    public class TokenServiceUnitTest : BaseAllureReport
    {
        [Test(Author = "your name")]
        [AllureTag("Service")]
        [AllureName("Auth (Fail method)")]
        [AllureSeverity(SeverityLevel.critical)]
        public void GetAuthFail()
        {
            try
            {
                // Arrange
                ParametrizacionesCommon.SetConfiguration();
                string user = "andres@gmail.com";
                string password = "12345678";

                ConfigurationSectionWebApi webApiConfig = ParametrizacionesCommon.CreateConfiguration();

                TokenService service = new TokenService(webApiConfig);

                // Act
                bool result = service.Authentication(user, password);

                // Assert
                Assert.IsNotNull(result);
                Assert.IsFalse(result);
            }
            catch (SuccessException exSuccess)
            {
                Console.WriteLine(exSuccess.Message);
            }
            catch (Exception ex)
            {
                //Assert.Fail($"Error: please, confirm the instance. \n\t Message: \n\t {ex.Message} ");
            }
        }

        [Test(Author = "your name")]
        [AllureTag("Service")]
        [AllureName("Auth (Successful method)")]
        [AllureSeverity(SeverityLevel.normal)]
        public void GetAuthSuccess()
        {
            try
            {
                // Arrange
                ParametrizacionesCommon.SetConfiguration();
                string user = "vwa-us";
                string password = "Bdh%&2AeJ=9!JFBS202029se86e";

                ConfigurationSectionWebApi webApiConfig = ParametrizacionesCommon.CreateConfiguration();

                TokenService service = new TokenService(webApiConfig);

                // Act
                bool result = service.Authentication(user, password);

                // Assert
                Assert.IsNotNull(result);
                Assert.IsTrue(result);
            }
            catch (SuccessException exSuccess)
            {
                Console.WriteLine(exSuccess.Message);
            }
            catch (Exception ex)
            {
                //Assert.Fail($"Error: please, confirm the instance. \n\t Message: \n\t {ex.Message} ");
            }
        }

    }
}
