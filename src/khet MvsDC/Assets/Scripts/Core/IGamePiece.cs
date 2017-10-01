using UnityEngine;

public interface IGamePiece {
  Board Board { get; set; }
  Point Position { get; }
  int Rotation { get; }
  PieceTypes PieceType { get; }
  PieceColor Color { get; }
  bool IsSelected { get; set; }

  Point[] GetAvailablePositions();
  int[] GetAvailableRotations();
  
  Vector3 GetPositionInWorld();
  Quaternion GetRotation();

  bool HandleLaser(Transform transform, ref Vector3 point, ref Vector3 normal);
  void MakeMove(bool sentByLocal, Point point);
  void MakeMove(bool sentByLocal, IGamePiece piece);
  void PositionChanged();

  Quaternion Rotate(int rot);
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