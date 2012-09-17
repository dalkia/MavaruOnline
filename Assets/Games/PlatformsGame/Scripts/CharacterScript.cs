using UnityEngine;
using System.Collections;

public class CharacterScript : MonoBehaviour {
	public GameObject stars;
	public GameObject character;
	public int lives;
	public bool isPlaying;
	
	void Start(){
		
		
	}
	
    void Update() {
        
		if(Input.GetKey(KeyCode.UpArrow)){
			transform.Translate(Vector3.forward * Time.deltaTime*5);
			
		}
		if(Input.GetKey(KeyCode.DownArrow)){
			transform.Translate(Vector3.back * Time.deltaTime*5);
			
		}
		if(Input.GetKey(KeyCode.LeftArrow)){
			transform.Translate(Vector3.left * Time.deltaTime*5);
			
		}
		if(Input.GetKey(KeyCode.RightArrow)){
			transform.Translate(Vector3.right * Time.deltaTime*5);
			
		}
		if(transform.position.y < -3f && isPlaying){	
			Instantiate(stars, transform.position,transform.rotation);
			Destroy(gameObject);
			
			lives--;
			isPlaying = false;
		}
		
    }
	
	public bool IsDead(){
		return lives == 0;
	}
	
	public void Revive(){
		if(!IsDead () && !isPlaying){
			//Instantiate(character, new Vector3(5,2,-15),transform.rotation);
			isPlaying = true;
		}
	}
	
}