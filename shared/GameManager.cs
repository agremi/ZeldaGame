using System;
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
        currentGame.CurrentRoom = deserializedResponse.First();
        currentGame.PlayerName = AskForName();

        WriteOpening();

        return currentGame;
    }
}
