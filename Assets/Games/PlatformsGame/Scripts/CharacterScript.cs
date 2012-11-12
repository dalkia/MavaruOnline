using UnityEngine;
using System.Collections;

public class CharacterScript : MonoBehaviour {
	
	private GameObject stars;
	public int lives;
	private bool isPlaying;
	
	private Rect rect;
	private Rect rect2;
	
	private bool showGameOverMessage;
	private bool winner;
	
	public GUIStyle customGUI;
	
	void Start(){
		isPlaying = true;
		stars = Resources.Load("Particle Systems/SmokeSystem") as GameObject;
		showGameOverMessage = false;
		
		float w = 0.45f; // proportional width (0..1)
  		float h = 0.2f; // proportional height (0..1)
		
  		rect.x = (Screen.width*(1-w))/2;
  		rect.y = (Screen.height*(1-h))/2;
  		rect.width = Screen.width*w;
  		rect.height = Screen.height*h;
		
		float p = 0.3f;
		float o = 0.1f;
		
		rect2.x = (Screen.width*(1-p))/2 ;
  		rect2.y = (Screen.height*(1-o))/2 + 40;
  		rect2.width = Screen.width*p;
  		rect2.height = Screen.height*o;
		
		customGUI = new GUIStyle();
		customGUI.fontSize = Mathf.RoundToInt(Screen.width * 0.06f);
		customGUI.normal.textColor = Color.white;
	}
	
    void Update() {
		if(transform.position.y < 9997f && isPlaying){	
			Instantiate(stars, transform.position,transform.rotation);
			GetComponent<Character>().Show = false;
			
			lives--;
			isPlaying = false;
			showGameOverMessage = true;
			winner = false;
			GameObject.Find("GameManager").GetComponent<PlatformGameClient>().GameOver();
		}
		
    }
	
	void OnGUI(){
		if(showGameOverMessage){
			GUI.Box(rect, winner? "YOU WON!" : "YOU ARE DEAD", customGUI);
			
			if(GUI.Button(rect2, "Go Back to Game Lobby")){
				MavaruOnlineMain.GameStateManager.GoToGameLobby();
				Destroy(this);
			}
			
		}
	}
	
	public void CharacterWonGame(){
		isPlaying = false;
		showGameOverMessage = true;
		winner = true;
	}
	
	public void CharacterLostGame(string username){
		//CharacterHolder.Instance.GetCharacter(username).Show = false;
	}
	
	public bool IsDead(){
		return lives == 0;
	}
	
	public void Revive(){
		if(!IsDead () && !isPlaying){
			isPlaying = true;
		}
	}
	
}