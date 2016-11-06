using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Player : NetworkBehaviour {
    [SyncVar] public int playerID;
    public ShowHover showHover;
    public DiceSpawner diceSpawner;
    public Hand hand;
    private PlayerManager playerManager;

	// Use this for initialization
	void Start () {
	    playerManager = GameObject.FindObjectOfType<PlayerManager>();
	    showHover = this.GetComponent<ShowHover>();
        diceSpawner = this.GetComponent<DiceSpawner>();
	    hand = GameObject.FindObjectOfType<Hand>();
        if (isLocalPlayer)
	        playerManager.localPlayer = this;
        playerManager.players.Add(this);
	    playerID = playerManager.players.IndexOf(this);
	}

    public static implicit operator int(Player player) {
        return player.playerID;
    }

    // Update is called once per frame
    void Update () {
	
	}

    public void enableHover() {
        playerManager.RpcEnableHover(this);
    }

    
    public void disableHover() {
        playerManager.RpcDisableHover(this);
    }

    
    public void setBuildable(int buildableIndex) {
        playerManager.RpcSetBuildable(this, buildableIndex);
    }

    
    public void enableDice() {
        playerManager.RpcEnableDice(this);
    }

    
    public void disableDice() {
        playerManager.RpcDisableDice(this);
    }

    
    public void addCard(HexType hexType) {
        playerManager.RpcAddCard(this, hexType);
    }
}
