using System;

namespace ZeldaGame.shared;

public static class Constants
{
    //USER COMMANDS
    public const string MOVE_COMMAND = "MOVE";
    public const string PICK_COMMAND = "PICK";
    public const string DROP_COMMAND = "DROP";
    public const string EXIT_COMMAND = "EXIT";
    public const string ATTACK_COMMAND = "ATTACK";
    public const string LOOK_COMMAND = "LOOK";

    //COMMANDS REGEX
    public const string MOVE_REGEX = @"MOVE|PICK|DROP|EXIT|ATTACK|LOOK";

    //MOVE COMMAND PARAMS
    public const string MOVE_NORTH = "NORTH";
    public const string MOVE_WEST = "WEST";
    public const string MOVE_SOUTH = "SOUTH";
    public const string MOVE_EAST = "EAST";
}
