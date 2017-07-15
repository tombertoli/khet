using UnityEngine;

public interface GamePiece {
  Board Board { get; set; }
  Point Position { get; }
  int Rotation { get; }
  PieceTypes PieceType { get; }
  PieceColor Color { get; }
  bool IsSelected { get; set; }

  Point[] GetAvailablePositions();
  int[] GetAvailableRotations();
  
  Vector3 ParsePosition(Point p);
  Quaternion GetRotation();

  bool HandleLaser(Transform transform, ref Vector3 point, ref Vector3 normal);
  void MakeMove(Point finalPosition);
  void Rotate(int rot);
}

public enum PieceTypes {
  Empty, 
  Scarab, 
  Sphynx, 
  Anubis, 
  Pharaoh, 
  Pyramid
}

public enum PieceColor {
  None,
  Silver,
  Red
}