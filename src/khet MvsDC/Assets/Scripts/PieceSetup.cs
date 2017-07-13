using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Renderer), typeof(Collider))]
public class PieceSetup : MonoBehaviour {
  [SerializeField] private GameObject bs, Laser;
  [SerializeField] private Material silverMaterial, redMaterial;

  public delegate void LaserHit();
  public event LaserHit OnLaserHit;
  
  public GamePiece Piece { get; set; }
  private Renderer r;
  private Collider collider;
  private bool isAbove = false;
    
	void Start () {    
    collider = GetComponent<Collider>();
    
    r = GetComponent <Renderer>();
    
    if (Piece.Color == PieceColor.Red)         r.material = redMaterial;
    else if (Piece.Color == PieceColor.Silver) r.material = silverMaterial;
    
  }
    
  void Update() {    
    if (Piece.IsSelected && Input.GetButtonDown("Submit") && Piece is Sphynx){
      LaserPointer.AddPosition(transform.position, transform.forward);
      StartCoroutine(LaserPointer.Fire(transform.position, transform.forward));
    }
    
    if (!isAbove && Piece.IsSelected && Input.GetButtonDown("Fire1")) {
      Piece.IsSelected = false;
      Debug.Log("Deselected");
    } else if (isAbove && Input.GetButtonDown("Fire1")) {
      Piece.IsSelected = !Piece.IsSelected;
      Debug.Log("Selected");
    }    
  }
  
  void OnMouseEnter() {
    isAbove = true;
  }
  
  void OnMouseExit() {
    isAbove = false;
  }
  
  void OnTriggerEnter(Collider col) {
    Debug.Log("trigger");
    if (!col.gameObject.CompareTag("Laser")) return;
    
    LaserPointer.AddPosition(transform.position, transform.forward);
    
    if (OnLaserHit != null)
      OnLaserHit();
  }
}