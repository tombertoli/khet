﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LAN : MonoBehaviour {
	#pragma warning disable 0649
	[SerializeField] private int sceneIndex;
  #pragma warning restore 0649

	public void Jugar() {
		TurnManager.IsLocalGame = false;
		SceneManager.LoadScene(sceneIndex);
	}
}
