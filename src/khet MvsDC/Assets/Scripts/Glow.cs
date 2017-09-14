using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Collider), typeof(Renderer))]
public class Glow : MonoBehaviour {
  [SerializeField] private string key, button = "Fire1";
  [SerializeField] private float outlineWidth;
  private Renderer r;
  private bool permanent;
  private bool over;

  void Start() {
    r = GetComponent<Renderer>();
  }

  void Update() {
    if (over) {
      if (Input.GetButtonDown (button))
        permanent = !permanent;

      if (!permanent) ToggleOutline (true);
    } else {
      if (Input.GetButtonDown (button)) 
        permanent = false;

      if (!permanent) ToggleOutline (false);
    }      
  }

  void OnMouseEnter() { over = true; }
  void OnMouseExit() { over = false; }

  private void ToggleOutline(bool state) {
    r.material.SetFloat(key, state ? outlineWidth : 0f);
  }
}
