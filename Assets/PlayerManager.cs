using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class PlayerManager : NetworkBehaviour {
    public List<Player> players;
    public Player localPlayer;
    public Player currentPlayer;
	// Use this for initialization
	void Start () {
	    players = new List<Player>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public int getNextPlayer() {
        return currentPlayer.playerID + 1 % players.Count;
    }

    public void localPlayerShowHover(Buildable buildable) {
        localPlayer.GetComponent<ShowHover>().buildablePrefab = buildable.gameObject;
    }

    public void setCurrentPlayer(int player) {
        this.currentPlayer = players[player];
    }

    public void nextPlayer() {
        setCurrentPlayer(getNextPlayer());
    }

    [ClientRpc]
    public void RpcEnableHover(int index) {
        players[index].GetComponent<ShowHover>().active = true;
    }

    [ClientRpc]
    public void RpcDisableHover(int index) {
        players[index].GetComponent<ShowHover>().active = false;
    }

    [ClientRpc]
    public void RpcSetBuildable(int playerIndex, int buildableIndex) {
        players[playerIndex].showHover.buildablePrefab = players[playerIndex].showHover.buildables[buildableIndex];
    }

    [ClientRpc]
    public void RpcEnableDice(int playerIndex) {
        players[playerIndex].diceSpawner.active = true;
    }

    [ClientRpc]
    public void RpcDisableDice(int playerIndex) {
        players[playerIndex].diceSpawner.active = false;
    }

    [ClientRpc]
    public void RpcAddCard(int playerIndex, HexType hexType) {
        players[playerIndex].hand.createCard(hexType);
    }
}
