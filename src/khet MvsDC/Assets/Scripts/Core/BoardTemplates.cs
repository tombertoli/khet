using System.IO;
using UnityEngine;

public static class BoardTemplates { 
  public static Board LoadCustom(string filePath) {
    string[] pieceFile = File.ReadAllLines(filePath + ".kbt");
    int pieceFileSize = pieceFile[0].Length / 2;
    string[] underlineFile = File.ReadAllLines(filePath + "-underline.kut");

    Board board = new Board(pieceFile.Length, pieceFileSize);
    GamePiece[,] pieces = new GamePiece[pieceFile.Length, pieceFileSize];
    Underline[,] underlines = new Underline[underlineFile.Length, underlineFile[0].Length];

    for (int i = 0; i < pieceFile.Length; i++) {
      for (int j = 0; j < pieceFile[0].Length; j++) {
        if (j % 2 != 0) continue;

        char c = pieceFile[i][j / 2];
        GamePiece gp;
        PieceColor pc = PieceColor.None;

        if (pieceFile[i][j + 1] == 'R')      pc = PieceColor.Red;
        else if (pieceFile[i][j + 1] == 'S') pc = PieceColor.Silver;
        else                             pc = PieceColor.None;

        if      (c == 'I') gp = new Pharaoh(new Point(i, j / 2), 0, pc, board);
        else if (c == 'S') gp = new Sphynx(new Point(i, j / 2), 0, pc, board);
        else if (c == 'C') gp = new Scarab(new Point(i, j / 2), 0, pc, board);
        else if (c == 'A') gp = new Anubis(new Point(i, j / 2), 0, pc, board);
        else if (c == 'P') gp = new Pyramid(new Point(i, j / 2), 0, pc, board);
        else               gp = new EmptyPoint(new Point(i, j / 2));

        pieces[i, j / 2] = gp;
      }
    }

    for (int i = 0; i < underlineFile.Length; i++) {
      for (int j = 0; j < underlineFile[i].Length; j++) {
        if (underlineFile[i][j] == 'R')      underlines[i, j] = Underline.RedHorus;
        else if (underlineFile[i][j] == 'S') underlines[i, j] = Underline.SilverAnkh;
        else                                 underlines[i, j] = Underline.Blank;
      }
    }

    return board.AssignPieces(pieces, underlines);
  }

  public static Board LoadClassic() {
    return LoadCustom("./layouts/classic");
  }
}
