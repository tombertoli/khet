using UnityEngine;

public class Movement : MonoBehaviour {
  public GamePiece piece;
  public Point point;
  public static bool mouseAbove;

  void OnMouseEnter() { mouseAbove = true; }
  void OnMouseExit() { mouseAbove = false; }

  void OnMouseOver() {
    if (!Input.GetButtonDown("Fire1")) return;

    Debug.Log(point);
    piece.MakeMove(point);
  }
}