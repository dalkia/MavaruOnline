using UnityEngine;
using System.Collections;
using Entidades;

public class CharacterNetworkManager : MonoBehaviour {
	
	public MainCharacter mainCharacter;
	public float timeInSeconds;
	float currentTime;
	bool inPublicWorld = false;
	
	void LateUpdate () {
		if(!inPublicWorld) return;
		
		currentTime += Time.deltaTime;
		if( currentTime >= timeInSeconds && mainCharacter.MovementManager.IsWalking()){
			Debug.Log("Sending Position");
			currentTime = 0;
			networkView.RPC("UpdateCharacterPosition", RPCMode.AllBuffered, mainCharacter.user.Username,
			               	transform.position, transform.rotation);
		}
	}
	
	[RPC]
	void UpdateCharacterPosition(string user, Vector3 position, Quaternion rotation) {
		if(inPublicWorld && user != mainCharacter.user.Username){
			Character character = CharacterHolder.Instance.GetCharacter(user);
			if(character != null){
				(character.MovementManager as CharacterMovement).Walk(position, rotation);
				character.Show = true;	
			}
		}
	}
	
	public bool InPublicWorld{
		set{
			this.inPublicWorld = value;
			CharacterHolder.Instance.EnableCharacters(inPublicWorld);
			if(!inPublicWorld){
				networkView.RPC("NotLongerInPublicWorld", RPCMode.AllBuffered, mainCharacter.user.Username);
			}
		}
	}
	
	[RPC]
	void NotLongerInPublicWorld(string user) {
		Character character = CharacterHolder.Instance.GetCharacter(user);
		if(character != null) character.Show = false;	
	}
	
	public void ChangeClothes(string descrip){
		networkView.RPC("ChangeCharacterClothes", RPCMode.AllBuffered, 
		                mainCharacter.user.Username, descrip);
	}
	
	[RPC]
	public void ChangeCharacterClothes(string username, string descrip){
		if(username != mainCharacter.user.Username){
			Character character = CharacterHolder.Instance.GetCharacter(username);
			if(character != null){
				CharacterHolder.Instance.DestroyCharacter(username);
				
				User u = new User();
				u.Username = username;
				u.Description = descrip;
				CharacterHolder.Instance.InitCharacter(u);
			}
		}
	}
}
