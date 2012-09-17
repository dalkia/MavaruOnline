using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Entidades;
using System.Net;

public class DatabaseManager : MonoBehaviour {
	
	User currentUser;
	bool loginCorrect;
	User friendUser;
	
	public User GetUser(string name){
		return null;
	}
	
	#region AddFriend
		
	public void AddFriend(string friend){
		networkView.RPC("AddFriendNetwork", RPCMode.Server, friend, CurrentUser.Username);		
	}
		
	[RPC]
	public void AddFriendNetwork(string username, string friend){
		UserService.AddFriend(username, friend);	
	}
	
	#endregion
	
	#region AddFriendRequest
		
	public void AddFriendRequest(string friend){
		networkView.RPC("AddFriendRequestNetwork", RPCMode.Server, CurrentUser.Username, friend);		
	}
		
	[RPC]
	public void AddFriendRequestNetwork(string username, string friend){
		UserService.AddFriendRequest(username, friend);	
	}
	
	#endregion
	
	
	#region RemoveFriend
		
	public void RemoveFriend(string friend){
		networkView.RPC("RemoveFriendNetwork", RPCMode.Server, friend, CurrentUser.Username);		
	}
		
	[RPC]
	public void RemoveFriendNetwork(string username, string friend){
		UserService.RemoveFriendship(username, friend);	
	}
	
	#endregion
	
	
	#region RemoveFriendRequest
		
	public void RemoveFriendAwaiting(string friend){
		networkView.RPC("RemoveFriendAwaitingNetwork", RPCMode.Server, CurrentUser.Username, friend);		
	}
		
	[RPC]
	public void RemoveFriendAwaitingNetwork(string username, string friend){
		Debug.Log("El due;o es " + username);
		UserService.RemoveAwaitingFriendship(username, friend);	
	}
	
	#endregion
	
	
	
	#region RemoveFriendRequestParte2,LaVenganzaDelFriendRequest
		
	public void RemoveFriendPending(string friend){
		networkView.RPC("RemoveFriendPendingNetwork", RPCMode.Server, CurrentUser.Username, friend);		
	}
		
	[RPC]
	public void RemoveFriendPendingNetwork(string username, string friend){
		UserService.RemovePendingFriendship(username, friend);
	}
	
	#endregion
	
	#region GetAllUsersList	
	
	public delegate void ShowAllUsersDelegate(ArrayList users, ArrayList friends, ArrayList awaiting, ArrayList pending);
	public ShowAllUsersDelegate showAllUsers;
	
	
	public void GetAllUser(ShowAllUsersDelegate showAllUsersDelegate){
		networkView.RPC("GetAllUsersNetwork", RPCMode.Server,  currentUser.Username);
		this.showAllUsers = showAllUsersDelegate;
	}
	
	[RPC]
	public void GetAllUsersNetwork(string username, NetworkMessageInfo info){		
		ArrayList users = UserService.GetAllUsers();
		ArrayList friends = UserService.GetAllFriends(username);
		ArrayList awaitingConfirmation = UserService.GetAllAwaitingConfirmation(username);
		ArrayList pendingConfirmation = UserService.GetAllPendingConfirmation(username);
		string allUsers = MiniJSON.JsonEncode(users);
		string allFriends = MiniJSON.JsonEncode(friends);
		string allAwaitingConfirmation = MiniJSON.JsonEncode(awaitingConfirmation);
		string allPendingConfirmation = MiniJSON.JsonEncode(pendingConfirmation);
		networkView.RPC("ReturnAllUsers", info.sender, allUsers, allFriends, allAwaitingConfirmation, allPendingConfirmation);
	}
	
