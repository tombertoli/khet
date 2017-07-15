using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Renderer), typeof(Collider))]
public class PieceSetup : MonoBehaviour {
  [SerializeField] private Material silverMaterial, redMaterial;

  public GamePiece Piece { get; set; }
  public bool willDestroyOnLaser { get; private set; }

  private Renderer r;
  private bool isAbove = false;
  private Point[] availablePositions;

	void Start () {        
    r = GetComponent <Renderer>();
    
    if (Piece.Color == PieceColor.Red)         r.material = redMaterial;
    else if (Piece.Color == PieceColor.Silver) r.material = silverMaterial;
  }
    
  void Update() {
    Debug.DrawRay(transform.position, transform.forward, Color.blue);

    if (Piece.IsSelected && Input.GetButtonDown("Fire1"))
      LaserPointer.TargetChanged();

    if (Input.GetButtonDown("Fire1")) {
      if (!isAbove) Piece.IsSelected = false;
      else Piece.IsSelected = !Piece.IsSelected;

      if (Piece.IsSelected && Piece.PieceType == PieceTypes.Sphynx) LaserPointer.AddPosition(transform.position, transform.forward);
    }

    if (Piece.IsSelected 
      && Piece.PieceType == PieceTypes.Sphynx 
      && Input.GetButtonDown("Submit")) {
      LaserPointer.FireLaser(transform.position, transform.forward);
    } 
  }
  
  void OnMouseEnter() { isAbove = true; }
  void OnMouseExit() { isAbove = false; }
  
#if UNITY_EDITOR
  void OnDrawGizmos() {
    if (!Piece.IsSelected) return;

    availablePositions = availablePositions == null ? Piece.GetAvailablePositions() : availablePositions;

    if (availablePositions != null)
      foreach (Point p in availablePositions)
        Gizmos.DrawCube(Piece.ParsePosition(p), new Vector3(.5f, .5f, .5f));
  }
#endif

  public void OnLaserHit(Vector3 point, Vector3 normal) {
    Debug.Log("hit" + Piece);
    Vector3 temp = transform.InverseTransformPoint(point);

    willDestroyOnLaser = Piece.HandleLaser(transform, ref temp, ref normal);
    /*if (Piece.PieceType == PieceTypes.Pyramid) {
      if (normal == -transform.right)
        norm = Quaternion.Euler(0, 90, 0) * normal;
      else if (normal == transform.forward)
        norm = Quaternion.Euler(0, -90, 0) * normal;
      
      temp.x = 0;
      temp.z = 0;
      pos = transform.TransformPoint(temp);
    }*/
  }
}