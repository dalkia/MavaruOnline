using UnityEngine;
using System.Collections;
using System;
using Entidades;

public class ReplyMessageBoard : MonoBehaviour {
	
	[HideInInspector]public User fromUser;
	[HideInInspector]public User toUser;
	[HideInInspector] public MessageBoardUI messageBoard;
	string replyText = "";
	bool isPrivate = false;
	Vector2 scroll;
	
	public Rect replyRect;
	public Rect messageRect;
	public Texture replyBackground;
	public Texture messageBackground;
	public float repHeight;
	public float repWidht;
	
	public RectOffset cancelMargin;
	public Texture2D cancelButton;
	public Texture2D cancelButtonSelected;
	public RectOffset writeMargin;
	public Texture2D writeButton;
	public Texture2D writeButtonSelected;
	
	protected GUIStyle replyButtonStyle;
	protected GUIStyle cancelButtonStyle;
	protected GUIStyle toggleStyle;
	
	public static void Create(MessageBoardUI mbui, User fromUser, User toUser){
		GameObject mb = GameObject.Instantiate(Resources.Load("Prefabs/ReplyMessageBoard")) as GameObject;
		ReplyMessageBoard replymb = mb.GetComponent<ReplyMessageBoard>();
		replymb.fromUser = fromUser;
		replymb.toUser = toUser;
		replymb.messageBoard = mbui;
	}
	
	void Start(){
		this.replyButtonStyle = new GUIStyle();
		replyButtonStyle.fixedWidth = 64;
		replyButtonStyle.fixedHeight = 32;
		replyButtonStyle.margin = writeMargin;
		replyButtonStyle.normal.background = writeButton;
		replyButtonStyle.hover.background = writeButtonSelected;
		
		this.cancelButtonStyle = new GUIStyle(replyButtonStyle);
		cancelButtonStyle.normal.background = cancelButton;
		cancelButtonStyle.hover.background = cancelButtonSelected;
		cancelButtonStyle.margin = cancelMargin;
	}
	
	void OnGUI() {
		GUI.depth = -1;
		
		GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height + 300), replyBackground);
		GUILayout.BeginArea(messageRect, messageBackground);
		GUILayout.BeginArea(replyRect);
		
		//Message
        replyText = GUILayout.TextArea(replyText, GUILayout.Height(repHeight), 
		                               GUILayout.Width(repWidht));
		if(toggleStyle == null) InitToggleStyle();
		isPrivate = GUILayout.Toggle(isPrivate, "Private", toggleStyle);
			
		// BUTTONS
		GUILayout.BeginHorizontal();
        if(GUILayout.Button("", replyButtonStyle)){ReplyMessage();}
		if(GUILayout.Button("", cancelButtonStyle)){CancelReplying();}
        GUILayout.EndHorizontal();
		
		GUILayout.EndArea();
		GUILayout.EndArea();
		GUILayout.EndArea();
    }
	
	void ReplyMessage(){
		Message message = new Message();
		message.Sender = fromUser;
		message.Receiver = toUser;
		message.MessageText = replyText;
		message.PublicMessage = !isPrivate;
		message.Date = System.DateTime.Now.ToString();
		
		//Debug.Log("sender: "+ message.Sender.ToString()+ "reciever: " +message.Receiver.ToString());
		MavaruOnlineMain.DatabaseManager.AddMessage(message);
		messageBoard.Reply(message);
		GameObject.Destroy(this.gameObject);
		
	}
	
	void CancelReplying(){
		messageBoard.CancelReply();
		GameObject.Destroy(this.gameObject);
	}
	
	void InitToggleStyle(){
		toggleStyle = GUI.skin.GetStyle("toggle");
		toggleStyle.normal.textColor = Color.black;
		toggleStyle.hover.textColor = Color.black;
		toggleStyle.active.textColor = Color.black;
		toggleStyle.focused.textColor = Color.black;
		toggleStyle.onNormal.textColor = Color.black;
		toggleStyle.onHover.textColor = Color.black;
		toggleStyle.onActive.textColor = Color.black;
		toggleStyle.onFocused.textColor = Color.black;
	}
	
}
