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
        if ((gp is EmptyPoint)) {
          continue;
        }
        
        if (gp == null) throw new UnityException("Invalid board");

        Vector3 position = new Vector3(
                             Mathf.Ceil(gp.Position.x + transform.position.x),
                             transform.position.y,
                             Mathf.Ceil(gp.Position.y + transform.position.z));
        
        GameObject instance = (GameObject)Instantiate(go, position, ParsePieceRotation(gp.Rotation));
        instance.transform.parent = transform;
      }
    }
	}

  public GamePiece GetPieceFromCoord(Vector3 position, int delta) {
    float x = (position.x * 10) + delta;
    float y = (position.z * 10) + delta;
    return b.GetPieceAt((int) x,(int) y);
  }

  public Quaternion ParsePieceRotation(int rotation) {
    return Quaternion.Euler(0, 45 * rotation, 0);
  }
}
