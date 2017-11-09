using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class HUDController : MonoBehaviour {
	[SerializeField]
	private NetworkManager networkManager;
	
	public void Host() {
		networkManager.StartHost();
	}

	public void Connect(string ip) {
		networkManager.StartClient();
		networkManager.client.Connect(ip, 7777);
	}
}
