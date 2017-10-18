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

  public override bool WillDie(Transform transform, Vector3 point, Vector3 normal) {
    point = transform.InverseTransformPoint(point);

    int norm = Mathf.RoundToInt(normal.x), 
    right = Mathf.RoundToInt(transform.right.x),
    forward = Mathf.RoundToInt(transform.forward.x);

    if (norm == right) {
      normal = Quaternion.AngleAxis(-90, Vector3.up) * normal;
      point = Quaternion.AngleAxis(-90, Vector3.up) * point;
    } else if (norm == forward) {
      normal = Quaternion.AngleAxis(90, Vector3.up) * normal;
      point = Quaternion.AngleAxis(90, Vector3.up) * point;
    } else {
      Die();
      return true;
    }

    normal.y = 0;

    LaserController.AddPosition(transform.TransformPoint(point), normal);
    return false;
  }
}