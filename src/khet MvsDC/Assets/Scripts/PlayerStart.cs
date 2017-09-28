using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerStart : NetworkBehaviour {
	void Awake() {
		if (localPlayerAuthority) return;
		
		gameObject.SetActive(false);
	}
}
