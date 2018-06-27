using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.SceneManagement;

public class volver : MonoBehaviour {
	public void Clickea1000()
	{
		if (SceneManager.GetActiveScene().buildIndex != 1 && SceneManager.GetActiveScene().buildIndex != 2) {
			SceneManager.LoadScene("MainMenu");
			return;
		}

        if (TurnManager.IsLocalGame) {
            SceneManager.LoadScene("MainMenu");
            return;
        }

        var go = GameObject.FindObjectOfType<ConnectionController>();
        if (go == null) return;
        go.Disconnect();

		//SceneManager.LoadScene ("MainMenu");
	}
}
