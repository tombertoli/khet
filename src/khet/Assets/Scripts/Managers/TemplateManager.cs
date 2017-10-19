using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class TemplateManager : MonoBehaviour {
	[SerializeField] private TextAsset[] files;
	private static TemplateManager instance;
	
	void Awake() {
		DontDestroyOnLoad(this);
		instance = this;

		CheckFiles();
	}

	private static bool CheckIntegrity(TextAsset asset) {
		string path = BoardTemplates.defPath + string.Format(@"\{0}.kbt", asset.name);

		if (File.Exists(path) && File.ReadAllLines(path) != new string[] { }) return true;
		return false;
	}

	public static void CreateFile(TextAsset asset) {
		string path = BoardTemplates.defPath + string.Format(@"\{0}.kbt", asset.name);
    
		using (StreamWriter sw = new StreamWriter(File.Create(path)))
			sw.Write(asset.text);
	}

	public static void CheckFiles() {
		if (!Directory.Exists(BoardTemplates.defPath)) Directory.CreateDirectory(BoardTemplates.defPath);

		for (int i = 0; i < instance.files.Length; i++) {
			if (!CheckIntegrity(instance.files[i])) CreateFile(instance.files[i]);
		}
	}
}
