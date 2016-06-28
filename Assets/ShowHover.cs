using UnityEngine;
using System.Collections;

public class ShowHover : MonoBehaviour {
    public GameObject roadPrefab;
    [HideInInspector]
    public GameObject roadInstance;
    private Hex currentHex;
    private Hex.Direction currentDirection;

	// Use this for initialization
	void Start () {
    }

    // Update is called once per frame
    void Update () {
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
            currentHex.buildingManager.addRoad(currentDirection, roadInstance);
        }
    }
}
