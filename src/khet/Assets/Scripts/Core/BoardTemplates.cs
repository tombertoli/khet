using System;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

public static class BoardTemplates {
  public static string classicText;
  public static readonly string defPath = Environment.ExpandEnvironmentVariables(@"%USERPROFILE%\Documents\My Games\khet\Templates");

  public static Board LoadCustom(string file) {
    return LoadFile(string.Format(@"{0}\{1}.kbt", defPath, file));
  }

  private static Board LoadFile(string file) {
    string[] pieceFile = File.ReadAllLines(file);
    Point pieceFileSize = GetPieceFileSize(pieceFile);

    Board board             = new Board(pieceFileSize.x, pieceFileSize.y);
    IPiece[,] pieces        = new IPiece[pieceFileSize.x, pieceFileSize.y];
    PieceColor[,] colors    = new PieceColor[pieceFileSize.x, pieceFileSize.y];
    Underline[,] underlines = new Underline[pieceFileSize.x, pieceFileSize.y];
    Quaternion[,] rotations = new Quaternion[pieceFileSize.x, pieceFileSize.y];
    
    for (int i = 0; i < pieceFile.Length; i++) {
      if (pieceFile[i].Equals("[Color]"))
        i = SetColors(pieceFile, i + 1, ref colors);

      if (pieceFile[i].Equals("[Underline]")) 
        i = SetUnderline(pieceFile, i + 1, ref underlines);
      
      if (pieceFile[i].Equals("[Rotation]"))
        i = SetRotations(pieceFile, i + 1, ref rotations);
      
      if (pieceFile[i].Equals("[Pieces]"))
        i = SetPieces(pieceFile, i + 1, colors, rotations, ref pieces);

      if (pieceFile[i].Trim().Equals("~")) break;
    }

    return board.AssignPieces(pieces, underlines);
  }

  public static Board LoadClassic() {
    TemplateManager.CheckFiles();

    return LoadCustom("classic");
  }

  public static Board LoadImhotep() {
    TemplateManager.CheckFiles();

    return LoadCustom("imhotep");
  }

  public static Board LoadDynasty() {
    TemplateManager.CheckFiles();

    return LoadCustom("dynasty");
  }

  private static int SetPieces(string[] pieceFile, int index, PieceColor[,] colors, Quaternion[,] rotations, ref IPiece[,] pieces) {
    int a = 0;

    for (int i = index; i < pieceFile.Length; i++) {
      if (pieceFile[i].Trim() == "~") return Mathf.Clamp(i + 1, 0, pieceFile.Length - 1);

      for (int j = 0; j < pieceFile[i].Length; j++) {
        char c = pieceFile[i][j];
        IPiece gp;

        switch (c) {
          case 'I':
            gp = new Pharaoh(new Point(a, j), rotations[a, j], colors[a, j], null);
            break;
          case 'S':
            gp = new Sphynx(new Point(a, j), rotations[a, j], colors[a, j], null);
            break;
          case 'C':
            gp = new Scarab(new Point(a, j), rotations[a, j], colors[a, j], null);
            break;
          case 'A':
            gp = new Anubis(new Point(a, j), rotations[a, j], colors[a, j], null);
            break;
          case 'P':
            gp = new Pyramid(new Point(a, j), rotations[a, j], colors[a, j], null);
            break;
          default:
            gp = new EmptyPoint(null, new Point(a, j));
            break;
        }

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
  
  private static int SetRotations(string[] pieceFile, int index, ref Quaternion[,] rotations) {
    int a = 0;

    for (int i = index; i < pieceFile.Length; i++) {
      if (pieceFile[i].Trim() == "~") return Mathf.Clamp(i + 1, 0, pieceFile.Length - 1);

      for (int j = 0; j < pieceFile[i].Length; j++) {
        char c = pieceFile[i][j];

        if (c == ' ') continue;
        int rotation = Convert.ToInt32(c);

        rotations[a, j] = BasePiece.ParseRotation(rotation);
      }

      a++;
    }

    return Mathf.Clamp(index + 1, 0, pieceFile.Length);
  }
  
  private static Point GetPieceFileSize(string[] pieceFile) {
    int index = pieceFile[0].IndexOf('(') + 1;
    string width = "", height = "";
    
    string curr = "";
    for (int i = index; i < pieceFile[0].Length; i++) {
      if (pieceFile[0][i] == ',') {
        width = curr;
        curr = "";
        continue;
      } else if (pieceFile[0][i] == ')') {
        height = curr;
        curr = "";
        break;
      }
      
      if (pieceFile[0][i] == ' ') continue;
      
      curr += pieceFile[0][i];
    }
    
    return new Point(Convert.ToInt32(width), Convert.ToInt32(height));
  }
}
