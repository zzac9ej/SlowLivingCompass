# Reqnroll & Gherkin Syntax Guide

## 1. Gherkin Feature File (`.feature`)

Gherkin uses plain-language to define specifications. In Reqnroll, these are compiled into test methods.

```gherkin
Feature: 尋找適合的避風港
  作為一個想要找地方休息的使用者
  我想要透過心情標籤來篩選店家
  這樣我才能找到最符合現在氛圍的地點

  @ui-test
  Scenario: 選擇「安靜」標籤並進行搜尋
    Given 使用者停留在首頁
    When 使用者點擊標籤 "安靜"
    And 使用者點擊 "尋找避難所" 按鈕
    Then 系統應該顯示 "契合度" 推薦結果
```

## 2. C# Step Definitions

Reqnroll matches the text in Gherkin files to C# methods using Regular Expressions or text matching within Attributes.

```csharp
using Reqnroll;

[Binding]
public class FindSanctuarySteps
{
    // You can use dependency injection!
    public FindSanctuarySteps(ScenarioContext scenarioContext)
    {
    }

    [Given("使用者停留在首頁")]
    public void Given使用者停留在首頁()
    {
        // 實作導航邏輯 (如觸發 bUnit 渲染 MainPage)
    }

    // You can extract parameters using {type} or regex
    [When(@"使用者點擊標籤 ""(.*)""")]
    public void When使用者點擊標籤(string tag)
    {
        // 實作點擊標籤邏輯
    }

    [Then("系統應該顯示 {string} 推薦結果")]
    public void Then系統應該顯示推薦結果(string expectedResult)
    {
        // Assert
    }
}
```

## 3. Sharing State between Steps
Avoid static variables. Inject `ScenarioContext` to pass data between steps safely.

```csharp
_scenarioContext["SelectedTag"] = "安靜";
var tag = _scenarioContext.Get<string>("SelectedTag");
```
