using UnityEngine;
using System.Collections.Generic;

public class Board {
  public int Width { get { return width; } }
  public int Height { get { return height; } }

  private int width, height;
  private GamePiece[,] board;

  public Board(GamePiece[,] board) {
    this.board = board;
    width = board.GetLength(0);
    height = board.GetLength(1);
  }

  public Board(int width, int height) {
    this.width = width;
    this.height = height;
    board = new GamePiece[width, height];

    for (int i = 0; i < board.GetLength(0); i++) {
      for (int j = 0; j < board.GetLength(1); j++) {
        board[i, j] = new EmptyPiece(i, j);
      }
    }
  }

  public void MovePiece(Point from, Point to, GamePiece piece) {
    DisoccupyPoint(from);
    OccupyPoint(to, piece);
  }

  private void OccupyPoint(Point position, GamePiece piece) {
    board[position.x, position.y] = piece;
  }

  private void DisoccupyPoint(Point position) {
    board[position.x, position.y] = new EmptyPiece(position.x, position.y);
  }
}

public static class BoardTemplates { 
  public static PieceTypes[,] Classic() {
    PieceTypes[,] board = new PieceTypes[8, 10];

    for (int i = 0; i < board.GetLength(0); i++) {
      for (int j = 0; j < board.GetLength(1); j++) {
        if (i == 0) {
          if (j == 0) board[i, j] = PieceTypes.Sphynx;
          else if (j == 4 || j == 6) board[i, j] = PieceTypes.Anubis;
          else if (j == 5) board[i, j] = PieceTypes.Pharaoh;
          else if (j == 7) board[i, j] = PieceTypes.Pyramid;
        } else if (i == 1) board[i, 2] = PieceTypes.Pyramid;

      }
    }
  }
}