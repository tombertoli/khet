﻿using UnityEngine;
using System.Collections.Generic;

public class Anubis : BasePiece {
  public Anubis(Point position, int rotation, PieceColor color, Board board)
    : base(position, rotation, color, board, PieceTypes.Anubis) { } 

  public override Point[] GetAvailablePositions() { 
    GamePiece[,] pieces = Board.GetAdjacent(this);
    List<Point> ret = new List<Point>();

    for (int i = 0; i < pieces.GetLength(0); i++) {
      for(int j = 0; j < pieces.GetLength(1); j++) {
        GamePiece gp = pieces[i, j];

        if (gp.PieceType == PieceTypes.Empty) {
          if (Board.UnderlineEqualsColor(this, gp.Position)) ret.Add(gp.Position);
        }
      }
    }

    return ret.ToArray();
  }

  public override int[] GetAvailableRotations() {
    return new int[] { -1, 0, 1 };
  }

  public override bool HandleLaser(Transform transform, ref Vector3 point, ref Vector3 normal) {
    if (normal != transform.forward) return false;

    Die();
    return true;
  }

  public override void MakeMove(Point finalPosition) {
    List<Point> positions = new List<Point>(GetAvailablePositions());
    if (!positions.Contains(finalPosition)) return;

    Board.MovePiece(this, finalPosition);
    position = finalPosition;
  }

  public override void Rotate(int rot) {
    List<int> rotations = new List<int>(GetAvailableRotations());
    if (!rotations.Contains(rot)) return;

    if ((rotation == 3 && rot == 1) || (rotation == 1 && rot == -1)) rotation = 0;
    else rotation += rot;
  }
}
