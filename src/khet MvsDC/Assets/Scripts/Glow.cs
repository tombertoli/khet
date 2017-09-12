using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Collider), typeof(Renderer))]
public class Glow : MonoBehaviour {
  [SerializeField] private string key;
  [SerializeField] private float outlineWidth;
  private Renderer r;
  private static bool permanent;

  void Start() {
    r = GetComponent<Renderer>();
  }

  void OnMouseEnter() {    
    if (!permanent) ToggleOutline(true);
    Debug.Log(permanent);
  }

  void OnMouseOver() {
    if (Input.GetMouseButtonDown(0)) permanent = !permanent;
    Debug.Log(permanent);
  }

  void OnMouseExit() {
    if (!permanent) ToggleOutline(false);
    Debug.Log(permanent);
  }

  private void ToggleOutline(bool state) {
    r.material.SetFloat(key, state ? outlineWidth : 0f);
  }
}
