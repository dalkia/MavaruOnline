using UnityEngine;
using System.Collections;

public class MavaruOnlineMain : MonoBehaviour {
	
	public static MavaruOnlineMain instance;
	public static MavaruOnlineMain Instance{
		get{
			if(instance == null) {
				GameObject main = GameObject.FindGameObjectWithTag("MavaruOnlineMain");
				if(main != null) instance = main.GetComponent<MavaruOnlineMain>();
			}
			return instance;
		}
	}
	
	public static DatabaseManager DatabaseManager{
		get{return MavaruOnlineMain.Instance.data;}
	}
	
	public static NetworkManager NetworkManager{
		get{return MavaruOnlineMain.Instance.network;}
	}
	
	public static GameStateManager GameStateManager{
		get{return MavaruOnlineMain.Instance.stateManager;}
	}
	
	public DatabaseManager data;
	public NetworkManager network;
	public GameStateManager stateManager;
	
	void Start(){
		GameObject.DontDestroyOnLoad(this.gameObject);
	}
}
