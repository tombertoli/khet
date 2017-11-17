using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class comenzarJuego : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Comenzar()
	{
		SceneManager.LoadScene ("Game");
	}
}
