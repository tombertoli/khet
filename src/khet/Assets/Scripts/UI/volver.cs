using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class volver : MonoBehaviour {
	public void Clickea1000()
	{
		if (!TurnManager.IsLocalGame)
			GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<ConnectionController>().Disconnect();
		
		SceneManager.LoadScene ("MainMenu");
	}
}
