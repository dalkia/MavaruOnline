using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BallManagerScript : MonoBehaviour
{
	public class Tuple{
		public string name;
		public int points;
		
		public Tuple(string name, int points){
			this.name = name;
			this.points = points;
		}				
	}
	
	private bool gameReady;
	
	public int characterCounter;
	private IList<Tuple> ranking;
	public GameObject gameover;
	public GameObject rankObject;
	public TextMesh rank;
	private bool finished;
	private MainCharacter character;
	
	public List<GameObject> balls;
	int mainBallId = -1;
	
	//private Dictionary<string, GameObject> ballDictionary;
	//private Dictionary<string, Vector3> ballPositionDictionary;
	
	public GameObject sphere;
	
	List<SumoGameClient.SumoGamePlayerInfo> playersSelection;
	SumoGameClient client;
	//private Vector3 diferentialHeight;
	
	public Vector3[] lastForces;
	
	
	void Start (){
		//character.transform.position = GameObject.Find("Spawn").transform.position;
		//character.cameras[character.CurrentCamera].enabled = false;
	
		//GameObject clone = Instantiate(sphere, character.transform.position, Quaternion.identity) as GameObject;
		//ballDictionary = new Dictionary<string, GameObject>();
		//ballPositionDictionary = new Dictionary<string, Vector3>();
		ranking = new List<Tuple> ();
		finished = false;
		lastForces = new Vector3[4];
		//diferentialHeight = new Vector3(0, 1,0);
	}
	
	public void StartSumoGame(List<SumoGameClient.SumoGamePlayerInfo> playersSelection){
		this.playersSelection = playersSelection;
		
		foreach(SumoGameClient.SumoGamePlayerInfo s in playersSelection){
			//Crear ball
			AddBall(s.username, s.balltype, s.pos);
			
			//Ranking
			ranking.Add (new Tuple(s.username, 0));
		}
		
		/*GameObject[] characters = GameObject.FindGameObjectsWithTag("ball");
		characterCounter = characters.Length;
		
		for (int i = 0; i < characters.Length; i++) {			
			ranking.Add (new Tuple(characters [i].name, 0));
	
		}*/
		
		rank.text = "";
	}
	
	public void AddBall(string username, int ballType,int pos){
		Debug.Log("Activando ball " + pos +" con tipo " + ballType + " para usuario " + username);
		
		GameObject ball = balls[pos];
		ball.name = username;
		ball.transform.parent.gameObject.SetActiveRecursively(true);
		ball.GetComponentInChildren<BallCharacterScript>().SetBallType(ballType);
		ball.GetComponentInChildren<BallCharacterScript>().ballId = pos;
		ball.GetComponentInChildren<BallCharacterScript>().ballManager = this;
		
		//Debug.Log ("Entro el user " + username);
		//ballDictionary.Add (username, ball);
		//ballPositionDictionary.Add (username, ball.transform.position);
		//TODO CharacterHolder.Instance.GetCharacter(username).transform.parent = ball.transform;
		
		if(username.Equals(CharacterHolder.Instance.MainCharacter.user.Username)){
			mainBallId = pos;
			ball.GetComponentInChildren<BallCharacterScript>().isMainCharacter = true;
			StartCoroutine(UpdateBallPositionCoroutine());
		}
	}
	
	IEnumerator UpdateBallPositionCoroutine(){
		while(true){
			
			if(client == null){
				GameObject managerGO = GameObject.Find("GameManager");
				if(managerGO == null)
					break;
				
				client = managerGO.GetComponent<SumoGameClient>();
			}
			
			client.UpdateMainBallPosition(mainBallId, balls[mainBallId].transform.position, balls[mainBallId].transform.rotation, balls[mainBallId].rigidbody.velocity);
			yield return new WaitForSeconds(0.005f);
		}
	}
	
	public void UpdateBallPosition(int pos, Vector3 position, Quaternion rotation, Vector3 velocity) {
		
		if( pos ==  mainBallId || balls[pos] == null)
			return;
		
		//Transform ballT = balls[pos].transform;
		//ballT.position = Vector3.Lerp(ballT.position, position, 0.003f);
		//ballT.rotation = Quaternion.Lerp(ballT.rotation, rotation, 0.003f);
		
		balls[pos].transform.position = position;
		balls[pos].transform.rotation = rotation;
		
		lastForces[pos] = velocity * balls[pos].rigidbody.mass;
	}
	
	void OnGUI(){
		if(!finished){
			if(ranking.Count != 0){
				for(int i = 0; i< ranking.Count; i++){
					GUI.Label(new Rect(10f, 50f+i*20, 100f, 30f), ranking[i].name+": "+ ranking[i].points);
			
				}
			}
		}else{
			if(GUI.Button(new Rect(100f, 100f, 200f, 100f), "Go Back to Game Lobby"))
				MavaruOnlineMain.GameStateManager.GoToGameLobby();
		}
	}
	
	void Update (){
//		foreach(string u in ballDictionary.Keys){
//			Debug.Log (ballDictionary[u]);
//			Debug.Log (ballPositionDictionary[u]);
			//if(ballDictionary[u]!= null && ballPositionDictionary[u]!=null){
//				ballDictionary[u].transform.position = Vector3.Lerp(ballDictionary[u].transform.position, ballPositionDictionary[u] - diferentialHeight, Time.deltaTime * 10); 		

			//}
			
//		}
		
		if(playersSelection == null)
			return;
		
		List<GameObject> characters = new List<GameObject>();
		foreach(GameObject ball in balls){
			BallCharacterScript ballcharacter = ball.GetComponentInChildren<BallCharacterScript>();
			if(ball.active && !ballcharacter.isDead)
				characters.Add(ball);
		}
		
		int activeBalls = characters.Count;
		
		Debug.Log("active balls: " + activeBalls + "   " + characters);
		
		//GameObject[] characters = GameObject.FindGameObjectsWithTag("ball");
		if(activeBalls > 1){
			for (int i =0; i < activeBalls; i++) {
				string name = characters[i].name;
				int points = characters[i].GetComponent<BallCharacterScript>().points;
				Tuple t = getTupleByName(name);
				if(t!=null){
					t.points = points;
				}
			}	
		} else if(activeBalls == 1){
			
			string name = characters[0].name;
			int points = characters[0].GetComponent<BallCharacterScript>().points;
			Tuple t = getTupleByName(name);
			if(t!=null){
				t.points = points + 15;
			}
				
			if(!finished){
				for(int i = 0; i< ranking.Count; i++){
					rank.text +=(i+1)+") "+ranking[i].name+": "+ ranking[i].points+"\n" ;
				}
				
				Instantiate(gameover, new Vector3(-10006.68f,-10000.33f,-9992.608f),new Quaternion(transform.rotation.x,transform.rotation.y,transform.rotation.z,transform.rotation.w));
				
				Instantiate(rank, new Vector3(-10004.55f,-10000.33f,-9998.128f),new Quaternion(transform.rotation.x,transform.rotation.y,transform.rotation.z,transform.rotation.w));
				
				finished = true;
				
				GameObject.Destroy(GameObject.Find("GameManager"));
				GameObject.Destroy(GameObject.Find ("Choice"));
			}
		}
	}
	
	Tuple getTupleByName(string name){
		for(int i = 0; i< ranking.Count; i++){
			if(ranking[i].name.Equals(name)){
				return ranking[i];
			}
		}
		return null;
	}
	
	//public void UpdateBallPosition(string user, Vector3 newPosition){
	//	ballPositionDictionary[user] = newPosition - diferentialHeight;
		//ballDictionary[user].transform.position = newPosition - diferentialHeight; 	
	//}

}
