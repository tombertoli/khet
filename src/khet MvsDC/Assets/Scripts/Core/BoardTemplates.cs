using System.IO;

public static class BoardTemplates { 
  public static Board Custom(string pathWithFile) {
    string[] piece = File.ReadAllLines(pathWithFile + ".kbt");
    string[] underline = File.ReadAllLines(pathWithFile + "-underline.kut");

    GamePiece[,] pieces = new GamePiece[piece[0].Length, piece.Length];
    Underline[,] underlines = new Underline[piece[0].Length, piece.Length];

    for (int i = 0; i < piece.Length; i++) {
      for (int j = 0; j < piece[i].Length; j++) {
        if (piece[i][j] == 'I')      pieces[i, j] = new Pharaoh(new Point(i, j), PieceColor);
        else if (piece[i][j] == 'S') pieces[i, j] = new Sphynx(new Point(i, j), PieceColor);
        else if (piece[i][j] == 'C') pieces[i, j] = new Scarab(new Point(i, j), PieceColor);
        else if (piece[i][j] == 'A') pieces[i, j] = new Anubis(new Point(i, j), PieceColor);
        else if (piece[i][j] == 'P') pieces[i, j] = new Pyramid(new Point(i, j), PieceColor);
      }
    }

    for (int i = 0; i < underline.Length; i++) {
      for (int j = 0; j < underline[i].Length; j++) {
        if (underline[i][j] == 'R')      underlines[i, j] = Underline.RedHorus;
        else if (underline[i][j] == 'S') underlines[i, j] = Underline.SilverAnkh;
        else                             underlines[i, j] = Underline.Blank;
      }
    }

    return new Board(pieces, underlines);
    /*
    for (int i = 0; i < board.GetLength(0); i++) {
      for (int j = 0; j < board.GetLength(1); j++) {
        if (i == 0) {
          if (j == 0) board[i, j] = PieceTypes.Sphynx;
          else if (j == 4 || j == 6) board[i, j] = PieceTypes.Anubis;
          else if (j == 5) board[i, j] = PieceTypes.Pharaoh;
          else if (j == 7) board[i, j] = PieceTypes.Pyramid;
        } else if (i == 1) board[i, 2] = PieceTypes.Pyramid;

      }
    }*/
  }
}
