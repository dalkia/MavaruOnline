using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Entidades;

public class PersonalMessageBoard : MessageBoardUI {
	
	List<Message> privateMessages;
	List<Message> publicMessages;
	
	bool showingPublicMessages;
	bool isReplying = false;
	
	public static void Create(){
		GameObject mb = GameObject.Instantiate(Resources.Load("Prefabs/MessageBoard")) as GameObject;
	}
	
	protected override void Start(){
		base.Start();
		this.showingPublicMessages = true;
		this.publicMessages = new List<Message>();
		this.privateMessages = new List<Message>();
		
		MavaruOnlineMain.DatabaseManager.GetMessages(true,SetPrivateMessages);
		MavaruOnlineMain.DatabaseManager.GetMessages(false,SetPublicMessages);
		
		writeButtonStyle.normal.background = null;
		writeButtonStyle.hover.background = null;
	}
	
	public void SetPublicMessages(List<Message> messages){
		Debug.Log("llego public");
		this.PublicMessages= messages;
	}
	
	public void SetPrivateMessages(List<Message> messages){
		Debug.Log("llego private");
		this.PrivateMessages= messages;
	}
	
	public List<Message> PrivateMessages{
		get{
			return privateMessages;
		}
		set{
			privateMessages = value;
		}
	}
	
	public List<Message> PublicMessages{
		get{
			return publicMessages;
		}
		set{
			publicMessages = value;
		}
	}
	
	protected override void PrintMessages(){
		
		if(isReplying){
			GUI.BeginScrollView(messageBoardRect, 
		                                     scrollPosition, 
		                                     new Rect(0, 0, messageWidth, messagesHeight));
		}else{
			
			scrollPosition = GUI.BeginScrollView(messageBoardRect, 
		                                     scrollPosition, 
		                                     new Rect(0, 0, messageWidth, messagesHeight));
		}
		
		
		if(showingPublicMessages){
			PrintMessages(publicMessages);
		}else{
			PrintMessages(privateMessages);
		}
		
		GUI.EndScrollView();
	}
	
	protected override void PrintButtons(){
		
		if(showingPublicMessages){
			privateButtonStyle.normal.background = privateButton;
			publicButtonStyle.normal.background = publicButtonSelected;
			
		}else{
			privateButtonStyle.normal.background = privateButtonSelected;
			publicButtonStyle.normal.background = publicButton;
		}
		
		if(GUILayout.Button("", publicButtonStyle)){
			showingPublicMessages = true;
		}
		
		if(GUILayout.Button("", privateButtonStyle)){
			showingPublicMessages = false;
		}
		
		GUILayout.Button("", writeButtonStyle);
		if(GUILayout.Button("", cancelButtonStyle)){
			if(isReplying) return;
			MavaruOnlineMain.GameStateManager.GoHome();
		}
		
	}
	
	protected override void PrintMessageButtons(Message message){
		GUILayout.BeginHorizontal();
        if(GUILayout.Button("", replyButtonStyle)){ReplyMessage(message);}
		if(GUILayout.Button("", deleteButtonStyle)){DeleteMessage(message);}
        GUILayout.EndHorizontal();
	}
	
	void DeleteMessage(Message message){
		if(isReplying) return;
		
		if(showingPublicMessages){
			publicMessages.Remove(message);
		}else{
			privateMessages.Remove(message);
		}
		
		MavaruOnlineMain.DatabaseManager.DeleteMessage(message);
	}
	
	void ReplyMessage(Message message){
		if(isReplying) return;
		ReplyMessageBoard.Create(this, message.Receiver, message.Sender);
		ToggleReply();
	}
	
	public override void CancelReply(){
		ToggleReply();
	}
	
	public override void Reply(Message message){
		CancelReply();
	}
	
	void ToggleReply (){
		isReplying = !isReplying;
		if(isReplying){
			privateButtonStyle.hover.background = privateButtonStyle.normal.background;
			publicButtonStyle.hover.background = publicButtonStyle.normal.background;
			cancelButtonStyle.hover.background = cancelButton;
			deleteButtonStyle.hover.background = deleteButton;
			replyButtonStyle.hover.background = replyButton;
		}else{
			privateButtonStyle.hover.background = privateButtonSelected;
			publicButtonStyle.hover.background = publicButtonSelected;
			cancelButtonStyle.hover.background = cancelButtonSelected;
			deleteButtonStyle.hover.background = deleteButtonSelected;
			replyButtonStyle.hover.background = replyButtonSelected;
		}
	}
}