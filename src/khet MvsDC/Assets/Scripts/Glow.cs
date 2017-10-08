using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Collider), typeof(Renderer))]
public class Glow : MonoBehaviour {
  [SerializeField] private string key, button = "Fire1", pieceTag = "Piece";
  [SerializeField] private float outlineWidth;

  private PieceSetup setup;
  private Renderer r;
  private bool permanent, over;

  void Start() {
    TurnManager.TurnFinished += () => DeselectRange(GameObject.FindGameObjectsWithTag(pieceTag));

    r = GetComponent<Renderer>();
    setup = GetComponent<PieceSetup>();
  }

  void Update() {
    if (setup.Piece.Color != TurnManager.GetTurn()) return;

    if (Input.GetButtonDown(button))
      permanent = over ? !permanent : false;

    if (!permanent) SetOutline(over ? true : false);
  }

  void OnMouseEnter() { over = true; }
  void OnMouseExit() { over = false; }

  public static void DeselectRange(GameObject[] gos) {
    for (int i = 0; i < gos.Length; i++) {
      Glow glow = gos[i].GetComponent<Glow>();

      if (glow == null) {
        Debug.Log("nulleadisimo");
        continue;
      }

      glow.SetOutline(false);
    }
  }
  
  public void SetOutline(bool state) {
    r.material.SetFloat(key, state ? outlineWidth : 0f);
  }
}
