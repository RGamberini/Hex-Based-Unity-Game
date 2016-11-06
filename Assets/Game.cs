using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MonsterLove.StateMachine;
using UnityEngine.Networking;

public class Game : NetworkBehaviour {
    public StateMachine<States> sm;
    public PlayerManager playerManager;
    public GameObject roadPrefab;
    public GameObject settlementPrefab;
    private Player currentPlayer;

    private ResourceManager resourceManager;
    // Local Player is set in the Player Class
    public enum States {
        Lobby,
        Setup,
        Play
    }

    // Use this for initialization
    void Start () {
        sm = StateMachine<States>.Initialize(this);
        sm.ChangeState(States.Lobby);
        resourceManager = this.GetComponent<ResourceManager>();
        currentPlayer = playerManager.currentPlayer;
    }

    void Setup_Enter() {
        resourceManager.createResourceList(playerManager.players.Count);
        playerManager.setCurrentPlayer(0);

        playerManager.currentPlayer.showHover.call = startingBuild();
        playerManager.currentPlayer.showHover.call.MoveNext();
    }

    private int iterations = -1;
    private IEnumerator startingBuild() {
        playerManager.currentPlayer.enableHover();
        playerManager.currentPlayer.setBuildable(0);
        yield return null;

        playerManager.currentPlayer.setBuildable(1);
        yield return null;

        playerManager.currentPlayer.disableHover();

        int nextPlayer = playerManager.getNextPlayer();
        if(nextPlayer == 0) iterations++;
        if (iterations < 1) {
            playerManager.nextPlayer();
            playerManager.currentPlayer.showHover.call = startingBuild();
            playerManager.currentPlayer.showHover.call.MoveNext();
        } else sm.ChangeState(States.Play);
    }

    void Play_Enter() {
        playerManager.setCurrentPlayer(0);
        playerManager.currentPlayer.enableDice();
        playerManager.currentPlayer.diceSpawner.callback = dieCallBack();
    }

    private IEnumerator dieCallBack() {
        foreach (Player player in playerManager.players)
            foreach ( HexType resource in resourceManager.getResourcesOfPlayer(player, playerManager.currentPlayer.diceSpawner.lastThrownDie))
                player.addCard(resource);

        playerManager.nextPlayer();
        playerManager.currentPlayer.enableDice();
        playerManager.currentPlayer.diceSpawner.callback = dieCallBack();
        yield return null;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
