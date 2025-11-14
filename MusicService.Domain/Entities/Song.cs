namespace MusicService.Domain.Entities;
public class Song
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Artist { get; set; } = null!;
    public string Album { get; set; } = null!;
    public TimeSpan Duration { get; set; }
}
