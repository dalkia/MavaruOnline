using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Entidades;

public abstract class MessageBoardUI : MonoBehaviour {
	
	GameStateManager.GameState previousGameState;
	
	#region MainMenuManagerFields
	public Rect messageBoardRect;
	protected Vector2 scrollPosition = Vector2.zero;
	protected Dictionary<int, Vector2> scrollPositions;
	protected int messagesHeight = 0;
	public int messageHeightDelta = 10;
	
	public int messageWidth = 400;
	public int messageHeight = 30;
	public int messageTextWidth;
	public int messageTextHeight;
	
	//Textures
	public Texture2D board;
	public Texture2D message;
	public Texture2D deleteButton;
	public Texture2D deleteButtonSelected;
	public Texture2D replyButton;
	public Texture2D replyButtonSelected;
	public RectOffset cancelMargin;
	public Texture2D cancelButton;
	public Texture2D cancelButtonSelected;
	public RectOffset writeMargin;
	public Texture2D writeButton;
	public Texture2D writeButtonSelected;
	public RectOffset publicMargin;
	public Texture2D publicButton;
	public Texture2D publicButtonSelected;
	public RectOffset privateMargin;
	public Texture2D privateButton;
	public Texture2D privateButtonSelected;
	
	//Styles
	protected GUIStyle boardStyle;
	protected GUIStyle messageStyle;
	protected GUIStyle pinStyle;
	protected GUIStyle labelsStyle;
	protected GUIStyle textStyle;
	protected GUIStyle messageTextStyle;
	protected GUIStyle deleteButtonStyle;
	protected GUIStyle replyButtonStyle;
	protected GUIStyle cancelButtonStyle;
	protected GUIStyle writeButtonStyle;
	protected GUIStyle privateButtonStyle;
	protected GUIStyle publicButtonStyle;
	public GUIStyle publicButtonStyle2;
	#endregion
	
	void OnDestroy(){
		MavaruOnlineMain.GameStateManager.State = GameStateManager.GameState.IN_MESSAGE_BOARD;
	}
	
	protected virtual void Start(){
		this.previousGameState = MavaruOnlineMain.GameStateManager.State;
		MavaruOnlineMain.GameStateManager.State = GameStateManager.GameState.IN_MESSAGE_BOARD;
		
		this.scrollPositions = new Dictionary<int, Vector2>();
		
		this.boardStyle = new GUIStyle();
		boardStyle.normal.background = board;
		
		this.messageStyle = new GUIStyle();
		messageStyle.padding = new RectOffset(25,0,40,0);
		messageStyle.normal.background = message;
		
		this.labelsStyle = new GUIStyle();
		labelsStyle.fixedWidth = 40;
		labelsStyle.normal.textColor = Color.black;
		
		this.textStyle = new GUIStyle();
		textStyle.fixedWidth = messageWidth - 40;
		textStyle.normal.textColor = Color.black;
		
		this.messageTextStyle = new GUIStyle();
		//messageTextStyle.fixedWidth = messageWidth;
		messageTextStyle.normal.textColor = Color.black;
		messageTextStyle.margin.top = 15;
		messageTextStyle.margin.bottom = 30;
		messageTextStyle.fontStyle = FontStyle.Bold; 
		
		this.deleteButtonStyle = new GUIStyle();
		deleteButtonStyle.fixedWidth = 64;
		deleteButtonStyle.fixedHeight = 32;
		deleteButtonStyle.alignment = TextAnchor.MiddleRight;
		deleteButtonStyle.normal.background = deleteButton;
		deleteButtonStyle.hover.background = deleteButtonSelected;
		
		this.replyButtonStyle = new GUIStyle(deleteButtonStyle);
		replyButtonStyle.normal.background = replyButton;
		replyButtonStyle.hover.background = replyButtonSelected;
		
		this.cancelButtonStyle = new GUIStyle(deleteButtonStyle);
		cancelButtonStyle.normal.background = cancelButton;
		cancelButtonStyle.hover.background = cancelButtonSelected;
		cancelButtonStyle.fixedWidth = 99;
		cancelButtonStyle.fixedHeight = 51;
		cancelButtonStyle.margin = cancelMargin;
		
		this.writeButtonStyle = new GUIStyle(cancelButtonStyle);
		writeButtonStyle.normal.background = writeButton;
		writeButtonStyle.hover.background = writeButtonSelected;
		writeButtonStyle.margin = writeMargin;
		
		this.privateButtonStyle = new GUIStyle(cancelButtonStyle);
		privateButtonStyle.normal.background = privateButton;
		privateButtonStyle.hover.background = privateButtonSelected;
		privateButtonStyle.margin = privateMargin;
		
		this.publicButtonStyle = new GUIStyle(cancelButtonStyle);
		publicButtonStyle.normal.background = publicButton;
		publicButtonStyle.hover.background = publicButtonSelected;
		publicButtonStyle.margin = publicMargin;
	}
	
	protected virtual void OnGUI() {
		
		GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height), boardStyle);
  
		//Messages
		PrintMessages();
		
		//Buttons
		GUILayout.BeginHorizontal();
		PrintButtons();
		GUILayout.EndHorizontal();
		
		GUILayout.EndArea();
    }
	
	protected abstract void PrintMessages();
	protected abstract void PrintButtons();
	protected abstract void PrintMessageButtons(Message message);
	
	public abstract void CancelReply();
	public abstract void Reply(Message message);
	
	protected void PrintMessages(List<Message> messages){
		
		messagesHeight = 0;

		for(int i = 0; i < messages.Count; i++){
			messagesHeight += DrawMessage(messages[i], messagesHeight,  i%2 != 0) ;
		}
		
		if(messages.Count % 2 != 0) 
			messagesHeight += messageHeight + messageHeightDelta;
		messagesHeight += 30;
	}
	
	protected int DrawMessage(Message message, int height, bool par){
		
		float widthStart = messageBoardRect.x;
		if(par) widthStart += messageWidth + messageHeightDelta;
		
		Rect r = new Rect(widthStart, height + messageBoardRect.y, messageWidth, messageHeight);
		GUILayout.BeginArea(r,messageStyle);
	
		// Sender
        GUILayout.BeginHorizontal();
        GUILayout.Label("From:", labelsStyle);
        GUILayout.Label(message.Sender.Username, textStyle);
        GUILayout.EndHorizontal();
		
		//Date
		GUILayout.BeginHorizontal();
        GUILayout.Label("Date:", labelsStyle);
        GUILayout.Label( message.Date, textStyle);
        GUILayout.EndHorizontal();
		
		//Message
		if(!scrollPositions.ContainsKey(message.Id)) scrollPositions.Add(message.Id, Vector2.zero);
		scrollPositions[message.Id] = GUILayout.BeginScrollView(scrollPositions[message.Id], 
		                                                   GUILayout.Width (messageTextWidth), 
		                                                   GUILayout.Height (messageTextHeight));
		GUILayout.Label(message.MessageText, messageTextStyle);
		GUILayout.EndScrollView ();

		// BUTTONS
		PrintMessageButtons(message);
		GUILayout.EndArea();
		
		if(!par) return 0;
		return messageHeight + messageHeightDelta;
	}
}