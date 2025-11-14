namespace MusicService.Shared.DTOs;
public class SongDto
{
    public string Title { get; set; } = null!;
    public string Artist { get; set; } = null!;
    public string Album { get; set; } = null!;
    public string Duration { get; set; } = null!;
}