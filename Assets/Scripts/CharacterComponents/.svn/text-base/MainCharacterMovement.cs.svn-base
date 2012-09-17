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
			float jump = Input.GetAxis("Jump");
		 
			if ( vertical > 0) {
				Walk(vertical);
			}
			
			if ( horizontal != 0) {
				Turn(horizontal);
				
			}
			
			/*if ( jump != 0 && !jumping && !falling) {
				jumping = true;
			}
			
			Jump();*/
			
			if( horizontal == 0 && vertical == 0 && jump == 0 ){
				Idle();
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

	void Walk(float delta){
		//this.transform.position += this.transform.forward * walkSpeed * delta;
		characterController.Move(thisTransform.forward * walkSpeed * delta);
		thisTransform.position = new Vector3(thisTransform.position.x, 0, thisTransform.position.z);
		Play(WALK);
	}
	
	void Turn(float delta){
		this.transform.Rotate(0, delta * rotationSpeed * Time.deltaTime , 0);
		Play(WALK);	
	}
	
	void Idle(){
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
}
