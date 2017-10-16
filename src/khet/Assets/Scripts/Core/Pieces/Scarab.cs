using UnityEngine;
using System.Collections.Generic;

public class Scarab : BasePiece {
  public Scarab(Point position, Quaternion rotation, PieceColor color, Board board)
    : base(position, rotation, color, board, PieceTypes.Scarab) { } 
  
  public override Point[] GetAvailablePositions() { 
    IGamePiece[,] pieces = Board.GetAdjacent(this);
    List<Point> ret = new List<Point>();

    foreach (IGamePiece gp in pieces) {
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

  public override bool WillDie(Transform transform, ref Vector3 point, ref Vector3 normal) {
    int norm = Mathf.RoundToInt(normal.x),
      forward = Mathf.RoundToInt(transform.forward.x), back = forward,
      right = Mathf.RoundToInt(transform.right.x), left = -right;
      
	  if (normal == -transform.forward || normal == transform.forward)
      normal = Quaternion.Euler(0, -90, 0) * normal;
	  else if (normal == transform.right || normal == -transform.right)
      normal = Quaternion.Euler(0, 90, 0) * normal;

    point.x = point.z = 0;

    LaserController.AddPosition(transform.TransformPoint(point), normal);
    return false;
  }
}
