using UnityEngine;
using System.Collections;

public class StateController : MonoBehaviour {
	void Awake() {
		if (GameObject.FindObjectsOfType<NetworkController>().Length > 0) {
			Destroy(gameObject);
			return;
		}
	}

	public static void EndGame(PieceColor won) {
		TextManager.EndGame(won);
		Debug.Log(won + " won");
	}
}
