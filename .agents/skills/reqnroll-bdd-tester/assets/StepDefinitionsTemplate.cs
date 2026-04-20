using System;
using Reqnroll;
// using Bunit; 
// using SlowLivingCompass.Client;

namespace SlowLivingCompass.Tests.StepDefinitions
{
    [Binding]
    public class TemplateStepDefinitions
    {
        private readonly ScenarioContext _scenarioContext;
        // private IBunitContext _bunitContext;

        public TemplateStepDefinitions(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Given(@"使用者開啟了功能頁面")]
        public void Given使用者開啟了功能頁面()
        {
            // Example: _bunitContext = new BunitContext();
            // _scenarioContext["Component"] = _bunitContext.RenderComponent<MyComponent>();
        }

        [When(@"點擊了 ""(.*)"" 按鈕")]
        public void When點擊了按鈕(string buttonName)
        {
            // Example Button Click
        }

        [Then(@"應該要看到預期的結果")]
        public void Then應該要看到預期的結果()
        {
            // Assert logic
        }
    }
}
