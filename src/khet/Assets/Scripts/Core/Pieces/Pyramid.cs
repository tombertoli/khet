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
    int norm    = Mathf.RoundToInt(normal.x), 
        right   = Mathf.RoundToInt(transform.right.x),
        forward = Mathf.RoundToInt(transform.forward.x);

    Vector3 temp = point;

    if (norm == right) {
      normal = Quaternion.AngleAxis(-90, Vector3.up) * normal;
      point = Quaternion.AngleAxis(-90, Vector3.up) * point;

      temp.x = 0;
    } else if (norm == forward) {
      normal = Quaternion.AngleAxis(90, Vector3.up) * normal;
      point = Quaternion.AngleAxis(90, Vector3.up) * point;

      temp.z = 0;
    } else {
      Die();
      return true;
    }   
    
    normal.y = 0;
    LaserController.AddPositionDirty(transform.TransformPoint(temp));
    LaserController.AddPosition(transform.TransformPoint(point), normal);   
    return false;
  }
}