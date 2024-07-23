using Allure.Commons;
using NUnit.Allure.Attributes;
using NUnit.Allure.Core;
using NUnit.Framework;

namespace UnitTestBase.Common
{
    [AllureNUnit]
    [AllureParentSuite("Root Suite")]
    public class BaseAllureReport
    {
        private static bool _isNew;

        [SetUp]
        public void CleanupResultDirectory()
        {
            if (!_isNew)
            {
                AllureExtensions.WrapSetUpTearDownParams(() => { AllureLifecycle.Instance.CleanupResultDirectory(); },
                    "Clear Allure Results Directory");
                _isNew = true;
            }
        }
    }
}
