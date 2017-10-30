using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Controles : MonoBehaviour {

	bool i = true;

	public void CambiarControles()
	{
		if (i) 
		{
			gameObject.GetComponentInChildren<Text> ().text = "Controles en Pantalla: OFF";
			i = false;
		} 
		else 
		{
			gameObject.GetComponentInChildren<Text> ().text = "Controles en Pantalla: ON";
			i = true;
		}
	}
}