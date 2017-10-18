using UnityEngine;
using System.Collections.Generic;

public class Sphynx : BasePiece {
  public Sphynx(Point position, Quaternion rotation, PieceColor color, Board board)
    : base(position, rotation, color, board, PieceTypes.Sphynx) { } 

  public override Point[] GetAvailablePositions() { return null; }
  public override Quaternion[] GetAvailableRotations() {
    if (rotation == Quaternion.Euler(Vector3.zero) || rotation == Quaternion.Euler(0, 180, 0))
      return new[] { Quaternion.Euler(0, rotation.eulerAngles.y + 90, 0) };
    else if (rotation == Quaternion.Euler(0, 90, 0) || rotation == Quaternion.Euler(0, 270, 0))
      return new[] { Quaternion.Euler(0, rotation.eulerAngles.y - 90, 0) };

    Debug.LogError("Rotacion sphynx mal");
    return new[] { Quaternion.identity };
  }

  public override int[] GetAvailableRotationsInInt() {
    if (rotation == Quaternion.Euler(Vector3.zero) || rotation == Quaternion.Euler(0, 180, 0))
      return new[] { 1 };
    else if (rotation == Quaternion.Euler(0, 90, 0) || rotation == Quaternion.Euler(0, 270, 0))
      return new[] { -1 };
    
    Debug.LogError("Rotacion sphynx mal");
    return new[] { 0 };
  }

  public override bool WillDie(Transform transform, Vector3 point, Vector3 normal) {
    return false; 
  }
}

