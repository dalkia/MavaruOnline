using UnityEngine;
using System.Collections;
using Entidades;

public class Character : MonoBehaviour {
	
	static Object characterPrefab = Resources.Load("Prefabs/Character");
	[HideInInspector] public GameObject characterModel;
	[HideInInspector] public User user;
	public CharacterBuilder builder;
	public MovementManager movement;
	protected Transform thisTransform;
	protected bool show = false;
	
	public static Character CreateCharacter(User user){
		GameObject chars = GameObject.Find("Characters");
		if(chars == null){
			chars = new GameObject("Characters");
			DontDestroyOnLoad(chars);
		} 
		
		GameObject go = Object.Instantiate(characterPrefab) as GameObject;
		go.name = user.Username;
		go.transform.parent = chars.transform;
		Character character = go.GetComponent<Character>();
		character.user = user;
		return character;
	}
	
	public virtual void Start () {
		builder.Init(user);
		builder.CharacterBuilt += CharacterBuilt;
	}
	
	public virtual void CharacterBuilt(GameObject characterGameObject){
		characterModel = builder.Character;
		characterModel.transform.parent = this.transform;
		characterModel.transform.localPosition = Vector3.zero;
		characterModel.transform.localRotation = Quaternion.identity;
		ShowModel(show);
	}
	
	public virtual void Restart(){
		builder.Init(user);
	}
	
	public virtual void ShowModel(bool show){
		foreach(Renderer r in GetComponentsInChildren<Renderer>()){
			r.enabled =  show;
		}
	}
	
	public bool Show{
		set{
			if(value != IsShowing){
				show = value;
				ShowModel(show);
			}
		}
	}
	
	void OnLevelWasLoaded(int level) {
        Show = false;
    }
	
	public CharacterBuilder Builder{
		get{
			return builder;
		}
	}
	
	public MovementManager MovementManager{
		get{
			return movement;
		}
	}
	
	public Animation Animation{
		get{
			return GetComponentInChildren<Animation>();
		}
	}
	
	public bool IsShowing{
		get{
			return show && characterModel != null;
		}
	}
}
