using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Renderer), typeof(Collider))]
public class PieceSetup : MonoBehaviour {
  [SerializeField] private GameObject bs;
  [SerializeField] private Material silverMaterial, redMaterial;

  public GamePiece Piece { get; set; }
  private Renderer r;
  private bool isAbove = false;
  Vector3 pos, norm;
    
	void Start () {        
    r = GetComponent <Renderer>();
    
    if (Piece.Color == PieceColor.Red)         r.material = redMaterial;
    else if (Piece.Color == PieceColor.Silver) r.material = silverMaterial;
  }
    
  void Update() {
    Debug.DrawRay(transform.position, transform.forward, Color.red);
    Debug.DrawRay(pos, norm, Color.green);
    
    if (Piece.IsSelected) {
      if (Input.GetButtonDown("Submit") && Piece is Sphynx) {
        Debug.Log("fired");
        LaserPointer.FireLaser(transform.position, transform.forward);
      }
    }
    
    if (!isAbove && Piece.IsSelected && Input.GetButtonDown("Fire1")) {
      Piece.IsSelected = false;
    } else if (isAbove && Input.GetButtonDown("Fire1")) {
      Piece.IsSelected = !Piece.IsSelected;

      LaserPointer.TargetChanged();
      
      if (Piece.PieceType == PieceTypes.Sphynx)
        LaserPointer.AddPosition(transform.position, transform.forward);
    }    
  }
  
  void OnMouseEnter() { isAbove = true; }
  
  #if UNITY_EDITOR
  void OnDrawGizmos() {
    if (!Piece.IsSelected) return;
    
    Point[] points = Piece.GetAvailablePositions();
      Debug.Log("Selected");
      
      if (points != null) {
        foreach (Point p in points) {
          Vector3 gizmo = Piece.ParsePosition(p);
                
          Gizmos.DrawCube(gizmo, new Vector3(.5f, .5f, .5f));
          Debug.Log(p);
        }
      } else 
        Debug.Log("no positions");
  }
  #endif
  
  void OnMouseExit() { isAbove = false; }

  public void OnLaserHit(Vector3 point, Vector3 normal) {
    Vector3 temp = transform.InverseTransformPoint(point);
    Debug.Log(temp);
    
    if (Piece.PieceType == PieceTypes.Pyramid) {
      if (temp.x < 0) {
        norm = Quaternion.Euler(0, 90, 0) * normal;
      } else if (temp.z < 0)
        norm = Quaternion.Euler(0, -90, 0) * normal;
      
      temp.x = 0;
      temp.z = 0;
      pos = transform.TransformPoint(temp);
    }
    Debug.Log("hit" + Piece);
    LaserPointer.AddPosition(pos, norm);
  }
}