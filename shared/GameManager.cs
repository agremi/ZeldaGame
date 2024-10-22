using System.Text.Json;
using System.Text.RegularExpressions;
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

    public static string OpenFile(string path)
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

    public static Game HandleMoveCommand(Game currentGame, string commandParameter)
    {
        switch (commandParameter)
        {
            case Constants.MOVE_NORTH:
                currentGame = HandleDirection(currentGame, commandParameter, 0);
                break;

            case Constants.MOVE_EAST:
                currentGame = HandleDirection(currentGame, commandParameter, 1);
                break;

            case Constants.MOVE_SOUTH:
                currentGame = HandleDirection(currentGame, commandParameter, 2);
                break;

            case Constants.MOVE_WEST:
                currentGame = HandleDirection(currentGame, commandParameter, 3);
                break;

            default:
                break;
        }

        return currentGame;
    }

    private static Game HandleDirection(Game currentGame, string commandParameter, int roomIndex)
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

                if (targetRoom.Name.Equals("Room9"))
                {
                    currentGame.IsPrincessSaved = true;
                }
            }
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"Move {commandParameter} is not possible");
        }
        return currentGame;
    }
    public static Game HandleUserCommand(string userCommand, Game game)
    {
        string mainCommand = Regex.Match(userCommand, Constants.MOVE_REGEX).ToString();
        string commandParameter;

        switch (mainCommand)
        {
            case Constants.MOVE_COMMAND:
                commandParameter = Regex.Match(userCommand, @"NORTH|WEST|SOUTH|EAST").ToString();
                game = GameManager.HandleMoveCommand(game, commandParameter);
                break;

            case Constants.PICK_COMMAND:
                game = HandlePickCommand(game, userCommand);
                break;

            case Constants.DROP_COMMAND:
                game = HandleDropCommand(game, userCommand);
                break;

            case Constants.EXIT_COMMAND:
                game.CurrentCommand = Constants.EXIT_COMMAND;
                break;

            case Constants.ATTACK_COMMAND:
                game = HandleAttackCommand(game);
                break;

            case Constants.LOOK_COMMAND:
                game.GetRoomDescription();
                break;

            default:
                break;
        }

        return game;
    }

    private static Game HandlePickCommand(Game game, string userCommand)
    {
        string commandParameter = userCommand.TrimStart().TrimEnd().Substring(4);
        string itemToPick = commandParameter.TrimStart().TrimEnd();
        GameItem itemToRemove = null;
        foreach (GameItem item in game.CurrentRoom.Items)
        {
            if (item.Name.ToLower() == itemToPick.ToLower())
            {
                game.PlayerBag.Add(item);
                itemToRemove = item;
            }
        }

        if (itemToRemove != null)
            game.CurrentRoom.Items.Remove(itemToRemove);

        return game;
    }

    private static Game HandleDropCommand(Game game, string userCommand)
    {
        string commandParameter = userCommand.TrimStart().TrimEnd().Substring(4);
        string itemToDrop = commandParameter.TrimStart().TrimEnd();
        GameItem itemToRemove = null;

        foreach (GameItem item in game.PlayerBag)
        {
            if (item.Name.ToLower() == itemToDrop.ToLower())
            {
                game.CurrentRoom.Items.Add(item);
                itemToRemove = item;
            }
        }

        if (itemToRemove != null)
            game.PlayerBag.Remove(itemToRemove);

        return game;
    }

    private static Game HandleAttackCommand(Game game)
    {
        if (game.CurrentRoom.Npc != null && game.CurrentRoom.Npc.IsEvil)
        {
            if (game.PlayerBag.Count < 1)
            {
                game.CurrentCommand = Constants.EXIT_COMMAND;
                string endDead = OpenFile("assets/EndDead.txt") ?? "Wasted";
                Console.WriteLine(endDead);
            }
            foreach (GameItem item in game.PlayerBag)
            {
                if (game.CurrentRoom.Npc.WeaponToKillIt == item.Name)
                {
                    Console.WriteLine($"You defeated {game.CurrentRoom.Npc.Name}");
                    // appare nuova porta
                    if (game.CurrentRoom.Name == "Room5")
                    {
                        game.CurrentRoom.ConnectedRooms[2] = "Room8";
                        Console.WriteLine("A new door appears at your south");
                    }
                    else if (game.CurrentRoom.Name == "Room6")
                    {
                        game.CurrentRoom.ConnectedRooms[2] = "Room8";
                        Console.WriteLine("A new door appears at your south");
                    }
                }
                else
                {
                    string closingtext = OpenFile("assets/EndDead");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(closingtext);
                }
            }
        }
        else
        {
            Console.WriteLine("There is no one to kill here");
        }
        return game;
    }
}