	[RPC]
	public void ReturnAllUsers(string encodeUsers, string encodeFriends, string encodeAwaiting, string encodePending){
		ArrayList userList = (ArrayList)MiniJSON.JsonDecode(encodeUsers);
		ArrayList friendList = (ArrayList)MiniJSON.JsonDecode(encodeFriends);
		ArrayList awaitingList = (ArrayList)MiniJSON.JsonDecode(encodeAwaiting);
		ArrayList pendingList = (ArrayList)MiniJSON.JsonDecode(encodePending);
		userList.Remove(CurrentUser.Username);
		foreach(string friend in friendList){
			userList.Remove(friend);	
		}
		foreach(string friend in awaitingList){
			userList.Remove(friend);	
		}
		foreach(string friend in pendingList){
			userList.Remove(friend);	
		}
		this.showAllUsers(userList, friendList, awaitingList, pendingList);
	}
	
	
	#endregion
	
	
	#region Register
	public void Register(string username, string password, string mail){
		this.loginUserName = username;
		networkView.RPC("RegisterNetwork", RPCMode.Server, username,password,mail);
		User user = new User();
		user.Username = username;
		user.Description = null;
		currentUser = user;	
	}
	
	[RPC]
	public void RegisterNetwork(string username, string password, string mail,NetworkMessageInfo info){
		UserService.Register(username, password, mail);
		networkView.RPC("ReturnRegister", info.sender);

	}
	[RPC]
	public void ReturnRegister(){
		MavaruOnlineMain.NetworkManager.Login(loginUserName);
		MavaruOnlineMain.GameStateManager.EnterWorld();
	}
	
	#endregion
	
	public void Logout(){
		MavaruOnlineMain.NetworkManager.Logout(currentUser.Username);
		currentUser = null;
		CharacterHolder.Destroy();
	}
	
	#region Login
	private string loginUserName;
	
	public void Login(string username){	
	   networkView.RPC("LoginNetwork", RPCMode.Server, username);
	   this.loginUserName = username;
	   //currentUser = UserService.GetUser(username);
	   //MavaruOnlineMain.NetworkManager.Login(username); lo pase a ReturnLogin
	}
	
	[RPC]
	public void LoginNetwork(string username,NetworkMessageInfo info){
		Hashtable userTable = UserService.GetUser(username).GetUserHashTable();
		string userJson = MiniJSON.JsonEncode(userTable);
		networkView.RPC("ReturnLogin", info.sender, userJson);
	}
	
	[RPC]
	public void ReturnLogin(string userJson){
		Hashtable userTable = (Hashtable)MiniJSON.JsonDecode(userJson);
		User user = new User();
		user.Username = userTable["username"].ToString();
		user.Description = userTable["description"].ToString();
		currentUser = user;
		
		MavaruOnlineMain.NetworkManager.Login(loginUserName);
		MavaruOnlineMain.GameStateManager.EnterWorld(); 
	}
	#endregion
	
	#region CheckUsernameAvailable
	public delegate void OnUsernameCheckDelegate(bool usernameCorrect);
	public OnUsernameCheckDelegate onCheck;
	
	[RPC]
	public void CheckUsernameAvailableNetwork(string username, NetworkMessageInfo info){
		bool usernameAvailableNetwork = !UserService.UserExists(username);
		networkView.RPC("ReturnUsernameAvailable", info.sender, usernameAvailableNetwork);
	}
	
	[RPC]
	public void ReturnUsernameAvailable(bool usernameAvailableNetwork){
		this.onCheck(usernameAvailableNetwork);
	}
	
	public void CheckUsernameAvailable(string username, OnUsernameCheckDelegate onCheck){
		networkView.RPC("CheckUsernameAvailableNetwork", RPCMode.Server, username);
		this.onCheck = onCheck;		
	}
	#endregion
	
	#region LoginVerification	
	public delegate void CheckLoginConfirmationDelegate(bool loginConfirmation);
	public CheckLoginConfirmationDelegate checkLogin;
	
	public void CheckLoginCorrect(string username, string password, CheckLoginConfirmationDelegate checkLogin){
		networkView.RPC("CheckLoginNetwork", RPCMode.Server, username,password);
		this.checkLogin = checkLogin;		
	}
	
