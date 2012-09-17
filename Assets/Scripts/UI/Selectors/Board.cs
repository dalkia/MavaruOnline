using UnityEngine;
using System.Collections;

public class Board : Selector {
	
	void OnGUI(){
		if(!selectorEnable) return;
		
		GUILayout.BeginArea(new Rect(mousePosition.x,Screen.height - mousePosition.y,130,70));
		if(MavaruOnlineMain.GameStateManager.State == GameStateManager.GameState.IN_HOME){
			if(GUILayout.Button("Show Messages")){
				PersonalMessageBoard.Create();
				NotifyHide();
				selectorEnable = false;
			}
		
		}else if(MavaruOnlineMain.GameStateManager.State == GameStateManager.GameState.IN_FRIENDS_HOUSE){
			if(GUILayout.Button("See Friend's Board")){
				PublicMessageBoard.Create(MavaruOnlineMain.DatabaseManager.FriendUser);
				NotifyHide();
				selectorEnable = false;
			} 
		}
		if(GUILayout.Button("...")) selectorEnable = false;
		GUILayout.EndArea();
	}
}
