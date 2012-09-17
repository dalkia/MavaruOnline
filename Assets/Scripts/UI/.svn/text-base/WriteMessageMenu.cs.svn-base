using UnityEngine;
using System.Collections;

public class WriteMessageMenu : MonoBehaviour {
	
	static Object prefab = Resources.Load("Prefabs/WriteMessageMenu");
	string username_from;
	string username_to;
	
	public static void Create(string username_from, string username_to){	
		GameObject board = GameObject.Instantiate(prefab) as GameObject;
		board.GetComponent<WriteMessageMenu>().SetUsers(username_from, username_to);	
	}
	
	void SetUsers(string username_from, string username_to){
		this.username_from = username_from;
		this.username_to = username_to;
	}
	
	void OnGUI () {
		GUI.Box (new Rect (Screen.width - 120,10,100,155), "Menu");
	
		if (GUI.Button (new Rect (Screen.width - 110,40,80,20), "Customize")) {
			Application.LoadLevel("DressingRoom");
		}
		
		if (GUI.Button (new Rect (Screen.width - 110,70,80,20), "Messages")) {
			//Application.LoadLevel("MessageBoard");
			GameObject.Instantiate(Resources.Load("Prefabs/MessageBoard"));
		}
	
		if (GUI.Button ( new Rect (Screen.width - 110,100,80,20), "Go outside")) {
			Application.LoadLevel("PrivateWorld");
		}
		
		if (GUI.Button ( new Rect (Screen.width - 110,130,80,20), "Logout")) {
			MavaruOnlineMain.DatabaseManager.Logout();
			Application.LoadLevel("Menu");
		}
	}
}
