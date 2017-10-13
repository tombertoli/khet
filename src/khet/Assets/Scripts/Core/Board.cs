using UnityEngine;
using System.Collections.Generic;

public class Board {
  public int Width { get { return width; } }
  public int Height { get { return height; } }
  public IGamePiece[,] Pieces { get { return pieces; } }
  public Underline[,] Underlines { get { return Underlines; } }

  private int width, height;
  private IGamePiece[,] pieces;
  private Underline[,] underlines;

  public Board(IGamePiece[,] board, Underline[,] underlines) {
    this.pieces = board;
    this.underlines = underlines;
    this.width = board.GetLength(0);
    this.height = board.GetLength(1);

    foreach (IGamePiece gp in this.pieces) {
      if (gp == null) throw new UnityException("Incomplete board");

      gp.Board = this;
    }
  }

  public Board(int width, int height) {
    this.width = width;
    this.height = height;
    pieces = new IGamePiece[width, height];
    underlines = new Underline[width, height];

    for (int i = 0; i < pieces.GetLength(0); i++) {
      for (int j = 0; j < pieces.GetLength(1); j++) {
        pieces[i, j] = new EmptyPoint(this, i, j);
        underlines[i, j] = Underline.Blank;
      }
    }
  }

  public void SwapPieces(bool sentByLocal, Point piece, Point target) {
    SwapPieces(sentByLocal, GetPieceAt(piece), GetPieceAt(target));
  }

  public void SwapPieces(bool sentByLocal, IGamePiece piece, IGamePiece target) {
    Point targetPoint = target.Position, piecePoint = piece.Position;
    //Debug.Log(targetPoint);
        
    OccupyPoint(piece, targetPoint);
    OccupyPoint(target, piecePoint);

    piece.PositionChanged(target.Color, targetPoint);
    target.PositionChanged(target.Color, piecePoint);

    if (sentByLocal) NetworkHandler.MovePiece(true, target.Position, piece.Position);
  }

  public void RemovePiece(IGamePiece piece) {
    DisoccupyPoint(piece.Position);
  }

  public IGamePiece[,] GetAdjacent(IGamePiece piece) {
    IGamePiece[,] res = new IGamePiece[3, 3];
    Point point = piece.Position;

    for (int i = -1; i < 2; i++) {
      for (int j = -1; j < 2; j++) {
        if ((point.x + i < 0 || point.x + i >= Width) || (point.y + j < 0 || point.y + j >= Height)) {
          res[i + 1, j + 1] = null;
          continue; 
        }

        res[i + 1, j + 1] = GetPieceAt(point.x + i, point.y + j);
      }
    }

    return res;
  }

  public void SetPieceAt(IGamePiece piece) {
    pieces[piece.Position.x, piece.Position.y] = piece;
  }

  public IGamePiece GetPieceAt(int x, int y) {
    return GetPieceAt(new Point(x, y));
  }

  public IGamePiece GetPieceAt(Point point) {
    return pieces[point.x, point.y];
  }

  public Point GetPositionFrom(IGamePiece piece) {
    for (int i = 0; i < pieces.GetLength(0); i++) {
      for (int j = 0; j < pieces.GetLength(1); j++) {
        if (pieces[i, j] == piece) return new Point(i, j);
      }
    }

    return new Point(-1, -1);
  }

  public Underline GetUnderline(int x, int y) {
    return GetUnderline(new Point(x, y));
  }

  public Underline GetUnderline(Point point) {
    return underlines[point.x, point.y];
  }

  public bool UnderlineEqualsColor(IGamePiece piece, Point position) {
    if ((underlines[position.x, position.y] == Underline.RedHorus   && piece.Color == PieceColor.Red)
     || (underlines[position.x, position.y] == Underline.SilverAnkh && piece.Color == PieceColor.Silver)
     || (underlines[position.x, position.y] == Underline.Blank)) return true;

    return false;
  }

  public Board AssignPieces(IGamePiece[,] pieces, Underline[,] underline) {
    this.pieces = pieces;
    underlines = underline;

    foreach (IGamePiece p in pieces)
      p.Board = this;
    
    return this;
  }

  private void OccupyPoint(IGamePiece piece, Point position) {
    pieces[position.x, position.y] = piece;
  }

  private void DisoccupyPoint(Point position) {
    pieces[position.x, position.y] = new EmptyPoint(this, position.x, position.y);
  }

  public override string ToString() {
    return string.Format("[Board: Width={0}, Height={1}]", Width, Height);
  }
}

public enum Underline {
  Blank, SilverAnkh, RedHorus
}

