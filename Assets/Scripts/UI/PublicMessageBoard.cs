using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Entidades;

public class PublicMessageBoard : MessageBoardUI {
	
	[HideInInspector]public User owner;
	[HideInInspector]public User visitor;
	List<Message> messages;
	bool isReplying = false;
	
	public static void Create(User owner){
		GameObject mb = GameObject.Instantiate(Resources.Load("Prefabs/FriendsMessageBoard")) as GameObject;
		mb.GetComponent<PublicMessageBoard>().SetUsers(owner, MavaruOnlineMain.DatabaseManager.CurrentUser);
	}
	
	public void SetUsers(User owner, User visitor){
		this.owner = owner;
		this.visitor = visitor;
		this.messages = new List<Message>();
		if(owner != null)
     		MavaruOnlineMain.DatabaseManager.GetMessages(owner.Username,SetMessages);
	}
	public void SetMessages(List<Entidades.Message> menssages){
		this.messages = menssages;	
	}
	
	protected override void Start(){
		base.Start();
		privateButtonStyle.normal.background = null;
		privateButtonStyle.hover.background = null;
		publicButtonStyle.normal.background = null;
		publicButtonStyle.hover.background = null;
	}
	
	protected override void PrintMessages(){
		
		if(!isReplying){
			scrollPosition = GUI.BeginScrollView(messageBoardRect, 
		                                     scrollPosition, 
		                                     new Rect(0, 0, messageWidth, messagesHeight));
		}else{
			GUI.BeginScrollView(messageBoardRect, 
		                                     scrollPosition, 
		                                     new Rect(0, 0, messageWidth, messagesHeight));
		}
		
		PrintMessages(messages);
		GUI.EndScrollView();
	}
	
	protected override void PrintButtons(){
		GUILayout.Button("", publicButtonStyle);
		GUILayout.Button("", privateButtonStyle);
		if(GUILayout.Button("", writeButtonStyle)){WriteMessage();}
		if(GUILayout.Button("", cancelButtonStyle)){Cancel();}
	}
	
	void WriteMessage(){
		if(isReplying) return;
		writeButtonStyle.normal.background = writeButtonSelected;
		cancelButtonStyle.hover.background = cancelButton;
		ReplyMessageBoard.Create(this, visitor, owner);
		isReplying = true;
	}
	
	void Cancel(){
		if(isReplying) return;
		Application.LoadLevel("InsideHouse");
	}
	
	public override void CancelReply(){
		writeButtonStyle.normal.background = writeButton;
		cancelButtonStyle.hover.background = cancelButtonSelected;
		isReplying = false;
	}
	
	public override void Reply(Message message){
		CancelReply();
		if(message.PublicMessage) 
			messages.Insert(0, message);
	}

	protected override void PrintMessageButtons(Message message){}
}
