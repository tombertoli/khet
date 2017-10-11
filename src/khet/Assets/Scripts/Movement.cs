using UnityEngine;

public class Movement : MonoBehaviour {
  public IGamePiece piece;
  public Point point;
  public static bool mouseAbove;

  void OnMouseEnter() { mouseAbove = true; }
  void OnMouseExit() { mouseAbove = false; }
  void OnDestroy() { OnMouseExit(); }

  void OnMouseOver() {
    if (!Input.GetButtonDown("Fire1")) return;
    piece.MakeMove(true, point);
  }
}