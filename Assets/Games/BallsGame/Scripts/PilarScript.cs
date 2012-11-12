using UnityEngine;
using System;

public class PilarScript : Selector
{
	
	void OnGUI(){
		if(!selectorEnable) return;
		
		GUILayout.BeginArea(new Rect(mousePosition.x,Screen.height - mousePosition.y,50,50));
			if(GUILayout.Button("OK")){
				ChoiceScript s = GameObject.Find ("Choice").GetComponent<ChoiceScript>();
				s.choice = Convert.ToInt32(this.name);
				s.texture = s.textures[s.choice-1];
				Application.LoadLevel("BallsGame");
				selectorEnable = false;
				GameObject.Find("GameManager").GetComponent<SumoGameClient>().OnBallTypeSelected(s.choice-1);
			}
	
		if(GUILayout.Button("...")) selectorEnable = false;
		GUILayout.EndArea();
		
	}
}
