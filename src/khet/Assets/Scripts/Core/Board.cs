using UnityEngine;
using System.Collections.Generic;

public class Board {
  public int Width { get { return width; } }
  public int Height { get { return height; } }
  public Underline[,] Underlines { get { return Underlines; } }

  private int width, height;
  private IPiece[,] pieces;
  private Underline[,] underlines;

  public Board(IPiece[,] board, Underline[,] underlines) {
    this.pieces = board;
    this.underlines = underlines;
    this.width = board.GetLength(0);
    this.height = board.GetLength(1);

    foreach (IPiece gp in this.pieces) {
      if (gp == null) Debug.LogError("Incomplete board");

      gp.Board = this;
    }
  }

  public Board(int width, int height) {
    this.width = width;
    this.height = height;
    pieces = new IPiece[width, height];
    underlines = new Underline[width, height];

    for (int i = 0; i < pieces.GetLength(0); i++) {
      for (int j = 0; j < pieces.GetLength(1); j++) {
        pieces[i, j] = new EmptyPoint(this, i, j);
        underlines[i, j] = Underline.Blank;
      }
    }
  }

  public void SwapPieces(bool sentByLocal, Point piece, Point target) {
    SwapPieces(sentByLocal, piece, GetPieceAt(target));
  }

  public void SwapPieces(bool sentByLocal, Point originPoint, IPiece target) {
    Point targetPoint = target.Position;
    
    OccupyPoint(pieces[originPoint.x, originPoint.y], targetPoint);
    OccupyPoint(target, originPoint);

    target.PositionChanged(target.Color, GetPieceAt(originPoint), originPoint);

    if (sentByLocal) NetworkController.MovePiece(true, targetPoint, originPoint);
  }

  public void RemovePiece(IPiece piece) {
    DisoccupyPoint(piece.Position);
  }

  public IPiece[,] GetAdjacent(IPiece piece) {
    IPiece[,] res = new IPiece[3, 3];
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

  public void SetPiece(IPiece piece) {
    pieces[piece.Position.x, piece.Position.y] = piece;
  }

  public IPiece GetPieceAt(int x, int y) {
    return GetPieceAt(new Point(x, y));
  }

  public IPiece GetPieceAt(Point point) {
    return pieces[point.x, point.y];
  }

  public Underline GetUnderline(int x, int y) {
    return GetUnderline(new Point(x, y));
  }

  public Underline GetUnderline(Point point) {
    return underlines[point.x, point.y];
  }

  public bool UnderlineEqualsColor(IPiece piece, Point position) {
    if ((underlines[position.x, position.y] == Underline.RedHorus   && piece.Color == PieceColor.Red)
     || (underlines[position.x, position.y] == Underline.SilverAnkh && piece.Color == PieceColor.Silver)
     || (underlines[position.x, position.y] == Underline.Blank)) return true;

    return false;
  }

  public Board AssignPieces(IPiece[,] pieces, Underline[,] underline) {
    this.pieces = pieces;
    underlines = underline;

    foreach (IPiece p in pieces)
      p.Board = this;
    
    return this;
  }

  private void OccupyPoint(IPiece piece, Point position) {
    pieces[position.x, position.y] = piece;
  }

  private void DisoccupyPoint(Point position) {
    OccupyPoint(new EmptyPoint(this, position.x, position.y), position);
  }

  public override string ToString() {
    return string.Format("[Board: Width={0}, Height={1}]", Width, Height);
  }
}

public enum Underline {
  Blank, SilverAnkh, RedHorus
}

