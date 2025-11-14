using HtmlAgilityPack;
using MediatR;
using Microsoft.Extensions.Logging;
using MusicService.Application.Services;
using MusicService.Shared.DTOs;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace MusicService.Application.Queries.GetPlaylist;
public class GetPlaylistQueryHandler(IWebDriverFactory _driverFactory) : IRequestHandler<GetPlaylistQuery, PlaylistDto>
{
    public async Task<PlaylistDto> Handle(GetPlaylistQuery request, CancellationToken cancellationToken)
    {
        // Використовуємо Selenium без HtmlAgilityPack, бо HtmlAgilityPack не бачить елементи, згенеровані JavaScript або всередині Shadow DOM.
        using var driver = _driverFactory.Create();

        driver.Navigate().GoToUrl(request.Url);

        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));

        var js = (IJavaScriptExecutor)driver;

        wait.Until(d =>
        {
            var result = js.ExecuteScript(@"
                const container = document.querySelector('#Web\\.TemplatesInterface\\.v1_0\\.Touch\\.DetailTemplateInterface\\.DetailTemplate_1 > music-container');
                if (!container) return false;

                const root = container.shadowRoot || container;
                const rows = root.querySelectorAll('music-image-row');
                return rows && rows.length > 0;
            ");
            return result is bool b && b;
        });

        var jsExecutor = (IJavaScriptExecutor)driver;

        var result = jsExecutor.ExecuteScript(@"
            const header = document.querySelector('#atf > music-detail-header');
            if (!header || !header.shadowRoot) return {};
            const root = header.shadowRoot;

            const nameEl = root.querySelector('#detailHeaderContainer > header > div:nth-child(2) > h1');
            const authorEl = root.querySelector('#detailHeaderContainer > header > div:nth-child(2) > p > music-link > span');
            const descriptionEl = root.querySelector('#detailHeaderContainer > header > div:nth-child(2) > p.secondaryText span');

            const musicImageEl = root.querySelector('#detailHeaderContainer > header > div.image-container.null > music-image');
            let avatarUrl = '';
            if (musicImageEl && musicImageEl.shadowRoot) {
                const img = musicImageEl.shadowRoot.querySelector('picture > img');
                avatarUrl = img ? img.src : '';
            }

            return {
                name: nameEl ? nameEl.innerText.trim() : '',
                author: authorEl ? authorEl.innerText.trim() : '',
                description: descriptionEl ? descriptionEl.innerText.trim() : '',
                avatarUrl: avatarUrl
            };
        ");

        var playlistData = result as IReadOnlyDictionary<string, object> ?? new Dictionary<string, object>();

        var songsResult = jsExecutor.ExecuteScript(@"
        function getShadowRoot(el) {
            return el && el.shadowRoot ? el.shadowRoot : el;
        }

        function queryAllDeep(selector, root = document) {
            const elements = [];
            const traverse = (node) => {
                if (!node) return;
                if (node.querySelectorAll) {
                    node.querySelectorAll(selector).forEach(el => elements.push(el));
                }
                const shadowHosts = node.querySelectorAll('*');
                shadowHosts.forEach(host => {
                    if (host.shadowRoot) traverse(host.shadowRoot);
                });
            };
            traverse(root);
            return elements;
        }

        const allRows = queryAllDeep('music-image-row');
        const songs = [];

        allRows.forEach(row => {
            const title = row.querySelector('div.content > div.col1 > music-link > a')?.innerText?.trim() || '';
            const artist = row.querySelector('div.content > div.col2 > div > music-link:nth-child(1) > a')?.innerText?.trim() || '';
            const album = row.querySelector('div.content > div.col2 > div > music-link:nth-child(3) > a')?.innerText?.trim() || '';
            const duration = row.querySelector('div.content > div.col4 > music-link > span')?.innerText?.trim() || '';

            if (title) {
                songs.push({ title, artist, album, duration });
            }
        });

        return songs;
        ");

        var songsArray = songsResult as IEnumerable<object> ?? Array.Empty<object>();
        var songs = new List<SongDto>();

        foreach (var item in songsArray)
        {
            if (item is IReadOnlyDictionary<string, object> songDict)
            {
                songs.Add(new SongDto
                {
                    Title = songDict.GetValueOrDefault("title")?.ToString() ?? "",
                    Artist = songDict.GetValueOrDefault("artist")?.ToString() ?? "",
                    Album = songDict.GetValueOrDefault("album")?.ToString() ?? "",
                    Duration = songDict.GetValueOrDefault("duration")?.ToString() ?? ""
                });
            }
        }

        var playlist = new PlaylistDto
        {
            Name = playlistData.GetValueOrDefault("name")?.ToString() ?? "",
            Description = playlistData.GetValueOrDefault("description")?.ToString() ?? "",
            AvatarUrl = playlistData.GetValueOrDefault("avatarUrl")?.ToString() ?? "",
            Songs = songs
        };

        return playlist;
    }
}
