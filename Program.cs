using System.Text.RegularExpressions;
using ZeldaGame.models;
using ZeldaGame.shared;

Game currentGame = GameManager.StartGame();

while (currentGame.CurrentCommand != Constants.EXIT_COMMAND)
{
    currentGame = GameManager.HandleUserCommand(Console.ReadLine() ?? string.Empty, currentGame);
}

Console.WriteLine("I'm leaving");