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
    int normx    = Mathf.RoundToInt(normal.x),            normz    = Mathf.RoundToInt(normal.z),
        rightx   = Mathf.RoundToInt(-transform.right.x),  rightz   = Mathf.RoundToInt(-transform.right.z),
        forwardx = Mathf.RoundToInt(transform.forward.x), forwardz = Mathf.RoundToInt(transform.forward.z);

    Vector3 temp = point;

    if (normx == rightx && normz == rightz) {
      normal = Quaternion.AngleAxis(90, Vector3.up) * normal;
      temp.x = 0;
    } else if (normx == forwardx && normz == forwardz) {
      normal = Quaternion.AngleAxis(-90, Vector3.up) * normal;
      temp.z = 0;
    } else {
      Die();
      return true;
    }   
    
    normal.y = 0;
    LaserController.AddPosition(transform.TransformPoint(0, point.y, 0), normal);
    return false;
  }
}