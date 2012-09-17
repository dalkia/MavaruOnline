using UnityEngine;
using System.Collections;

public class LabelAboveCharacter : MonoBehaviour {
	
	Camera cam ;
	Transform thisTransform;
	
	protected string textEntry = "";
	
	public Character character;
	public Transform target;
	public float offsetMultiplier = 1;
	public Vector2 windowSize = new Vector2(100, 45);
	public Vector2 windowCorrection = new Vector2(60, 35);
	public Vector2 buttonSize = new Vector2(80, 20);
	public Vector2 buttonCorrection = new Vector2(50, 15);
	
	int timer = 0;
	Rect popupPositionRec;
	private bool hasWriten = false;
	private string username;
	int xCorrection = 60;
	int yCorrection = 35;
	
	int auxZ = 0;
	//Camera camera;
	
	public string TextEntry {
		get{
			return textEntry;
		}
		set{
			textEntry = value;
		}
	}
	
	public bool HasWriten {
		get{
			return hasWriten;
		}
		set{
			hasWriten = value;
		}
	}
	
	// Use this for initialization
	void Start () {
		username = character.user.Username;
	}
	
	void OnLevelWasLoaded(){
		//cam = Camera.main;
	}
	// Update is called once per frame
	void Update () {
		if(hasWriten){
			if(timer == 120){
				textEntry = "";
				hasWriten = false;
				timer = 0;
			}
			timer++;
		}
	}
	
	protected virtual void OnGUI(){
		if(!character.IsShowing) return;
		if(MavaruOnlineMain.DatabaseManager.IsMainUser(character.user)) return;
		
		Vector3 screenPos = Camera.main.WorldToScreenPoint(target.position);
		
		if(screenPos.z < 0) return;
		Debug.Log("x: " + screenPos.x + "y: " + screenPos.y + "z: " + screenPos.z);
		
		
		if(hasWriten) {
			popupPositionRec.x = screenPos.x - windowCorrection.x;
			//popupPositionRec.y = screenPos.y - windowCorrection.y;
			if(screenPos.z > 5){
				popupPositionRec.y = 240 + screenPos.z;
			} else {
				popupPositionRec.y = 245;
			}
			popupPositionRec.width = windowSize.x;
			popupPositionRec.height = windowSize.y + (textEntry.Length / 8) * 10;
			popupPositionRec = GUI.Window(2, popupPositionRec, ShowTextAbove, username);
			
		} else {
			popupPositionRec.x = screenPos.x - buttonCorrection.x;
			//popupPositionRec.y = screenPos.y - buttonCorrection.y;
			if(screenPos.z > 5) {
				popupPositionRec.y = 240 + screenPos.z;
			} else {
				popupPositionRec.y = 245;
			}
			popupPositionRec.width = buttonSize.x;
			popupPositionRec.height = buttonSize.y;
			GUI.Button(popupPositionRec, username);
		}
	}
	
	public void ShowTextAbove(int id){
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace ();
		GUILayout.Label (textEntry);
		GUILayout.EndHorizontal();
	}
}
