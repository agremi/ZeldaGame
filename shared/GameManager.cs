using System.Text.Json;
using ZeldaGame.models;

namespace ZeldaGame.shared;

public static class GameManager
{
    public static Game StartGame()
    {
        Game currentGame = MakeSetup();
        return currentGame;
    }

    private static void WriteOpening()
    {
        string opening = OpenFile("assets/Start.txt");
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine(opening);
    }

    private static string OpenFile(string path)
    {
        try
        {
            StreamReader reader = new StreamReader(path);
            string text = reader.ReadToEnd();

            //close the file
            reader.Close();
            return text;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Errore durante la lettura del file: {e}");
            throw new IOException();
        }
    }

    private static string AskForName()
    {
        string userName = string.Empty;
        while (string.IsNullOrWhiteSpace(userName))
        {
            Console.WriteLine("Enter username: ");
            userName = Console.ReadLine() ?? string.Empty;
        }

        return userName;
    }

    private static Game MakeSetup()
    {
        Game currentGame = new Game();

        string jsonText = OpenFile("assets/Rooms.txt");
        List<Room>? deserializedResponse = JsonSerializer.Deserialize<List<Room>>(jsonText);

        if (deserializedResponse == null)
        {
            throw new JsonException("Error while deserializing rooms file");
        }

        currentGame.RoomsList = deserializedResponse;
        currentGame.PlayerName = AskForName();

        // Stampo apertura
        WriteOpening();

        // Setto la prima stanza
        currentGame.CurrentRoom = deserializedResponse.First();

        return currentGame;
    }

    public static Room HandleMoveCommand(Game currentGame, string commandParameter)
    {
        switch (commandParameter)
        {
            case Constants.MOVE_NORTH:
                HandleDirection(currentGame, commandParameter, 0);
                break;

            case Constants.MOVE_WEST:
                HandleDirection(currentGame, commandParameter, 1);
                break;

            case Constants.MOVE_SOUTH:
                HandleDirection(currentGame, commandParameter, 2);
                break;

            case Constants.MOVE_EAST:
                HandleDirection(currentGame, commandParameter, 3);
                break;

            default:
                break;
        }
    }

    private static void HandleDirection(Game currentGame, string commandParameter, int roomIndex)
    {
        if (currentGame?.CurrentRoom?.ConnectedRooms != null &&
            roomIndex >= 0 &&
            roomIndex < currentGame.CurrentRoom.ConnectedRooms.Count &&
            !string.IsNullOrEmpty(currentGame.CurrentRoom.ConnectedRooms[roomIndex]))
        {
            var targetRoom = currentGame.RoomsList
                .FirstOrDefault<Room>(x => x.Name.Equals(currentGame.CurrentRoom.ConnectedRooms[roomIndex]));
            if (targetRoom != null)
            {
                currentGame.CurrentRoom = targetRoom;
            }
        }
        else
        {
            Console.ForegroundColor=ConsoleColor.DarkRed;
            Console.WriteLine($"Move {commandParameter} is not possible");
        }
    }

}
