using UnityEngine;
using System.Collections;

public class enterScript : MonoBehaviour {
	void OnTriggerEnter(Collider other){
		Application.LoadLevel("GameLobby");
	}
}
