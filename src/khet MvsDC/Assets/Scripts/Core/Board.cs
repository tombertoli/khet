using UnityEngine;
using System.Collections.Generic;

public class Board {
  public int Width { get { return width; } }
  public int Height { get { return height; } }
  public GamePiece[,] Pieces { get { return pieces; } }
  public Underline[,] Underlines { get { return Underlines; } }

  private int width, height;
  private GamePiece[,] pieces;
  private Underline[,] underlines;
  private BoardSetup setup;

  public Board(BoardSetup setup, GamePiece[,] board, Underline[,] underlines) {
    this.setup = setup;
    this.pieces = board;
    this.underlines = underlines;
    this.width = board.GetLength(0);
    this.height = board.GetLength(1);

    foreach (GamePiece gp in this.pieces) {
      if (gp == null) throw new UnityException("Incomplete board");

      gp.Board = this;
    }
  }

  public Board(BoardSetup setup, int width, int height) {
    this.setup = setup;
    this.width = width;
    this.height = height;
    pieces = new GamePiece[width, height];
    underlines = new Underline[width, height];

    for (int i = 0; i < pieces.GetLength(0); i++) {
      for (int j = 0; j < pieces.GetLength(1); j++) {
        pieces[i, j] = new EmptyPoint(i, j);
        underlines[i, j] = Underline.Blank;
      }
    }
  }

  public void MovePiece(GamePiece piece, Point to) {
    DisoccupyPoint(piece.Position);
    OccupyPoint(piece, to);

    setup.MoveMade(piece);
  }

  public void RemovePiece(GamePiece piece) {
    DisoccupyPoint(piece.Position);
  }

  public GamePiece[,] GetAdjacent(GamePiece piece) {
    GamePiece[,] res = new GamePiece[3, 3];
    Point point = piece.Position;

    for (int i = -1; i < 2; i++) {
      for (int j = -1; j < 2; j++) {
        if ((point.x + i < 0 || point.x + i >= Width) || (point.y + j < 0 || point.y + j >= Height)) {
          res[i + 1, j + 1] = null;
          continue; 
        }

        res[i + 1, j + 1] = GetPieceAt(new Point(point.x + i, point.y + j));
      }
    }

    return res;
  }

  public void SetPieceAt(GamePiece piece) {
    pieces[piece.Position.x, piece.Position.y] = piece;
  }

  public GamePiece GetPieceAt(int x, int y) {
    return GetPieceAt(new Point(x, y));
  }

  public GamePiece GetPieceAt(Point point) {
    return pieces[point.x, point.y];
  }

  public Underline GetUnderline(Point point) {
    return underlines[point.x, point.y];
  }

  public bool UnderlineEqualsColor(GamePiece piece, Point position) {
    if ((underlines[position.x, position.y] == Underline.RedHorus   && piece.Color == PieceColor.Red)
     || (underlines[position.x, position.y] == Underline.SilverAnkh && piece.Color == PieceColor.Silver)
     || (underlines[position.x, position.y] == Underline.Blank)) return true;

    return false;
  }

  public Board AssignPieces(GamePiece[,] pieces, Underline[,] underline) {
    this.pieces = pieces;
    underlines = underline;

    foreach (GamePiece p in pieces) {
      if (p.PieceType != PieceTypes.Empty)
        p.Board = this;
    }
    
    return this;
  }

  private void OccupyPoint(GamePiece piece, Point position) {
    pieces[position.x, position.y] = piece;
  }

  private void DisoccupyPoint(Point position) {
    pieces[position.x, position.y] = new EmptyPoint(position.x, position.y);
  }

  public override string ToString() {
    return string.Format("[Board: Width={0}, Height={1}]", Width, Height);
  }
}

public enum Underline {
  Blank, SilverAnkh, RedHorus
}

