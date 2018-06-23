using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Instrucciones : MonoBehaviour 
{
	RectTransform rectTransform;
	Vector2 startingPosition;// = new Vector2 (0.411f, 0.4731066f); 
	Vector2 endPosition;// = new Vector2(0.22f, 0.4731066f);
	float timeOfTravel=1; //time after object reach a target place 
	float currentTime=0; // actual floting time 
	float normalizedValue;

	private static readonly string defPath = Environment.ExpandEnvironmentVariables(@"%USERPROFILE%\Documents\My Games\khet\Instructions");
	[SerializeField] private Texture2D[] pics;

	void Start()
	{
		rectTransform = gameObject.GetComponent<RectTransform> ();
		startingPosition = rectTransform.anchorMin;
    endPosition = new Vector2(rectTransform.anchorMin.x - .191f, rectTransform.anchorMin.y);

		CheckFiles();
	}

  public void Redirect() 
	{			
    	Application.OpenURL(defPath + @"\Instrucciones.html");
	}

	public void CreateFile(TextAsset asset) {
		string path = defPath + string.Format(@"\{0}.html", asset.name);
    
		using (StreamWriter sw = new StreamWriter(File.Create(path)))
			sw.Write(asset.text);
	}

	public void CreateImage(string name, byte[] asset) {
		string path = defPath + string.Format(@"\{0}.png", name);
    
		File.WriteAllBytes(path, asset);
	}

	public void CheckFiles() {
		if (!Directory.Exists(defPath)) Directory.CreateDirectory(defPath);

		var ta = Resources.Load<TextAsset>("Instrucciones/Instrucciones");
		CreateFile(ta);

		for (int i = 0; i < pics.Length; i++) {	
			CreateImage(pics[i].name, pics[i].EncodeToPNG());
		}
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