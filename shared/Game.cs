using System;
using ZeldaGame.models;

namespace ZeldaGame.shared;

public class Game
{
    public string PlayerName { get; set; }
    public List<GameItem> PlayerBag { get; set; }

    public Room ActualRoom { get; set; }
}