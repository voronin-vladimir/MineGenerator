namespace MazeGenerator
{
    public struct MazeCell
    {
        //Replace with Vector2Int        
        public readonly int X;
        public readonly int Y;

        public bool IsVisited;
        public WallPosition Walls;

        public MazeCell(int x, int y) : this()
        {
            X = x;
            Y = y;
            IsVisited = false;
            Walls = WallPosition.Left | WallPosition.Top | WallPosition.Right | WallPosition.Bottom;
        }

        public void Visit() => IsVisited = true;

        public void RemoveWall(WallPosition wallToRemove) => Walls &= ~wallToRemove;
    }
}