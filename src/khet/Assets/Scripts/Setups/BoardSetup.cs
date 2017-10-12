using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoardSetup : MonoBehaviour {
  [SerializeField] private GameObject piecePrefab;
  public static Board b;
  
	void Awake () {
    b = BoardTemplates.LoadClassic(this);

    BasePiece.transPos = transform.position;
    EmptyPoint.transPos = transform.position;
    
    foreach (IGamePiece gp in b.Pieces) {
      if (gp.PieceType == PieceTypes.Empty) continue;
      
      if (gp == null) throw new UnityException("Invalid board");

      Vector3 position = new Vector3(
                           gp.Position.x + transform.position.x,
                           transform.position.y,
                           gp.Position.y + transform.position.z);
      
      GameObject pieceInstance = (GameObject)Instantiate(piecePrefab, position, gp.Rotation);
      pieceInstance.transform.parent = transform;
      
      PieceSetup ps = pieceInstance.GetComponent<PieceSetup>();
      ps.Piece = gp;

      if (gp.PieceType != PieceTypes.Sphynx) continue;
      if (gp.Color == PieceColor.Red)
        TurnManager.Red = ps.transform;
      else
        TurnManager.Silver = ps.transform;
    }
  }

  public void MoveMade(IGamePiece piece, PieceColor color, Point point) {
    PieceSetup[] gos = FindObjectsOfType<PieceSetup>();

    for (int i = 0; i < gos.Length; i++) {
      if (gos[i].Piece != piece) continue;
      gos[i].PieceMoved(color, point);
    }
  }

  public void RotationMade(Point point, Quaternion rotation) {
    PieceSetup[] gos = GameObject.FindObjectsOfType<PieceSetup>();

    for (int i = 0; i < gos.Length; i++) {
      if (gos[i].Piece != b.GetPieceAt(point)) continue;

      gos[i].PieceRotated(rotation);
    }
  }
}
