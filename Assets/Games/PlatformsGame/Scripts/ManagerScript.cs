using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ManagerScript : MonoBehaviour
{
	private bool gameReady;
	
	//private Color[] colors;
	public GameObject flag;
	private GameObject[] platforms;
	private int platformCounter = 9;
	//private IList<int> indexes;
	private IList<Color> previousColors;
	private Color flagColor;
	private int randomColorIndex;
	
	private bool flagChanged;
	private Coroutine coroutine;
	private Coroutine coroutine1;
	
	private bool platformsSet;
	private bool changeFlagColor;
	private bool platformsWhite;
	private bool goingDown;
	private bool goingUp;
	
	public float settingPlatformsTime;
	public float settingFlagTime;
	public float settingPlatformsWhiteTime;
	public float movingPlatformsTime;
	
	public int turn;
	public int round;
	private bool colorsReady;
	private bool firstLoop;
	
	public GameObject round1Texture;
	public GameObject round2Texture;
	public GameObject round3Texture;
	public Object obj;	
	
	private MainCharacter character;
	public int charactersCounter;
	private PlatformGameClient platformClient;
	
	
	// Use this for initialization
	void Start ()
	{
		platformClient =gameObject.GetComponent<PlatformGameClient>();	
		gameReady = false;
		character = CharacterHolder.Instance.MainCharacter;
		character.transform.rotation = Quaternion.identity;
		character.Show = true;
		character.MovementManager.Enable(false);
		character.EnableCamera(false);
		//character.cameras[character.CurrentCamera].enabled = false;
		character.EnterPublicWorld(true);
		
		turn = 0;
		round = 1;
		
		settingPlatformsTime = 2f;
		settingFlagTime = 1f;
		settingPlatformsWhiteTime = 1f;
		movingPlatformsTime = 4f;
		//colorsForRound = new Dictionary<int, List<Color>>();	
		platformsSet = false;
		randomColorIndex = -1;		
		flag = GameObject.Find ("Flag");
		flagChanged = false;
		//colors = new Color[platformCounter];
		platforms = new GameObject[platformCounter];		
		//indexes = new List<int> ();
		
		character = CharacterHolder.Instance.MainCharacter;		
		character.gameObject.AddComponent("CharacterScript");
		
		colorsReady = false;
		SetupPlatforms();
		SetCharacters();

		if (coroutine1 == null) {
			coroutine1 = StartCoroutine (ChangeTexture ());					
		}
		
		
		
	}
	
	void SetCharacters(){
	/*	for(int i = 0; i < charactersCounter; i++){
			character = GameObject.Find ("Character" + (i + 1)); 
		}
		*/
	}
	
	void Update ()
	{	
		if(gameReady){
			if (!platformsSet) {
				if(turn == 0)
					obj = Instantiate (round1Texture, round1Texture.transform.position, round1Texture.transform.rotation);
					
				if (turn == 0 || turn == 2 || turn == 4 || turn == 11) {
					Debug.Log ("SET PLATFORMS");
					GetColors(round);//indexes lo maneja el server
					if(colorsReady){
						Debug.Log("hola:" + turn);
						if (turn == 2) {
								round++;
								obj = Instantiate (round2Texture, round2Texture.transform.position, round2Texture.transform.rotation);
								if (coroutine1 == null) {
									coroutine1 = StartCoroutine (ChangeTexture ());					
								}
								
								settingPlatformsWhiteTime = 0f;
								movingPlatformsTime = 2f;
							}
							else if (turn == 4) {
								round++;
								SaveColors();
								
								obj = Instantiate(round3Texture, round3Texture.transform.position, round3Texture.transform.rotation);
			
								if (coroutine1 == null) {
										coroutine1 = StartCoroutine (ChangeTexture());					
								}
								settingPlatformsWhiteTime = 1f;
								settingFlagTime = 0.5f;				
								
							}
							else if (turn == 11) {
								//a definir.
								
							}
						colorsReady = false;
					}
				}	
					if(round ==3 ){
						SetPreviousColors();
					}
					if(!flagChanged){
					platformClient.GetFlag(turn);
					}
					platformsSet = true;
					if (coroutine == null ) {
						coroutine = StartCoroutine (WaitToChangeFlagColor());					
					}
							
				} else {
					if (randomColorIndex != -1) {
					
						if (changeFlagColor) {
							Debug.Log ("CHANGE FLAG COLOR");
							if (coroutine == null) {
								coroutine = StartCoroutine (WaitToTurnPlatformsWhite ());					
							}
							
						} 
						if (platformsWhite) {
							Debug.Log ("SET PLATFORMS WHITE");
							if (coroutine == null) {
								coroutine = StartCoroutine (WaitToLowerPlatforms ());					
							}
						}
						if (goingDown) {
							Debug.Log ("LOWER PLATFORMS");
							LowerPlatforms ();
							if (coroutine == null) {
								coroutine = StartCoroutine (WaitToElevatePlatforms ());					
							}
						} 
						if (goingUp) {
							Debug.Log ("ELEVATE PLATFORMS");
							ElevatePlatforms ();
							if (coroutine == null) {
								coroutine = StartCoroutine (WaitToChangeColor ());	
								turn++;
								//ReviveEveryone();
							}
						}	
					}
				}
			}
				
	  }
	
	
	
	void ReviveEveryone(){
		for(int i = 0; i<charactersCounter; i++){
			character.SendMessage("Revive");			
		}
	}
	
	public bool IsGameReady {
		set{
			gameReady = value;
		}
		get{
			return gameReady;
		}
	}
	
	
	void SaveColors ()
	{
		previousColors = new List<Color>();
		for(int i =0; i < platformCounter; i++){
			previousColors.Add(platforms[i].renderer.material.color);
		}
	}
	
	private void SetPreviousColors(){
		for(int i =0; i < platformCounter; i++){
			Color color = previousColors[i];
			platforms[i].renderer.material.color = color;
		}
	}
	
	private IEnumerator ChangeTexture ()
	{
		yield return new WaitForSeconds(1.5f);
		Destroy (obj);
		obj=null;
		coroutine1 = null;
	}	
	
	private void SetupPlatforms()
	{
		for (int i = 0; i<platformCounter; i++) {
			platforms [i] = GameObject.Find ("Platform" + (i + 1));
		}	
		
	}
	

	
	private void GetColors(int round)
	{
	 	platformClient.GetColors(round);
	}
	
	public void SetPlatforms1(List<Color> colors)
	{
		Debug.Log("setPlatForm");
		for (int i = 0; i<platformCounter; i++) {
			SetPlatformsColor(i,colors[i]);
		}
			colorsReady = true;
	}
	
	
	
	private void SetPlatformsColor (int platformIndex,Color color)
	{
		platforms [platformIndex].renderer.material.color = color;
		PlatformScript script = (PlatformScript)platforms [platformIndex].GetComponent<PlatformScript>();
		script.setPreviousColor (color);
	}
	
	IEnumerator WaitToChangeFlagColor ()
	{
		yield return new WaitForSeconds(settingPlatformsTime);
		
		flag.renderer.material.color = GetFlagColor();
		Stop ();		
		coroutine = null;			
		changeFlagColor = true;
	}
	
	IEnumerator WaitToTurnPlatformsWhite ()
	{
		yield return new WaitForSeconds(settingFlagTime);
		if (round > 2) {
			setPlatformsWhite ();
		}
		changeFlagColor = false;
		platformsWhite = true;
		coroutine = null;
	}
	
	IEnumerator WaitToLowerPlatforms ()
	{
		yield return new WaitForSeconds(settingPlatformsWhiteTime);
		platformsWhite = false;
		coroutine = null;			
		goingDown = true;
	}
	
	IEnumerator WaitToElevatePlatforms ()
	{
		yield return new WaitForSeconds(movingPlatformsTime);
		Stop ();
		coroutine = null;
		goingDown = false;
		goingUp = true;
	}
	
	IEnumerator WaitToChangeColor ()
	{
		yield return new WaitForSeconds(movingPlatformsTime);
		Stop ();
		coroutine = null;		
		platformsSet = false;
		goingUp = false;
		ResetPlatformsPosition ();
	}

	private void setPlatformsWhite ()
	{
		for (int i =0; i< platformCounter; i++) {
			platforms [i].renderer.material.color = Color.grey;
		}
		
	}
	
	private void ResetPlatformsPosition ()
	{
		for (int i =0; i< platformCounter; i++) {			
			((PlatformScript)platforms [i].GetComponent<PlatformScript> ()).ResetPosition ();
		}
	}
	
	private void Stop ()
	{
		for (int i =0; i< platformCounter; i++) {
			((PlatformScript)platforms [i].GetComponent<PlatformScript> ()).Stop ();		
		}
	}
	
	private Color GetFlagColor ()
	{	
		randomColorIndex = 1;
		flagChanged = false;
		if(flagColor == null) flagColor = Color.green;
		return flagColor;//randomColor;
	}
	public void SetFlagColor(Color newColor){
		flagChanged = true;
		flagColor = newColor;
	}
	
	private void LowerPlatforms ()
	{
		for (int i =0; i< platformCounter; i++) {
			PlatformScript script = (PlatformScript)platforms [i].GetComponent<PlatformScript> ();		
			script.SetUp (false);
			Debug.Log("previous: "+script.getPreviousColor() +" ,color: "+flagColor);
			if (script.getPreviousColor() != flagColor) {
				script.SetDown (true);				
			}
		}		
	}
	
	private void ElevatePlatforms ()
	{
		for (int i =0; i< platformCounter; i++) {
			PlatformScript script = (PlatformScript)platforms [i].GetComponent<PlatformScript> ();		
			script.SetDown (false);				
			if (script.getPreviousColor () != flagColor) {
				script.SetUp (true);
			}
		}		
	}
}