namespace MusicService.Domain.Entities;
public class Playlist
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string AvatarUrl { get; set; } = null!;
    public List<Song> Songs { get; set; } = new();
}
