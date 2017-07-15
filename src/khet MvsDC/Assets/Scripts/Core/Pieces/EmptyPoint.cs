using UnityEngine;

public class EmptyPoint : GamePiece {
  public Board Board { get { return null; } set { Board = null; } }
  public Point Position { get { return position; } }
  public int Rotation { get { return 0; } }
  public PieceTypes PieceType { get { return PieceTypes.Empty; } }
  public PieceColor Color { get { return PieceColor.None; } }
  public bool IsSelected { get { return false; } set { IsSelected = false; } }
  public static Vector3 transPos { get; set; }

  private Point position;

  public EmptyPoint(int x, int y) : this(new Point(x, y)) {}
  public EmptyPoint(Point position) {
    this.position = position;
  }

  public Point[] GetAvailablePositions() { return null; }
  public int[] GetAvailableRotations() { return new int[] { 0 }; }

  public bool HandleLaser(Transform transform, ref Vector3 point, ref Vector3 normal) { 
    throw new System.NotImplementedException("An empty can't handle a laser");
  }

  public Vector3 GetPositionInWorld() {
    return new Vector3(
      position.x + transPos.x,
      transPos.y,
      position.y + transPos.z
    );
  }

  public Quaternion GetRotation() { return Quaternion.identity; }    
  
  public void MakeMove(Point finalPosition) { 
    Debug.LogError("Porque se llama MakeMove en una EmptyPiece?");
  }

  public void Rotate(int rot) {
    Debug.LogError("Porque se llama Rotate en una EmptyPiece?");
  }

  public override string ToString() {
    return string.Format("[EmptyPiece{0}, Position={1}, Rotation={2}, PieceType={3}, Color={4}, IsSelected={5}]", Board, Position, Rotation, PieceType, Color, IsSelected);
  }
}

