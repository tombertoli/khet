using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent (typeof(Image))]
public class SonidoToggle : MonoBehaviour {
	public static bool Enabled { get; set; }
	private Image image;
	[SerializeField] private AudioSource source;
	[SerializeField] private Sprite audioEnabled, audioDisabled;

	// Use this for initialization
	void Start () {
		image = GetComponent<Image>();
		source.mute = Enabled;
		image.sprite = !Enabled ? audioEnabled : audioDisabled;
	}

	public void Toggle() {
		Enabled = !Enabled;
		source.mute = Enabled;
		image.sprite = !Enabled ? audioEnabled : audioDisabled;
	}
}
