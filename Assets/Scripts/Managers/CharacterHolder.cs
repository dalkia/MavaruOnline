using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Entidades;

public class CharacterHolder{
	
	Dictionary<string, Character> characters;
	User mainUser;
	MainCharacter mainCharacter;
	
	public static CharacterHolder instance;
	public static CharacterHolder Instance{
		get{
			if(instance == null) 
				instance = new CharacterHolder(MavaruOnlineMain.DatabaseManager.CurrentUser);
			return instance;
		}
	}
	
	public static void Destroy(){
		if(instance == null || instance.characters == null) return;
		foreach(Character c in instance.characters.Values){
			GameObject.Destroy(c.gameObject);
		}
		GameObject.Destroy(Instance.mainCharacter.gameObject);
		instance = null;
	}
	
	CharacterHolder(User user){
		this.mainUser = user;
		this.mainCharacter = MainCharacter.CreateMainCharacter(user);
		this.characters = new Dictionary<string, Character>();
		//characters.Add(mainUser.Username, mainCharacter);
	}
	
	public MainCharacter MainCharacter{
		get{
			return mainCharacter;
		}
	}
	
	public Character GetCharacter(string username){
		if(characters.ContainsKey(username)){
			return characters[username];
		}
		return null;
	}
	
	public void InitCharacter(User user){
		characters.Add(user.Username, Character.CreateCharacter(user));
	}
	
	public void DestroyCharacter(string username){
		if(characters.ContainsKey(username)){
			GameObject.Destroy(characters[username].gameObject);
			characters.Remove(username);
		}
	}
	
	public void EnableCharacters(bool enable){
		foreach(Character c in characters.Values){
			c.gameObject.SetActiveRecursively(enable);
			c.transform.position = Vector3.zero;
			c.ShowModel(false);
		}
	}
}
