﻿using System.IO;
using UnityEngine;

public static class BoardTemplates { 
  public static Board LoadCustom(string filePath) {
    string[] pieceFile = File.ReadAllLines(filePath + ".kbt");
    int pieceFileSize = pieceFile[1].Length;
    string[] underlineFile = File.ReadAllLines(filePath + "-underline.kut");

    Board board = new Board(pieceFile.Length, pieceFileSize);
    GamePiece[,] pieces = new GamePiece[pieceFile.Length, pieceFileSize];
    PieceColor[,] colors = new PieceColor[pieceFile.Length, pieceFileSize];
    Underline[,] underlines = new Underline[underlineFile.Length, underlineFile[0].Length];

    for (int i = 0; i < pieceFile.Length; i++) {
      if (pieceFile[i].Equals("[Color]"))
        i = SetColors(pieceFile, i + 1, ref colors);

      if (pieceFile[i].Equals("[Underline]")) 
        i = SetUnderline(pieceFile, i + 1, ref underlines);
      
      if (pieceFile[i].Equals("[Pieces]"))
        i = SetPieces(pieceFile, i + 1, colors, ref pieces);

      if (pieceFile[i].Trim().Equals("~")) break;
    }

    return board.AssignPieces(pieces, underlines);
  }

  public static Board LoadClassic() {
    return LoadCustom("./layouts/classic");
  }

  private static int SetPieces(string[] pieceFile, int index, PieceColor[,] colors, ref GamePiece[,] pieces) {
    int a = 0;

    for (int i = index; i < pieceFile.Length; i++) {
      if (pieceFile[i].Trim() == "~") return Mathf.Clamp(i + 1, 0, pieceFile.Length - 1);

      for (int j = 0; j < pieceFile[i].Length; j++) {
        char c = pieceFile[i][j];
        GamePiece gp;

        if (c == 'I') gp = new Pharaoh(new Point(a, j), 0, colors[a, j], null);
        else if (c == 'S') gp = new Sphynx(new Point(a, j), 0, colors[a, j], null);
        else if (c == 'C') gp = new Scarab(new Point(a, j), 0, colors[a, j], null);
        else if (c == 'A') gp = new Anubis(new Point(a, j), 0, colors[a, j], null);
        else if (c == 'P') gp = new Pyramid(new Point(a, j), 0, colors[a, j], null);
        else gp = new EmptyPoint(new Point(a, j));

        pieces[a, j] = gp;
      }

      a++;
    }

    return Mathf.Clamp(index + 1, 0, pieceFile.Length - 1);
  }

  private static int SetColors(string[] pieceFile, int index, ref PieceColor[,] colors) {
    int a = 0;

    for (int i = index; i < pieceFile.Length; i++) {
      if (pieceFile[i].Trim() == "~") return Mathf.Clamp(i + 1, 0, pieceFile.Length - 1);

      for (int j = 0; j < pieceFile[i].Length; j++) {
        char c = pieceFile[i][j];
        PieceColor pc = PieceColor.None;

        if (c == 'R')      pc = PieceColor.Red;
        else if (c == 'S') pc = PieceColor.Silver;

        colors[a, j] = pc;
      }

      a++;
    }

    return Mathf.Clamp(index + 1, 0, pieceFile.Length);
  }

  private static int SetUnderline(string[] pieceFile, int index, ref Underline[,] underline) {
    int a = 0;

    for (int i = index; i < pieceFile.Length; i++) {
      if (pieceFile[i].Trim() == "~") return Mathf.Clamp(i + 1, 0, pieceFile.Length - 1);

      for (int j = 0; j < pieceFile[i].Length; j++) {
        char c = pieceFile[i][j];
        Underline pc = Underline.Blank;

        if (c == 'R')      pc = Underline.RedHorus;
        else if (c == 'S') pc = Underline.SilverAnkh;

        underline[a, j] = pc;
      }

      a++;
    }

    return Mathf.Clamp(index + 1, 0, pieceFile.Length - 1);
  }
}
