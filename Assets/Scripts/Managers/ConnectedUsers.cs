using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Entidades;

public class ConnectedUser{
	public User user;
	public NetworkPlayer networkPlayer;
	public bool inCommonPlace = true;
}

public class ConnectedUsers{
	
	Dictionary<NetworkPlayer, ConnectedUser> players = new Dictionary<NetworkPlayer, ConnectedUser>();
	Dictionary<string, ConnectedUser> users = new Dictionary<string, ConnectedUser>();
	
	public ConnectedUser GetUser(string username){
		if(users.ContainsKey(username)) return users[username];
		return null;
	}
	
	public ConnectedUser GetUser(NetworkPlayer player){
		if(players.ContainsKey(player)) return players[player];
		return null;
	}
	
	public Dictionary<NetworkPlayer, ConnectedUser>.ValueCollection GetAllPlayers(){
		return players.Values;
	}
	
	public void Disconnect(NetworkPlayer np){
		Debug.Log("Disconnecting user: " + players[np].user.Username );
		users.Remove(players[np].user.Username);
		players.Remove(np);	
	}
	
	public void Login(NetworkPlayer np, User user){
		ConnectedUser uc = new ConnectedUser();
		uc.networkPlayer = np;
		uc.user = user;
		players.Add(np, uc);
		users.Add(user.Username, uc);
	}
}
