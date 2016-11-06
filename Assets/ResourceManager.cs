using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class ResourceManager : NetworkBehaviour {
    private List<HexType>[][] resourcesOnTurn;
	// Use this for initialization
	void Start () {
	}

    public void createResourceList(int numplayers) {
        resourcesOnTurn = new List<HexType>[numplayers][];
        for (int i = 0; i < numplayers; i++) {
            resourcesOnTurn[i] = new List<HexType>[13];
            for (int j = 0; j < 13; j++) {
                resourcesOnTurn[i][j] = new List<HexType>();
            }
        }
    }

    public void addResource(int playerIndex, int turn, HexType resource) {
        resourcesOnTurn[playerIndex][turn].Add(resource);
    }

    public List<HexType> getResourcesOfPlayer(int playerIndex, int turn) {
        return resourcesOnTurn[playerIndex][turn];
    }

    // Update is called once per frame
	void Update () {
	}
}
