using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class volver : MonoBehaviour {
	public void Clickea1000()
	{
		if (SceneManager.GetActiveScene().buildIndex != 4 || SceneManager.GetActiveScene().buildIndex != 1) {
			SceneManager.LoadScene ("MainMenu");
			return;
		}

		if (!TurnManager.IsLocalGame)
			GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<ConnectionController>().Disconnect();

		SceneManager.LoadScene ("MainMenu");
	}
}
