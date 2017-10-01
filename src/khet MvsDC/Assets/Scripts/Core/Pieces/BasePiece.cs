using UnityEngine;
using System.Collections.Generic;

public abstract class BasePiece : IGamePiece {
  public static Vector3 transPos { get; set; }

  public Board Board { get; set; }
  public Point Position { get { return position; } }
  public int Rotation { get { return rotation; } }
  public PieceTypes PieceType { get { return type; } }
  public PieceColor Color { get { return color; } }
  public bool IsSelected { get; set; }

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

  public abstract int[] GetAvailableRotations();

  public abstract bool HandleLaser(Transform transform, ref Vector3 point, ref Vector3 normal);

  public void MakeMove(bool sentByLocal, Point point) {
    MakeMove(sentByLocal, Board.GetPieceAt(point));
  }

  public void MakeMove(bool sentByLocal, IGamePiece piece) {
    Board.SwapPieces(sentByLocal, this, piece);
  }

  public void PositionChanged() {
    position = Board.GetPositionFrom(this);
  }

  public Quaternion Rotate(int rot) {
    List<int> rotations = new List<int>(GetAvailableRotations());
    if (!rotations.Contains(rot)) return GetRotation();

    if ((rotation == 3 && rot == 1)) rotation = 0;
    else rotation += rot;

    return GetRotation();
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

  public Quaternion GetRotation() {
    return Quaternion.Euler(0, rotation * 90, 0);
  }

  public override string ToString() {
    return string.Format("[PieceType={2}, Position={0}, Rotation={1}, Color={3}, IsSelected={4}]", Position, Rotation, PieceType, Color, IsSelected);
  }
}