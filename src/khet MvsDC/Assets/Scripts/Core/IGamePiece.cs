using UnityEngine;

public interface IGamePiece {
  Board Board { get; set; }
  Point Position { get; }
  Quaternion Rotation { get; }
  PieceTypes PieceType { get; }
  PieceColor Color { get; }
  bool IsSelected { get; set; }

  Point[] GetAvailablePositions();
  Quaternion[] GetAvailableRotations();
  int[] GetAvailableRotationsInInt();
  
  Vector3 GetPositionInWorld();

  bool HandleLaser(Transform transform, ref Vector3 point, ref Vector3 normal);
  void MakeMove(bool sentByLocal, Point point);
  void MakeMove(bool sentByLocal, IGamePiece piece);
  void PositionChanged();

  void Rotate(bool sentByLocal, int rot);
  void Rotate(bool sentByLocal, Quaternion rot);
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