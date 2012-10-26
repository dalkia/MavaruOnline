using UnityEngine;
using System.Collections;

public class InsideDoor : Selector {

	void OnGUI(){
		if(MavaruOnlineMain.GameStateManager.State == GameStateManager.GameState.IN_MESSAGE_BOARD) return;
		if(selectorEnable){
			GUILayout.BeginArea(new Rect(mousePosition.x,Screen.height - mousePosition.y,150,130));
			if(MavaruOnlineMain.GameStateManager.State != GameStateManager.GameState.IN_HOME){
				if(GUILayout.Button("Return Home")) MavaruOnlineMain.GameStateManager.GoHome();
			}
			//if(GUILayout.Button("Go Outside")) MavaruOnlineMain.GameStateManager.GoOutside();
			if(GUILayout.Button("Go To Common Place")) MavaruOnlineMain.GameStateManager.GoToCommonPlace();
			//if(GUILayout.Button("Go To Common Place")) MavaruOnlineMain.GameStateManager.GoToGameLobby();
			//if(GUILayout.Button("Go To Juani's Home")) MavaruOnlineMain.GameStateManager.GoToFriendsHome("Juani");
			if(GUILayout.Button("...")) selectorEnable = false;
			GUILayout.EndArea();
		}
	}
}
