using UnityEngine;
using System.Collections;

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

      if (gp.PieceType == PieceTypes.Sphynx) {
        if (gp.Color == PieceColor.Red)
          TurnManager.Red = ps.transform;
        else
          TurnManager.Silver = ps.transform;
      }
    }
  }

  public void MoveMade(IGamePiece piece, PieceColor color, Point point) {
    PieceSetup[] gos = FindObjectsOfType<PieceSetup>();

    for (int i = 0; i < gos.Length; i++) {
      if (gos[i].Piece != piece) continue;
      gos[i].OnPieceMoved(color, point);
    }
  }
}
