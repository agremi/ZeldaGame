using System.Text;

namespace ZeldaGame.models;

public class Game
{
    public string? PlayerName { get; set; }
    public List<GameItem> PlayerBag { get; set; } = new List<GameItem>();
    private Room? _currentRoom;

    public Room? CurrentRoom
    {
        get { return _currentRoom; }
        set
        {
            _currentRoom = value;
            GetRoomDescription();
        }
    }

    public List<Room>? RoomsList { get; set; }
    public float PlayerCash { get; set; }

    public void GetRoomDescription()
    {
        StringBuilder sb = new StringBuilder();

        if (CurrentRoom == null)
        {
            throw new InvalidOperationException("Current room cannot be null");
        }

        // DESCRIZIONE DELLA STANZA
        sb.Append($"{CurrentRoom.Description} \n");

        if (CurrentRoom.ConnectedRooms == null || CurrentRoom.ConnectedRooms.Count < 1)
        {
            throw new InvalidOperationException("A room must have at least one door");
        }

        // USCITE DISPONIBILI
        for (int i = 0; i < CurrentRoom.ConnectedRooms.Count; i++)
        {
            switch (i)
            {
                // NORD
                case 0:
                    if (CurrentRoom.ConnectedRooms[0] != null)
                    {
                        sb.Append("There's a rooom to your NORTH.");
                    }
                    break;

                // EST
                case 1:
                    if (CurrentRoom.ConnectedRooms[1] != null)
                    {
                        sb.Append("There's a rooom to your EAST.");
                    }
                    break;

                // SUD
                case 2:
                    if (CurrentRoom.ConnectedRooms[2] != null)
                    {
                        sb.Append("There's a rooom to your SOUTH.");
                    }
                    break;

                // OVEST
                case 3:
                    if (CurrentRoom.ConnectedRooms[3] != null)
                    {
                        sb.Append("There's a rooom to your WEST.");
                    }
                    break;

                default:
                    break;
            }
        }

        // OGGETTI
        if (CurrentRoom.Items != null)
        {
            CurrentRoom.Items.ForEach(item =>
            {
                sb.Append($"The {item.Name} is lying on the floor.");
            });
        }

        // BAG
        sb.Append("Your bag contains the following items: \n");

        if (PlayerBag.Count < 1)
        {
            sb.Append("Currently, your bag is empty");
        }
        else
        {
            PlayerBag.ForEach(item =>
            {
                sb.Append($"{item.Name} \n");
            });
        }

        // CASH
        sb.Append($"Current cash is: {PlayerCash}");

        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine(sb.ToString());
    }
}