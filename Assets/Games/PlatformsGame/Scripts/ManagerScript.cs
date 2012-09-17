using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ManagerScript : MonoBehaviour
{
	private Color[] colors;
	public GameObject flag;
	private GameObject[] platforms;
	private int platformCounter = 9;
	private IList<int> indexes;
	private IList<Color> previousColors;
	private Color randomColor;
	private int randomColorIndex;
	
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
	
	public GameObject round1Texture;
	public GameObject round2Texture;
	public GameObject round3Texture;
	public Object obj;	
	
	private GameObject[] characters;
	public int charactersCounter;
	
	
	// Use this for initialization
	void Start ()
	{
		turn = 0;
		round = 1;
		
		settingPlatformsTime = 2f;
		settingFlagTime = 1f;
		settingPlatformsWhiteTime = 1f;
		movingPlatformsTime = 4f;
			
		platformsSet = false;
		randomColorIndex = -1;		
		flag = GameObject.Find ("Flag");
		colors = new Color[platformCounter];
		platforms = new GameObject[platformCounter];		
		indexes = new List<int> ();
		
		characters = new GameObject[charactersCounter];		
		
		addColors ();
		setIndexes ();		
		setStartPlatforms ();
		SetCharacters();
		
		
		obj = Instantiate (round1Texture, round1Texture.transform.position, round1Texture.transform.rotation);

		if (coroutine1 == null) {
			coroutine1 = StartCoroutine (ChangeTexture ());					
		}
		
		
		
	}
	
	void SetCharacters(){
		for(int i = 0; i < charactersCounter; i++){
			characters[i] = GameObject.Find ("Character" + (i + 1)); 
		}
	}
	
	void Update ()
	{		
		if (!platformsSet) {
			if (turn == 0 || turn == 2 || turn == 4 || turn == 11) {
				Debug.Log ("SET PLATFORMS");
				setIndexes ();
				SetPlatforms ();
				if (turn == 2) {
					round++;
					
					obj = Instantiate (round2Texture, round2Texture.transform.position, round2Texture.transform.rotation);

					if (coroutine1 == null) {
						coroutine1 = StartCoroutine (ChangeTexture ());					
					}
					
					settingPlatformsWhiteTime = 0f;
					movingPlatformsTime = 2f;
				}
				if (turn == 4) {
					round++;
					SaveColors ();
					
					
					obj = Instantiate(round3Texture, round3Texture.transform.position, round3Texture.transform.rotation);

					if (coroutine1 == null) {
							coroutine1 = StartCoroutine (ChangeTexture());					
					}
					settingPlatformsWhiteTime = 1f;
					settingFlagTime = 0.5f;				
					
				}
				if (turn == 11) {
					//a definir.
					
				}
				
			}	
			if(round ==3){
				SetPreviousColors();
			}
			platformsSet = true;
			if (coroutine == null) {
				coroutine = StartCoroutine (WaitToChangeFlagColor ());					
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
	
	void ReviveEveryone(){
		for(int i = 0; i<charactersCounter; i++){
			characters[i].SendMessage("Revive");			
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
	
	private void setStartPlatforms ()
	{
		for (int i = 0; i<platformCounter; i++) {
			platforms [i] = GameObject.Find ("Platform" + (i + 1));
			SetPlatformsColor (i);
		}	
		
	}
	
	private void setIndexes ()
	{
		for (int i =0; i<platformCounter; i++) {
			indexes.Add (i);
		}
		
	}
	
	private void SetPlatforms ()
	{
		
		for (int i = 0; i<platformCounter; i++) {			
			SetPlatformsColor (i);
		}
	}
	
	private void addColors ()
	{				
		colors [0] = new Color(255f/255f,102f/255f,0);
		colors [1] = Color.red;
		colors [2] = Color.green;
		colors [3] = Color.yellow;
		colors [4] = Color.cyan;
		colors [5] = new Color(255f/255f,102f/255f,102f/255f);
		colors [6] = new Color(0f/255f,0,152f/255f);
		colors [7] = Color.white;
		colors [8] = new Color(102f/255f,0f/255f,51f/255f);		
		
	}
	
	private void SetPlatformsColor (int platformIndex)
	{
		int index = Random.Range (0, indexes.Count);
		Color color = colors [indexes [index]];
		platforms [platformIndex].renderer.material.color = color;
		PlatformScript script = (PlatformScript)platforms [platformIndex].GetComponent<PlatformScript> ();
		script.setPreviousColor (color);
		indexes.Remove (indexes [index]);			
	}
	
	IEnumerator WaitToChangeFlagColor ()
	{
		yield return new WaitForSeconds(settingPlatformsTime);
		
		flag.renderer.material.color = getRandomColor ();
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
	
	private Color getRandomColor ()
	{	
		randomColorIndex = UnityEngine.Random.Range (0, platformCounter - 1); 
		randomColor = colors [randomColorIndex];
		return randomColor;
	}
	
	private void LowerPlatforms ()
	{
		for (int i =0; i< platformCounter; i++) {
			PlatformScript script = (PlatformScript)platforms [i].GetComponent<PlatformScript> ();		
			script.SetUp (false);			
			if (script.getPreviousColor () != randomColor) {
				script.SetDown (true);				
			}
		}		
	}
	
	private void ElevatePlatforms ()
	{
		for (int i =0; i< platformCounter; i++) {
			PlatformScript script = (PlatformScript)platforms [i].GetComponent<PlatformScript> ();		
			script.SetDown (false);				
			if (script.getPreviousColor () != randomColor) {
				script.SetUp (true);
			}
		}		
	}
}