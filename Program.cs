using ZeldaGame.models;
using ZeldaGame.shared;

Game currentGame = GameManager.StartGame();

while (currentGame.CurrentCommand != Constants.EXIT_COMMAND)
{
    currentGame = GameManager.HandleUserCommand(Console.ReadLine() ?? string.Empty, currentGame);
}

if (currentGame.IsPrincessSaved)
{
    string message = GameManager.OpenFile("assets/EndWin.txt") ?? "You won";
    Console.WriteLine(message);
}
else
{
    string message = GameManager.OpenFile("assets/EndLose.txt") ?? "You lost";
    Console.WriteLine(message);
}