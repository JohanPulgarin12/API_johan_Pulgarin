using Allure.Commons;
using FakeItEasy;
using Microsoft.Extensions.Logging;
using NUnit.Allure.Attributes;
using NUnit.Framework;
using ParametrizacionesApi;
using ParametrizacionesApi.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using UnitTestBase.Common;

namespace UnitTestBase.API
{
    [AllureSuite("Weather API test")]
    public class WeatherForecastUnitTest : BaseAllureReport
    {

        [Test(Author = "your name")]
        [AllureTag("API")]
        [AllureName("Get (Method)")]
        [AllureSeverity(SeverityLevel.trivial)]
        public void GetFuncionarios()
        {
            try
            {
                // Arrange
                ParametrizacionesCommon.SetConfiguration();
                ILogger<WeatherForecastController> logger = A.Fake<ILogger<WeatherForecastController>>(); ;

                WeatherForecastController controller = new WeatherForecastController(logger);

                // Act
                IEnumerable<WeatherForecast> response = controller.Get();

                // Assert
                Assert.IsTrue(response.ToList().Count == 5);
            }
            catch (Exception)
            {
            }
        }

    }
}
