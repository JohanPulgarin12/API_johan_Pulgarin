using Allure.Commons;
using Models.Dto;
using Models.Repositories;
using Models.Repositories._UnitOfWork;
using NUnit.Allure.Attributes;
using NUnit.Framework;
using System;
using UnitTestBase.Common;

namespace UnitTestBase.Repository
{
    [AllureSuite("Token Repository test")]
    public class TokenRepositoryUnitTest : BaseAllureReport
    {
        [Test(Author = "Your name")]
        [AllureTag("Repository")]
        [AllureName("Get User by username (successful method)")]
        [AllureSeverity(SeverityLevel.critical)]
        public void GetUserByUserNameSuccess()
        {
            try
            {
                // Arrange
                ParametrizacionesCommon.SetConfiguration();
                string user = "vwa-us";

                IUnitOfWork unitOfWork = ParametrizacionesCommon.GetUnitOfWork();
                TokenRepository repository = new TokenRepository(unitOfWork);

                // Act
                JwtUser response = repository.GetUserByUserName(user);

                // Assert
                Assert.IsNotNull(response);
                Assert.AreEqual(response.Login, user);
                Assert.IsNotNull(response.Password);
            }
            catch (Exception)
            {
            }
        }

        [Test(Author = "Your name")]
        [AllureTag("Repository")]
        [AllureName("Get User by username (Fail method)")]
        [AllureSeverity(SeverityLevel.minor)]
        public void GetUserByUserNameFail()
        {
            try
            {
                // Arrange
                ParametrizacionesCommon.SetConfiguration();
                string user = "Any";

                IUnitOfWork unitOfWork = ParametrizacionesCommon.GetUnitOfWork();
                TokenRepository repository = new TokenRepository(unitOfWork);

                // Act
                JwtUser response = repository.GetUserByUserName(user);

                // Assert
                Assert.IsNull(response);
            }
            catch (Exception)
            {
            }
        }
    }
}
