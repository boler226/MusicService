using OpenQA.Selenium;

namespace MusicService.Application.Services;
public interface IWebDriverFactory
{
    IWebDriver Create();
}