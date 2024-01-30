using UnityEngine;

public enum Direction
{
    Up,
    Down,
    Left,
    Right,
    None
}

public class DirectionManager
{
    public static Direction[] directions = { Direction.Up, Direction.Down, Direction.Left, Direction.Right };

    public Direction current;

    public void changeDirection(Direction input)
    {
        current = input;
    }

    public bool isCurrentDirection(Direction input)
    {
        return current == input;
    }

    public static Direction GetCurrentInputDirection()
    {
        // prioritize horizontal movement over vertical
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            return Direction.Left;
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            return Direction.Right;
        }

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            return Direction.Up;
        }

        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            return Direction.Down;
        }

        return Direction.None;
    }

    public static Vector3 DirectionToVector3(Direction input)
    {
        return input switch
        {
            Direction.Left => Vector3.left,
            Direction.Right => Vector3.right,
            Direction.Up => Vector3.up,
            Direction.Down => Vector3.down,
            _ => Vector3.zero
        };
    }
}