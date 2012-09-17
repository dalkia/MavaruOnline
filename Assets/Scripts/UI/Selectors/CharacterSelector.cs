using UnityEngine;
using System.Collections;

public class CharacterSelector : Selector {
	
	public Character character;
	
	public override void Start(){
		selectedRenderer = character.transform.renderer;
    }
	
	void OnGUI(){
		if(!selectorEnable || !character.IsShowing) return;
		if(MavaruOnlineMain.GameStateManager.State == GameStateManager.GameState.IN_DRESSING_ROOM ||
		   MavaruOnlineMain.GameStateManager.State == GameStateManager.GameState.IN_MESSAGE_BOARD) return;
		
		if(MavaruOnlineMain.DatabaseManager.IsMainUser(character.user)){
			GUILayout.BeginArea(new Rect(mousePosition.x,Screen.height - mousePosition.y,100,100));
			if(GUILayout.Button("Change Look")) MavaruOnlineMain.GameStateManager.GoToDressingRoom();
			if(MavaruOnlineMain.GameStateManager.State != GameStateManager.GameState.IN_HOME 
			   && GUILayout.Button("Return Home")) MavaruOnlineMain.GameStateManager.GoHome();
			if(GUILayout.Button("...")) selectorEnable = false;
			GUILayout.EndArea();
			
		}/* else if(MavaruOnlineMain.GameStateManager.State == GameStateManager.GameState.IN_COMMON_PLACE){
			bool areFriends = ((ArrayList)this.transform.parent.GetComponent<FriendsList>().friends).Contains(character.user.Username);
			bool pendingFriendRequest = ((ArrayList)this.transform.parent.GetComponent<FriendsList>().pendingConfirmation).Contains(character.user.Username);
			bool awatingFriendRequest = ((ArrayList)this.transform.parent.GetComponent<FriendsList>().awaitingConfirmation).Contains(character.user.Username);
			
			GUILayout.BeginArea(new Rect(mousePosition.x,Screen.height - mousePosition.y,180,100));
			if(areFriends && GUILayout.Button("Visit " + character.user.Username + "'s home")) 
				MavaruOnlineMain.GameStateManager.GoToFriendsHome(character.user.Username);
			if(pendingFriendRequest && GUILayout.Button("Accept " + character.user.Username + " friend request")){
				//Accept fiends request
			}
			if(GUILayout.Button("...")) selectorEnable = false;
			GUILayout.EndArea();
		}*/
	}
}
