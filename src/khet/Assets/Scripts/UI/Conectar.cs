using UnityEngine;
using UnityEngine.UI;
using System.Net;
using System.Collections;

public class Conectar : MonoBehaviour {
	[SerializeField] private Text text;
	[SerializeField] private HUDController controller;

	public void OnClick() {
		if (!IsValidIP(text.text)) {
			Debug.Log("invalid ip");
			return;
		}

		controller.Connect(text.text);
	}

	private bool IsValidIP(string ip) {
		IPAddress address;
		return IPAddress.TryParse(ip, out address);
	}
}
