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

  public void Rotate(bool sentByLocal, Quaternion rot) {
    List<int> rotations = new List<int>(GetAvailableRotations());
    int inverse = InverseParseRotation(rot);

    foreach(int i in rotations) Debug.Log(i + " rotation");
    Debug.Log(rot.eulerAngles + " inverse: " + inverse);

    if (!rotations.Contains(inverse)) {
      return;
    }

    if ((rotation == 3 && inverse == 1)) rotation = 0;
    else rotation += inverse;

    Board.RotatePiece(sentByLocal, Position, GetRotation());
  }

  public void Rotate(bool sentByLocal, int rot) {
    List<int> rotations = new List<int>(GetAvailableRotations());
    Debug.Log(rot);

    if (!rotations.Contains(rot)) { 
      //Board.RotatePiece(sentByLocal, Position, GetRotation());
      Debug.LogError("Algo con la rotacion");
      return;
    }

    if ((rotation == 3 && rot == 1)) rotation = 0;
    else rotation += rot;
    
    Board.RotatePiece(sentByLocal, Position, GetRotation());
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

  public Quaternion GetRotation() {
    return Quaternion.Euler(0, rotation * 90, 0);
  }

  public static Quaternion ParseRotation(int rot) {
    return Quaternion.Euler(0, rot * 90, 0);
  }

  public static int InverseParseRotation(Quaternion rot) {
    return (int)rot.eulerAngles.y / 90;
  }

  public override string ToString() {
    return string.Format("[PieceType={2}, Position={0}, Rotation={1}, Color={3}, IsSelected={4}]", Position, Rotation, PieceType, Color, IsSelected);
  }
}