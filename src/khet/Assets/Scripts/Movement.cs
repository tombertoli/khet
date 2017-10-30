using UnityEngine;

public class Movement : MonoBehaviour {
  public IPiece piece;
  public Point point;
  public static bool mouseAbove;

  void OnMouseEnter() { mouseAbove = true; }
  void OnMouseExit() { mouseAbove = false; }
  void OnDestroy() { OnMouseExit(); }

  void OnMouseOver() {
    if (!Input.GetButtonDown("Fire1")) return;
    piece.Move(true, point);
  }
}