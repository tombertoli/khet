using UnityEngine;

public class EmptyPiece : GamePiece {
  public Board Board { get { return null; } set { Board = null; } }
  public Point Position { get { return position; } }
  public int Rotation { get { return 0; } }
  public PieceTypes PieceType { get { return PieceTypes.None; } }
  public PieceColor Color { get { return PieceColor.None; } }
  public bool IsSelected { get { return false; } }  

  private Point position;

  public EmptyPiece(int x, int y) : this(new Point(x, y)) {}
  public EmptyPiece(Point position) {
    this.position = position;
  }

  public Point[] GetAvailablePositions() { return null; }
  public int[] GetAvailableRotations() { return new int[] { 0 }; }

  public void MakeMove(Point finalPosition) { 
    Debug.LogError("Porque se llama MakeMove en una EmptyPiece?");
    return; 
  }
}