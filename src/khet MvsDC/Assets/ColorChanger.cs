using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Renderer))]
public class ColorChanger : MonoBehaviour {
  Renderer r;
  public BoardSetup bs;
	// Use this for initialization
	void Start () {
    r = GetComponent < Renderer>();
    GamePiece gp = bs.GetPieceFromCoord(transform.position);

    if (gp.Color == PieceColor.Red) r.material.color = Color.red;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
