using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class NetworkGUI : MonoBehaviour {
    public InputField IPInput;
    public Button startGamePrefab;
    public Canvas canvas;
    public Game game;

    public void hostGame() {
        this.GetComponent<NetworkManager>().StartHost();
        Button newButton = Instantiate(startGamePrefab);
        newButton.transform.SetParent(canvas.gameObject.transform, false);


        newButton.onClick.AddListener((() =>{
            game.sm.ChangeState(Game.States.Setup);
            Destroy(newButton.gameObject);
        }));
        
        this.removeGUI();
    }

    public void connectToGame() {
        NetworkManager networkManager = this.GetComponent<NetworkManager>();
        networkManager.networkAddress = IPInput.text;
        networkManager.StartClient();
        this.removeGUI();
    }

    private void removeGUI() {
        Destroy(IPInput.transform.parent.gameObject);
    }
}
