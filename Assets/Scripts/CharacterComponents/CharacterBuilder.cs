using UnityEngine;
using System.Collections;
using Entidades;

public class CharacterBuilder : MonoBehaviour {
	
    CharacterGenerator generator;
    GameObject characterGameObject;
	[HideInInspector] public User user;
	[HideInInspector] public bool initBuilder = true;
	
    bool usingLatestConfig;
    bool newCharacterRequested = true;
    bool firstCharacter = true;
    string nonLoopingAnimationToPlay;

    const float fadeLength = .6f;
    const int typeWidth = 80;
    const int buttonWidth = 20;
	
	public delegate void CharacterBuildEventHandler(GameObject character);
	public event CharacterBuildEventHandler CharacterBuilt;
	
	public void Init(User user){
		if(characterGameObject != null) GameObject.Destroy(characterGameObject);
		
		this.initBuilder = true;
		this.user = user;
		usingLatestConfig = false;
        newCharacterRequested = true;
	}
	
    void Update(){
		if(initBuilder && CharacterGenerator.ReadyToUse && user != null){
			initBuilder = false;
			if( user.Description == null){
				generator = CharacterGenerator.CreateWithRandomConfig();
			}else{
				generator = CharacterGenerator.CreateWithConfig(user.Description);
			}
		}
		
        if (generator == null) return;
        if (usingLatestConfig) return;
        if (!generator.ConfigReady) return;

        usingLatestConfig = true;

        if (newCharacterRequested){
            Destroy(characterGameObject);
            characterGameObject = generator.Generate();
			DontDestroyOnLoad(characterGameObject);
            characterGameObject.animation.Play("idle1");
            characterGameObject.animation["idle1"].wrapMode = WrapMode.Loop;
            newCharacterRequested = false;
			if(CharacterBuilt != null) CharacterBuilt(characterGameObject);

            // Start the walkin animation for the first character.
            if (!firstCharacter) return;
            firstCharacter = false;
        }
        else{
            characterGameObject = generator.Generate(characterGameObject);
            if (nonLoopingAnimationToPlay == null) return;
            characterGameObject.animation[nonLoopingAnimationToPlay].layer = 1;
            characterGameObject.animation.CrossFade(nonLoopingAnimationToPlay, fadeLength);
            nonLoopingAnimationToPlay = null;
        }
    }
	
	public void ChangeCharacter(bool next){
        generator.ChangeCharacter(next);
        usingLatestConfig = false;
        newCharacterRequested = true;
    }

    public void ChangeElement(string catagory, bool next, string anim) {
        generator.ChangeElement(catagory, next);
        usingLatestConfig = false;
        
        if (!characterGameObject.animation.IsPlaying(anim))
            nonLoopingAnimationToPlay = anim;
    }
	
	public bool ReadyToUse{
		get{
			return characterGameObject != null;
		}
	}
	
	public GameObject Character{
		get{
			return characterGameObject;
		}
		set{
			characterGameObject = value;
		}
	}
	
	public string GetConfig(){
        return generator.GetConfig();
    }
	
	public float CurrentConfigProgress{
        get{
			if (!ReadyToUse) return 0;
            return generator.CurrentConfigProgress;
        }
    }
	
	public bool UsingLastConfiguration{
        get{
            return usingLatestConfig;
        }
    }
}
