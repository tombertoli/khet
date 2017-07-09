using UnityEngine;

public interface GamePiece {
  Board Board { get; set; }
  Point Position { get; }
  int Rotation { get; }
  PieceTypes PieceType { get; }
  PieceColor Color { get; }
  bool IsSelected { get; }

  Point[] GetAvailablePositions();
  int[] GetAvailableRotations();

  void MakeMove(Point finalPosition);
  void Rotate(int rot);
}

public enum PieceTypes {
  None, 
  Scarab, 
  Sphynx, 
  Anubis, 
  Pharaoh, 
  Pyramid
}

public enum PieceColor {
  None,
  Grey,
  Red
}