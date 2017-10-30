using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class volver : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Clickea1000()
	{
		SceneManager.LoadScene ("MainMenu");
	}
}
