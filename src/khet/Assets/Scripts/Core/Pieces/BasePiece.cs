using UnityEngine;
using System.Collections.Generic;

public abstract class BasePiece : IGamePiece {
  public static Vector3 transPos { get; set; }

  public Board Board { get; set; }
  public Point Position { get { return position; } }
  public Quaternion Rotation { get { return rotation; } }
  public PieceTypes PieceType { get { return type; } }
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

        if (gp.PieceType == PieceTypes.Empty) {
          if (Board.UnderlineEqualsColor(this, gp.Position)) ret.Add(gp.Position);
        }
      }
    }

    return ret.ToArray();
  }

  public abstract Quaternion[] GetAvailableRotations();
  public abstract int[] GetAvailableRotationsInInt();

  public abstract bool HandleLaser(Transform transform, ref Vector3 point, ref Vector3 normal);

  public void MakeMove(bool sentByLocal, Point point) {
    MakeMove(sentByLocal, Board.GetPieceAt(point));
  }

  public void MakeMove(bool sentByLocal, IGamePiece piece) {
    Board.SwapPieces(sentByLocal, this, piece);
  }

  public void Rotate(bool sentByLocal, Quaternion finalRotation) {
    List<Quaternion> rotations = new List<Quaternion>(GetAvailableRotations());

    if (!rotations.Contains(finalRotation)) {
      Debug.LogError("rotation not contained");
      return;
    }
    
    rotation = finalRotation;
    Board.RotatePiece(sentByLocal, Position, Rotation);
  }

  public void Rotate(bool sentByLocal, int rot) {
	  Rotate (sentByLocal, Quaternion.Euler(0, rotation.eulerAngles.y + (rot * 90), 0));
  }
  
  public void PositionChanged() {
    position = Board.GetPositionFrom(this);
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

  public static Vector3 ParsePosition(Point point) {
    return new Vector3(
      point.x + transPos.x,
      transPos.y,
      point.y + transPos.z
    );
  }

  public static Vector3[] ParsePositions(Point[] point) {
    Vector3[] ret = new Vector3[point.Length];

    for(int i = 0; i < point.Length; i++) {
      ret[i] = BasePiece.ParsePosition(point[i]);
    }

    return ret;
  }

  public static Quaternion ParseRotation(int rot) {
	return Quaternion.Euler(0, rot * 90, 0);
  }

  /*public Quaternion GetRotation() {
    return Quaternion.Euler(0, rotation * 90, 0);
  } 

  public static int InverseParseRotation(Quaternion rot) {
    return (int)rot.eulerAngles.y / 90;
  }*/

  public override string ToString() {
    return string.Format("[PieceType={2}, Position={0}, Rotation={1}, Color={3}, IsSelected={4}]", Position, Rotation, PieceType, Color, IsSelected);
  }
}