using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class SumoGameServer : MonoBehaviour {
	
	[RPC]//server
	void OnUpdateBallPosition(int pos, Vector3 position, Quaternion rotation,Vector3 vel) {
		networkView.RPC("ReceiveBallPosition", RPCMode.All, pos, position, rotation, vel);
	}
	
	[RPC]//server
	void OnBallTypeSelected(NetworkPlayer player, int minigameId, string username, int ballType){
		ConnectedUsers cu = GetComponent<NetworkManager>().users;
		Debug.Log ("OnBallTypeSelected server");
		
		SumoMiniGame miniGame = MiniGameManager.Instance.GetMiniGame(minigameId) as SumoMiniGame;
		int pos = miniGame.SetBallTypeToPlayer(username, ballType);
		
		foreach(string usernameAux in miniGame.allPlayers){
			//if(!username.Equals(usernameAux))
			networkView.RPC("ReceiveBallTypeSelected", cu.GetUser(usernameAux).networkPlayer, username, ballType,pos);
			
			if(miniGame.AllPlayersSelectBall()){
				networkView.RPC("ReceiveStartSumoGame", cu.GetUser(usernameAux).networkPlayer);
			}
		}
	}
	
	[RPC]//server
	void PlayerFinishedGame(NetworkPlayer player, int minigameId, string username){
		
		ConnectedUsers cu = GetComponent<NetworkManager>().users;
		
		MiniGame miniGame = MiniGameManager.Instance.GetMiniGame(minigameId);
		miniGame.PlayerFinishedGame(username);
		
		foreach(string usernameAux in miniGame.allPlayers)
			networkView.RPC("ReceiveClientLostGame", cu.GetUser(usernameAux).networkPlayer, username);
		
		if(miniGame.GetPlayers().Count == 1)
			networkView.RPC("ReceiveClientWonGame", cu.GetUser(miniGame.GetPlayers()[0]).networkPlayer);
	}
	
	[RPC]//client
	void ReceiveBallPosition(int pos, Vector3 position, Quaternion rotation, Vector3 vel) {
		if(GameObject.Find("GameManager") != null)
			GameObject.Find("GameManager").GetComponent<SumoGameClient>().ReceiveBallPosition(pos, position, rotation, vel);
	}
	
	[RPC]//client
	void ReceiveBallTypeSelected(string username, int ballType,int pos){
		if(GameObject.Find("GameManager") != null)
			GameObject.Find("GameManager").GetComponent<SumoGameClient>().ReceiveBallTypeSelected(username, ballType,pos);
	}
	
	[RPC]//client
	void ReceiveStartSumoGame(){
		if(GameObject.Find("GameManager") != null)
			GameObject.Find("GameManager").GetComponent<SumoGameClient>().ReceiveStartSumoGame();
	}
	
	[RPC]//client
	void ReceiveClientWonGame(){
		if(GameObject.Find("GameManager") != null)
			GameObject.Find("GameManager").GetComponent<SumoGameClient>().ReceiveClientWonGame();
	}
	
	[RPC]//client
	void ReceiveClientLostGame(string username){
		if(GameObject.Find("GameManager") != null)
			GameObject.Find("GameManager").GetComponent<SumoGameClient>().ReceiveClientLostGame(username);
	}
	
	
	
}
