using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Collider), typeof(Renderer))]
public class Glow : MonoBehaviour {
  [SerializeField] private Material outlineMaterial;
  [SerializeField] private GameObject go;
  private Material baseMaterial;
  private Renderer r;
  private bool permanent, above;

  void Start() {
    r = GetComponent<Renderer>();
    baseMaterial = r.material;

    Board b = BoardTemplates.LoadClassic();

    for (int i = 0; i < b.Width; i++) {
      for (int j = 0; j < b.Height; j++) {
        GameObject ga = (GameObject)Instantiate(go, new Vector3(b.GetPieceAt(i, j).Position.x, go.transform.position.y, b.GetPieceAt(i, j).Position.y), Quaternion.identity);
        ga.transform.parent = transform.parent;
      }
    }
  }

  void Update() {
    if (above && Input.GetMouseButtonDown(0)) permanent = !permanent;
  }

  void OnMouseEnter() {
    r.material = outlineMaterial;
    above = true;
  }

  void OnMouseExit() {
    if (!permanent)
      r.material = baseMaterial;

    above = false;
  }
}
