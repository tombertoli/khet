using UnityEngine;
using System.IO;
using System.Collections;

public class TemplateManager : MonoBehaviour {
	[SerializeField] private TextAsset text;
	private static TemplateManager instance;
	
	void Awake() {
		instance = this;
    CreateFiles();
	}

	public static void CreateFiles() {
		string file = BoardTemplates.defPath + @"\classic.kbt";

    if (File.Exists(file) && File.ReadAllLines(file) != new string[] { }) return; 
		Directory.CreateDirectory(BoardTemplates.defPath);

		using (StreamWriter sw = new StreamWriter(File.Create(file)))
			sw.Write(instance.text.text);
	}
}
