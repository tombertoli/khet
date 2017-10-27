using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Opciones : MonoBehaviour 
{
	RectTransform rectTransform;
	Vector2 startingPosition = new Vector2 (0.411f, 0.3812132f); 
	Vector2 endPosition = new Vector2(0.22f, 0.3812132f);
    float timeOfTravel = 1, timeOfTravelColor = 6; //time after object reach a target place 
    float currentTime = 0, currentTimeColor = 0; // actual floting time 
    float normalizedValue, normalizedValueColor;
    public GameObject instru, jugar, creditos;
    public GameObject musica, sonido, controles, musicaText, sonidoText, controlesText;
	bool clickeoBotonDeOpciones = false, clickeoBotonDeJugar = false;
    bool terminoFadeInJugar = false; //cambia de valor cuando terminae el fade in de jugar.
    bool puedeFadearOut = false;

	void Start()
	{
		rectTransform = gameObject.GetComponent<RectTransform> ();
		musica.SetActive (false);
		sonido.SetActive (false);
		controles.SetActive (false);
	}

    void MostrarOpciones()
    {
        if (clickeoBotonDeJugar)
        {
            if (terminoFadeInJugar)
            {
                jugar.GetComponent<Jugar>().CallFadeOut();
                clickeoBotonDeJugar = false;
                clickeoBotonDeOpciones = true;
                terminoFadeInJugar = false;
                jugar.GetComponent<Jugar>().CallOpcionesClickeado();
            }
        }
        else
        if (!clickeoBotonDeOpciones)
        {
            clickeoBotonDeOpciones = true;
            jugar.GetComponent<Jugar>().CallOpcionesClickeado();
            StartCoroutine(Mover());
            instru.GetComponent<Instrucciones>().OLA();
            jugar.GetComponent<Jugar>().OLA();
            creditos.GetComponent<Creditos>().OLA();
        }
    }

    public void CallPuedeTocarBoton()
    {
        terminoFadeInJugar = true;
    }

    public void CallFadeOut()
    {
        StartCoroutine(FadeOut());
    }

    public void OLA()
	{
		StartCoroutine (Mover ());
	}

    public void CallFadeIn()
    {
        musica.SetActive(true);
        sonido.SetActive(true);
        controles.SetActive(true);

        StartCoroutine(FadeIn());
    }

    public void CallJugarClickeado()
    {
        clickeoBotonDeJugar = true;
        clickeoBotonDeOpciones = false;
    }

	IEnumerator Mover() 
	{
		while (currentTime <= timeOfTravel) 
		{
			currentTime += Time.deltaTime;
			normalizedValue = currentTime / timeOfTravel;

			rectTransform.anchorMin	 = Vector3.Lerp(startingPosition, endPosition, normalizedValue);
			yield return null;
		}

        if (clickeoBotonDeOpciones)
        {
            musica.SetActive(true);
            sonido.SetActive(true);
            controles.SetActive(true);

            StartCoroutine(FadeIn());
        }
        else
            clickeoBotonDeJugar = true;
    }
	
	IEnumerator FadeIn()
	{
        Debug.Log("Empezo Fade-In Opciones");
        timeOfTravelColor = 0.8f;
		currentTimeColor=0;
		normalizedValueColor = 0;

		Image musicaBg = musica.GetComponentInChildren<Image>();
		Image sonidoBg = sonido.GetComponentInChildren<Image>();
        Image controlesBg = controles.GetComponentInChildren<Image>();
		Color targetBg = musicaBg.color;
		targetBg.a = 1;

		Text musicaTx = musicaText.GetComponent<Text> ();
		Text sonidoTx = sonidoText.GetComponent<Text> ();
        Text controlesTx = controlesText.GetComponent<Text>();
		Color targetTx = musicaTx.color;
		targetTx.a = 1;

		while (currentTimeColor <= timeOfTravelColor)
		{
			currentTimeColor += Time.deltaTime;
			normalizedValueColor = currentTimeColor / timeOfTravelColor;

			musicaBg.color = Color.Lerp(musicaBg.color, targetBg, normalizedValueColor);
			sonidoBg.color = Color.Lerp(sonidoBg.color, targetBg, normalizedValueColor);
            controlesBg.color = Color.Lerp(controlesBg.color, targetBg, normalizedValueColor);

            musicaTx.color = Color.Lerp(musicaTx.color, targetTx, normalizedValueColor);
			sonidoTx.color = Color.Lerp(sonidoTx.color, targetTx, normalizedValueColor);
            controlesTx.color = Color.Lerp(controlesTx.color, targetTx, normalizedValueColor);

            yield return null;
        }
        
        puedeFadearOut = true;
        jugar.GetComponent<Jugar>().CallPuedeTocarBoton();
        Debug.Log("Termino Fade-In Opciones");
	}

    IEnumerator FadeOut()
    {
        if (puedeFadearOut)
        {
            Debug.Log("Empezo Fade-Out Opciones");
            timeOfTravelColor = 0.8f;
            currentTimeColor = 0;
            normalizedValueColor = 0;

            Image musicaBg = musica.GetComponentInChildren<Image>();
            Image sonidoBg = sonido.GetComponentInChildren<Image>();
            Image controlesBg = controles.GetComponentInChildren<Image>();

            Color targetBg = musicaBg.color;
            targetBg.a = 0;

            Text musicaTx = musicaText.GetComponent<Text>();
            Text sonidoTx = sonidoText.GetComponent<Text>();
            Text controlesTx = controlesText.GetComponent<Text>();
            Color targetTx = musicaTx.color;
            targetTx.a = 0;

            while (currentTimeColor <= timeOfTravelColor)
            {
                currentTimeColor += Time.deltaTime;
                normalizedValueColor = currentTimeColor / timeOfTravelColor;

                musicaBg.color = Color.Lerp(musicaBg.color, targetBg, normalizedValueColor);
                sonidoBg.color = Color.Lerp(sonidoBg.color, targetBg, normalizedValueColor);
                controlesBg.color = Color.Lerp(controlesBg.color, targetBg, normalizedValueColor);

                musicaTx.color = Color.Lerp(musicaTx.color, targetTx, normalizedValueColor);
                sonidoTx.color = Color.Lerp(sonidoTx.color, targetTx, normalizedValueColor);
                controlesTx.color = Color.Lerp(controlesTx.color, targetTx, normalizedValueColor);

                yield return null;
            }
            puedeFadearOut = false;
            musica.SetActive(false);
            sonido.SetActive(false);
            controles.SetActive(false);
            Debug.Log("Termino Fade-Out Opciones");
            jugar.GetComponent<Jugar>().CallFadeIn();
        }
    }

}