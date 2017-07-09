﻿using System.Collections.Generic;

public class Scarab : BasePiece {
  public Scarab(int x, int y, int rotation, PieceColor color, Board board) 
    : base(new Point(x, y), rotation, color, board) { }

  public Scarab(Point position, int rotation, PieceColor color, Board board) : base(position, rotation, color, board) { } 

  public Scarab(int x, int y, PieceColor color, Board board) : base(new Point(x, y), color, board) { }
  public Scarab(Point position, PieceColor color, Board board) : base(position, color, board) { }

  public override Point[] GetAvailablePositions() { 
    GamePiece[,] pieces = Board.GetAdjacent(this);
    List<Point> ret = new List<Point>();

    foreach (GamePiece gp in pieces) {
      if (gp != null && (!(gp is Scarab) || !(gp is Sphynx) || !(gp is Pharaoh))) {
        if (gp is EmptyPoint) {
          if (Board.UnderlineEqualsColor(this, gp.Position)) ret.Add(gp.Position);
        }
      }
    }

    return ret.ToArray();
  }

  public override int[] GetAvailableRotations() {
    return new int[] { -1, 0, 1 };
  }

  public override void MakeMove(Point finalPosition) {
    List<Point> positions = new List<Point>(GetAvailablePositions());
    if (!positions.Contains(finalPosition)) return;

    GamePiece piece = Board.GetPieceAt(finalPosition);

    if (!(piece is EmptyPoint))
      piece.MakeMove(position);

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
