﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Jugar : MonoBehaviour {
  [SerializeField] private AudioClip movingClip;
  private AudioSource audioSource;

  RectTransform rectTransform;
  Vector2 startingPosition;// = new Vector2(0.411f, 0.5693408f);
  Vector2 endPosition;// = new Vector2(0.22f, 0.5693408f);
  float timeOfTravel = 1, timeOfTravelColor = 0.5f; //time after object reach a target place 
  float currentTime = 0, currentTimeColor = 0; // actual floting time 
  float normalizedValue, normalizedValueColor;
  public GameObject instru, creditos;
  public GameObject local, lan, localText, lanText;
  bool clickeoBotonDeJugar = false, clickeoBotonDeOpciones = false;
  bool terminoFadeInOpciones = false;
  bool puedeFadearOut = false;

  void Start()
  {
    rectTransform = gameObject.GetComponent<RectTransform>();
    audioSource = Camera.main.GetComponent<AudioSource>();

    startingPosition = rectTransform.anchorMin;
    endPosition = new Vector2(rectTransform.anchorMin.x - .191f, rectTransform.anchorMin.y);

    local.SetActive(false);
    lan.SetActive(false);

    GameObject go = GameObject.FindGameObjectWithTag("NetworkManager");
    if (go != null) {
      print("Destroying Network Manager");
      Destroy(go);
    }
  }

  void MostrarModos()
  {
    if (clickeoBotonDeOpciones)
    {
      if (terminoFadeInOpciones)
      {
        clickeoBotonDeJugar = true;
        clickeoBotonDeOpciones = false;
        terminoFadeInOpciones = false;
      }
    }
    else
    if (!clickeoBotonDeJugar)
    {
      clickeoBotonDeJugar = true;
      StartCoroutine(Mover());
      instru.GetComponent<Instrucciones>().OLA();
      creditos.GetComponent<Creditos>().OLA();
    }
  }

  public void CallPuedeTocarBoton()
  {
    terminoFadeInOpciones = true;
  }

  public void CallFadeOut()
  {
    StartCoroutine(FadeOut());
  }

  public void OLA()
  {
    StartCoroutine(Mover());
  }

  public void CallFadeIn()
  {
    local.SetActive(true);
    lan.SetActive(true);

    StartCoroutine(FadeIn());
  }

  public void CallOpcionesClickeado()
  {
    clickeoBotonDeJugar = false;
    clickeoBotonDeOpciones = true;
  }

  IEnumerator Mover()
  {
    audioSource.clip = movingClip;
    audioSource.Play();

    while (currentTime <= timeOfTravel)
    {
      currentTime += Time.deltaTime;
      normalizedValue = currentTime / timeOfTravel;

      rectTransform.anchorMin = Vector3.Lerp(startingPosition, endPosition, normalizedValue);
      yield return null;
    }

    audioSource.Stop();

    

    if (clickeoBotonDeJugar)
    {
      local.SetActive(true);
      lan.SetActive(true);
      StartCoroutine(FadeIn());
    }
    else
      clickeoBotonDeOpciones = true;
  }

  IEnumerator FadeIn()
  {
    //Debug.Log("Empezo Fade-In Jugar");
    timeOfTravelColor = 0.8f;
    currentTimeColor = 0;
    normalizedValueColor = 0;

    Image localBg = local.GetComponentInChildren<Image>();
    Image lanBg = lan.GetComponentInChildren<Image>();
    Color targetBg = localBg.color;
    targetBg.a = 1;

    Text localTx = localText.GetComponent<Text>();
    Text lanTx = lanText.GetComponent<Text>();
    Color targetTx = localTx.color;
    targetTx.a = 1;

    while (currentTimeColor <= timeOfTravelColor)
    {
      currentTimeColor += Time.deltaTime;
      normalizedValueColor = currentTimeColor / timeOfTravelColor;

      localBg.color = Color.Lerp(localBg.color, targetBg, normalizedValueColor);
      lanBg.color = Color.Lerp(lanBg.color, targetBg, normalizedValueColor);

      localTx.color = Color.Lerp(localTx.color, targetTx, normalizedValueColor);
      lanTx.color = Color.Lerp(lanTx.color, targetTx, normalizedValueColor);

      yield return null;
    }

    puedeFadearOut = true;
    //Debug.Log("Termino Fade-In Jugar");
  }

  IEnumerator FadeOut()
  {
    if (!puedeFadearOut) yield break;

    Debug.Log("Empezo Fade-Out Jugar");
    timeOfTravelColor = 0.8f;
    currentTimeColor = 0;
    normalizedValueColor = 0;

    Image localBg = local.GetComponentInChildren<Image>();
    Image lanBg = lan.GetComponentInChildren<Image>();
    Color targetBg = localBg.color;
    targetBg.a = 0;

    Text localTx = localText.GetComponent<Text>();
    Text lanTx = lanText.GetComponent<Text>();
    Color targetTx = localTx.color;
    targetTx.a = 0;

    while (currentTimeColor <= timeOfTravelColor)
    {
      currentTimeColor += Time.deltaTime;
      normalizedValueColor = currentTimeColor / timeOfTravelColor;

      localBg.color = Color.Lerp(localBg.color, targetBg, normalizedValueColor);
      lanBg.color = Color.Lerp(lanBg.color, targetBg, normalizedValueColor);

      localTx.color = Color.Lerp(localTx.color, targetTx, normalizedValueColor);
      lanTx.color = Color.Lerp(lanTx.color, targetTx, normalizedValueColor);

      yield return null;
    }
    puedeFadearOut = false;
    local.SetActive(false);
    lan.SetActive(false);
    //Debug.Log("Termino Fade-Out Jugar");
  }
}