using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Renderer))]
public class PieceSetup : MonoBehaviour {
  [SerializeField] private GameObject bs;
  [SerializeField] private float delta = .125f;

  public GamePiece Piece { get; set; }
  private Renderer r;
    
	void Start () {    
    r = GetComponent <Renderer>();
    
    if (Piece.Color == PieceColor.Red)         r.material.color = Color.red;
    else if (Piece.Color == PieceColor.Silver) r.material.color = Color.grey;
	}
}
