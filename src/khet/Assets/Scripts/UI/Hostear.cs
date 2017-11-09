using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Hostear : MonoBehaviour {

<<<<<<< HEAD:src/khet/Assets/Scripts/UI/Hostear.cs
	float timeOfTravelColor, currentTimeColor, normalizedValueColor;
	public GameObject jugar, eliminarJugador, jugadores, jugarText, eliminarJugadorText, jugadoresText;
	public GameObject conectar, ip, conectarText, ipText, hostearText;

	public void CallFadeIn()
	{
		jugar.SetActive (true);
		eliminarJugador.SetActive (true);
		jugadores.SetActive (true);

		StartCoroutine (FadeIn ());
		
	}

	IEnumerator FadeOut()
	{
		timeOfTravelColor = 0.8f;
		currentTimeColor = 0;
		normalizedValueColor = 0;

		Image conectarBg = conectar.GetComponentInChildren<Image> ();
		Image hostearBg = gameObject.GetComponentInChildren<Image> ();
		Image ipBg = ip.GetComponent<Image> ();
		Color targetBg = conectarBg.color;
		targetBg.a = 0;

		Text conectarTx = conectarText.GetComponent<Text> ();
		Text ipTx = ipText.GetComponent<Text> ();
		Text hostearTx = hostearText.GetComponent<Text> ();
		Color targetTx = conectarTx.color;
		targetTx.a = 0;

		while (currentTimeColor <= timeOfTravelColor) {
			currentTimeColor += Time.deltaTime;
			normalizedValueColor = currentTimeColor / timeOfTravelColor;

			conectarBg.color = Color.Lerp (conectarBg.color, targetBg, normalizedValueColor);
			hostearBg.color = Color.Lerp (hostearBg.color, targetBg, normalizedValueColor);
			ipBg.color = Color.Lerp (ipBg.color, targetBg, normalizedValueColor);

			conectarTx.color = Color.Lerp (conectarTx.color, targetTx, normalizedValueColor);
			ipTx.color = Color.Lerp (ipTx.color, targetTx, normalizedValueColor);
			hostearTx.color = Color.Lerp (hostearTx.color, targetTx, normalizedValueColor);

			yield return null;
		}
		conectar.SetActive (false);
		ip.SetActive (false);
		gameObject.SetActive (false);
	}

	IEnumerator FadeIn()
	{
		//Debug.Log("Empezo Fade-In Jugar");
		timeOfTravelColor = 0.8f;
		currentTimeColor = 0;
		normalizedValueColor = 0;

		Image jugarBg = jugar.GetComponentInChildren<Image>();
		Image eliminarJugadorBg = eliminarJugador.GetComponentInChildren<Image>();
		Color targetBg = jugarBg.color;
		Color targetBg2 = jugadores.GetComponent<RawImage> ().color;
		targetBg.a = 1;
		targetBg2.a = 1;

		Text jugarTx = jugarText.GetComponent<Text>();
		Text eliminarJugadorTx = eliminarJugadorText.GetComponent<Text>();
		Text jugadoresTx = jugadoresText.GetComponent<Text> (); 
		Color targetTx = jugarTx.color;
		targetTx.a = 1;

		while (currentTimeColor <= timeOfTravelColor)
		{
			currentTimeColor += Time.deltaTime;
			normalizedValueColor = currentTimeColor / timeOfTravelColor;

			jugarBg.color = Color.Lerp(jugarBg.color, targetBg, normalizedValueColor);
			eliminarJugadorBg.color = Color.Lerp(eliminarJugadorBg.color, targetBg, normalizedValueColor);
			jugadores.GetComponent<RawImage>().color = Color.Lerp(jugadores.GetComponent<RawImage>().color, targetBg2, normalizedValueColor);

			jugarTx.color = Color.Lerp(jugarTx.color, targetTx, normalizedValueColor);
			eliminarJugadorTx.color = Color.Lerp(eliminarJugadorTx.color, targetTx, normalizedValueColor);
			jugadoresTx.color = Color.Lerp(jugadoresTx.color, targetTx, normalizedValueColor);
			yield return null;
		}

		//puedeFadearOut = true;
		//Debug.Log("Termino Fade-In Jugar");
	}
}
