using UnityEngine;
using System.Collections;

public class FriendsList : MonoBehaviour {
	
	public GUISkin skin;
	protected bool showFriendsList = false;
	protected string searchField = "";
	protected bool display = true;
	protected Vector2 scrollPosition;
	public ArrayList users;	
	public ArrayList friends;
	public ArrayList awaitingConfirmation;
	public ArrayList pendingConfirmation;
	public int listSelected = 0;
	public float timeInSeconds;
	float currentTime;
	
	private Rect window = new Rect(200, 0, Screen.width-200, Screen.height - 35);
	
	public float ScrollPositionY {
			get{
				return scrollPosition.y;
			}
			set{
				scrollPosition.y = value;
			}
	}
	
	void Start (){
		MavaruOnlineMain.DatabaseManager.GetAllUser(ShowAllUsers);
	}
	
	void Update () {		
		currentTime += 1;
		if( currentTime >= 120){
			currentTime = 0;
			MavaruOnlineMain.DatabaseManager.GetAllUser(ShowAllUsers);
		}
	}

	public void HideUI(MonoBehaviour mb){
		if(mb != this)
			showFriendsList = false;
	}
	
	void NotifyHide(){
		BroadcastMessage("HideUI", this);
	}
	
	protected virtual void OnGUI()
	{
		GUI.skin = skin;
		if(MavaruOnlineMain.GameStateManager.State == GameStateManager.GameState.IN_MESSAGE_BOARD ||
		   MavaruOnlineMain.GameStateManager.State == GameStateManager.GameState.IN_DRESSING_ROOM) 
			return;
		
		//if (GUILayout.Button(showChat ? "Hide Chat" : "Display Chat"))
		if (GUI.Button(new Rect(Screen.width-220, Screen.height-30, 110, 20), showFriendsList ? "Hide Friends" : "Display Friends"))
		{
			// Focus first element
			if (showFriendsList)
			{	
				CloseFriendsWindow ();
			}
			else
			{
				showFriendsList = true;
				FocusControl();
				NotifyHide();
			}
		}
		
		if (showFriendsList)
			window = GUI.Window (1, window, GlobalFriendsWindow, "Friends");
	}
	
	protected virtual void CloseFriendsWindow ()
	{
		showFriendsList = false;
		//inputField = "";
		//entries = new ArrayList();
	}
	
	protected virtual void FocusControl ()
	{
		// We can't select it immediately because the control might not have been drawn yet.
		// Thus it is not known to the system!
		//yield;
		//yield;
		//yield;
		//GUI.FocusControl("Chat input field");
	}
	
	protected virtual void GlobalFriendsWindow (int id) {
	
		// Begin a scroll view. All rects are calculated automatically - 
	    // it will use up any available screen space and make sure contents flow correctly.
   	    // This is kept small with the last two parameters to force scrollbars to appear.
		//scrollPosition = GUILayout.BeginScrollView (scrollPosition);
		GUILayout.BeginHorizontal();
		if(GUILayout.Button("Friends")){
			listSelected = 1;	
		};
		if(GUILayout.Button("Pending Confirmation")){
			listSelected = 3;	
		};
		if(GUILayout.Button("Awaiting Confirmation")){
			listSelected = 4;	
		};
		
		if(GUILayout.Button("Others")){
			listSelected = 0;	
		};
		GUILayout.EndHorizontal();
		
		scrollPosition = GUILayout.BeginScrollView (scrollPosition);
		if(listSelected == 0){
			foreach (string userString in this.users)
			{	
			GUILayout.BeginHorizontal();
			//GUILayout.Label (userString, "chat_leftaligned");
			GUILayout.Label(userString);
			if(GUILayout.Button("Add as Friend")){
				AddFriendRequest(userString);	
			}	
			GUILayout.FlexibleSpace ();
			GUILayout.EndHorizontal();
			//GUILayout.Space(3);
			}
		}else if(listSelected == 1){
			foreach (string userString in this.friends)
			{	
			GUILayout.BeginHorizontal();
			//GUILayout.Label (userString, "chat_leftaligned");
			GUILayout.Label(userString);
			if(GUILayout.Button("Go to House")){
				MavaruOnlineMain.GameStateManager.GoToFriendsHome(userString);	
			}
			if(GUILayout.Button("Delete Friend")){
				MavaruOnlineMain.DatabaseManager.RemoveFriend(userString);	
			}
			GUILayout.FlexibleSpace ();
			GUILayout.EndHorizontal();
			//GUILayout.Space(3);
			}
		}else if(listSelected == 3){
			foreach (string userString in this.pendingConfirmation)
			{	
			GUILayout.BeginHorizontal();
			//GUILayout.Label (userString, "chat_leftaligned");
			GUILayout.Label(userString);
			if(GUILayout.Button("Accept Friend Request")){
				AddFriend(userString);	
			}
			if(GUILayout.Button("Decline Friend Request")){
				MavaruOnlineMain.DatabaseManager.RemoveFriendPending(userString);
			}
			GUILayout.FlexibleSpace ();
			GUILayout.EndHorizontal();
			//GUILayout.Space(3);
			}
		}else{
			foreach (string userString in this.awaitingConfirmation)
			{	
			GUILayout.BeginHorizontal();
			//GUILayout.Label (userString, "chat_leftaligned");
			GUILayout.Label(userString + "- Awaiting Friend Confirmation");
			if(GUILayout.Button("Cancel Request")){
				MavaruOnlineMain.DatabaseManager.RemoveFriendAwaiting(userString);
			}
			GUILayout.FlexibleSpace ();
			GUILayout.EndHorizontal();
			//GUILayout.Space(3);
			}
		}
		
		
		// End the scrollview we began above.
   	    GUILayout.EndScrollView ();	
		
		//Event e = Event.current;		
		//GUI.SetNextControlName("Friends search field");
		//searchField = GUILayout.TextField(searchField);
		
		//GUI.DragWindow();	

	}
	
	public void ShowAllUsers(ArrayList users, ArrayList friends, ArrayList awaiting, ArrayList pending){
		this.users = users;
		this.friends = friends;
		this.awaitingConfirmation = awaiting;
		this.pendingConfirmation = pending;
	}	
		
	
	public void AddFriend(string userString){
		MavaruOnlineMain.DatabaseManager.AddFriend(userString);
		MavaruOnlineMain.DatabaseManager.GetAllUser(ShowAllUsers);
	}
	
	public void AddFriendRequest(string userString){
		MavaruOnlineMain.DatabaseManager.AddFriendRequest(userString);	
		Debug.Log("paso 1");
		MavaruOnlineMain.DatabaseManager.GetAllUser(ShowAllUsers);
		Debug.Log("paso 2");
	}
	
	
	
	
		
	
}



