using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof(Renderer), typeof(Collider))]
public class PieceSetup : MonoBehaviour {
  [SerializeField] private GameObject placeholderGO;
  [SerializeField] private Material silverMaterial, redMaterial;

  public IGamePiece Piece { get; set; }
  public bool willDestroyOnLaser { get; private set; }

  private Renderer r;
  private bool isAbove = false;
  private List<GameObject> movementPH = new List<GameObject>();
  private static bool placeholdersActive = false, selectionLocked = false;

	void Start () {        
    r = GetComponent <Renderer>();
    
    if (Piece.Color == PieceColor.Red)         r.material = redMaterial;
    else if (Piece.Color == PieceColor.Silver) r.material = silverMaterial;
  }

  void Update() { if (Piece.PieceType != PieceTypes.Sphynx) DoUpdate(); }
  void LateUpdate() { if (Piece.PieceType == PieceTypes.Sphynx) DoUpdate(); }

  void DoUpdate() {
    if (!selectionLocked && Piece.IsSelected && Input.GetButtonDown("Fire1")) {
      if (!isAbove && !Movement.mouseAbove) {
        Piece.IsSelected = false;
        LaserPointer.TargetChanged();

        HidePlaceholders();
      } else {
        if (Piece.PieceType == PieceTypes.Sphynx) StartCoroutine(CalculateLaser());
        else ShowPlaceholders();
      }
    } 

    if (Input.GetButtonDown("Fire1")) {
      if (Piece.IsSelected && !placeholdersActive) ShowPlaceholders();
      else if (!Piece.IsSelected && placeholdersActive) HidePlaceholders();
    }

    if (Piece.IsSelected 
      && Piece.PieceType == PieceTypes.Sphynx 
      && Input.GetButtonDown("Submit")) {
      LaserPointer.FireLaser(transform.position, transform.forward);
      LaserPointer.TargetChanged();
      StartCoroutine(CalculateLaser());
    }

    if (Piece.IsSelected) {
      if (Input.GetButtonDown("TurnLeft")) StartCoroutine(Rotate(-1));
      else if (Input.GetButtonDown("TurnRight")) StartCoroutine(Rotate(1));
    }
  }
    
  void OnMouseEnter() { if (!Movement.mouseAbove) isAbove = true; }
  void OnMouseExit() { if (!Movement.mouseAbove) isAbove = false; }

  void OnMouseOver() {
    if (!selectionLocked && Input.GetButtonDown("Fire1")) {
      Piece.IsSelected = !Piece.IsSelected;

      if (!Piece.IsSelected) {
        HidePlaceholders();
        LaserPointer.TargetChanged();
      }
    }
  }

  public void OnLaserHit(Vector3 point, Vector3 normal) {
    Vector3 temp = transform.InverseTransformPoint(point);

    willDestroyOnLaser = Piece.HandleLaser(transform, ref temp, ref normal);
  }

  public void OnPieceMoved(Point point, bool shouldSelect) {
    Piece.IsSelected = false;
    StartCoroutine(Move(BasePiece.ParsePosition(point), shouldSelect));
  }

  private IEnumerator Move(Vector3 position, bool shouldSelect) {
    HidePlaceholders();
    selectionLocked = true;

    while (transform.position != position) {
      transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime * 5);
      yield return null;
    }

    Piece.IsSelected = shouldSelect;
    selectionLocked = false;

    if (!placeholdersActive && shouldSelect)
      ShowPlaceholders(); 

    Debug.Log(Piece.Position.ToString() + Piece);
  }

  private void ShowPlaceholders() {
    if (Piece.PieceType == PieceTypes.Sphynx) return;

    Point[] points = Piece.GetAvailablePositions();
    if (points == null) return;

    Vector3[] positions = BasePiece.ParsePositions(points);
    HidePlaceholders();

    for (int i = 0; i < positions.Length; i++) {
      movementPH.Add(Instantiate(placeholderGO, positions[i], Piece.GetRotation()) as GameObject);
      Movement m = movementPH[movementPH.Count - 1].GetComponent<Movement>();
      m.piece = Piece;
      m.point = points[i];
      m.transform.parent = transform;
    }

    placeholdersActive = true;
  }

  private void HidePlaceholders() {
    if (Piece.PieceType == PieceTypes.Sphynx) return;

    for(int i = 0; i < movementPH.Count; i++) {
      Destroy(movementPH[i]);
    }

    movementPH.Clear();
    placeholdersActive = false;
  }

  private IEnumerator CalculateLaser() {
    int limit = 20;
    while (placeholdersActive && limit <= 0) {
      limit--;
      yield return null;
    }

    if (limit <= 0) yield break;
    LaserPointer.AddPosition(transform.position, transform.forward);
  }

  private IEnumerator Rotate(int rotation) {
    Quaternion rot = Piece.Rotate(rotation);

    while (transform.rotation != rot) {
      transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, 5);
      yield return null;
    }
  }
}