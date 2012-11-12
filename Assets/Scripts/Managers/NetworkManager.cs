using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Entidades;

public class NetworkManager : MonoBehaviour {
	MiniGameManager minigameManager;
	private NetworkView networkManagerView;
	void Start () {
		GameObject.DontDestroyOnLoad(this.gameObject);
		gameObject.AddComponent<NetworkView>();
		networkManagerView = gameObject.GetComponent<NetworkView>();
		networkManagerView.stateSynchronization = NetworkStateSynchronization.ReliableDeltaCompressed;
		networkManagerView.observed = this.transform;
		minigameManager = MiniGameManager.Instance;
	}
	
	#region SERVER
	public ConnectedUsers users = new ConnectedUsers();
	
	void OnPlayerConnected(NetworkPlayer player) {}
	
	void OnPlayerDisconnected(NetworkPlayer player) {
		ConnectedUser cu = users.GetUser(player);
		if(cu != null){
			networkView.RPC("OnUserLogout", RPCMode.AllBuffered, cu.user.Username);
			users.Disconnect(player);
		}
    }
	
	[RPC]
	void OnUserLoginServer(NetworkPlayer player, string username) {
		User user = UserService.GetUser(username);
		foreach(ConnectedUser cu in users.GetAllPlayers()){
			NetworkPlayer connectedPlayer = cu.networkPlayer;
			User connectedUser = cu.user;
			
			//Avisa a todos que un user hizo login, lo genera
			networkView.RPC("OnUserLogin", connectedPlayer, user.Username, user.Description);
			
			//Avisa al user connectado los users que ya estaban conectados asi los crea
			networkView.RPC("OnUserLogin", player, connectedUser.Username, connectedUser.Description);
		}
		users.Login(player, user);
	}
	
	[RPC]
	void OnUserLogoutServer(NetworkPlayer player, string username) {
		OnPlayerDisconnected(player);
	}
	
	public bool IsUserLogged(string username){
		return users.GetUser(username) != null;
	}
	
	// MINI GAMES /////////////////////////////////////////////////////////
	
	//client
	public void EnterGame(int gameType){
		Debug.Log("entering Game");
		networkView.RPC("ConnectToMinigame", RPCMode.Server, Network.player, gameType);
	}
	
	[RPC]//client
	void OnGameAssigned(int gameId,Vector3 spawn, int gameType){ // SE INSTANCIA MAIN CHARACTER?
	
		MainCharacter character = CharacterHolder.Instance.MainCharacter;
		character.MinigameId = gameId;
		
		if((MiniGameManager.GameType) gameType == MiniGameManager.GameType.PLATFORM){
			Application.LoadLevel("PlatformsGame");
			character.transform.position += spawn;
		}
		
		/*string sceneName = "PlatformsGame";
		MiniGameManager.GameType type = (MiniGameManager.GameType) gameType;
		switch(type){
			case MiniGameManager.GameType.PLATFORM: sceneName = "PlatformsGame"; break; 
			case MiniGameManager.GameType.SUMO: sceneName = "ballMenu"; break; 
		}
		
		Application.LoadLevel(sceneName);
	
		character.transform.position += spawn;*/
		
		networkView.RPC("CheckGameFull", RPCMode.Server, gameId);
		
		//StartCoroutine(WaitForLevelToLoad(gameId, spawn, character));
	}
	/*
	IEnumerator WaitForLevelToLoad(int gameId, Vector3 spawn, MainCharacter character)
	{
		yield return new WaitForSeconds(5f);
		Debug.Log (character.transform.position);
		character.transform.position += spawn;
		Debug.Log (Application.loadedLevel.ToString());
		Debug.Log (GameObject.Find("Spawn").transform.position);
		Debug.Log (character.transform.position);
		networkView.RPC("CheckGameFull", RPCMode.Server, gameId);
	
		
	}	
	
	
	public void WaitForLevelToLoad(string sceneName){
		while(!Application.loadedLevelName.Equals(sceneName)){
			
		}
	}
	*/
	[RPC]//server
	void ConnectToMinigame(NetworkPlayer player, int gameType){
		Tuple<int,Vector3> gameInfo = minigameManager.ConnectToGame(users.GetUser(player).user.Username, gameType);
		Debug.Log("gameid :"+ gameInfo.First()+ ",spawn "+ gameInfo.Second());
		int gameId = gameInfo.First();
		Vector3 spawn = gameInfo.Second();
		networkView.RPC("OnGameAssigned", player, gameId,spawn,gameType);
	}
	
	[RPC]//server
	void CheckGameFull(int gameId){
		MiniGame g = minigameManager.GetMiniGame(gameId);
		if(g.IsFull()){
			List<string> usernames = g.GetPlayers();
			Debug.Log("Minigame is full: " + usernames.Count);
			foreach(string username in usernames){
				Debug.Log("OnGameIsReady send to: "+ username);
				networkView.RPC("OnGameIsReady", users.GetUser(username).networkPlayer, (int) g.gameType);
			}
	     }
	}
	
	[RPC]//client
	void OnGameIsReady(int gameType){
		Debug.Log("GAME IS READY");
		//ManagerScript manager = GameObject.Find("GameManager").GetComponent<ManagerScript>();
		//manager.IsGameReady = true;
		//CharacterHolder.Instance.MainCharacter.MovementManager.Enable(true);
		
		if((MiniGameManager.GameType) gameType == MiniGameManager.GameType.SUMO){
			Application.LoadLevel("ballMenu");
		}else{
			GameObject.Find("GameManager").SendMessage("GameReady");
		}
	}
	#endregion
	
	#region CLIENT
	
	void OnConnectedToServer() {
		MavaruOnlineMain.GameStateManager.GoToMainMenu();
    }
	
	void OnDisconnectedFromServer() {
		MavaruOnlineMain.GameStateManager.GoToNetworkConnectionMenu();
    }
	
	public void Login(string username){
		networkView.RPC("OnUserLoginServer", RPCMode.Server, Network.player, username);
	}
	
	public void Logout(string username){
		networkView.RPC("OnUserLogoutServer", RPCMode.Server, Network.player, username);
	}
	
	[RPC]
	void OnUserLogin(string username, string description) {
		
		if (Network.isClient && UserConnected){
			User u = new User();
			u.Username = username;
			u.Description = description;
			
			CharacterHolder.Instance.InitCharacter(u);
			Debug.Log("Adding " + username + " with descript: " + description);
		}
	}
	
	[RPC]
	void OnUserLogout(string username) {
		if (Network.isClient && UserConnected){
			CharacterHolder.Instance.DestroyCharacter(username);
			Debug.Log("Destroying " + username);
		}
	}
	
	public bool UserConnected{
		get{
			return MavaruOnlineMain.GameStateManager.State != GameStateManager.GameState.IN_LOGIN 
				&& MavaruOnlineMain.GameStateManager.State != GameStateManager.GameState.IN_NETWORK_CONNECTION;
		}
	}
	
	// MINI GAMES /////////////////////////////////////////////////////////
	
	
	
	//MENSAJES DE VUELTA DEL SERVER
	
	
	
	[RPC]
	void UpdatePlayerPosition(string username, Vector3 position){
		//Actualizar la posicion del jugador
	}
	
	[RPC]
	void OnGameFinishedForPlayer(string username, Vector3 position){
		//Un jugador termino de jugar
	}
	////////////////////////////////////////////////////////////////////
	#endregion
}
