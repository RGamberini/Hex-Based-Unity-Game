using UnityEngine;
public enum Direction {
    EAST, NORTHEAST, NORTHWEST, WEST, SOUTHWEST, SOUTHEAST, NORTH, SOUTH
}

public static class Extensions {
    public static Vector2 directionVector(this Direction direction) {
        switch(direction) {
            case Direction.EAST:
                return new Vector2(1, 0);
            case Direction.WEST:
                return new Vector2(-1, 0);
            case Direction.NORTHEAST:
                return new Vector2(1, 1);
            case Direction.NORTHWEST:
                return new Vector2(0, 1);
            case Direction.SOUTHEAST:
                return new Vector2(0, -1);
            case Direction.SOUTHWEST:
                return new Vector2(-1, -1);
            default:
                if(direction == Direction.NORTH || direction == Direction.SOUTH) {
                    Debug.LogError("ERROR North and South have no direction vector");
                } else {
                    Debug.LogError("ERROR Unknown direction: " + direction);
                }
                return new Vector2();
        }
    }

    public static Direction oppositeDirection(this Direction direction) {
        switch(direction) {
            case Direction.EAST:
                return Direction.WEST;
            case Direction.WEST:
                return Direction.EAST;
            case Direction.NORTHEAST:
                return Direction.SOUTHWEST;
            case Direction.NORTHWEST:
                return Direction.SOUTHEAST;
            case Direction.SOUTHEAST:
                return Direction.NORTHWEST;
            case Direction.SOUTHWEST:
                return Direction.NORTHEAST;
            case Direction.NORTH:
                return Direction.SOUTH;
            case Direction.SOUTH:
                return Direction.NORTH;
            default:
                Debug.LogError("ERROR Unknown direction: " + direction);
                return Direction.NORTH;
        }
    }
}