using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Collider), typeof(Renderer))]
public class Glow : MonoBehaviour {
  [SerializeField] private string key, button = "Fire1";
  [SerializeField] private float outlineWidth;
  private PieceSetup setup;
  private Renderer r;
  private bool permanent, over;

  void Start() {
    TurnManager.OnTurnFinished += () => {
      ToggleOutline(false);
      permanent = false;
      setup.Piece.IsSelected = false;
    };

    r = GetComponent<Renderer>();
    setup = GetComponent<PieceSetup>();
  }

  void Update() {
    if (setup.Piece.Color != TurnManager.turn) return;

    if (over) {
      if (Input.GetButtonDown(button))
        permanent = !permanent;

      if (!permanent) {
        ToggleOutline(true);
      }
    } else {
      if (Input.GetButtonDown(button))
        permanent = false;

      if (!permanent) ToggleOutline(false);
    }
  }

  void OnMouseEnter() { over = true; }
  void OnMouseExit() { over = false; }
  
  private void ToggleOutline(bool state) {
    r.material.SetFloat(key, state ? outlineWidth : 0f);
  }
}
