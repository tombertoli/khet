using UnityEngine;
using System;
using System.Collections;

public class PostRender : MonoBehaviour {
  public static event Action DrawOnPostRender;

	void OnPostRender() {
    if (DrawOnPostRender == null) return;
    DrawOnPostRender();
	}

	void OnDrawGizmos() {
    if (DrawOnPostRender == null) return;
    DrawOnPostRender();
	}	
}
