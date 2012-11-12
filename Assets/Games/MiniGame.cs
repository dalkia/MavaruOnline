using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MiniGame {
	
	static int nextId;
	static int maxPlayers = 3;
	
	static Vector3[] spawnPositions = {new Vector3(5,1,-15),new Vector3(5,1,-18),new Vector3(3,1,-15),new Vector3(3,1,-18)};
	
	public MiniGameManager.GameType gameType;
	public List<string> allPlayers;
	public List<string> players;
	
	public int gameId;
	
	public MiniGame(MiniGameManager.GameType gameTypeParam){
		players = new List<string>();
		allPlayers = new List<string>();
		gameId = nextId;
		nextId++;
		gameType = gameTypeParam;
	}
	
	public bool IsFull(){
		Debug.Log("IsFull? player count :" + players.Count + " maxplayers: " + maxPlayers);
		return players.Count >= maxPlayers;
	}
	
	public Vector3 AddPlayerToGame(string username){
		players.Add(username);
		allPlayers.Add(username);
		return spawnPositions[players.Count-1];
	}
	
	public List<string> GetPlayers(){
		return players;
	}
	
	public void PlayerFinishedGame(string userId){
		players.Remove(userId);
	}
}

public class SumoMiniGame : MiniGame{
	public Dictionary<string, int> ballType;
	
	public SumoMiniGame(MiniGameManager.GameType gameTypeParam) : base(gameTypeParam){
		ballType = new Dictionary<string, int>();
	}
	
	public int SetBallTypeToPlayer(string userId, int ballTypeId){
		ballType.Add(userId, ballTypeId);
		return players.IndexOf(userId);
	}
	
	public bool AllPlayersSelectBall(){
		if(!IsFull())
			return false;
		
		foreach(string s in players){
			if(!ballType.ContainsKey(s))
				return false;
		}
		return true;
	}
}