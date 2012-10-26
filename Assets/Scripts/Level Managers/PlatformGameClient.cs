using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlatformGameClient : MonoBehaviour {
	
	private ManagerScript manager;
	private NetworkView serverView;
	
	void Start () {
		manager = gameObject.GetComponent<ManagerScript>();
		serverView = GameObject.Find("Main").GetComponent<NetworkView>();
	}
	
	public void GetColors(int round){
   	 	serverView.RPC("GenerateColors", RPCMode.Server,Network.player,round);
	}
	
	public void GetFlag(int turn){
		serverView.RPC("GenerateFlagColor", RPCMode.Server,Network.player,turn);
	}
	
	public void GameOver(){
		MainCharacter character = CharacterHolder.Instance.MainCharacter;
		serverView.RPC("PlayerFinishedGame", RPCMode.Server,Network.player, character.MinigameId, character.user.Username);
	}
	
	public void SetPlatforms(string genColors){
		List<Color> newColors = UnRustyfy(genColors);
		manager.SetPlatforms1(newColors);
	}
	
	public void SetFlag(string flagColor){
		Color newColor = ParseColor(flagColor);
		manager.SetFlagColor(newColor);
	}
	
	public void ReceiveClientWonGame(){
		CharacterHolder.Instance.MainCharacter.GetComponent<CharacterScript>().CharacterWonGame();
	}
	
	public void ReceiveClientLostGame(string username){
		CharacterHolder.Instance.MainCharacter.GetComponent<CharacterScript>().CharacterLostGame(username);
	}
	
	private Color ParseColor(string flagColor){
	    string[] rgb = flagColor.Split(',');
		float r, g, b;
		float.TryParse(rgb[0],out r);
		float.TryParse(rgb[1],out g);
		float.TryParse(rgb[2],out b);
		return new Color(r,g,b);
	}
	
	private List<Color> UnRustyfy(string genColors){
		List<Color> newColors = new List<Color>();
		string[] splittedColors =genColors.Split(';');
		Debug.Log("colorsize: "+splittedColors.Length);
		for(int i = 0; i<splittedColors.Length-1; i++){
			string[] rgb = splittedColors[i].Split(',');
			Debug.Log("size rgb: "+rgb.Length);
			float r, g, b;
			float.TryParse(rgb[0],out r);
			float.TryParse(rgb[1],out g);
			float.TryParse(rgb[2],out b);
			newColors.Add(new Color(r,g,b));
		}
		
		return newColors;
	}
	
	//[RPC]
//	void UpdatePlayerPositionInGame(NetworkPlayer player, int gameId, Vector3 position){
	//	string playerUsername = users.GetUser(player).user.Username;
	//	List<string> usernames = minigameManager.GetMiniGame(gameId).GetPlayers();
		
		//foreach(string username in usernames)
		//	networkView.RPC("UpdatePlayerPosition", users.GetUser(username).networkPlayer, playerUsername, position);
	//}
	
//	[RPC]
	//void GameFinished(NetworkPlayer player, int gameId){
	//	string playerUsername = users.GetUser(player).user.Username;
		//List<string> usernames = minigameManager.GetMiniGame(gameId).GetPlayers();
	//	
		//foreach(string username in usernames)
		//	networkView.RPC("OnGameFinishedForPlayer", users.GetUser(username).networkPlayer, playerUsername);
	//}
}
