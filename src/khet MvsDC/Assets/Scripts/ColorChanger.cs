using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Renderer))]
public class ColorChanger : MonoBehaviour {
  [SerializeField] private GameObject bs;
  [SerializeField] private float delta = .125f;
  Renderer r;
    
	void Start () {    
    r = GetComponent < Renderer>();
    Vector3 pos = transform.localPosition;
    
    GamePiece gp = bs.GetComponent<BoardSetup>().GetPieceFromCoord(pos, delta);
    
    if (gp.Color == PieceColor.Red) r.material.color = Color.red;
	}
}