	[RPC]
	public void CheckLoginNetwork(string username,string password, NetworkMessageInfo info){
		bool correctLogin = UserService.CheckLogin(username,password);
		bool userAlreadyConnected = MavaruOnlineMain.NetworkManager.IsUserLogged(username);
		networkView.RPC("ReturnLoginCorrect", info.sender, correctLogin && !userAlreadyConnected);
	}
	
	[RPC]
	public void ReturnLoginCorrect(bool usernameAvailableNetwork){
		this.checkLogin(usernameAvailableNetwork);
	}
	#endregion
	
	public List<User> GetFriends(){
		return null;
	}
	
	#region Messages
	public delegate void MessagesDelegate(List<Message> messages);
	public MessagesDelegate privateMessages;
	public MessagesDelegate publicMessages;
	
	public void GetMessages(string username,MessagesDelegate md){
		GetMessagesFriend(username,false,md);
	}
	public void GetMessagesFriend(string username,bool isPrivate,MessagesDelegate messageSetter){
		
		if(isPrivate){
			this.privateMessages = messageSetter;
			networkView.RPC("GetMessagesNetworkPrivate", RPCMode.Server,username);
			
		}else{
			this.publicMessages = messageSetter;	
			networkView.RPC("GetMessagesNetworkPublic", RPCMode.Server,username);
		}
		
		}
	
	
	public void GetMessages(bool isPrivate,MessagesDelegate messageSetter){
		
		if(isPrivate){
			this.privateMessages = messageSetter;
			networkView.RPC("GetMessagesNetworkPrivate", RPCMode.Server,this.currentUser.Username);
			
		}else{
			this.publicMessages = messageSetter;	
			networkView.RPC("GetMessagesNetworkPublic", RPCMode.Server,this.currentUser.Username);
		}
		
		}
	[RPC]
	public void GetMessagesNetworkPrivate(string username, NetworkMessageInfo info){
		List<Entidades.Message> messages = MessageService.GetMessages(username, false);
		ArrayList messagesTransformed = TransformMessages(messages); 
		string messagesJson = MiniJSON.JsonEncode(messagesTransformed);
		networkView.RPC("ReturnGetMessagesPrivate", info.sender,messagesJson);
	}
	
	[RPC]
	public void ReturnGetMessagesPrivate(string messagesJson){
		List<Entidades.Message> messages  =DecodeMessages(messagesJson);
		this.privateMessages(messages);
	}
	[RPC]
	public void GetMessagesNetworkPublic(string username, NetworkMessageInfo info){
		List<Entidades.Message> messages = MessageService.GetMessages(username, true);
		ArrayList messagesTransformed = TransformMessages(messages);
		
		string messagesJson = MiniJSON.JsonEncode(messagesTransformed);
		networkView.RPC("ReturnGetMessagesPublic", info.sender,messagesJson);
	}
	public ArrayList TransformMessages(List<Entidades.Message> messages){
		ArrayList messagesTransformed = new ArrayList();
		
		foreach (Message m in messages){
			string JsonMessage = MiniJSON.JsonEncode(m.GetHashTable());	
			messagesTransformed.Add(JsonMessage);
		}
		
		return messagesTransformed;
	}
	
	public List<Entidades.Message> DecodeMessages(string messagesJson){
		List<Entidades.Message> listMessages = new List<Entidades.Message>();
			
		ArrayList messageList = (ArrayList)MiniJSON.JsonDecode(messagesJson);
		foreach (string s in messageList){
			Hashtable messageTable = (Hashtable)MiniJSON.JsonDecode(s);	
			Message m = new Message();
				
			User sender = new User();
			sender.Username = messageTable["from"].ToString();
			m.Sender = sender;
			
			User reciever = new User();
			reciever.Username = messageTable["to"].ToString();
			m.Receiver = reciever;
			
			if(messageTable["privacy"].ToString()== "true"){
				m.PublicMessage = true;
			}else{
				m.PublicMessage=false;
			}
			
			m.MessageText= messageTable["text"].ToString();
			m.Id = int.Parse(messageTable["id"].ToString());	
			listMessages.Add(m);	
		}
			
		return listMessages;	
	}
	
