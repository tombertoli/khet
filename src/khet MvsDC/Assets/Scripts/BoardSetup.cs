using UnityEngine;
using System.Collections;

public class BoardSetup : MonoBehaviour {
  [SerializeField] private GameObject go;
  [SerializeField] private GameObject laser;
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
                             gp.Position.x + transform.position.x,
                             transform.position.y,
                             gp.Position.y + transform.position.z);
        
        GameObject instance = (GameObject)Instantiate(go, position, ParsePieceRotation(gp.Rotation));
        instance.transform.parent = transform;
        instance.GetComponent<PieceSetup>().Piece = gp;

        if (gp is Sphynx) {
          instance.AddComponent<LaserPointer>();

          GameObject laserInstance = (GameObject)Instantiate(laser, position, ParsePieceRotation(gp.Rotation));
          instance.GetComponent<LaserPointer>().line = laserInstance.GetComponent<LineRenderer>();
        }
      }
    }
	}

  public Quaternion ParsePieceRotation(int rotation) {
    return Quaternion.Euler(0, 45 * rotation, 0);
  }
}
