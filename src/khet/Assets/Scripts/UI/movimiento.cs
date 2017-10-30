using UnityEngine;
using System.Collections;

public class movimiento : MonoBehaviour 
{

	// Update is called once per frame
	void Update () 
	{
		transform.Translate(0, 0.01f, 0);
	}
}