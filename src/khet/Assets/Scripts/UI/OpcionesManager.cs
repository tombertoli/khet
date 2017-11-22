using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class OpcionesManager : MonoBehaviour
{
  #pragma warning disable 0649
	[SerializeField] private int onlineIndex, offlineIndex;
  #pragma warning restore 0649
  
	private static bool musica = true, sonido = true, controles = true;
	public Button bmusica, bsonido, bcontroles;
  
	public void Awake()
	{
		Debug.Log ("Musica = " + musica);
		Debug.Log ("Sonido = " + sonido);
		Debug.Log ("Controles = " + controles);

		DontDestroyOnLoad (gameObject);
	}
  
	public void CallMusica(bool opcionSeleccionada)
	{
		musica = !musica;
	}

    public void CallSonido(bool opcionSeleccionada)
	{
		sonido = !sonido;
	}

    public void CallControles(bool opcionSeleccionada)
	{
		controles = !controles;
	}

    public void CallJugar(bool online)
	{
		TurnManager.IsLocalGame = !online;
		SceneManager.LoadScene (online ? onlineIndex : offlineIndex);
	}
}
