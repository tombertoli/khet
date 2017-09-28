﻿using UnityEngine;
using System.Collections.Generic;

public class Pharaoh : BasePiece {
  public Pharaoh(Point position, Quaternion rotation, PieceColor color, Board board)
    : base(position, rotation, color, board, PieceTypes.Pharaoh) { } 

  public override Quaternion[] GetAvailableRotations() {
    Quaternion left = Quaternion.Euler(0, rotation.eulerAngles.y - 90, 0);
    Quaternion right = Quaternion.Euler (0, rotation.eulerAngles.y + 90, 0);

    return new Quaternion[] { left, right };
  }

  public override bool HandleLaser(Transform transform, ref Vector3 point, ref Vector3 normal) {
    Die();
    return true;
  }
}
