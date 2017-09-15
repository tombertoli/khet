using UnityEngine;
using System.Collections.Generic;

public class Anubis : BasePiece {
  public Anubis(Point position, int rotation, PieceColor color, Board board)
    : base(position, rotation, color, board, PieceTypes.Anubis) { } 

  public override int[] GetAvailableRotations() {
    return new int[] { -1, 0, 1 };
  }

  public override bool HandleLaser(Transform transform, ref Vector3 point, ref Vector3 normal) {
    if (normal == transform.forward) return false;

    Die();
    return true;
  }
}
