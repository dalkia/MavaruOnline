using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SumoGameClient : MonoBehaviour {
	
	public class SumoGamePlayerInfo{
		public string username;
		public int balltype;
		public int pos;
	}
	
	MainCharacter character;
	private NetworkView serverView;
	private BallManagerScript ballScriptManager;
	List<SumoGamePlayerInfo> playersSelection;
	
	void Start () {
		character = CharacterHolder.Instance.MainCharacter;
		character.EnableCamera(false);
		character.MovementManager.Enable(false);
		serverView = GameObject.Find("Main").GetComponent<NetworkView>();
		playersSelection = new List<SumoGamePlayerInfo>();
		
		//ballScriptManager = GameObject.Find("GameManager").GetComponent<BallManagerScript>();
		GameObject.DontDestroyOnLoad(gameObject);
	}
	
	public void GameReady(){
	}
	
	public void OnBallTypeSelected(int ballType){
		serverView.RPC("OnBallTypeSelected", RPCMode.Server,Network.player, character.MinigameId, character.user.Username, ballType);
	}
	
	public void UpdateMainBallPosition(int pos, Vector3 position, Quaternion rotation, Vector3 vel){
		serverView.RPC("OnUpdateBallPosition", RPCMode.Server, pos, position, rotation, vel);
	}
	
	public void ReceiveBallPosition(int pos, Vector3 position, Quaternion rotation, Vector3 vel) {
		if(ballScriptManager != null)
			ballScriptManager.UpdateBallPosition(pos, position, rotation, vel);
	}
	
	public void ReceiveStartSumoGame(){
		//CharacterHolder.Instance.MainCharacter.MovementManager.Enable(true);
		
		if(ballScriptManager == null)
			ballScriptManager = GameObject.Find("Ball Game Manager").GetComponent<BallManagerScript>();
		
		ballScriptManager.StartSumoGame(playersSelection);
		Debug.Log("ReceiveStartSumoGame, manager: " + GameObject.Find("Ball Game Manager").GetComponent<BallManagerScript>());
	}
	
	public void GameOver(){
		serverView.RPC("PlayerFinishedGame", RPCMode.Server,Network.player, character.MinigameId, character.user.Username);
	}
	
	public void ReceiveBallTypeSelected(string username, int ballType,int pos){
		Debug.Log("ReceiveBallTypeSelected de " + username);
		
		SumoGamePlayerInfo info = new SumoGamePlayerInfo();
		info.balltype = ballType;
		info.pos = pos;
		info.username = username;
		
		playersSelection.Add(info);
		
		//ballScriptManager.AddBall(username, ballType, pos);	
	}
	
	public void ReceiveClientWonGame(){
		//CharacterHolder.Instance.MainCharacter.GetComponent<CharacterScript>().CharacterWonGame();
	}
	
	public void ReceiveClientLostGame(string username){
		//TODO CharacterHolder.Instance.MainCharacter.GetComponent<CharacterScript>().CharacterLostGame(username);
	}
	
}
