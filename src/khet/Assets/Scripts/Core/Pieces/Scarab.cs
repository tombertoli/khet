using UnityEngine;
using System.Collections.Generic;

public class Scarab : BasePiece {
  public Scarab(Point position, Quaternion rotation, PieceColor color, Board board)
    : base(position, rotation, color, board, PieceTypes.Scarab) { } 
  
  public override Point[] GetAvailablePositions() { 
    IPiece[,] pieces = Board.GetAdjacent(this);
    List<Point> ret = new List<Point>();

    foreach (IPiece gp in pieces) {
      if (gp == null) continue;
      
      switch (gp.Type) {
        case PieceTypes.Anubis:
        case PieceTypes.Pyramid:
        case PieceTypes.Empty:
          if (Board.UnderlineEqualsColor(this, gp.Position)) 
            ret.Add(gp.Position);
          break;
        default:
          continue;
      }
    }

    return ret.ToArray();
  }

  public override Quaternion[] GetAvailableRotations() {
    Quaternion left = Quaternion.Euler(0, rotation.eulerAngles.y - 90, 0);
    Quaternion right = Quaternion.Euler (0, rotation.eulerAngles.y + 90, 0);

    return new Quaternion[] { left, right };
  }

  public override int[] GetAvailableRotationsInInt() {
    return new[] { 1, -1 };
  }

  public override bool WillDie(Transform transform, Vector3 point, Vector3 normal) {      
    Vector3 temp = point;

    if (normal == -transform.forward || normal == transform.forward) {
      normal = Quaternion.AngleAxis(-90, Vector3.up) * normal;
      temp.z = 0;
    } else if (normal == transform.right || normal == -transform.right) {
      normal = Quaternion.AngleAxis(90, Vector3.up) * normal;
      temp.x = 0;
    }

    normal.y = 0;
    LaserController.AddPosition(transform.TransformPoint(Vector3.zero), normal);
    return false;
  }
}
