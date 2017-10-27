using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextManager : MonoBehaviour {
	[SerializeField] private Text winText;
	[SerializeField] private Graphic[] image;
	
	private static float timeInSeconds = 2;
	private static TextManager instance;

	void Start () {
		instance = this;
	}

	public static void EndGame(PieceColor won) {
		string color = won == PieceColor.Red ? "Rojo" : "Gris";
		instance.winText.text = "¡" + color + instance.winText.text;

		instance.winText.transform.parent.gameObject.SetActive(true);

		instance.StartCoroutine(FadeColor(instance.winText, instance.winText.color));
		
		for (int i = 0; i < instance.image.Length; i++)
			instance.StartCoroutine(FadeColor(instance.image[i], instance.image[i].color));
	}

	private static IEnumerator FadeColor(Graphic graphic, Color finalColor) {
		graphic.color = new Color(0,0,0,0);
		float currentTime = 0, normalizedValueColor = 0;

		while (currentTime <= timeInSeconds) {
      currentTime += Time.deltaTime;
      normalizedValueColor = currentTime / timeInSeconds;
			
			graphic.color = Color.Lerp(graphic.color, finalColor, normalizedValueColor);
      yield return null;
    }
	}
}
