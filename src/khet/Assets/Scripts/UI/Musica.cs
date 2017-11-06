using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Musica : MonoBehaviour 
{
	bool i = true;

	public void CambiarMusica()
	{
		if (i) 
		{
			gameObject.GetComponentInChildren<Text> ().text = "Musica: OFF";
			i = false;
		} 
		else 
		{
			gameObject.GetComponentInChildren<Text> ().text = "Musica: ON";
			i = true;
		}
	}

	public void EmpezoFalse()
	{
		i = false;
		gameObject.GetComponentInChildren<Text> ().text = "Musica: OFF";
	}
}
