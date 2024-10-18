namespace ZeldaGame.models;

public class Game
{
    public string? PlayerName { get; set; }
    public List<GameItem>? PlayerBag { get; set; }
    public Room? CurrentRoom { get; set; }
    public List<Room>? RoomsList { get; set; }
}