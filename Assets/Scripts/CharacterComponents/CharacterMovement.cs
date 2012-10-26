using UnityEngine;
using System.Collections;

public class CharacterMovement : MovementManager {
	
	bool walking = false;
	float timeLimit = 1.1f;
	float currentTime;
	
	protected Vector3 startPos;
	protected Quaternion startRot;
	protected Vector3 endPos;
	protected Quaternion endRot;
	public CharacterController characterController;
	
	private const float updateTime = 0.2f;

	public override void Start () {
		base.Start();
	}
	
	public override void Update () {
		base.Update();
		if(walking){
			if(Time.time - currentTime >= updateTime){
				this.walking = false;
				Debug.Log("Start" + thisTransform.position);
				Debug.Log("End" + endPos);

				Play(IDLE);
			
			} else  {
				float i = Time.deltaTime;
				thisTransform.position = Vector3.Lerp(thisTransform.position, endPos,i);
				thisTransform.rotation = Quaternion.Lerp(thisTransform.rotation, endRot,i);
				Debug.Log ("Esta caminando");
				Play(WALK);
			} 
			
			//thisTransform.position = new Vector3(thisTransform.position.x, thisTransform.position.y, thisTransform.position.z);
		
		}else{
			Play(IDLE);
		}
		
//		if(walking){
//			currentTime += Time.deltaTime;
//			if(currentTime >= timeLimit){
//				walking = false;
//				//thisTransform.position = endPos;
//				//thisTransform.rotation = endRot;
//				Play(IDLE);
//				
//			}else{
//				
//				float i = 0;
//				float rate = 1/ timeLimit;
//				while (i < 1.0) {
//					i += Time.deltaTime *rate;
//					thisTransform.position = Vector3.Lerp(startPos, endPos, i);
//					thisTransform.rotation = Quaternion.Lerp(startRot, endRot, i);
//					yield;
//				}	
//				
//				Play(WALK);
//			}
//			
//		}else{
//			Play(IDLE);
//		}
	}
	
//	public void MoveObject(Vector3 endPos,Quaternion rotation, float time) {
//    var i = 0.0;
//    var rate = 1.0/time;
//    while (i < 1.0) {
//        i += Time.deltaTime * rate;
//        thisTransform.position = Vector3.Lerp(startPos, endPos, i);
//         
//    }
//}
	
	
	public void Walk(Vector3 position, Quaternion rotation){
		endPos = position;
		endRot = rotation;
		this.walking = true;
		
		characterController.Move(position);
		this.transform.rotation = rotation;
		this.transform.position = position;
		Debug.Log ("Esta caminando");
		
		currentTime = Time.time;
		Play(WALK);
	}
	
	
	public override bool IsWalking(){
		return walking;
	}
}
