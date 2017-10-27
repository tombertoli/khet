using UnityEngine;
using System;

public interface IPiece {
  event MoveEvent Moved;
  event RotationEvent Rotated;

  Board Board { get; set; }
  Point Position { get; }
  Quaternion Rotation { get; }
  PieceTypes Type { get; }
  PieceColor Color { get; }
  bool IsSelected { get; set; }

  Point[] GetAvailablePositions();
  Quaternion[] GetAvailableRotations();
  int[] GetAvailableRotationsInInt();
  
  Vector3 GetPositionInWorld();

  bool WillDie(Transform transform, Vector3 point, Vector3 normal);
  void PositionChanged(PieceColor color, IPiece swappedWith, Point position);

  void Move(bool sentByLocal, Point point);
  void Move(bool sentByLocal, IPiece piece);
  void Rotate(bool sentByLocal, int rot);
  void Rotate(bool sentByLocal, Quaternion rot);
}

public delegate void MoveEvent(PieceColor color, IPiece swappedWith, Point point);
public delegate void RotationEvent(Quaternion rotation);

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