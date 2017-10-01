﻿using UnityEngine;
using System.Collections.Generic;

public class Sphynx : BasePiece {
  public Sphynx(Point position, int rotation, PieceColor color, Board board)
    : base(position, rotation, color, board, PieceTypes.Sphynx) { } 

  public override Point[] GetAvailablePositions() { return null; }
  public override int[] GetAvailableRotations() {
    if (rotation == 0) return new int[] { 1 };
    else if (rotation == 1) return new int[] { -1 };

    return new int[] { 0 };
  }

  public override bool HandleLaser(Transform transform, ref Vector3 point, ref Vector3 normal) {
    return false; 
  }
}

