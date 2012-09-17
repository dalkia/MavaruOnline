using UnityEngine;
using System.Collections;

public class RigidAssign : MonoBehaviour {
	
	void OnNetworkInstantiate (NetworkMessageInfo msg) { 
		NetworkRigidbody nrb = gameObject.GetComponent<NetworkRigidbody>();
		nrb.enabled = !networkView.isMine;
	}
}
