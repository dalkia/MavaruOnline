using UnityEngine;
using System.Collections;

public class HouseSelector : Selector {

	void OnGUI(){
		if(selectorEnable){
			GUILayout.BeginArea(new Rect(mousePosition.x,Screen.height - mousePosition.y,100,70));
			if(GUILayout.Button("Go Inside")) MavaruOnlineMain.GameStateManager.GoHome();
			if(GUILayout.Button("...")) selectorEnable = false;
			GUILayout.EndArea();
		}
	}
}
