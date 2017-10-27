using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Instrucciones : MonoBehaviour 
{
	RectTransform rectTransform;
	Vector2 startingPosition = new Vector2 (0.411f, 0.4731066f); 
	Vector2 endPosition = new Vector2(0.22f, 0.4731066f);
	float timeOfTravel=1; //time after object reach a target place 
	float currentTime=0; // actual floting time 
	float normalizedValue;

	void Start()
	{
		rectTransform = gameObject.GetComponent<RectTransform> ();
	}

  	void Redirect(string url) 
	{
    	Application.OpenURL(url);
	}

	public void OLA()
	{
		StartCoroutine (Mover ());
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
	}
}