using System;

namespace MazeGenerator
{
    [Flags]
    public enum WallPosition
    {
        None = 0,
        Left = 1 << 0, // 0b_00000001 // 1
        Top = 1 << 1, // 0b_00000010 // 2
        Right = 1 << 2, // 0b_00000100 // 4
        Bottom = 1 << 3, // 0b_00001000 // 8
    }
}