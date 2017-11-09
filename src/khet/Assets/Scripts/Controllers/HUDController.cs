using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class HUDController : NetworkLobbyManager {
	[SerializeField] private Text playerList;
	private CurrentScene currentScene = CurrentScene.Lobby;
	private List<NetworkConnection> connections = new List<NetworkConnection>();

	public override void OnClientConnect(NetworkConnection conn) {
		connections.Add(conn);
		if (currentScene == CurrentScene.Game) return;

		playerList.text += playerList.text.Equals("") ? "" : "\n";
		playerList.text += string.Format("Player{0}", conn.connectionId);
	}

	public override void OnClientDisconnect(NetworkConnection conn) {
		connections.Remove(conn);
		if (currentScene == CurrentScene.Game) return;

		playerList.text = "\n";

		for (int i = 0; i > connections.Count; i++)
			playerList.text += string.Format("\nPlayer{0}", connections[i].connectionId);
	}

	public void Host() {
		StartHost();
	}

	public void Connect(string ip) {
		StartClient();
		client.Connect(ip, 7777);
	}

	public void StartGame() {
	}

	private enum CurrentScene {
		Lobby, Game
	}
}