	[RPC]
	public void ReturnGetMessagesPublic(string messagesJson){
		List<Entidades.Message> messages  =DecodeMessages(messagesJson);
		this.publicMessages(messages);
	}
	

	public void DeleteMessage(Message message){
		int messageId = message.Id;
		networkView.RPC("DeleteMessageNetwork", RPCMode.Server,messageId);
		
	}

	[RPC]
	public void DeleteMessageNetwork(int id){
		
		MessageService.DeleteMessageById(id);	
	}
	
	
	public void AddMessage(Message message){
		Hashtable messageTable = message.GetHashTable();
		string messageJson = MiniJSON.JsonEncode(messageTable);
		networkView.RPC("AddMessageNetwork", RPCMode.Server,messageJson);
		
	}
	[RPC]
	public void AddMessageNetwork(string message){
	Hashtable messageTable = (Hashtable)MiniJSON.JsonDecode(message);
		Message m = new Message();
		
		m.Sender = UserService.GetUser(messageTable["from"].ToString());//to, privacy, text
		m.Receiver= UserService.GetUser(messageTable["to"].ToString());
		if(messageTable["privacy"].ToString() == true.ToString()){
			m.PublicMessage = true;
		}else{
			m.PublicMessage = false;
		}
		m.MessageText= messageTable["text"].ToString();
		m.Date = messageTable["date"].ToString();
		MessageService.AddMessage(m);
	
	}
	
	
	#endregion

	
	
	public User CurrentUser{
		get{
			return currentUser;
		}
	}
	
	public void VisitFriend(string username){
		networkView.RPC("VisitFriendNetwork", RPCMode.Server, username);
	}

		
	[RPC]
	public void VisitFriendNetwork(string username,NetworkMessageInfo info ){
		 Hashtable userTable = UserService.GetUser(username).GetUserHashTable();
		string userJson = MiniJSON.JsonEncode(userTable);
		networkView.RPC("ReturnVisitFriend", info.sender, userJson);
	}
	
	[RPC]
	public void ReturnVisitFriend(string userJson){
		Hashtable userTable = (Hashtable)MiniJSON.JsonDecode(userJson);
		User user = new User();
		user.Username = userTable["username"].ToString();
		user.Description = userTable["description"].ToString();
		friendUser = user;
		Debug.Log( "FriendUser: " + friendUser.Username);
	}
	
	public bool IsMainUser(User u){
		return u.Username == currentUser.Username;
	}
	
	public User FriendUser{
		get{
			Debug.Log("Getting FrindUser: " + friendUser);
			return friendUser;
		}
		set{
			friendUser = value;
		}
	}
#region CaracterConfiguration	
	private string descriptionValue =null;
	public string CharacterConfiguration{
		get{
			if(currentUser == null) return null;
			return currentUser.Description;
		}
		set{
			if(currentUser == null) return;
			this.descriptionValue = value;
			networkView.RPC("ChangeCaracterNetwork",RPCMode.Server,value,currentUser.Username);
		}
	}
	[RPC]
	public void ChangeCaracterNetwork(string description,string username,NetworkMessageInfo info){
		UserService.ChangeCharacterConfiguration(username, description);
		networkView.RPC("ReturnCheckCaracter", info.sender);
	}
	
	[RPC]
	public void ReturnCheckCaracter(){
	   	currentUser.Description = descriptionValue;
		MavaruOnlineMain.GameStateManager.GoHome();
	}
	
	
	
#endregion	
	public string HibernateConfigURL{
        get
        {
            if (Application.platform == RuntimePlatform.WindowsWebPlayer || Application.platform == RuntimePlatform.OSXWebPlayer)
                return Application.dataPath+"/Database/hibernate.cfg.xml";
            else
                return "file://" + Application.dataPath + "/Database/hibernate.cfg.xml";
        }
    }
	
	
}
