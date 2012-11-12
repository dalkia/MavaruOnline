using UnityEngine;
using System.Collections;

public class BallsScript : Selector {

	void OnGUI(){
		if(!selectorEnable) return;
		
		GUILayout.BeginArea(new Rect(mousePosition.x,Screen.height - mousePosition.y,130,70));
			if(GUILayout.Button("Enter Game")){
				MavaruOnlineMain.NetworkManager.EnterGame((int)MiniGameManager.GameType.SUMO);
				selectorEnable = false;
			}
	
		if(GUILayout.Button("...")) selectorEnable = false;
		GUILayout.EndArea();
		
	}
}
