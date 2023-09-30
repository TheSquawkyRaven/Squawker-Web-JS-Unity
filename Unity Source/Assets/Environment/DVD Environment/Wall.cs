using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{

    public enum Direction
    {
        North,
        South,
        West,
        East
    }
    public Direction direction;
    public DVDEnvironment dvdEnvironment;

    public Vector2 GetNormal()
    {
        switch (direction)
        {
            case Direction.North:
                return new Vector2(0, -1);
            case Direction.South:
                return new Vector2(0, 1);
            case Direction.West:
                return new Vector2(1, 0);
            case Direction.East:
                return new Vector2(-1, 0);
        }
        return Vector2.zero;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        dvdEnvironment.currentDirection = Vector2.Reflect(dvdEnvironment.currentDirection, collision.contacts[0].normal);
        dvdEnvironment.Bounce();
    }

}
