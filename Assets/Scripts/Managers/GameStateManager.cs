using UnityEngine;
using System.Collections;

public class GameStateManager : MonoBehaviour {
	
	public enum GameState{IN_NETWORK_CONNECTION, IN_LOGIN, IN_DRESSING_ROOM, IN_HOME, 
							IN_FRIENDS_HOUSE, IN_COMMON_PLACE, IN_MESSAGE_BOARD, WAITING_LOBBY, IN_GAME_LOBBY};
	GameState state = GameStateManager.GameState.IN_NETWORK_CONNECTION;
	
	string lodingScene;
	GameState loadingState;
	
	void OnLevelWasLoaded(int level) {
        State = loadingState;
		//Debug.Log(loadingState);
    }
	
	void LoadScene(string name, GameState state){
		lodingScene = name;
		loadingState = state;
		Application.LoadLevel(lodingScene);
	}
	
	public void GoHome(){
		LoadScene("InsideHouse", GameState.IN_HOME);
	}
	
	public void GoToDressingRoom(){
		LoadScene("DressingRoom", GameState.IN_DRESSING_ROOM);
	}
	
	public void GoToFriendsHome(string username){
		LoadScene("InsideHouse", GameState.IN_FRIENDS_HOUSE);
		MavaruOnlineMain.DatabaseManager.VisitFriend(username);
	}
	
	public void GoToCommonPlace(){
		LoadScene("OutsideWorld", GameState.IN_COMMON_PLACE);
	}
	
	public void GoOutside(){
		LoadScene("PrivateWorld", GameState.IN_COMMON_PLACE);
	}
	
	public void WaitingLobby(){
		LoadScene("WaitingLobby", GameState.WAITING_LOBBY);
	}
	public void GoToGameLobby(){
		LoadScene("GameLobby", GameState.IN_GAME_LOBBY);
	}
	
	
	public void EnterWorld(){
		if (MavaruOnlineMain.DatabaseManager.CharacterConfiguration == null){
			GoToDressingRoom();	
		} else{
			GoHome();
		}
	}
	
	public void GoToMainMenu(){
		LoadScene("Menu", GameState.IN_LOGIN);
		CharacterHolder.Destroy();
	}
	
	public void GoToNetworkConnectionMenu(){
		LoadScene("Root", GameState.IN_NETWORK_CONNECTION);
		CharacterHolder.Destroy();
		GameObject.Destroy(MavaruOnlineMain.Instance.gameObject);
	}
	
	public GameState State{
		get{
			return state;
		}
		set{
			state = value;
		}
	}
	
	
	
}
