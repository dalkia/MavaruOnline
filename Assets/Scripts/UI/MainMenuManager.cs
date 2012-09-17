using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
//using Service;

public class MainMenuManager : MonoBehaviour {
	
	#region MainMenuManagerFields
	enum State { LOGIN, REGISTER};
	State menuState;
	bool inLogin;
	bool inRegister;
	
	public Rect mavaruTitle;
	public Texture mavaruTitleTexture;
	public Rect mavaruData;
	public Texture mavaruDataTexture;
	
	public Texture errorSignTexture;
	
	//Login
	public Rect loginButton;
	public Rect loginUserNameLabel;
	public Rect loginUserNameField;
	public Rect loginPasswordLabel;
	public Rect loginPasswordField;
	public Rect loginRegisterButton;
	public Rect loginBackground;
	public Rect loginErrorSign;
	public Rect loginErrorMessage;
	
	string login_username = "";
	string login_password = "";
	
	bool loginButtonPressed = false;
	public bool loginCorrect = false;
	
	//Register
	public Rect registerButton;
	public Rect registerUserNameLabel;
	public Rect registerUserNameField;
	public Rect registerUserNameError;
	public Rect registerMailLabel;
	public Rect registerMailField;
	public Rect registerMailError;
	public Rect registerPasswordLabel;
	public Rect registerPasswordField;
	public Rect registerPasswordError;
	public Rect registerPasswordRepeatLabel;
	public Rect registerPasswordRepeatField;
	public Rect registerPasswordRepeatError;
	public Rect registerLoginButton;
	public Rect registerBackground;
	
	string register_username = "";
	string register_mail = "";
	string register_password = "";
	string register_passwordRepeat = "";
	
	bool registerButtonPressed = false;
	bool registerCorrect = false;
	bool registerUserNameCorrect = false;
	bool registerMailCorrect = false;
	bool registerPasswordCorrect = false;
	bool registerPaswordRepeatCorrect = false;
	#endregion
	
	void Login(){
		MavaruOnlineMain.DatabaseManager.Login(login_username);
		//MavaruOnlineMain.GameStateManager.EnterWorld(); esperar a que termine el call lo pase a Database Manager ReturnLogin
	}
	
	/*void Update() {
        if (Input.GetKeyDown(KeyCode.Return)){
			if(menuState == MainMenuManager.State.LOGIN){
				loginButtonPressed = true;
				CheckLogin();
			} else{
				registerButtonPressed = true;
				CheckRegister();
			} 
			
			Debug.Log("Enter");
		}
    }*/
	
	void SignIn(){
		MavaruOnlineMain.DatabaseManager.Register(register_username, register_password, register_mail);
		//MavaruOnlineMain.GameStateManager.EnterWorld();	
	}

	#region CheckLogin
	void CheckLogin(){
		MavaruOnlineMain.DatabaseManager.CheckLoginCorrect(login_username,login_password,CheckLoginConfirmation);
		//loginCorrect = UserService.CheckLogin(login_username, login_password);
	}
	void CheckLoginConfirmation(bool loginBoolean){
		loginCorrect = loginBoolean;	
		loginButtonPressed = true;
		
	}
	#endregion
	
	void CheckRegister(){
		MavaruOnlineMain.DatabaseManager.CheckUsernameAvailable(register_username, OnCheckUsername);
	}
	
	public void OnCheckUsername(bool registerUserNameValue){
		registerUserNameCorrect = registerUserNameValue;
		registerMailCorrect = TestEmail.IsEmail(register_mail);
		registerPasswordCorrect = register_password.Length > 0;
		registerPaswordRepeatCorrect = register_passwordRepeat == register_password;
		
		registerCorrect = registerUserNameCorrect && registerMailCorrect 
			&& registerPasswordCorrect && registerPaswordRepeatCorrect;
		registerButtonPressed = true;
	}
	
	void OnGUI () {	
		//Title & Credits
		GUI.Label(mavaruTitle, mavaruTitleTexture);
		GUI.Label(mavaruData, mavaruDataTexture);
		
		//Login
		if(menuState == MainMenuManager.State.LOGIN ){
			
			GUI.Box (loginBackground, "");
			GUI.Label(loginUserNameLabel, "User Name");
			login_username = GUI.TextField(loginUserNameField, login_username);
			GUI.Label(loginPasswordLabel, "Password");
			login_password = GUI.PasswordField(loginPasswordField, login_password, "*"[0]);
			
			if(GUI.Button(loginButton, "Login")){
				CheckLogin();
			}
			
			if(GUI.Button(loginRegisterButton, "Sign in!")) menuState = MainMenuManager.State.REGISTER;
			
			if(loginButtonPressed && !loginCorrect){
				GUI.Label(loginErrorSign, errorSignTexture);
				GUI.Label(loginErrorMessage, "Login failed");
				
				
			}else if(loginButtonPressed && loginCorrect && !inLogin){
				inLogin = true;
				Login();
			}
		}
		
		//Register
		if(menuState == MainMenuManager.State.REGISTER){
			
			GUI.Box (registerBackground, "");
			GUI.Label(registerUserNameLabel, "User Name");
			register_username = GUI.TextField(registerUserNameField, register_username);
			
			GUI.Label(registerMailLabel, "Mail");
			register_mail = GUI.TextField(registerMailField, register_mail);
			
			GUI.Label(registerPasswordLabel, "Password");
			register_password = GUI.PasswordField(registerPasswordField, register_password, "*"[0]);
			
			GUI.Label(registerPasswordRepeatLabel, "Repeat Password");
			register_passwordRepeat = GUI.PasswordField(registerPasswordRepeatField, register_passwordRepeat, "*"[0]);
			
			if(GUI.Button(registerLoginButton, "Login")) menuState = MainMenuManager.State.LOGIN;
			
			if(GUI.Button(registerButton, "Sign in")){ 
				CheckRegister();
			}
			
			if(registerButtonPressed && !registerCorrect){
				
				if(!registerUserNameCorrect) GUI.Label(registerUserNameError, errorSignTexture);
				if(!registerMailCorrect) GUI.Label(registerMailError, errorSignTexture);
				if(!registerPasswordCorrect) GUI.Label(registerPasswordError, errorSignTexture);
				if(!registerPaswordRepeatCorrect) GUI.Label(registerPasswordRepeatError, errorSignTexture);
				
			}else if(registerButtonPressed && registerCorrect && !inRegister){
				inRegister = true;
				SignIn();
			}
			
		}
		
	}

}

public static class TestEmail
{
  /// <summary>
  /// Regular expression, which is used to validate an E-Mail address.
  /// </summary>
  public const string MatchEmailPattern = 
			@"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
     + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
     + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
     + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";

  /// <summary>
  /// Checks whether the given Email-Parameter is a valid E-Mail address.
  /// </summary>
  /// <param name="email">Parameter-string that contains an E-Mail address.</param>
  /// <returns>True, when Parameter-string is not null and 
  /// contains a valid E-Mail address;
  /// otherwise false.</returns>
  public static bool IsEmail(string email)
  {
     if (email != null) return Regex.IsMatch(email, MatchEmailPattern);
     else return false;
  }
}
