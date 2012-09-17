using UnityEngine;
using System.Collections;

public class Logout : MonoBehaviour {
	
	void OnGUI () {
		
		GameStateManager.GameState state = MavaruOnlineMain.GameStateManager.State;
		if(state == GameStateManager.GameState.IN_MESSAGE_BOARD ||
		   state == GameStateManager.GameState.IN_NETWORK_CONNECTION ||
		   state == GameStateManager.GameState.IN_LOGIN) return;
		
		if(MavaruOnlineMain.GameStateManager.State == GameStateManager.GameState.IN_COMMON_PLACE
		   && GUI.Button ( new Rect (10,Screen.height - 110,100,20), "Go Home")){
			MavaruOnlineMain.GameStateManager.GoHome();
		}
		
		if (GUI.Button ( new Rect (10,Screen.height - 80,100,30), "Logout")) {
			MavaruOnlineMain.DatabaseManager.Logout();
			MavaruOnlineMain.GameStateManager.GoToMainMenu();
		}
		
		if (GUI.Button ( new Rect (10,Screen.height - 40,100,30), "Disconnect")) {
			Network.Disconnect(200);
		}
	}
	
}
