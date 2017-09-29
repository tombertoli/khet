using UnityEngine;
using System.IO;
using System.Collections;

public class TemplateManager : MonoBehaviour {
	[SerializeField] private TextAsset text;
	private static TemplateManager reference;
	
	void Awake() {
		reference = this;
    CreateFiles();
	}

	public static void CreateFiles() {
		string file = BoardTemplates.defPath + @"\classic.kbt";

    if (!File.Exists(file)) { 
      Directory.CreateDirectory(BoardTemplates.defPath);
      File.Create(file);
      File.AppendAllText(file, reference.text.text);
    }
	}
}
