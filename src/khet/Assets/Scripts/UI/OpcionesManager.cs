using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class OpcionesManager : MonoBehaviour
{
	[SerializeField] private int onlineIndex, offlineIndex;
  private bool musica, sonido, botones;

  public void Awake()
  {
    DontDestroyOnLoad(gameObject);
  }

  public void CallMusica(bool opcionSeleccionada)
  {
    if (musica)
      musica = false;
    else
      musica = true;
  }

  public void CallSonido(bool opcionSeleccionada)
  {
    if (sonido)
      sonido = false;
    else
      sonido = true;
  }

  public void CallBotones(bool opcionSeleccionada)
  {
    if (botones)
      botones = false;
    else
      botones = true;
  }

  public void CallJugar(bool online)
  {
		TurnManager.IsSinglePlayer = !online;
		SceneManager.LoadScene(online ? onlineIndex : offlineIndex);
  }
}
