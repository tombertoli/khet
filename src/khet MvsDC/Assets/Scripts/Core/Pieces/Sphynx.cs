using UnityEngine;
using System.Collections.Generic;

public class Sphynx : BasePiece {
  public Sphynx(Point position, Quaternion rotation, PieceColor color, Board board)
    : base(position, rotation, color, board, PieceTypes.Sphynx) { } 

  public override Point[] GetAvailablePositions() { return null; }
  public override Quaternion[] GetAvailableRotations() {
    if (rotation == Quaternion.Euler(Vector3.zero))
      return new Quaternion[] { Quaternion.Euler(0, rotation.eulerAngles.y + 90, 0) };
    else if (rotation == Quaternion.Euler(0, 90, 0))
      return new Quaternion[] { Quaternion.Euler(0, rotation.eulerAngles.y - 90, 0) };

    Debug.LogError("Rotacion sphynx mal");
    return new Quaternion[] { Quaternion.identity };
  }

  public override bool HandleLaser(Transform transform, ref Vector3 point, ref Vector3 normal) {
    return false; 
  }
}

