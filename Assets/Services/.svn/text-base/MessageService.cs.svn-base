using System;
using Entidades;
using System.Collections.Generic;
public class MessageService {

	public static List<Entidades.Message> GetMessages(string username, bool isPrivate)
	{
		MessageDAO messageDAO = new MessageDAO();
		List<Entidades.Message> messages = messageDAO.GetMessages(username, isPrivate);
		return messages;
	}			
	
	public static void DeleteMessage(Entidades.Message message)
	{
		MessageDAO messageDAO = new MessageDAO();
		messageDAO.DeleteMessage(message);
	}
	public static void DeleteMessageById(int id)
	{
		MessageDAO messageDAO = new MessageDAO();
		messageDAO.DeleteMessageById(id);
	}
	
	public static void AddMessage(Entidades.Message message)
	{
		MessageDAO messageDAO = new MessageDAO();
		messageDAO.AddMessage(message);
	}
		
		
	public static Entidades.Message AddMessage(string sender, string receiver, string messageText, bool isPrivate)
	{
		MessageDAO messageDAO = new MessageDAO();
		return messageDAO.AddMessage(sender, receiver, messageText, isPrivate);
	}
	
	public static Entidades.Message GetMessageById(int Id)
	{
		MessageDAO messageDAO = new MessageDAO();
		return messageDAO.GetMessageById(Id);
	}
	
}
