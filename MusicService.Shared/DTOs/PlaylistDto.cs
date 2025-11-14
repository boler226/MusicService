namespace MusicService.Shared.DTOs;
public class PlaylistDto
{
    public string Name { get; set; } = null!;
    public string AvatarUrl { get; set; } = null!;
    public string Description { get; set; } = null!;
    public List<SongDto> Songs { get; set; } = new();
}