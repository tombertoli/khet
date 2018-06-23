using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.SceneManagement;

public class volver : MonoBehaviour {
	public void Clickea1000()
	{
		if (SceneManager.GetActiveScene().buildIndex != 1) {
			SceneManager.LoadScene("MainMenu");
			return;
		}

		if (TurnManager.IsLocalGame) return; 
		
		NetworkManager.singleton.StopHost();//GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<ConnectionController>().Disconnect();
		NetworkManager.Shutdown();

		//SceneManager.LoadScene ("MainMenu");
	}
}
