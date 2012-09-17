using UnityEngine;
using System.Collections;

public class Chat : MonoBehaviour
{
	public GUISkin skin;
	protected bool showChat = false;
	protected string inputField = "";
	protected bool display = true;
	protected ArrayList entries = new ArrayList();
	protected Vector2 scrollPosition;

	private Rect window = new Rect(Screen.width - 205, 0, 200, Screen.height - 35);
	
	public ArrayList Entries {
			get{
				return entries;
			}
			set{
				entries = value;
			}
	}
	
	public float ScrollPositionY {
			get{
				return scrollPosition.y;
			}
			set{
				scrollPosition.y = value;
			}
	}
	
	public void HideUI(MonoBehaviour mb){
		if(mb != this)
			showChat = false;
	}
	
	void NotifyHide(){
		BroadcastMessage("HideUI", this);
	}
	
	protected virtual void OnGUI()
	{
		if(MavaruOnlineMain.GameStateManager.State == GameStateManager.GameState.IN_MESSAGE_BOARD ||
		   MavaruOnlineMain.GameStateManager.State == GameStateManager.GameState.IN_DRESSING_ROOM) 
			return;
		
		GUI.skin = skin;
	
		//if (GUILayout.Button(showChat ? "Hide Chat" : "Display Chat"))
		if (GUI.Button(new Rect(Screen.width-100, Screen.height-30, 90, 20), showChat ? "Hide Chat" : "Display Chat"))
		{
			// Focus first element
			if (showChat)
			{	
				CloseChatWindow ();
			}
			else
			{
				showChat = true;
				FocusControl();
				NotifyHide();
			}
		}
		
		if (showChat)
			window = GUI.Window (1, window, GlobalChatWindow, "Chat");
	}
	
	protected virtual void CloseChatWindow ()
	{
		showChat = false;
		inputField = "";
		//entries = new ArrayList();
	}
	
	protected virtual void FocusControl ()
	{
		// We can't select it immediately because the control might not have been drawn yet.
		// Thus it is not known to the system!
		//yield;
		//yield;
		//yield;
		GUI.FocusControl("Chat input field");
	}
	
	protected virtual void GlobalChatWindow (int id) {
	
		// Begin a scroll view. All rects are calculated automatically - 
	    // it will use up any available screen space and make sure contents flow correctly.
   	    // This is kept small with the last two parameters to force scrollbars to appear.
		scrollPosition = GUILayout.BeginScrollView (scrollPosition);

		foreach (ChatEntry entry in entries)
		{
			GUILayout.BeginHorizontal();
			//GUILayout.Label (entry.Text, "chat_leftaligned");
			GUILayout.Label (entry.Text);
			GUILayout.FlexibleSpace ();
			GUILayout.EndHorizontal();
			//GUILayout.Space(3);
		}
		// End the scrollview we began above.
   	    GUILayout.EndScrollView ();	
		
		Event e = Event.current;		
		
		if (e.type.Equals(EventType.keyDown) && e.keyCode == KeyCode.Return && inputField.Length > 0){
			//@TODO: This should be dependent on who actually sent the message
			//var mine = entries.Count % 2 == 0;
			string characterName = MavaruOnlineMain.DatabaseManager.CurrentUser.Username;
			//GetComponentInChildren<LabelAboveCharacter>().TextEntry = inputField;
			//GetComponentInChildren<LabelAboveCharacter>().HasWriten = true;
			//networkView.RPC("ApplyGlobalChatText", RPCMode.All, characterName + ": " + inputField);
			networkView.RPC("ApplyGlobalChatText", RPCMode.All, characterName, inputField);
			inputField = "";
		}
		GUI.SetNextControlName("Chat input field");
		inputField = GUILayout.TextField(inputField);
		
		//GUI.DragWindow();
	}
	
		
	[RPC]
	public void ApplyGlobalChatText (string username, string message){
		
		ChatEntry entry = new ChatEntry();
		entry.Sender = "Not Implemented";
		entry.Text =  username + ": " + message;

		entries.Add(entry);
		
		if (entries.Count > 50)
			entries.RemoveAt(0);
			
		scrollPosition.y = 1000000;	
		
		if(MavaruOnlineMain.GameStateManager.State == GameStateManager.GameState.IN_COMMON_PLACE
			||	MavaruOnlineMain.GameStateManager.State == GameStateManager.GameState.IN_HOME
			||	MavaruOnlineMain.GameStateManager.State == GameStateManager.GameState.IN_FRIENDS_HOUSE){
			
			Character ch = CharacterHolder.Instance.GetCharacter(username);
			if(ch != null){
				ch.GetComponentInChildren<LabelAboveCharacter>().TextEntry = message;
				ch.GetComponentInChildren<LabelAboveCharacter>().HasWriten = true;
			}
		} 
	}
}

public class ChatEntry {
		private string sender = "";
		private string text = "";
		bool mine = true;
		
		public string Sender {
			get{
				return sender;
			}
			set{
				sender = value;
			}
		}
		public string Text {
			get{
				return text;
			}
			set{
				text = value;
			}
		}
		public bool Mine {
			get{
				return mine;
			}
			set{
				mine = value;
			}
		}
			
}
