using MusicService.Application.Services;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace MusicService.Infrastructure.Selenium;
public class ChromeDriverFactory : IWebDriverFactory
{
    public IWebDriver Create()
    {
        var options = new ChromeOptions();
        options.AddArgument("--start-maximized");
        options.AddArgument("--headless=new");
        options.AddArgument("--disable-gpu");
        options.AddArgument("--disable-blink-features=AutomationControlled");
        options.AddExcludedArgument("enable-automation");
        options.AddAdditionalOption("useAutomationExtension", false);
        options.AddArgument("--user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) " +
                            "AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36");

        return new ChromeDriver(options);
    }
}
