using UnityEngine;
using System.Collections;

public class InsideHouseManager : MonoBehaviour {
	
	public Transform characterSpawn;
	protected MainCharacter character;
	
	public Transform friendSpawn;
	protected Character friend;
	
	public virtual void Start(){
		character = CharacterHolder.Instance.MainCharacter;
		character.transform.position = characterSpawn.position;
		character.transform.rotation = Quaternion.identity;
		character.Show = true;
		//character.MovementManager.Enable(false);
		//character.EnableNetwork(false);
		//character.EnableCamera(false);
		
		/*if(MavaruOnlineMain.GameStateManager.State == GameStateManager.GameState.IN_FRIENDS_HOUSE){
			friend = CharacterHolder.Instance.GetCharacter(MavaruOnlineMain.DatabaseManager.FriendUser);
			friend.transform.position = friendSpawn.position;
			friend.transform.rotation = Quaternion.identity;
			friend.Show = true;
			friend.MovementManager.Enable(false);
		}*/
    }
}
