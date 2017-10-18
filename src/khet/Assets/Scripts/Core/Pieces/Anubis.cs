using UnityEngine;
using System.Collections.Generic;

public class Anubis : BasePiece {
  public Anubis(Point position, Quaternion rotation, PieceColor color, Board board)
    : base(position, rotation, color, board, PieceTypes.Anubis) { } 

  public override Quaternion[] GetAvailableRotations() {
    Quaternion left = Quaternion.Euler(0, rotation.eulerAngles.y - 90, 0);
    Quaternion right = Quaternion.Euler (0, rotation.eulerAngles.y + 90, 0);

    return new Quaternion[] { left, right };
  }

  public override int[] GetAvailableRotationsInInt() {
    return new[] { 1, -1 };
  }

  public override bool WillDie(Transform transform, Vector3 point, Vector3 normal) {
    if (normal == transform.forward) return false;

    Die();
    return true;
  }
}
