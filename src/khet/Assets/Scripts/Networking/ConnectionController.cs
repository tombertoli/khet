using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class ConnectionController : NetworkManager {
	public void Host() {
		StartHost();
	}

	public void Connect(string ip) {
		StartClient();
		client.Connect(ip, 7777);
	}
}
