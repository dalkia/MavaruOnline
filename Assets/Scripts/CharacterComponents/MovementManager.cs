using UnityEngine;
using System.Collections;

public class MovementManager : MonoBehaviour {
	
	public static string WALK = "walk";
	public static string IDLE = "idle1";
	
	
	public Character character;
	protected Transform thisTransform;
	protected bool movementEnabled;
	protected Vector3 lastPos;
	protected Quaternion lastRot;
	
	public virtual void Start () {
		this.movementEnabled = false;
		this.thisTransform = character.transform;
		this.lastPos = thisTransform.position;
		this.lastRot = thisTransform.rotation;
	}
	
	public virtual void Update () {
	}
	
	public virtual bool IsWalking(){
		return false;
	}
	
	public void Play(string animName){
		Animation anim = character.Animation;
		if( anim != null && !anim.IsPlaying(animName))
			anim.Play(animName);
	}
	
	public void Enable(bool enable){
		this.movementEnabled = enable;
	}
	
	public bool Enabled{
		get{
			return movementEnabled;
		}
	}
}
