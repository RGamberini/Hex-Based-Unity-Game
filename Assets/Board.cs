using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Board : NetworkBehaviour {
    public Hex centerHex;
    public Hex[][] board;
    private Vector3 size;
    private const int boardSize = 2;
    private BoardSetup setup;
    // Use this for initialization
    void Start() {
        if(board == null) populateBoardArray();
        if(setup == null) setup = new BoardSetup();

        if(!isServer) return;

        for (int yCoord = -boardSize; yCoord <= boardSize; yCoord++) {
            for (int xCoord = -boardSize; xCoord <= boardSize; xCoord++) {
                int distance = axialHexdistance(new int[] { xCoord, yCoord }, new int[] { 0, 0 });
                if(distance <= boardSize) {
                    HexType hexType = setup.getRandomHex();
                    int token;
                    if(hexType != HexType.DESERT) token = setup.getRandomToken();
                    else token = -1;
                    createHex(xCoord, yCoord, hexType, token);
                }
            }
        }
    }

    /** 
     * Code smell right here but let me try to explain
     *  for some reason Hex will call back to the board 
     *  before it finishes starting, so to avoid the 
     *  race condition I use two conditionals leading back
     *  to this function
     **/
    public void populateBoardArray() {
        board = new Hex[(boardSize * 2) + 1][];
        size = centerHex.GetComponent<Renderer>().bounds.size;

        for(int i = 0; i < board.Length; i++)
            board[i] = new Hex[(boardSize * 2) + 1];
    }

    private void createHex(int xCoord, int yCoord, HexType hexType, int token) {
        // Create a new hex
        Hex newHex = Instantiate(centerHex);
        newHex.xCoord = xCoord;
        newHex.yCoord = yCoord;
        newHex.diceRoll = token;
        newHex.hexType = hexType;

        newHex.transform.parent = this.transform;
        Vector3 localPosition = gridCoordstoWorldCoords(xCoord, yCoord);

        // The transform is then applyled
        newHex.transform.localPosition = localPosition;

        // This will register the Hex for the Server / Host
        // Client side the Hex itself with register back with the board
        registerHex(xCoord, yCoord, newHex);

        newHex.parentNetId = this.netId;
        NetworkServer.Spawn(newHex.GetComponent<Collider>().gameObject);
    }

    // Necessary because the client's board never builds this array
    public void registerHex(int xCoord, int yCoord, Hex hex) {
        board[xCoord + boardSize][yCoord + boardSize] = hex;
    }

    public Vector3 gridCoordstoWorldCoords(int xCoord, int yCoord) {
        Vector3 localPosition = new Vector3();
        // First X and Y are calculated from the Y coordinate
        // The Y coordinate represents which axial the hex falls on
        localPosition.x = yCoord * (size.x / -2f);
        localPosition.z = yCoord * (size.z - (size.z / 4));

        // After that the X coordinate is used to place the hex within it's row
        localPosition.x = localPosition.x + (xCoord * size.x);
        return localPosition;
    }

    private int axialHexdistance(int[] xy1, int[] xy2) {
        int deltaX = xy1[0] - xy2[0];
        int deltaY = xy1[1] - xy2[1];
        int deltaZ = deltaX - deltaY;
        return Mathf.Max(Mathf.Abs(deltaX), Mathf.Abs(deltaY), Mathf.Abs(deltaZ));
    }

    public bool inRange(int xCoord, int yCoord) {
        int properXCoord = xCoord + boardSize;
        int properYCoord = yCoord + boardSize;

        return (properXCoord >= 0 && properXCoord < board.Length) && (properYCoord >= 0 && properYCoord < board[0].Length);
    }

    public Hex getHex(int xCoord, int yCoord) {
        if (inRange(xCoord, yCoord))
            return board[xCoord + boardSize][yCoord + boardSize];
        return null;
    }

    // Update is called once per frame
    void Update() {

    }
}
