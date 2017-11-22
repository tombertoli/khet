using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Sonido : MonoBehaviour {
	[SerializeField] private AudioSource audioSource;

	void Start() {
		if (audioSource.mute) {
			gameObject.GetComponentInChildren<Text> ().text = "Sonido: OFF";
		} else {
			gameObject.GetComponentInChildren<Text> ().text = "Sonido: ON";
		}	
	}

	public void CambiarSonido()
	{
		if (!audioSource.mute) {
			gameObject.GetComponentInChildren<Text> ().text = "Sonido: OFF";
			Debug.Log("false");
			audioSource.mute = false;
		} else {
			gameObject.GetComponentInChildren<Text> ().text = "Sonido: ON";
			audioSource.mute = true;
			Debug.Log(true);
		}

		SonidoToggle.Enabled = audioSource.mute;
	}
}
