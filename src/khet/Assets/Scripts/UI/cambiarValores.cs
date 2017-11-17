using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;

public class cambiarValores : MonoBehaviour {
	[SerializeField] private Text text;

	// Use this for initialization
	void Start () {
		Dropdown dp = GetComponent<Dropdown>();
		dp.ClearOptions();
		string[] files = Directory.GetFiles (BoardTemplates.defPath, "*.kbt");

		for (int i = 0; i < files.Length; i++) {
			if (!files [i].Contains (BoardTemplates.defPath))
				continue;
			
			files [i] = files [i].Substring (BoardTemplates.defPath.Length);
			files [i] = files [i].Substring (1, files [i].Length - 5);
		}
		dp.AddOptions (new List<string> (files));
		TemplateManager.SetBoard (text.text);	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetBoard() {
		TemplateManager.SetBoard(text.text);
	}
}
