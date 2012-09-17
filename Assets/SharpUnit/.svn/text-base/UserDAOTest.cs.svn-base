using UnityEngine;
using System.Collections;
using SharpUnit;
using Entidades;
public class UserDAOTest : TestCase {

    public bool m_wasRun = false;       // True if this test was run.
    public bool m_wasSetup = false;     // True if this test was setup.
    
    public override void SetUp(){
        m_wasSetup = true;
    }
	
	[UnitTest]
	public void TestCreateUser(){
		UserService.Register("username", "password", "mail");
	 
		Assert.True(UserService.UserExists("username"));
		Assert.False(UserService.UserExists("otherUsername"));
	}
	
	[UnitTest]
	public void TestPopulateDatabase(){
		UserService.Register("username", "password", "mail");
		UserService.Register("username2", "password2", "mail2");
		UserService.Register("username3", "password3", "mail3");
		UserService.Register("username4", "password4", "mail4");
		UserService.Register("username5", "password5", "mail5");
		UserService.Register("username6", "password6", "mail6");
		
		Assert.True(UserService.UserExists("username"));
		Assert.True(UserService.UserExists("username2"));
		Assert.True(UserService.UserExists("username3"));
		Assert.True(UserService.UserExists("username4"));
		Assert.True(UserService.UserExists("username5"));
		Assert.True(UserService.UserExists("username6"));
	}
	
	[UnitTest]
	public void TestValidateUser(){
		User user = UserService.GetUser("username");
		
		//Assert.True(UserService.CheckLogin(user.Username, user.Password));
		Assert.True(UserService.CheckLogin("username", "password"));
		Assert.False(UserService.CheckLogin("username", "somePassword"));
		Assert.False(UserService.CheckLogin("otheUsername", "somePassword"));
	}

    [UnitTest]
    public void TestModifyUser(){            
		UserService.ChangeCharacterConfiguration("username", "newDescription");	
		UserService.ChangeCharacterConfiguration("username2", "newDescription2");	
	
		Assert.Equal(UserService.GetUser("username").Description, "newDescription");
		Assert.Equal(UserService.GetUser("username2").Description, "newDescription2");
    }

	[UnitTest]
	public void TestDeleteUser(){
		UserService.Delete("username");
		UserService.Delete("username2");
		
		Assert.False(UserService.UserExists("username"));
		Assert.False(UserService.UserExists("username2"));
		Assert.True(UserService.UserExists("username3"));
		Assert.True(UserService.UserExists("username4"));
		Assert.True(UserService.UserExists("username5"));
		Assert.True(UserService.UserExists("username6"));
	}
}
