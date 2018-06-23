using UnityEngine;
using System.Collections;

public class Host : MonoBehaviour {
	[SerializeField] private ConnectionController controller;

	public void OnClick() {
		if (controller == null) controller = GameObject.FindObjectOfType<ConnectionController>();

		controller.Host();
	}
}
