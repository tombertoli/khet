using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Collider), typeof(Renderer))]
public class Glow : MonoBehaviour {
  [SerializeField] private Material outlineMaterial;
  private Material baseMaterial;
  private Renderer r;
  private bool permanent, above;

  void Start() {
    r = GetComponent<Renderer>();
    baseMaterial = r.material;
  }

  void OnMouseEnter() {    
    r.material = outlineMaterial;
    above = true;
  }

  void OnMouseOver() {
    if (Input.GetMouseButtonDown(0)) permanent = !permanent;
  }

  void OnMouseExit() {
    if (!permanent)
      r.material = baseMaterial;

    above = false;
  }
}
