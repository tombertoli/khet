using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Connect : MonoBehaviour {
	[SerializeField] private InputField iField;
	[SerializeField] private HUDController controller;
	private Button button;

	// Use this for initialization
	void Start () {
		button = GetComponent<Button>();
		button.onClick.AddListener(() => controller.Connect(iField.text));
		button = null;
	}
}
