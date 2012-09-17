
function Update () {
}

var Character : Transform;

function OnNetworkLoadedLevel () { 
	// Instantiating Character when Network is loaded 
	Debug.Log("OnNetworkLoadedLevel");
	//Network.Instantiate(Character, transform.position, transform.rotation, 0);
}

function OnPlayerDisconnected (player : NetworkPlayer) { 
	// Removing player if Network is disconnected
	Debug.Log("Server destroying player"); 
	Network.RemoveRPCs(player, 0); 
	Network.DestroyPlayerObjects(player);
}