using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ShowHover : NetworkBehaviour {
    public GameObject roadPrefab;
    [HideInInspector]
    public GameObject roadInstance;
    private Board board;
    private GameObject roadContainer;
    private Hex currentHex;
    private Direction currentDirection;

	// Use this for initialization
	void Start () {
        board = GameObject.Find("Board").GetComponent<Board>();
        roadContainer = GameObject.Find("Roads");
    }

    // Update is called once per frame
    void Update () {
        if(!isLocalPlayer) return;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, 100.0f)) {
            if(hit.collider.name.Contains("Hexagon")) {
                if(roadInstance == null) {
                    roadInstance = Instantiate(roadPrefab);
                    roadInstance.transform.parent = this.transform;
                    roadInstance.transform.localScale = new Vector3(1, 1, 1);
                }

                currentHex = hit.collider.gameObject.GetComponent<Hex>();
                Vector3 localHit = hit.point;

                localHit = currentHex.transform.InverseTransformPoint(localHit);

                float angleFromCenter = Mathf.Atan2(localHit.z, localHit.x) * 180 / Mathf.PI;
                currentDirection = currentHex.buildingManager.angleToDirection((int) Mathf.Round(angleFromCenter));

                if(currentHex.buildingManager.roads.ContainsKey(currentDirection)) {
                    Destroy(roadInstance);
                    roadInstance = null;
                } else {
                    Vector3 transformPosition = currentHex.buildingManager.directionToGlobalPosition(currentDirection);

                    float angleToRotate = currentHex.buildingManager.directionToRotateAngle(currentDirection);

                    roadInstance.transform.localPosition = transformPosition;
                    roadInstance.transform.eulerAngles = new Vector3(0, angleToRotate);
                }
            }
        } else {
            Destroy(roadInstance);
            roadInstance = null;
            currentHex = null;
        }

        if (Input.GetMouseButtonDown(0) && roadInstance != null) {
            Debug.Log("Spawning road");
            CmdSpawnRoad(currentHex .xCoord, currentHex.yCoord, currentDirection, roadInstance.transform.localPosition, roadInstance.transform.localRotation);

            Destroy(roadInstance);
            roadInstance = null;
        }
    }

    [Command]
    void CmdSpawnRoad(int xCoord, int yCoord, Direction direction, Vector3 roadPosition, Quaternion roadRotation) {
        GameObject newRoad = Instantiate(
            roadPrefab, 
            roadPosition, 
            roadRotation) as GameObject;
        newRoad.transform.localScale = new Vector3(1, 1, 1);
        newRoad.transform.parent = roadContainer.transform;

        Road roadAsRoad = newRoad.GetComponent<Road>();
        roadAsRoad.preview = false;
        roadAsRoad.xCoord = xCoord;
        roadAsRoad.yCoord = yCoord;
        roadAsRoad.direction = direction;

        NetworkServer.Spawn(newRoad);
    }
}
