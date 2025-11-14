using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using MusicService.Shared.DTOs;

namespace MusicService.UI.ViewModels;

public partial class MainWindowViewModel : ObservableObject, INotifyPropertyChanged
{
    private readonly HttpClient _httpClient = new();

    private PlaylistDto? _playlist;
    public PlaylistDto? Playlist
    {
        get => _playlist;
        set
        {
            _playlist = value;
            OnPropertyChanged(nameof(Playlist));
        }
    }
    public ObservableCollection<SongDto> Songs { get; } = new();
    public MainWindowViewModel()
    {
        _ = LoadPlayList();
    }

    private async Task LoadPlayList()
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
            Console.WriteLine(ex.Message);
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged(string propertyName)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}