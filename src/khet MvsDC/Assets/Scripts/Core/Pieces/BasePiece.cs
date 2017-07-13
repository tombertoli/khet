﻿public abstract class BasePiece : GamePiece {
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

  public abstract Point[] GetAvailablePositions();
  public abstract int[] GetAvailableRotations();

  public abstract void MakeMove(Point finalPosition);
  public abstract void Rotate(int rot);

  public override string ToString() {
    return string.Format("[PieceType={2}, Position={0}, Rotation={1}, Color={3}, IsSelected={4}]", Position, Rotation, PieceType, Color, IsSelected);
  }
}