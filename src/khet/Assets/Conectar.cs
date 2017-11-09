using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Conectar : MonoBehaviour {
	[SerializeField] private Text text;
	[SerializeField] private HUDController controller;

	public void OnClick() {
		if (!IsValidIP(text.text)) return;

		controller.Connect(text.text);
	}

	private bool IsValidIP(string ip) {
		
	}
}
