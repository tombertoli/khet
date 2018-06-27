using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class ConnectionController : NetworkManager {
	private bool isHost = false;
	private static bool started = false;

	/*private void Start() {
		var asd = GameObject.FindObjectsOfType<ConnectionController>();
		int i = 0;
		for (; i < asd.Length; i++) {
			if (asd[i] != this) break;
		}

		if (started) Destroy(asd[i]);
		else started = true;
	}*/

	public void Host() {
		NetworkServer.Reset();
		NetworkManager.singleton.StartHost();
		isHost = true;
	}

	public void Connect(string ip) {
		SetIPAddress(ip);
		SetPort();
        NetworkServer.Reset();
		NetworkManager.singleton.StartClient();
	}

	void SetIPAddress(string ip)
	{
		NetworkManager.singleton.networkAddress = ip;
	}

	void SetPort()
	{
		NetworkManager.singleton.networkPort = 7777;
	}

	public void Disconnect() {
        NetworkController.PlayerLeft();
		NetworkManager.singleton.StopHost();
	}
	public override void OnServerDisconnect(NetworkConnection conn) {
		base.OnServerDisconnect(conn);
		print(conn + " disconnected");
	}
 
	public override void OnServerAddPlayer (NetworkConnection conn, short playerControllerId) { 
		GameObject player;

		player = (GameObject)Object.Instantiate(this.playerPrefab, Vector3.zero, Quaternion.identity);
		NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
	}

	public override void OnClientSceneChanged(NetworkConnection conn) {
		ClientScene.AddPlayer(conn, 0);
	}

	public override void OnClientConnect(NetworkConnection conn) {
		//base.OnClientConnect(conn);
	}
}
