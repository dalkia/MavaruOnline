using UnityEngine;
using System.Collections;

public class GamesLobbyManager : MonoBehaviour {

	protected MainCharacter character;
	public Transform spawn;
	
	public void Start(){
		character = CharacterHolder.Instance.MainCharacter;
		character.transform.position = spawn.position;
		character.transform.rotation = Quaternion.identity;
		character.Show = true;
		character.MovementManager.Enable(true);
		character.EnableCamera(true);
		character.EnterPublicWorld(false);
    }
	
	public void OnDisbale(){
		character.EnterPublicWorld(false);
	}
	
}
