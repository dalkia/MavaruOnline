using UnityEngine;
using System.Collections.Generic;
using SharpUnit;
using Entidades;

public class MessageDAOTest : TestCase {
	
	public bool m_wasRun = false;       // True if this test was run.
    public bool m_wasSetup = false;     // True if this test was setup.
    
    public override void SetUp(){
        m_wasSetup = true;
    }
	/*
	[UnitTest]
	public void TestCreateMessage(){
		Entidades.Message message = MessageService.AddMessage("username4", "username5", "chau1", true);
	 	Entidades.Message message2 = MessageService.AddMessage("username5", "username6", "chau2", false);
		
		
		Assert.NotNull(message);
		Assert.NotNull(message2);
	}
	
	
	[UnitTest]
	public void TestDeleteMessage(){
		Entidades.Message message = MessageService.AddMessage("username3", "username4", "chau1", true);
	 	Entidades.Message message2 = MessageService.AddMessage("username3", "username5", "chau2", false);
		
		MessageService.DeleteMessage(message);
		MessageService.DeleteMessage(message2);
		
		Assert.Null(MessageService.GetMessageById(message.Id));
		Assert.Null(MessageService.GetMessageById(message2.Id));
	}	
	 */
	
	[UnitTest]
	public void TestGetMessages(){
		MessageDAO messageDAO = new MessageDAO();
		messageDAO.AddMessage("pepe34", "pepe24", "aaaaaaa", true);
	 	/*
		Entidades.Message message2 = messageDAO.AddMessage("pepe", "pepe2", "chau2", false);
		Entidades.Message message3 = messageDAO.AddMessage("pepe", "pepe2", "chau2", false);
		
		List<Entidades.Message> messagesTrue = MessageService.GetMessages("pepe2", true);
		List<Entidades.Message> messagesFalse = MessageService.GetMessages("pepe2", false);
		
		Assert.Equal(messagesTrue.Count, 1);
		Assert.Equal(messagesFalse.Count, 2);
		*/
	}
	
}
