using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class OpcionesManager : MonoBehaviour
{
  #pragma warning disable 0649
	[SerializeField] private int onlineIndex, offlineIndex;
  #pragma warning restore 0649
  
  private bool musica, sonido, botones;

  public void Awake()
  {
    DontDestroyOnLoad(gameObject);
  }

  public void CallMusica(bool opcionSeleccionada)
  {
    musica = !musica;
  }

  public void CallSonido(bool opcionSeleccionada)
  {
    sonido = !sonido;
  }

  public void CallBotones(bool opcionSeleccionada)
  {
    botones = !botones;
  }

  public void CallJugar(bool online)
  {
		TurnManager.IsSinglePlayer = !online;
		SceneManager.LoadScene(online ? onlineIndex : offlineIndex);
  }
}
