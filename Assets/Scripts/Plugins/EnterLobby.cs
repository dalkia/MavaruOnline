using UnityEngine;
using System.Collections;

public class EnterLobby : MonoBehaviour {

	void OnTriggerEnter(Collider c){
		MavaruOnlineMain.GameStateManager.GoToGameLobby();
	}
}
