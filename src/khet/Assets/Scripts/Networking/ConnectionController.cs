using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class ConnectionController : NetworkManager {
	private bool isHost = false;

	public void Host() {
		StartHost ();
		isHost = true;
	}

	public void Connect(string ip) {
		StartClient();
		client.Connect(ip, 7777);
	}

	public void Disconnect() {
		Debug.Log ("desconectando" + isHost);
		if (isHost)
			StopHost();
		else
			client.Disconnect();
	}
}
