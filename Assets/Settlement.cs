using UnityEngine;
using System.Collections;
using System;

public class Settlement : Buildable {
    public GameObject settlementPrefab;
    private Vector3 settlementSize;

    // Use this for initialization
    void Start() {
        base.Start();
        settlementSize = settlementPrefab.GetComponent<MeshFilter>().sharedMesh.bounds.size;

        // Hexes are scaled 5% to make them look nice
//        hexSize.x *= 1.05f;
//        hexSize.y *= 1.05f;
//        hexSize.z *= 1.05f;

    }

    // Update is called once per frame
    void Update() {

    }

    protected override void addToBoard() {
        board.getHex(xCoord, yCoord).buildingManager.roads.Add(direction, this);

        Hex adjacentHex = board.getHex(xCoord + (int) direction.directionVector().x, yCoord + (int) direction.directionVector().y);
        if(adjacentHex != null) adjacentHex.buildingManager.roads.Add(direction.oppositeDirection(), this);
    }

    public override Vector3 directionToLocalPosition(Direction direction) {
        float xRadius = hexSize.x / 2, zRadius = hexSize.z / 2f;
        switch(direction) {
            //Work out the vector for half the directions and for the opposite direction just grab it from the adjacent hex
            case Direction.EAST:
                return new Vector3(xRadius, hexSize.y, -zRadius / 2);
            case Direction.NORTHEAST:
                return new Vector3(xRadius, hexSize.y, zRadius / 2);
            case Direction.SOUTHEAST:
                return new Vector3(0, hexSize.y, -zRadius);
            case Direction.NORTHWEST:
                return new Vector3(0, hexSize.y, zRadius);
            case Direction.WEST:
                return new Vector3(-xRadius, hexSize.y, zRadius / 2);
            case Direction.SOUTHWEST:
                return new Vector3(-xRadius, hexSize.y, -zRadius / 2);
            default:
                Debug.LogError("ERROR Unknown direction: " + direction);
                return new Vector3();
        }
    }

    public override float directionToRotateAngle(Direction direction) {
        switch(direction) {
            case Direction.EAST:
            case Direction.WEST:
                return 30;

            case Direction.NORTHEAST:
            case Direction.SOUTHWEST:
                return -30;

            case Direction.SOUTHEAST:
            case Direction.NORTHWEST:
                return 90;
            default:
                Debug.Log("ERROR INVALID DIRECTION");
                return 0;
        }
    }
}
