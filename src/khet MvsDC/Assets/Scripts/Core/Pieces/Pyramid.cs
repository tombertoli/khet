using UnityEngine;
using System.Collections.Generic;

public class Pyramid : BasePiece {
  public Pyramid(Point position, int rotation, PieceColor color, Board board)
    : base(position, rotation, color, board, PieceTypes.Pyramid) { } 
  
  public override int[] GetAvailableRotations() {
    return new int[] { -1, 0, 1 };
  }

  public override bool HandleLaser(Transform transform, ref Vector3 point, ref Vector3 normal) {
    if (normal == -transform.right)
      normal = Quaternion.Euler(0, 90, 0) * normal;
    else if (normal == transform.forward)
      normal = Quaternion.Euler(0, -90, 0) * normal;
    else {
      Die();
      return true;
    }

    point.x = point.z = 0;

    LaserPointer.AddPosition(transform.TransformPoint(point), normal);
    return false;
  }
}