using UnityEngine;

public class EmptyPoint : IPiece {
  public event MoveEvent Moved;
  public event RotationEvent Rotated;

  public Board Board { get; set; }
  public Point Position { get { return position; } }
  public Quaternion Rotation { get { return Quaternion.identity; } }
  public PieceTypes Type { get { return PieceTypes.Empty; } }
  public PieceColor Color { get { return PieceColor.None; } }
  public bool IsSelected { get { return false; } set { IsSelected = false; } }
  public static Vector3 transPos { get; set; }

  private Point position;

  public EmptyPoint(Board board, int x, int y) : this(board, new Point(x, y)) {}
  public EmptyPoint(Board board, Point position) {
    this.position = position;
    this.Board = board;
  }

  public Point[] GetAvailablePositions() { return null; }
	public Quaternion[] GetAvailableRotations() { return new[] { Quaternion.identity }; }
  public int[] GetAvailableRotationsInInt() { return new[] { 0 }; }

  public bool WillDie(Transform transform, Vector3 point, Vector3 normal) { 
    throw new System.NotImplementedException("An empty can't handle a laser");
  }

  public Vector3 GetPositionInWorld() {
    return new Vector3(
      position.x + transPos.x,
      transPos.y,
      position.y + transPos.z
    );
  }

  public void Move(bool sentByLocal, Point point) {
    Debug.LogError("Porque se llama Move en una EmptyPiece?");
  }

  public void Move(bool sentByLocal, IPiece finalPosition) { 
    Debug.LogError("Porque se llama Move en una EmptyPiece?");
  }

  public void PositionChanged(PieceColor color, IPiece swappedWith, Point position) { 
    this.position = position;
  }

  public void Rotate(bool sentByLocal, int rot) {
  	Debug.LogError("Porque se llama Rotate en una EmptyPiece?");
  }

  public void Rotate(bool sentByLocal, Quaternion rot) {
    Debug.LogError("Porque se llama Rotate en una EmptyPiece?");
  }

  public override string ToString() {
    return string.Format("[EmptyPoint{0}, Position={1}, Rotation={2}, PieceType={3}, Color={4}, IsSelected={5}]", Board, Position, Rotation, Type, Color, IsSelected);
  }
}

