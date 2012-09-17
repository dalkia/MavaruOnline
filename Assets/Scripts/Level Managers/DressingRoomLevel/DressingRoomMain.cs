using System.Collections;
using UnityEngine;

class DressingRoomMain : MonoBehaviour{
	
	MainCharacter character;
	const float fadeLength = .6f;
    const int typeWidth = 80;
    const int buttonWidth = 20;
	
	void Start(){
		character = CharacterHolder.Instance.MainCharacter;
		character.transform.position = Vector3.zero;
		character.transform.rotation = Quaternion.identity;
		character.Show = true;
		//character.EnableCamera(false);
		//character.MovementManager.Enable(false);
		//character.EnableNetwork(false);
    }
	
	void SaveConfiguration(){
		string descrip = character.Builder.GetConfig();
		MavaruOnlineMain.DatabaseManager.CharacterConfiguration = descrip;
		character.networkManager.ChangeClothes(descrip);
		//MavaruOnlineMain.GameStateManager.GoHome();//
	}
	
	void CancelConfiguration(){
		character.Restart();
		MavaruOnlineMain.GameStateManager.GoHome();
	}

    void OnGUI(){
		if(character == null) return;
        GUI.enabled = character.Builder.UsingLastConfiguration && 
			character.Animation != null && !character.Animation.IsPlaying("walkin");
        GUILayout.BeginArea(new Rect(10, 10, typeWidth + 2 * buttonWidth + 8, 500));

        // Buttons for changing the active character.
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("<", GUILayout.Width(buttonWidth)))
            ChangeCharacter(false);

        GUILayout.Box("Gender", GUILayout.Width(typeWidth));

        if (GUILayout.Button(">", GUILayout.Width(buttonWidth)))
            ChangeCharacter(true);

        GUILayout.EndHorizontal();

        // Buttons for changing character elements.
        AddCategory("face", "Head", null);
        AddCategory("eyes", "Eyes", null);
        AddCategory("hair", "Hair", null);
        AddCategory("top", "Body", "item_shirt");
        AddCategory("pants", "Legs", "item_pants");
        AddCategory("shoes", "Feet", "item_boots");

        // Buttons for saving and deleting configurations.
        if (GUILayout.Button("Save")){
			SaveConfiguration();
		}
         
		bool inRegistration = MavaruOnlineMain.DatabaseManager.CharacterConfiguration == null;
       if (!inRegistration && GUILayout.Button("Cancel")){
			CancelConfiguration();
		} 

        // Show download progress or indicate assets are being loaded.
        GUI.enabled = true;
        if (!character.Builder.UsingLastConfiguration)
        {
            //float progress = character.Builder.CurrentConfigProgress;
            string status = "Loading";
            //if (progress != 1) status = "Downloading " + (int)(progress * 100) + "%";
            GUILayout.Box(status);
        }

        GUILayout.EndArea();
    }

    // Draws buttons for configuring a specific category of items, like pants or shoes.
    void AddCategory(string category, string displayName, string anim)
    {
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("<", GUILayout.Width(buttonWidth)))
            ChangeElement(category, false, anim);

        GUILayout.Box(displayName, GUILayout.Width(typeWidth));

        if (GUILayout.Button(">", GUILayout.Width(buttonWidth)))
            ChangeElement(category, true, anim);

        GUILayout.EndHorizontal();
    }

    void ChangeCharacter(bool next){
		character.Builder.ChangeCharacter(next);
    }

    void ChangeElement(string catagory, bool next, string anim){
		character.Builder.ChangeElement(catagory, next, anim);
    }
}

/*class DressingRoomMain : MonoBehaviour{
	GameObject character;
	
	const float fadeLength = .6f;
    const int typeWidth = 80;
    const int buttonWidth = 20;
	
	IEnumerator Start(){
		while (!character.Builder.ReadyToUse) yield return 0;
		character = character.Builder.Character;
		character.transform.position = Vector3.zero;
		character.transform.rotation = Quaternion.identity;
    }
	
	void Update(){
		character = character.Builder.Character;
	}
	
	void SaveConfiguration(){
		MavaruOnlineData.CharacterConfiguration = character.Builder.GetConfig();
		MavaruOnlineMain.GoHome();
	}
	
	void CancelConfiguration(){
		character.Builder.Restart();
		MavaruOnlineMain.GoHome();
	}

    void OnGUI(){
        GUI.enabled = character.Builder.UsingLastConfiguration && 
			character != null && !characterAnimation.IsPlaying("walkin");
        GUILayout.BeginArea(new Rect(10, 10, typeWidth + 2 * buttonWidth + 8, 500));

        // Buttons for changing the active character.
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("<", GUILayout.Width(buttonWidth)))
            ChangeCharacter(false);

        GUILayout.Box("Gender", GUILayout.Width(typeWidth));

        if (GUILayout.Button(">", GUILayout.Width(buttonWidth)))
            ChangeCharacter(true);

        GUILayout.EndHorizontal();

        // Buttons for changing character elements.
        AddCategory("face", "Head", null);
        AddCategory("eyes", "Eyes", null);
        AddCategory("hair", "Hair", null);
        AddCategory("top", "Body", "item_shirt");
        AddCategory("pants", "Legs", "item_pants");
        AddCategory("shoes", "Feet", "item_boots");

        // Buttons for saving and deleting configurations.
        if (GUILayout.Button("Save")){
			SaveConfiguration();
		}
         
		bool inRegistration = MavaruOnlineData.CharacterConfiguration == null;
       if (!inRegistration && GUILayout.Button("Cancel")){
			CancelConfiguration();
		} 

        // Show download progress or indicate assets are being loaded.
        GUI.enabled = true;
        if (!character.Builder.UsingLastConfiguration)
        {
            //float progress = character.Builder.CurrentConfigProgress;
            string status = "Loading";
            //if (progress != 1) status = "Downloading " + (int)(progress * 100) + "%";
            GUILayout.Box(status);
        }

        GUILayout.EndArea();
    }

    // Draws buttons for configuring a specific category of items, like pants or shoes.
    void AddCategory(string category, string displayName, string anim)
    {
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("<", GUILayout.Width(buttonWidth)))
            ChangeElement(category, false, anim);

        GUILayout.Box(displayName, GUILayout.Width(typeWidth));

        if (GUILayout.Button(">", GUILayout.Width(buttonWidth)))
            ChangeElement(category, true, anim);

        GUILayout.EndHorizontal();
    }
	
	void ChangeCharacter(string configuration){
		character.Builder.ChangeCharacter(configuration);
    }

    void ChangeCharacter(bool next){
		character.Builder.ChangeCharacter(next);
    }

    void ChangeElement(string catagory, bool next, string anim){
		character.Builder.ChangeElement(catagory, next, anim);
    }
}*/