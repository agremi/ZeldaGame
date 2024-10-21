using System.Text.RegularExpressions;
using ZeldaGame.models;
using ZeldaGame.shared;

Game currentGame = GameManager.StartGame();

string currentCommand = string.Empty;

while (currentCommand != Constants.EXIT_COMMAND)
{
    currentCommand = handleUserCommand(Console.ReadLine() ?? string.Empty);
}

Console.WriteLine("Fuck you I'm eaving");


string handleUserCommand(string userCommand)
{
    string mainCommand = Regex.Match(userCommand, Constants.MOVE_REGEX).ToString();
    string commandParameter;
    string result = string.Empty;

    switch (mainCommand)
    {
        case Constants.MOVE_COMMAND:
            commandParameter = Regex.Match(userCommand, @"NORTH|WEST|SOUTH|EAST").ToString();
            GameManager.HandleMoveCommand(currentGame, commandParameter);
            result = Constants.MOVE_COMMAND;
            break;
        
        case Constants.PICK_COMMAND:
        break;

        case Constants.DROP_COMMAND:
        break;

        case Constants.EXIT_COMMAND:

        result = Constants.EXIT_COMMAND;
        break;

        case Constants.ATTACK_COMMAND:
        break;

        case Constants.LOOK_COMMAND:
        break;

        default:
            break;
    }

    return result;
}