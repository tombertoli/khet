using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof(Renderer), typeof(Collider))]
public class PieceSetup : MonoBehaviour {
  [SerializeField] private GameObject placeholderGO;
  [SerializeField] private Material silverMaterial, redMaterial;

  public GamePiece Piece { get; set; }
  public bool willDestroyOnLaser { get; private set; }

  private Renderer r;
  private bool isAbove = false;
  private List<GameObject> movementPH = new List<GameObject>();

	void Start () {        
    r = GetComponent <Renderer>();
    
    if (Piece.Color == PieceColor.Red)         r.material = redMaterial;
    else if (Piece.Color == PieceColor.Silver) r.material = silverMaterial;
  }
    
  void Update() {
    if (Piece.IsSelected && Input.GetButtonDown("Fire1")) {
      LaserPointer.TargetChanged();

      if (!Movement.mouseAbove) {
        HidePlaceholders();
      }
    }

    if (Input.GetButtonDown("Fire1")) {
      if (!isAbove || Movement.mouseAbove) Piece.IsSelected = false;
      else Piece.IsSelected = !Piece.IsSelected;

      if (Piece.IsSelected) {
        if (Piece.PieceType == PieceTypes.Sphynx) LaserPointer.AddPosition(transform.position, transform.forward);

        ShowPlaceholders();
      }
    }

    if (Piece.IsSelected 
      && Piece.PieceType == PieceTypes.Sphynx 
      && Input.GetButtonDown("Submit")) {
      LaserPointer.FireLaser(transform.position, transform.forward);
    } 

    Debug.Log(Piece.IsSelected);
  }
  
  void OnMouseEnter() { isAbove = true; }
  void OnMouseExit() { isAbove = false; }

  public void OnLaserHit(Vector3 point, Vector3 normal) {
    Debug.Log("hit" + Piece);
    Vector3 temp = transform.InverseTransformPoint(point);

    willDestroyOnLaser = Piece.HandleLaser(transform, ref temp, ref normal);
  }

  public void OnPieceMoved() {
    HidePlaceholders();
    Piece.IsSelected = false;
    StartCoroutine(Move(Piece.GetPositionInWorld()));
  }

  private IEnumerator Move(Vector3 position) {
    while (transform.position != position) {
      transform.position = Vector3.Lerp(transform.position, position, .15f);
      yield return null;
    }

    Piece.IsSelected = true;
    ShowPlaceholders();
  }

  private void ShowPlaceholders() {
    if (Piece.PieceType == PieceTypes.Sphynx) return;

    Point[] points = Piece.GetAvailablePositions();
    if (points == null) return;

    Vector3[] positions = BasePiece.ParsePositions(points);

    for (int i = 0; i < positions.Length; i++) {
      movementPH.Add(Instantiate(placeholderGO, positions[i], Piece.GetRotation()) as GameObject);
      Movement m = movementPH[movementPH.Count - 1].GetComponent<Movement>();
      m.piece = Piece;
      m.point = points[i];
      m.transform.parent = transform;
    }
  }

  private void HidePlaceholders() {
    for(int i = 0; i < movementPH.Count; i++) {
      Destroy(movementPH[i]);
    }

    movementPH.Clear();
  }
}