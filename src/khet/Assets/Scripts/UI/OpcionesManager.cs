using UnityEngine;
using System.Collections;

public class OpcionesManager : MonoBehaviour 
{
	bool musica, sonido, botones;

	public void Awake()
	{
		DontDestroyOnLoad (gameObject);
	}

	public void callMusica (bool opcionSeleccionada)
	{
		if (musica)
			musica = false;
		else
			musica = true;
	}

	public void callSonido (bool opcionSeleccionada)
	{
		if (sonido)
			sonido = false;
		else
			sonido = true;
	}

	public void callBotones (bool opcionSeleccionada)
	{
		if (botones)
			botones = false;
		else
			botones = true;
	}

	public void callJugar()
	{
		Debug.Log ("Aca le ponemos musica, sonido y botones");
		Debug.Log ("Musica = " + musica);
		Debug.Log ("Sonido = " + sonido);
		Debug.Log ("Botones = " + botones);
	}
}
