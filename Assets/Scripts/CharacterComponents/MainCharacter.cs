using UnityEngine;
using System.Collections;
using Entidades;

public class MainCharacter : Character {
	
	public CharacterNetworkManager networkManager;
	public Camera[] cameras;
	int currentCamera;
	
	private int minigameId;
	
	public static MainCharacter CreateMainCharacter(User user){
		GameObject chars = GameObject.Find("Characters");
		if(chars == null){
			chars = new GameObject("Characters");
			DontDestroyOnLoad(chars);
		} 
		
		GameObject go = Object.Instantiate(Resources.Load("Prefabs/MainCharacter")) as GameObject;
		go.name = user.Username;
		go.transform.parent = chars.transform;
		MainCharacter character = go.GetComponent<MainCharacter>();
		character.user = user;
		return character;
	}
	
	public int CurrentCamera{
		set{
			currentCamera = value;
		}
		get{
			return currentCamera;
		}
	}
	
	public int MinigameId{
		set{
			minigameId = value;
		}
		get{
			return minigameId;
		}
	}
	
	public override void Start () {
		base.Start();
		currentCamera = 0;
	}

	public void EnableCamera(bool enable){
		cameras[currentCamera].enabled = enable;
	}
	
	public void ToggleCamera(){
		int totalCameras = cameras.Length;
		currentCamera = ++currentCamera % totalCameras;
		
		for(int i = 0; i< totalCameras; i++){
			cameras[i].enabled = currentCamera == i;
		}
	}
	
	public void EnterPublicWorld(bool enable){
		networkManager.InPublicWorld = enable;
	}
	

}
