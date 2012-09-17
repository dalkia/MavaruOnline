using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Entidades;

public class NetworkManager : MonoBehaviour {
	
	void Start () {
		GameObject.DontDestroyOnLoad(this.gameObject);
		gameObject.AddComponent<NetworkView>();
		networkView.stateSynchronization = NetworkStateSynchronization.ReliableDeltaCompressed;
		networkView.observed = this.transform;
	}
	
	#region SERVER
	ConnectedUsers users = new ConnectedUsers();
	
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
	#endregion
}
