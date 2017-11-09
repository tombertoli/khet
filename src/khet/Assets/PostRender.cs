using UnityEngine;
using System.Collections;

public class PostRender : MonoBehaviour {
	[SerializeField] private Material material;

	// Use this for initialization
	void OnPostRender() {
		if (!LaserController.fire) return;
		DrawLines();
	}

	void OnDrawGizmos() {
		if (!LaserController.fire) return;
		DrawLines();
	}

	private void DrawLines() {
    for (int i = 0; i < LaserController.points.Count - 1; i++) {      
      GL.Begin(GL.LINES);      
      material.SetPass(0);
      GL.Color(material.color);
      GL.Vertex(LaserController.points[i]);
      GL.Vertex(LaserController.points[i + 1]);
      GL.End();
    }
  }
}
