using System;
using Entidades;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Collections;
public class UserService
{
	public UserService ()
	{
	}
	
	public static bool CheckLogin(string username, string password){
		User user = GetUser(username);
		
		if(user == null) return false;
		return user.Password.Equals(EncodePassword(password));
	}
	
	public static void Register(string username, string password, string mail){
		UserDAO userDAO = new UserDAO();
		userDAO.CreateUser(username, EncodePassword(password), mail);
	}
	
	public static void Delete(string username){
		UserDAO userDAO = new UserDAO();
		userDAO.Delete(username);
	}
	
	public static User GetUser(string username){
		UserDAO userDAO = new UserDAO();
		return userDAO.GetUserByUserName(username);
	}
	
	public static bool UserExists(string username){
		return GetUser(username) != null;
	}
	
	public static void ChangeCharacterConfiguration(string username, string configuration){
		UserDAO userDAO = new UserDAO();
		userDAO.ChangeConfiguration(username, configuration);
	}
	
	public static string GetCharacterConfiguration(string username){
		UserDAO userDAO = new UserDAO();
		return userDAO.GetConfiguration(username);
	}
	
	static string EncodePassword(string originalPassword){
	  //Declarations
	  Byte[] originalBytes;
	  Byte[] encodedBytes;
	  MD5 md5;
	
	  //Instantiate MD5CryptoServiceProvider, get bytes for original password and compute hash (encoded password)
	  md5 = new MD5CryptoServiceProvider();
	  originalBytes = ASCIIEncoding.Default.GetBytes(originalPassword);
	  encodedBytes = md5.ComputeHash(originalBytes);
	
	  //Convert encoded bytes back to a 'readable' string
	  return BitConverter.ToString(encodedBytes);
	}
	
	public static ArrayList GetAllUsers(){
		UserDAO userDAO = new UserDAO();
		return userDAO.GetAllUsers();
	}

	public static ArrayList GetAllFriends(string username){
		UserDAO userDAO = new UserDAO();
		return userDAO.GetAllFriends(username);
	}
	
	public static ArrayList GetAllPendingConfirmation(string username){
		UserDAO userDAO = new UserDAO();
		return userDAO.GetPendingConfirmation(username);
	}
	
	public static ArrayList GetAllAwaitingConfirmation(string username){
		UserDAO userDAO = new UserDAO();
		return userDAO.GetAwaitingConfirmation(username);
	}
	
	public static void AddFriend(string username, string friend){
		UserDAO userDAO = new UserDAO();
		
		userDAO.addFriend(username, friend);
	}
	
	public static void AddFriendRequest(string username, string friend){
		UserDAO userDAO = new UserDAO();
		userDAO.addFriendRequest(username, friend);
	}
	
	public static bool GetFriendStatus(string username, string friend){
		UserDAO userDAO = new UserDAO();
		return userDAO.GetFriendStatus(username, friend);
	}
	
	public static bool GetAwaitingStatus(string username, string friend){
		UserDAO userDAO = new UserDAO();
		return userDAO.GetFriendStatus(username, friend);
	}
	
	public static bool GetPendingStatus(string username, string friend){
		UserDAO userDAO = new UserDAO();
		return userDAO.GetFriendStatus(username, friend);
	}
	
	public static void RemovePendingFriendship(string username, string friend){
		UserDAO userDAO = new UserDAO();
		userDAO.removePendingFriendRequest(username, friend);
	}
	
	public static void RemoveAwaitingFriendship(string username, string friend){
		UserDAO userDAO = new UserDAO();
		userDAO.removeAwaitingFriendRequest(username, friend);
	}
	
	public static void RemoveFriendship(string username, string friend){
		UserDAO userDAO = new UserDAO();
		userDAO.removeFriendship(username, friend);
	}
	

	
}

