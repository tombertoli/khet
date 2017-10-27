using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Creditos : MonoBehaviour
{
  RectTransform rectTransform;
  Vector2 startingPosition = new Vector2(0.411f, 0.2838527f);
  Vector2 endPosition = new Vector2(0.22f, 0.2838527f);
  float timeOfTravel = 1; //time after object reach a target place 
  float currentTime = 0; // actual time 
  float normalizedValue;

  void Start()
  {
    rectTransform = gameObject.GetComponent<RectTransform>();
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