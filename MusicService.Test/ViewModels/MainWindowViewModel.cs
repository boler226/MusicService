using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using MusicService.Shared.DTOs;
using System.Reactive.Linq;

public class MainWindowViewModel : ReactiveObject
{
    private readonly HttpClient _httpClient = new();

    private PlaylistDto? _playlist;
    public PlaylistDto? Playlist
    {
        get => _playlist;
        set => this.RaiseAndSetIfChanged(ref _playlist, value);
    }

    public ObservableCollection<SongDto> Songs { get; } = new();

    public MainWindowViewModel()
    {
        _ = LoadPlaylist();
    }

    private async Task LoadPlaylist()
    {
        try
        {
            string url = "https://localhost:7157/api/Playlist?Url=https://music.amazon.com/playlists/B01M11SBC8D";
            var playlist = await _httpClient.GetFromJsonAsync<PlaylistDto>(url);

            Playlist = playlist;

            Songs.Clear();
            foreach (var song in playlist.Songs)
            {
                Songs.Add(new SongDto
                {
                    Title = song.Title ?? "",
                    Artist = song.Artist ?? "",
                    Album = song.Album ?? "",
                    Duration = song.Duration ?? ""
                });
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
}
