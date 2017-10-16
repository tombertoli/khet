using UnityEngine;
using System.Collections.Generic;

public class Pyramid : BasePiece {
  public Pyramid(Point position, Quaternion rotation, PieceColor color, Board board)
    : base(position, rotation, color, board, PieceTypes.Pyramid) { } 
  
  public override Quaternion[] GetAvailableRotations() {
    Quaternion left = Quaternion.Euler(0, rotation.eulerAngles.y - 90, 0);
    Quaternion right = Quaternion.Euler (0, rotation.eulerAngles.y + 90, 0);

    return new Quaternion[] { left, right };
  }

  public override int[] GetAvailableRotationsInInt() {
    return new[] { 1, -1 };
  }

  public override bool WillDie(Transform transform, ref Vector3 point, ref Vector3 normal) {
    if (normal == -transform.right)
      normal = Quaternion.Euler(0, 90, 0) * normal;
    else if (normal == transform.forward)
      normal = Quaternion.Euler(0, -90, 0) * normal;
    else {
      Die();
      return true;
    }

    point.x = point.z = 0;

    LaserController.AddPosition(transform.TransformPoint(point), normal);
    return false;
  }
}