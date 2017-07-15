using UnityEngine;

public abstract class BasePiece : GamePiece {
  public Board Board { get; set; }
  public Point Position { get { return position; } }
  public int Rotation { get { return rotation; } }
  public PieceTypes PieceType { get { return type; } }
  public PieceColor Color { get { return color; } }
  public bool IsSelected { get; set; }
  public static Vector3 transPos { get; set; }

  protected Point position;
  protected PieceColor color;
  protected int rotation;
  protected PieceTypes type;

  protected BasePiece(Point position, int rotation, PieceColor color, Board board, PieceTypes type) {
    this.position = position;
    this.color = color;
    this.Board = board;
    this.rotation = rotation;
    this.type = type;
  }

  public abstract Point[] GetAvailablePositions();
  public abstract int[] GetAvailableRotations();

  public abstract bool HandleLaser(Transform transform, ref Vector3 point, ref Vector3 normal);
  public abstract void MakeMove(Point finalPosition);
  public abstract void Rotate(int rot);

  protected void Die() {
    Board.RemovePiece(this);
  }
  
  public Vector3 ParsePosition(Point point) {
    return new Vector3(
      point.x + transPos.x,
      transPos.y,
      point.y + transPos.z
    );
  }

  public Quaternion GetRotation() {
    return Quaternion.Euler(0, rotation * 90, 0);
  }

  public override string ToString() {
    return string.Format("[PieceType={2}, Position={0}, Rotation={1}, Color={3}, IsSelected={4}]", Position, Rotation, PieceType, Color, IsSelected);
  }
}