using UnityEngine;
using UnityEngine.UI;
using System.Net;
using System.Collections;

public class Conectar : MonoBehaviour {
	[SerializeField] private Text text;
	[SerializeField] private ConnectionController controller;

	public void OnClick() {
		if (controller == null) controller = GameObject.FindObjectOfType<ConnectionController>();

    if (!IsValidIP(text.text) && text.text != "localhost") {
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
