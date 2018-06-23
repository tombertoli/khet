using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Creditos : MonoBehaviour
{
  RectTransform rectTransform;
  Vector2 startingPosition;
  Vector2 endPosition;
  float timeOfTravel = 1; //time after object reach a target place 
  float currentTime = 0; // actual time 
  float normalizedValue;

  void Start()
  {
    rectTransform = gameObject.GetComponent<RectTransform>();

    startingPosition = rectTransform.anchorMin;
    endPosition = new Vector2(rectTransform.anchorMin.x - .191f, rectTransform.anchorMin.y);
  }

  public void abrirCreditos()
  {
    SceneManager.LoadScene("Credits");
  }

  public void OLA()
  {
    StartCoroutine(Mover());
  }

  IEnumerator Mover()
  {
    while (currentTime <= timeOfTravel)
    {
      currentTime += Time.deltaTime;
      normalizedValue = currentTime / timeOfTravel;

      rectTransform.anchorMin = Vector3.Lerp(startingPosition, endPosition, normalizedValue);
      yield return null;
    }

  }
}