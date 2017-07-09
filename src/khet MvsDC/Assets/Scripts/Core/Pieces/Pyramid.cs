using UnityEngine;

public class Pyramid : BasePiece {
  public Pyramid(int x, int y, int rotation, PieceColor color, Board board) 
    : base(new Point(x, y), rotation, color, board) {}

  public Pyramid(Point position, int rotation, PieceColor color, Board board) : base(position, rotation, color, board) { } 

  public Pyramid(int x, int y, PieceColor color, Board board) : base(new Point(x, y), color, board) {}
  public Pyramid(Point position, PieceColor color, Board board) : base(position, color, board) { }

  public override Point[] GetAvailablePositions() { 
    GamePiece[,] ad = Board.GetAdjacent(this);
    Debug.Log(ad);
    return new Point[] { new Point(1,2) };
  }

  public override int[] GetAvailableRotations() {
    return new int[] { -1, 0, 1 };
  }

  public override void MakeMove(Point finalPosition) { 
    Board.MovePiece(this, finalPosition);
  }
}