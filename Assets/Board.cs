using UnityEngine;
using System.Collections;

public class Board : MonoBehaviour {
    public Hex centerHex;
    private Vector3 size;
    // Use this for initialization
    void Start() {
        size = centerHex.GetComponent<Collider>().bounds.size;
        int boardSize = 2;

        for (int yCoord = -boardSize; yCoord <= boardSize; yCoord++) {
            for (int xCoord = -boardSize; xCoord <= boardSize; xCoord++) {
                int distance = axialHexdistance(new int[] { xCoord, yCoord }, new int[] { 0, 0 });
                if (distance <= boardSize) createHex(xCoord, yCoord);
            }
        }
    }

    private void createHex(int xCoord, int yCoord) {
        // Create a new hex
        Hex newHex = Instantiate(centerHex);
        newHex.xCoord = xCoord;
        newHex.yCoord = yCoord;

        Vector3 localPosition = gridCoordstoWorldCoords(xCoord, yCoord);

        // The transform is then applyled
        newHex.transform.localPosition = localPosition;
        // This stops lines from appearing between hexes
        newHex.transform.localScale = new Vector3(1.05f, 1.05f, 1.05f);

        newHex.transform.parent = this.transform;
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

    // Update is called once per frame
    void Update() {

    }
}
