using UnityEngine;

public class Sphynx : BasePiece {
  public Sphynx(int x, int y, int rotation, PieceColor color, Board board) 
    : base(new Point(x, y), rotation, color, board) { }
  
  public Sphynx(Point position, int rotation, PieceColor color, Board board)
    : base(position, rotation, color, board) { } 

  public Sphynx(int x, int y, PieceColor color, Board board) : base(new Point(x, y), color, board) { }
  public Sphynx(Point position, PieceColor color, Board board) : base(position, color, board) { }

  public override Point[] GetAvailablePositions() { return null; }
  public override int[] GetAvailableRotations() {
    if (rotation == 0) return new int[] { 1 };
    else if (rotation == 1) return new int[] { -1 };

    return new int[] { 0 };
  }

  public override void MakeMove(Point finalPosition) { 
    Debug.LogError("Porque se llama MakeMove en un Sphynx?");
    return; 
  }
}

