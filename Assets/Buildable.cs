using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public abstract class Buildable : NetworkBehaviour {
    public string identifer;
    [SyncVar]
    public bool preview = true;
    [SyncVar]
    public int xCoord, yCoord;
    [SyncVar]
    public Direction direction;

    public GameObject hexPrefab;
    protected Vector3 hexSize;
    protected Board board;

	// Use this for initialization
	protected void Start ()
	{
	    hexSize = hexPrefab.GetComponent<MeshFilter>().sharedMesh.bounds.size;

        board = GameObject.Find("Board").GetComponent<Board>();

        Color color = this.GetComponent<Renderer>().material.color;
        if(preview) color.a = .76f;
        else {
            color.a = 1;
            addToBoard();
        }
        this.GetComponent<Renderer>().material.color = color;
    }

    // Update is called once per frame
    void Update () {
	
	}

    protected abstract void addToBoard();
    public abstract float directionToRotateAngle(Direction direction);
    public abstract Vector3 directionToLocalPosition(Direction direction);

    protected int startRotation;
    public static Direction[] directionArray = new Direction[] { Direction.NORTHEAST, Direction.NORTHWEST, Direction.WEST, Direction.SOUTHWEST, Direction.SOUTHEAST, Direction.EAST };
    public Direction angleToDirection(int angle) {
        if(angle < 0) angle = 360 + angle;
        int start = startRotation;
        foreach (Direction dir in directionArray) {
            if(angle > start && angle <= start + 60) return dir;
            start += 60;
        }
        return Direction.EAST;
    }
}
