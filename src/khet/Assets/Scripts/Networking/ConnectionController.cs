using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
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
		if (SceneManager.GetActiveScene().buildIndex != 4 || SceneManager.GetActiveScene().buildIndex != 1)
			return;
			
		if (isHost)
			StopHost();
		else
			client.Disconnect();
	}
}
