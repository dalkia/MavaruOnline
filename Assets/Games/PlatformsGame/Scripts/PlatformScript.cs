using UnityEngine;
using System.Collections;

public class PlatformScript : MonoBehaviour {
	public float speed;
	private bool up;
	private bool down;
	private Color previousColor;
	
	void Start(){
		speed = 5f;
	}
	
	void Update(){
		if(up){
			GoUp();
		} else if (down){
			GoDown();
		}
		
	}
	
	public void ResetPosition(){
		
		transform.position = new Vector3(transform.position.x,1,transform.position.z);
	}
	
	public void Stop(){
		up = false;
		down = false;
	}
	
	public void SetUp(bool up){
		this.up = up;
	}	
	
	public void SetDown(bool down){
		this.down = down;
	}
	
	public Color getPreviousColor(){
		return previousColor;
	}
	public void setPreviousColor(Color color){
		previousColor = color;
	}
	
	
	
	void GoDown(){
		transform.Translate(Vector3.down*speed*Time.deltaTime);
	}
	
	void GoUp(){
		transform.Translate(Vector3.up*speed*Time.deltaTime);
	}
}
