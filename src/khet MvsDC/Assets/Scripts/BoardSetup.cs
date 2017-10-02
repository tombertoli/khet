using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoardSetup : MonoBehaviour {
  [SerializeField] private GameObject go;
  [System.NonSerialized] public static Board b;

	void Awake () {
    b = BoardTemplates.LoadClassic(this);
    BasePiece.transPos = transform.position;
    EmptyPoint.transPos = transform.position;
    
    foreach (IGamePiece gp in b.Pieces) {
      if ((gp.PieceType == PieceTypes.Empty)) {
        continue;
      }
      
      if (gp == null) throw new UnityException("Invalid board");

      Vector3 position = new Vector3(
                           gp.Position.x + transform.position.x,
                           transform.position.y,
                           gp.Position.y + transform.position.z);
      
      GameObject instance = (GameObject)Instantiate(go, position, gp.GetRotation());
      instance.transform.parent = transform;
      
      PieceSetup ps = instance.GetComponent<PieceSetup>();
      ps.Piece = gp;
    }
  }

  public void MoveMade(IGamePiece piece, Point point, bool shouldSelect) {
    if (piece.PieceType == PieceTypes.Empty) return;
    
    PieceSetup[] gos = GameObject.FindObjectsOfType<PieceSetup>();

    for (int i = 0; i < gos.Length; i++) {
      if (gos[i].Piece != piece) continue;
      gos[i].OnPieceMoved(point, shouldSelect);
    }
  }

  public void RotationMade(Point point, Quaternion rotation) {
    PieceSetup[] gos = GameObject.FindObjectsOfType<PieceSetup>();

    for (int i = 0; i < gos.Length; i++) {
      if (gos[i].Piece != b.GetPieceAt(point)) continue;

      gos[i].OnRotated(rotation);
      Debug.Log(rotation.eulerAngles);
    }
  }
}
