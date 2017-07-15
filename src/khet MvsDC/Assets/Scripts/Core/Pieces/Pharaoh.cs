using UnityEngine;
using System.Collections.Generic;

public class Pharaoh : BasePiece {
  public Pharaoh(Point position, int rotation, PieceColor color, Board board)
    : base(position, rotation, color, board, PieceTypes.Pharaoh) { } 

  public override int[] GetAvailableRotations() {
    return new int[] { -1, 0, 1 };
  }

  public override bool HandleLaser(Transform transform, ref Vector3 point, ref Vector3 normal) {
    Die();
    return true;
  }
}
