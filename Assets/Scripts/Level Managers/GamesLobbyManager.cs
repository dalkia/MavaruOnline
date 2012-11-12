using UnityEngine;
using System.Collections;

public class GamesLobbyManager : MonoBehaviour {

	protected MainCharacter character;
	public Transform spawn;
	
	public void Start(){
		character = CharacterHolder.Instance.MainCharacter;
		character.Show = true;
		character.MovementManager.Enable(true);
		character.EnableCamera(true);
		character.EnterPublicWorld(false);
		character.rigidbody.useGravity = false;
    }
	
	void OnLevelWasLoaded(int l){
		CharacterHolder.Instance.MainCharacter.transform.position = spawn.position;
		CharacterHolder.Instance.MainCharacter.transform.rotation = Quaternion.identity;
	}
	
	void OnDestroy(){
		character.EnterPublicWorld(false);
		character.rigidbody.useGravity = true;
	}
	
}
