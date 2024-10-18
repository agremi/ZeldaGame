namespace ZeldaGame.models;

public class Room
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public List<string>? ConnectedRooms { get; set; }
    public List<GameItem>? Items { get; set; }
    public Npc? Npc { get; set; }
    public bool IsHidden { get; set; }
}