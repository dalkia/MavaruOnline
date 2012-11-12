using UnityEngine;
using System.Collections;

public class BallCharacterScript : MonoBehaviour {
	
	public bool isMainCharacter = false;
	
	int characterNumber = 1;
	public int ball;
	
	public GameObject manager;
	
	public ParticleSystem power;
	public GameObject powerPlane;
	public GameObject character;
	
	//private MainCharacter mavaruCharacter;
	
	private static float normalSpeed = 10f;
	private static float normalMass = 1f;
	
	private float speed;
	
	public float powerTime;
	public float chargeTime;
	
	public float powerSpeed;
	public float powerMass;
	
	public int points;
	
	public static int killPoints = 10;
	
	public GameObject killer;
	public float pushedTime;
	
	private Vector3 diferentialHeight;
	
	public int ballId;
	public BallManagerScript ballManager;
	
	public bool isDead;
	
	void Start () {
		chargeTime = powerTime;
		speed = normalSpeed;
		rigidbody.mass = normalMass;
		points = 0;
		diferentialHeight = new Vector3(0,1,0);
		
		//mavaruCharacter = CharacterHolder.Instance.MainCharacter;
		//mavaruCharacter.transform.position = transform.position;
		//mavaruCharacter.transform.position += diferentialHeight;
		//mavaruCharacter.MovementManager.Enable(true);
		//mavaruCharacter.Show = true;
		//mavaruCharacter.EnableCamera(false);
		//mavaruCharacter.EnterPublicWorld(true);
		//mavaruCharacter.rigidbody.useGravity = false;
		
		//mavaruCharacter.collider.enabled = false;
		//ChoiceScript s = GameObject.Find("Choice").GetComponent<ChoiceScript>();
		//ball = s.choice;
		//renderer.material.mainTexture = s.texture;
		//SetParameters();
		//SetBallType(s.choice);
	}
	
	public void SetBallType(int ballType){
		ball = ballType;
		renderer.material.mainTexture = GameObject.Find("Choice").GetComponent<ChoiceScript>().textures[ballType];
		SetParameters();
	}
	
	void SetParameters(){
		switch(ball){
		case 1:
			powerSpeed = 20f;
			powerMass = 1.7f;	
			powerTime = 4f;
			
			break;
		case 2:
			powerSpeed = 30f;
			powerMass = 1.5f;	
			powerTime = 3f;
			break;
		case 3:
			powerSpeed = 40f;
			powerMass = 1.2f;	
			powerTime = 3f;
			break;
		case 4:
			powerSpeed = 50f;
			powerMass = 1f;	
			powerTime = 2f;
			break;
		}
	}
	
	void OnGUI() {
		
		if(!isMainCharacter)
			return;
		
		//al integrar, sacar el character number, esta solo para probar
		if(characterNumber==1){
		    GUI.backgroundColor = Color.red;
		    GUI.Button(new Rect(560f, 570f, 50 * chargeTime, 20), "Power");
		}else {
			GUI.backgroundColor = Color.blue;
	    	GUI.Button(new Rect(10f, 10f, 50 * chargeTime, 20), "Power");			
		}
			
	}
	
	void Update () {
		
		if(!isMainCharacter)
			return;
		
		power.emissionRate = 2 + ((int)chargeTime)*10;
		//character.transform.position = new Vector3(this.transform.position.x, character.transform.position.y, this.transform.position.z);
		//mavaruCharacter.transform.position += diferentalHeight;
//		mavaruCharacter.transform.position = transform.position + diferentialHeight;
		if(characterNumber == 1){
			if(Input.GetKey(KeyCode.W)){
				this.rigidbody.AddForce(Vector3.forward *speed);
				//character.transform.rotation = new Quaternion(0, 90f, 0, character.transform.rotation.w);
		
			}
			
			if(Input.GetKey(KeyCode.A)){
				this.rigidbody.AddForce(Vector3.left *speed);
				//character.transform.rotation = new Quaternion(0, 180f, 0, character.transform.rotation.w);
		
			}
			
			if(Input.GetKey(KeyCode.S)){
				this.rigidbody.AddForce(0,0,1*-speed);
				//character.transform.rotation = new Quaternion(0, 270f, 0, character.transform.rotation.w);
		
			}
			
			if(Input.GetKey(KeyCode.D)){
				this.rigidbody.AddForce(Vector3.right *speed);
				//character.transform.rotation = new Quaternion(0, 0, 0, character.transform.rotation.w);
		
			}
			
			if(Input.GetKey(KeyCode.Space)){
				if(IsCharged()){
					speed = powerSpeed;
					rigidbody.mass = powerMass;
					chargeTime-= Time.deltaTime;
				} else {
					speed = normalSpeed;
					rigidbody.mass =normalMass;					
				}
			}else {
				speed = normalSpeed;
				rigidbody.mass = normalMass;
				if(chargeTime < powerTime){
					chargeTime+= Time.deltaTime;
				}
			}
					
		} else {
			if(Input.GetKey(KeyCode.R)){
				this.rigidbody.AddForce(Vector3.forward *10);
			}
			
			if(Input.GetKey(KeyCode.F)){
				this.rigidbody.AddForce(0,0,1*-10);
			}
			
			if(Input.GetKey(KeyCode.D)){
				this.rigidbody.AddForce(Vector3.left *10);
			}
			
			if(Input.GetKey(KeyCode.G)){
				this.rigidbody.AddForce(Vector3.right *10);
			}	
			
			if(Input.GetKey(KeyCode.A)){
				if(IsCharged()){
					speed = powerSpeed;
					rigidbody.mass = powerMass;
					chargeTime-= Time.deltaTime;
				} else {
					speed = normalSpeed;
					rigidbody.mass =normalMass;					
				}
			}else {
				speed = normalSpeed;
				rigidbody.mass = normalMass;
				if(chargeTime < powerTime){
					chargeTime+= Time.deltaTime;
				}
			}
		}			
	}
	
	
	bool IsCharged(){
		return chargeTime > 0;
	}
	
	void OnCollisionEnter(Collision collision){
		
		if(collision.collider.tag == "ball"){
			pushedTime = Time.time;
			killer = collision.collider.gameObject;
			
			if(isMainCharacter){
				Vector3 force = ballManager.lastForces[collision.rigidbody.gameObject.GetComponent<BallCharacterScript>().ballId];
				rigidbody.AddForce(force * 0.8f, ForceMode.Impulse);
			
				Debug.Log("Adding force from player: " + collision.gameObject.GetComponent<BallCharacterScript>().ballId);
			}
		}
		
		if(collision.collider.name == "DieZone"){
			if((Time.time-pushedTime) < 7f && killer != null){
				
				killer.GetComponent<BallCharacterScript>().points += killPoints;
				
				//if(isMainCharacter)
				//	GameObject.Find("GameManager").GetComponent<SumoGameClient>().GameOver();
			}
			
			gameObject.SetActiveRecursively(false);
			isDead = true;
		}
	}
}