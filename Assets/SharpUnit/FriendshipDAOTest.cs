using UnityEngine;
using System.Collections.Generic;
using SharpUnit;
using Entidades;

public class FriendshipDAOTest : TestCase {

	public bool m_wasRun = false;       // True if this test was run.
    public bool m_wasSetup = false;     // True if this test was setup.
    
    public override void SetUp(){
        m_wasSetup = true;
    }
	
	[UnitTest]
	public void TestGetFriends(){
		UserService.AddFriendRequest("username3", "username4");		
		Assert.True(UserService.GetAllAwaitingConfirmation("username3").Contains("username4"));
		Assert.True(UserService.GetAllPendingConfirmation("username4").Contains("username3"));
	}
	
	
	
	[UnitTest]
	public void TestSetFriends(){
		UserService.AddFriend("username3", "username4");		
		Assert.True(UserService.GetAllFriends("username3").Contains("username4"));
	}
}
