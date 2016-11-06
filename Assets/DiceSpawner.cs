using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class DiceSpawner: NetworkBehaviour {
    public Die diePrefab;
    public bool active = false;
    [SyncVar]
    public int lastThrownDie;
    public IEnumerator callback;

    private Text dieCount;
    private Die[] dice = new Die[2];
    private const int dieForce = 5200;
    private Player player;
    // Use this for initialization
    void Start () {
        if (isServer) dieCount = GameObject.Find("Die Count").GetComponent<Text>();
        player = GetComponent<Player>();
    }

    private Vector3 startPos;
    private bool rolling;
	// Update is called once per frame
	void Update () {
        if(!isLocalPlayer) return;
	    if (!active) return;

        if(Input.GetMouseButtonDown(1)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit, 100.0f)) {
                if(hit.collider.GetComponent<Hex>() != null) {
                    CmdRollDice(hit.point);
                }
            }
        }
    }

    private static readonly Vector3[] DieRotations = {
        new Vector3(0, 0, 270), new Vector3(0, 0, 90), new Vector3(180, 0),
        new Vector3(), new Vector3(270, 0), new Vector3(90, 0)};

    [Command]
    void CmdRollDice(Vector3 toPosition) {
        Vector3 randomness = new Vector3(15f, 0, 15f);
        for(int delta = -1, index = 0; delta <= 1; delta += 2, index++) {
            Vector3 localPosition = new Vector3(0, 15) + toPosition + (randomness * delta);
            RpcRollDice(index, 
                localPosition,
                Random.Range(0, DieRotations.Length),
                toPosition,
                new Vector3(Random.Range(-360, 360), Random.Range(-360, 360), Random.Range(-360, 360)));
        }
        StartCoroutine(getNumberOnDice());
    }
    [ClientRpc]
    void RpcRollDice(int index, Vector3 localPosition, int dieRotation, Vector3 toPosition, Vector3 torque) {
        GameObject die = Instantiate(diePrefab.gameObject) as GameObject;
        dice[index] = die.GetComponent<Die>();

        die.transform.localPosition = localPosition;
        die.transform.Rotate(DieRotations[dieRotation]);

        Rigidbody body = die.GetComponent<Rigidbody>();
        body.AddForce((toPosition - die.transform.localPosition).normalized * dieForce * body.mass);
        body.AddTorque(torque);

        //if(index == 1 && isServer) StartCoroutine(getNumberOnDice());
    }

    public IEnumerator getNumberOnDice() {
        player.disableDice();
        yield return new WaitForSeconds(3f);
        int total = 0;
        for(int i = 0; i < dice.Length; i++) {
            total += dice[i].getNumber();
            RpcDeleteDie(i);
        }
        lastThrownDie = total;
        callback.MoveNext();
    }
    [ClientRpc]
    void RpcDeleteDie(int index) {
        GameObject.Destroy(dice[index].gameObject);
        dice[index] = null;
    }
}
