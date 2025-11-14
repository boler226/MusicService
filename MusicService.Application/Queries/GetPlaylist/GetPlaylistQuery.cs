using MediatR;
using MusicService.Shared.DTOs;

namespace MusicService.Application.Queries.GetPlaylist;
public record GetPlaylistQuery(string Url) : IRequest<PlaylistDto>;