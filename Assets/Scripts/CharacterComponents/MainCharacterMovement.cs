using UnityEngine;
using System.Collections;

public class MainCharacterMovement : MovementManager {
	
	public float jumpingTime;
	float currentJumpingTime;
	bool jumping;
	bool falling;
	public float walkSpeed = 0.03f;
	public float rotationSpeed = 100;
	public float jumpingSpeed;
	
	public override void Start () {
		base.Start();
		currentJumpingTime = jumpingTime;
		jumping = false;
	}
	
	public override void Update () {
		base.Update();
		
		if(movementEnabled){
			
			float vertical = Input.GetAxis("Vertical");
			float horizontal = Input.GetAxis("Horizontal");
			//float jump = Input.GetAxis("Jump");
        
			if(Input.GetKey(KeyCode.W)){
				transform.Translate(Vector3.forward * Time.deltaTime * 3);
				Play(WALK);
				
			}
			if(Input.GetKey(KeyCode.S)){
				transform.Translate(Vector3.back * Time.deltaTime * 3);
				Play(WALK);
				if(!audio.isPlaying)
					audio.Play();
				
			} else {
				if(audio.isPlaying)
					audio.Stop();
			}
			if(Input.GetKey(KeyCode.A)){
				this.transform.Rotate(0, -1 * rotationSpeed * Time.deltaTime , 0);
				Play(WALK);
				
			}
			if(Input.GetKey(KeyCode.D)){
				this.transform.Rotate(0, rotationSpeed * Time.deltaTime , 0);
				Play(WALK);
			}
			
			
		 /*
			if ( vertical > 0) {
				Walk(vertical);
			}
			
			if ( horizontal != 0) {
				Turn(horizontal);
				
			}*/
			
			/*if ( jump != 0 && !jumping && !falling) {
				jumping = true;
			}
			
			Jump();*/
			
			if( horizontal == 0 && vertical == 0){
				Play(IDLE);	
			}
		}
	}
	
	public override bool IsWalking(){
		bool walking = false;
		if(movementEnabled){
			walking = lastPos != thisTransform.position ||
				lastRot != thisTransform.rotation;
			
			lastPos = thisTransform.position;
			lastRot = thisTransform.rotation;
		}
		return walking;
	}
	
	/*
	void Walk(float delta){
		//this.transform.position += this.transform.forward * walkSpeed * delta;
		
		thisTransform.position += new Vector3(0, (Physics.gravity.y * Time.deltaTime), 0);
		characterController.Move(thisTransform.forward * walkSpeed * delta);
		
		//thisTransform.position = new Vector3(thisTransform.position.x, yPosition , thisTransform.position.z);
		Play(WALK);
	}
	
	void Turn(float delta){
		this.transform.Rotate(0, delta * rotationSpeed * Time.deltaTime , 0);
		Vector3 gravityEffect = new Vector3(0, (Physics.gravity.y * Time.deltaTime), 0);
		characterController.Move (gravityEffect * Time.deltaTime);
		Play(WALK);	
	}
	
	void Idle(){
		Vector3 gravityEffect = new Vector3(0, (Physics.gravity.y * Time.deltaTime), 0);
		characterController.Move (gravityEffect * Time.deltaTime);
		Play(IDLE);	
	}
	
	void Jump(){
		
		if(jumping){
			
			currentJumpingTime -= Time.deltaTime;
			if(currentJumpingTime < 0){
				currentJumpingTime = jumpingTime;
				jumping = false;
				falling = true;
				return;
			}
			
			this.transform.position += this.transform.up * jumpingSpeed;
			Play(WALK);	
		
		}else if(falling){
			
			this.transform.position -= this.transform.up * jumpingSpeed;
			falling = this.transform.position.y > 0;
		}
		
	}
	*/
}
