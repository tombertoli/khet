using UnityEngine;
using System.Collections.Generic;

public abstract class BasePiece : IGamePiece {
  public static Vector3 transPos { get; set; }

  public event MoveEvent Moved;
  public event RotationEvent Rotated;

  public Board Board { get; set; }
  public Point Position { get { return position; } }
  public Quaternion Rotation { get { return rotation; } }
  public PieceTypes Type { get { return type; } }
  public PieceColor Color { get { return color; } }
  public bool IsSelected { get; set; }

  protected Point position;
  protected PieceColor color;
  protected Quaternion rotation;
  protected PieceTypes type;

  protected BasePiece(Point position, Quaternion rotation, PieceColor color, Board board, PieceTypes type) {
    this.position = position;
    this.color = color;
    this.Board = board;
    this.rotation = rotation;
    this.type = type;
  }

  public virtual Point[] GetAvailablePositions() {
    IGamePiece[,] pieces = Board.GetAdjacent(this);
    List<Point> ret = new List<Point>();

    for (int i = 0; i < pieces.GetLength(0); i++) {
      for(int j = 0; j < pieces.GetLength(1); j++) {
        IGamePiece gp = pieces[i, j];
        if (gp == null) continue;

        if (gp.Type == PieceTypes.Empty) {
          if (Board.UnderlineEqualsColor(this, gp.Position)) ret.Add(gp.Position);
        }
      }
    }

    return ret.ToArray();
  }

  public abstract Quaternion[] GetAvailableRotations();
  public abstract int[] GetAvailableRotationsInInt();

  public abstract bool WillDie(Transform transform, Vector3 point, Vector3 normal);

  public void Move(bool sentByLocal, Point point) {
    Move(sentByLocal, Board.GetPieceAt(point));
  }

  public void Move(bool sentByLocal, IGamePiece piece) {
    Board.SwapPieces(sentByLocal, this, piece);
  }

  public void Rotate(bool sentByLocal, Quaternion finalRotation) {    
    rotation = finalRotation;
    Rotated(rotation);

    if (sentByLocal) NetworkController.RotatePiece(true, Position, Rotation);
  }

  public void Rotate(bool sentByLocal, int rot) {
	  Rotate(sentByLocal, Quaternion.Euler(0, rotation.eulerAngles.y + (rot * 90), 0));
  }
  
  public void PositionChanged(PieceColor color, Point position) {
    position = Board.GetPositionFrom(this);
    Moved(color, position);
  }

  protected void Die() {
    Board.RemovePiece(this);
  }
  
  public Vector3 GetPositionInWorld() {
    return new Vector3(
      position.x + transPos.x,
      transPos.y,
      position.y + transPos.z
    );
  }

  public static Vector3 ParsePosition(Transform transform, Point point) {
    return new Vector3(
      point.x + transPos.x,
      transform.position.y,
      point.y + transPos.z
    );
  }

  public static Vector3[] ParsePositions(Transform transform, Point[] point) {
    Vector3[] ret = new Vector3[point.Length];

    for(int i = 0; i < point.Length; i++) {
      ret[i] = BasePiece.ParsePosition(transform, point[i]);
    }

    return ret;
  }

  public static Quaternion ParseRotation(int rot) {
	  return Quaternion.Euler(0, rot * 90, 0);
  }

  public override string ToString() {
    return string.Format("[PieceType={2}, Position={0}, Rotation={1}, Color={3}, IsSelected={4}]", Position, Rotation, Type, Color, IsSelected);
  }
}