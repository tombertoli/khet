using UnityEngine;
using System.Collections.Generic;

public class Board {
  public int Width { get { return width; } }
  public int Height { get { return height; } }

  private int width, height;
  private GamePiece[,] board;
  private Underline[,] underlines;

  public Board(GamePiece[,] board, Underline[,] underlines) {
    this.board = board;
    this.underlines = underlines;
    this.width = board.GetLength(0);
    this.height = board.GetLength(1);

    foreach (GamePiece gp in this.board) {
      gp.Board = this;
    }
  }

  public Board(int width, int height) {
    this.width = width;
    this.height = height;
    board = new GamePiece[width, height];
    underlines = new Underline[width, height];

    for (int i = 0; i < board.GetLength(0); i++) {
      for (int j = 0; j < board.GetLength(1); j++) {
        board[i, j] = new EmptyPoint(i, j);
        underlines[i, j] = Underline.Blank;
      }
    }
  }

  public void MovePiece(GamePiece piece, Point to) {
    DisoccupyPoint(piece.Position);
    OccupyPoint(piece, to);
  }

  public GamePiece[,] GetAdjacent(GamePiece piece) {
    GamePiece[,] res = new GamePiece[3, 3];
    Point point = piece.Position;

    for (int i = -1; i < 2; i++) {
      for (int j = -1; j < 2; j++) {
        if ((point.x + i < 0 || point.x + i > Width) || (point.y + j < 0 || point.y + j > Height)) {
          res[i + 1, j + 1] = null;
          continue; 
        }

        res[i + 1, j + 1] = GetPieceAt(new Point(point.x + i, point.y + j));
      }
    }

    return res;
  }

  public void SetPieceAt(GamePiece piece) {
    board[piece.Position.x, piece.Position.y] = piece;
  }

  public GamePiece GetPieceAt(Point point) {
    return board[point.x, point.y];
  }

  public Underline GetUnderline(Point point) {
    return underlines[point.x, point.y];
  }

  public bool UnderlineEqualsColor(GamePiece piece, Point position) {
    if (underlines[position.x, position.y] == Underline.RedHorus   && piece.Color == PieceColor.Red
     || underlines[position.x, position.y] == Underline.SilverAnkh && piece.Color == PieceColor.Grey
     || underlines[position.x, position.y] == Underline.Blank) return true;

    return false;
  }

  private void OccupyPoint(GamePiece piece, Point position) {
    board[position.x, position.y] = piece;
  }

  private void DisoccupyPoint(Point position) {
    board[position.x, position.y] = new EmptyPoint(position.x, position.y);
  }
}

public enum Underline {
  Blank, SilverAnkh, RedHorus
}

