using UnityEngine;
using System.Collections;

public class CameraToggle : MonoBehaviour {
	
	public MainCharacter mainCharacter;

	void OnGUI () {
		if(MavaruOnlineMain.GameStateManager.State != GameStateManager.GameState.IN_COMMON_PLACE) return;
		
		if (GUI.Button ( new Rect (10,10,100,30), "Toggle Camera")) {
			mainCharacter.ToggleCamera();
		}
	}
}
