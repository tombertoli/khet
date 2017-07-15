using UnityEngine;
using System.Collections;

public class BoardSetup : MonoBehaviour {
  [SerializeField] private GameObject go;
  [System.NonSerialized] public static Board b;

	void Awake () {
    b = BoardTemplates.LoadClassic();
    BasePiece.transPos = transform.position;
    EmptyPoint.transPos = transform.position;
    
    foreach (GamePiece gp in b.Pieces) {
      if ((gp.PieceType == PieceTypes.Empty )) {
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
}
