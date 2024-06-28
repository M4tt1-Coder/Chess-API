using Chess_API.Models;

namespace Chess_API.utils;

public interface IProtectionService
{
    public bool HasGameEverythingForPlaying(GameModel? game);
}