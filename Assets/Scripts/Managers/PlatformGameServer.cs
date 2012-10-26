using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;


public class PlatformGameServer : MonoBehaviour {
    
	private const int platformCounter = 9;
	private IList<int> indexes;
	private Dictionary<int,string> colorsForRound;
	private Dictionary<int,string> flagForRound;
	private Color[] colors;
	
	// Use this for initialization
	void Start () {
		indexes = new List<int> ();
		colors = new Color[platformCounter];
		addColors ();
    	colorsForRound = new Dictionary<int, string>();
		flagForRound = new Dictionary<int, string>();
	}
	
	private void addColors (){				
		colors[0] = new Color(255f/255f,102f/255f,0);
		colors[1] = Color.red;
		colors[2] = Color.green;
		colors[3] = Color.yellow;
		colors[4] = Color.cyan;
		colors[5] = new Color(255f/255f,102f/255f,102f/255f);
		colors[6] = new Color(0f/255f,0,152f/255f);
		colors[7] = Color.white;
		colors[8] = new Color(102f/255f,0f/255f,51f/255f);		
		
	}
	
	[RPC]//server
	void GenerateFlagColor(NetworkPlayer player, int turn){
    	string newColor;
		try{
			 newColor= flagForRound[turn];
		}catch(KeyNotFoundException e){
			int randomColorIndex=UnityEngine.Random.Range (0, platformCounter - 1);
			//generar un flag random usar un dictionary para que todos sean igual
		    Color randomColor = colors[randomColorIndex];
			newColor = randomColor.r+","+randomColor.g+","+randomColor.b;
			flagForRound[turn] = newColor;
		}
			networkView.RPC("ReceiveFlag",player,newColor);
	
	}
	
	[RPC]//server
	void GenerateColors(NetworkPlayer player, int round){
		Color[] genColors;
		string colorString;
		try{
			 colorString= colorsForRound[round];
		}catch(KeyNotFoundException e){
		    setIndexes();
			genColors = new Color[platformCounter];
			
			for(int i = 0; i<platformCounter; i++){
				int index = Random.Range(0, indexes.Count);
				Debug.Log("indexes:"+ indexes.Count);
				Debug.Log("index: "+index);
				Color color = colors[indexes[index]];
				genColors[i] =color;
				indexes.Remove (indexes[index]);
			}
			colorString = TransformColors(genColors);
			colorsForRound[round] = colorString;
		}
		
		Debug.Log(colorString);
		networkView.RPC("ReceiveColors",player,colorString);
	}
	
	[RPC]//server
	void PlayerFinishedGame(NetworkPlayer player, int minigameId, string username){
		
		ConnectedUsers cu = GetComponent<NetworkManager>().users;
		
		MiniGame miniGame = MiniGameManager.Instance.GetMiniGame(minigameId);
		miniGame.PlayerFinishedGame(username);
		
		foreach(string usernameAux in miniGame.allPlayers)
			networkView.RPC("ReceiveClientLostGame", cu.GetUser(usernameAux).networkPlayer, username);
		
		if(miniGame.GetPlayers().Count == 1)
			networkView.RPC("ReceiveClientWonGame", cu.GetUser(miniGame.GetPlayers()[0]).networkPlayer);
	}
	
	[RPC]//client
	void ReceiveClientWonGame(){
		GameObject.Find("GameManager").GetComponent<PlatformGameClient>().ReceiveClientWonGame();
	}
	
	[RPC]//client
	void ReceiveClientLostGame(string username){
		GameObject.Find("GameManager").GetComponent<PlatformGameClient>().ReceiveClientLostGame(username);
	}
	
	[RPC]//client
	void ReceiveColors(string genColors){
		GameObject.Find("GameManager").GetComponent<PlatformGameClient>().SetPlatforms(genColors);
	}
	
	[RPC]//client
	void ReceiveFlag(string flagColor){
		GameObject.Find("GameManager").GetComponent<PlatformGameClient>().SetFlag(flagColor);
	}
	
	private string TransformColors(Color[] genColors){
		//TransformMessages() look for changing rusty ness
		StringBuilder rustybuilder = new StringBuilder();
		foreach(Color c in genColors){
			rustybuilder.Append(c.r).Append(",").Append(c.g).Append(",").Append(c.b).Append(";");
		}
		return rustybuilder.ToString();	
	}
	
	 private void setIndexes(){
		for (int i =0; i<platformCounter; i++) {
			indexes.Add(i);
		}
		
	}
}
