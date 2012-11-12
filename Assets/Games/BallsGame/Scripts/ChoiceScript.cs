using UnityEngine;
using System.Collections;

public class ChoiceScript : MonoBehaviour {
	public int choice;
	public Texture2D texture;
	public Texture2D[] textures;
	
 	void Awake(){
        DontDestroyOnLoad(this.gameObject);
    }
}
