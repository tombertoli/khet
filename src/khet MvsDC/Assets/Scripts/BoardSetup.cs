using UnityEngine;
using System.Collections;

public class BoardSetup : MonoBehaviour {
  [SerializeField] private GameObject go;
  [System.NonSerialized] public static Board b;

	void Awake () {
    b = BoardTemplates.LoadClassic();

    for (int i = 0; i < b.Width; i++) {
      for (int j = 0; j < b.Height; j++) {
        GamePiece gp = b.GetPieceAt(i, j);
        if ((gp is EmptyPoint)) continue;

        Vector3 position = new Vector3(
                             gp.Position.x + transform.position.x,
                             transform.position.y,
                             gp.Position.y + transform.position.z);
        
        GameObject ga = (GameObject)Instantiate(go, position, ParsePieceRotation(gp.Rotation));
        ga.transform.parent = transform;
      }
    }
	}

  public GamePiece GetPieceFromCoord(Vector3 position) {
    return b.GetPieceAt((int)(position.x - transform.position.x),(int)( position.z - transform.position.z));
  }

  public Quaternion ParsePieceRotation(int rotation) {
    return Quaternion.Euler(0, 45 * rotation, 0);
  }
}
