using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MiniGameManager {
	
	private const int MINIGAME_QTY = 2;
	
	Dictionary<int, MiniGame> minigames;
	MiniGame[] currentMiniGames;
	
	public enum GameType{PLATFORM,SUMO}
	
	public static MiniGameManager instance;
	
	protected MainCharacter character;
	public Transform spawn;
	
	public static MiniGameManager Instance {
		get{
			Debug.Log("minigameman1");
			if(instance == null) {
				Debug.Log("minigameman2");
				instance = new MiniGameManager();
			}
			return instance;
		}
	}
	
	private MiniGameManager(){
		minigames = new Dictionary<int, MiniGame>();
		currentMiniGames = new MiniGame[MINIGAME_QTY];
		
		for (int i = 0; i < currentMiniGames.Length; i++)
			CreateNewMiniGame((GameType)i);
	}
	
	public Tuple<int,Vector3> ConnectToGame (string userName, int gameType){
		MiniGame currentMiniGame = currentMiniGames[gameType];
		if(currentMiniGame.IsFull())
			currentMiniGame = CreateNewMiniGame((GameType)gameType);
		
		Vector3 spawnPlace =currentMiniGame.AddPlayerToGame(userName);
		return new Tuple<int, Vector3>(currentMiniGame.gameId,spawnPlace);
	}
	
	public MiniGame CreateNewMiniGame(GameType gameType){
		MiniGame game = new MiniGame(gameType);
		minigames.Add(game.gameId, game);
		currentMiniGames[(int)gameType] = game;
		return game;
	}
	
	public MiniGame GetMiniGame(int gameId){
		return minigames[gameId];
	}
}
