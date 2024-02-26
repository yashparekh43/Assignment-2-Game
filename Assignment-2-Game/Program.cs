using System;

public class Position
{
    public int X { get; }
    public int Y { get; }

    public Position(int x, int y)
    {
        X = x;
        Y = y;
    }
}

public class Player
{
    public string Name { get; }
    public Position Position { get; set; }
    public int GemCount { get; set; }

    public Player(string name, Position position)
    {
        Name = name;
        Position = position;
        GemCount = 0;
    }

    public void Move(char direction)
    {
        switch (direction)
        {
            case 'U':
                Position = new Position(Position.X, Position.Y - 1);
                break;
            case 'D':
                Position = new Position(Position.X, Position.Y + 1);
                break;
            case 'L':
                Position = new Position(Position.X - 1, Position.Y);
                break;
            case 'R':
                Position = new Position(Position.X + 1, Position.Y);
                break;
            default:
                Console.WriteLine("Wrong statement!");
                break;
        }
    }
}

public class Cell
{
    public string Occupant { get; set; }

    public Cell(string occupant)
    {
        Occupant = occupant;
    }
}

public class Board
{
    public Cell[,] Grid { get; }

    public Board()
    {
        Grid = new Cell[6, 6];
        InitializeBoard();
    }

    private void InitializeBoard()
    {
        // Initialize cells with empty spaces
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                Grid[i, j] = new Cell("-");
            }
        }

        // Place players
        Grid[0, 0].Occupant = "P1";
        Grid[5, 5].Occupant = "P2";

        // Place gems (for simplicity, let's place 3 gems randomly)
        Random random = new Random();
        for (int i = 0; i < 3; i++)
        {
            int x = random.Next(6);
            int y = random.Next(6);
            if (Grid[x, y].Occupant == "-")
            {
                Grid[x, y].Occupant = "G";
            }
            else
            {
                i--; // Try again if the cell is not empty
            }
        }

        // Place obstacles (for simplicity, let's place 5 obstacles randomly)
        for (int i = 0; i < 5; i++)
        {
            int x = random.Next(6);
            int y = random.Next(6);
            if (Grid[x, y].Occupant == "-")
            {
                Grid[x, y].Occupant = "O";
            }
            else
            {
                i--; // Try again if the cell is not empty
            }
        }
    }

    public void Display()
    {
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                Console.Write(Grid[i, j].Occupant + " ");
            }
            Console.WriteLine();
        }
    }

    public bool IsValidMove(Player player, char direction)
    {
        int newX = player.Position.X;
        int newY = player.Position.Y;

        switch (direction)
        {
            case 'U':
                newY--;
                break;
            case 'D':
                newY++;
                break;
            case 'L':
                newX--;
                break;
            case 'R':
                newX++;
                break;
            default:
                Console.WriteLine("Wrong statement!");
                return false;
        }

        if (newX < 0 || newX >= 6 || newY < 0 || newY >= 6)
        {
            return false; // Out of bounds
        }

        if (Grid[newX, newY].Occupant == "O")
        {
            return false; // Obstacle
        }

        return true;
    }

    public void CollectGem(Player player)
    {
        if (Grid[player.Position.X, player.Position.Y].Occupant == "G")
        {
            player.GemCount++;
            Grid[player.Position.X, player.Position.Y].Occupant = "-";
        }
    }
}

public class Game
{
    public Board Board { get; }
    public Player Player1 { get; }
    public Player Player2 { get; }
    public Player CurrentTurn { get; private set; }
    public int TotalTurns { get; private set; }

    public Game()
    {
        Board = new Board();
        Player1 = new Player("P1", new Position(0, 0));
        Player2 = new Player("P2", new Position(5, 5));
        CurrentTurn = Player1;
        TotalTurns = 0;
    }

    public void Start()
    {
        while (!IsGameOver())
        {
            Console.WriteLine($"Turn {TotalTurns + 1}: {CurrentTurn.Name}'s turn");
            Board.Display();
            Console.WriteLine("Enter move (U/D/L/R): ");
            char direction = char.ToUpper(Console.ReadKey().KeyChar);
            Console.WriteLine();

            if (Board.IsValidMove(CurrentTurn, direction))
            {
                CurrentTurn.Move(direction);
                Board.CollectGem(CurrentTurn);
                TotalTurns++;
                SwitchTurn();
            }
            else
            {
                Console.WriteLine("That's a bad move! Do it once more..");
            }
        }

        DeclareTheWinner();
    }

    public void SwitchTurn()
    {
        CurrentTurn = (CurrentTurn == Player1) ? Player2 : Player1;
    }

    public bool IsGameOver()
    {
        return TotalTurns >= 30;
    }

    public void DeclareTheWinner()
    {
        Console.WriteLine("Game Over!");
        Console.WriteLine($"Player 1 collected {Player1.GemCount} gems");
        Console.WriteLine($"Player 2 collected {Player2.GemCount} gems");

        if (Player1.GemCount > Player2.GemCount)
        {
            Console.WriteLine("Player 1 has won.!");
        }
        else if (Player2.GemCount > Player1.GemCount)
        {
            Console.WriteLine("Player 2 has won.!");
        }
        else
        {
            Console.WriteLine("It's a tie for the match!");
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        Game gemHunters = new Game();
        gemHunters.Start();
    }
}