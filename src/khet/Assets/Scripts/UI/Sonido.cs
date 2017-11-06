using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Sonido : MonoBehaviour {
	bool i = true;

	public void CambiarSonido()
	{
		if (i) {
			gameObject.GetComponentInChildren<Text> ().text = "Sonido: OFF";
			i = false;
		} else {
			gameObject.GetComponentInChildren<Text> ().text = "Sonido: ON";
			i = true;
		}
	}

	public void EmpezoFalse()
	{
		i = false;
		gameObject.GetComponentInChildren<Text> ().text = "Sonido: OFF";
	}
}
