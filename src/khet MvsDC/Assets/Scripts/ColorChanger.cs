using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Renderer))]
public class ColorChanger : MonoBehaviour {
  Renderer r;
  public GameObject bs;
  public int delta = 0;
  
	void Start () {    
    r = GetComponent < Renderer>();
    GamePiece gp = bs.GetComponent<BoardSetup>().GetPieceFromCoord(transform.localPosition, delta);
    
    Debug.Log(gp);
    if (gp.Color == PieceColor.Red) r.material.color = Color.red;
	}
}
