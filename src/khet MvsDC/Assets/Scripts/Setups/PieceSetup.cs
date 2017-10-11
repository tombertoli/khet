using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof(Renderer), typeof(Collider))]
public class PieceSetup : MonoBehaviour {
  [SerializeField] private GameObject placeholderGO;
  [SerializeField] private short multiplier = 5;

  #pragma warning disable 0649
  [SerializeField] private Material silverMaterial, redMaterial;
  #pragma warning restore 0649

  public IGamePiece Piece { get; set; }
  public bool willDestroyOnLaser { get; private set; }

  private Renderer r;
  private bool isAbove = false;
  private List<GameObject> movementPH = new List<GameObject>();
  private static bool placeholdersActive = false, selectionLocked = false;

	void Start() {        
    r = GetComponent<Renderer>();
    
    if (Piece.Color == PieceColor.Red)         r.material = redMaterial;
    else if (Piece.Color == PieceColor.Silver) r.material = silverMaterial;
  }

  void Update() {
    if (Piece.Color != NetworkHandler.Color) return;

    if (!Piece.IsSelected) {
      if (Input.GetButtonDown("Fire1") && placeholdersActive) HidePlaceholders();
      return;
    }

    if (Input.GetButtonDown("Fire1")) {
      if (!placeholdersActive) ShowPlaceholders();

      if (!selectionLocked && !isAbove && !Movement.mouseAbove) {
        SetSelection(false);
        HidePlaceholders();
      }
    }

    Debug.Log(Piece.Rotation.eulerAngles);
    if (Input.GetButtonDown("TurnLeft") && Contains(Piece.GetAvailableRotationsInInt(), -1))
      Piece.Rotate(true, -1);
    else if (Input.GetButtonDown("TurnRight") && Contains(Piece.GetAvailableRotationsInInt(), 1))
      Piece.Rotate(true, 1);
  }
    
  void OnMouseEnter() { isAbove = true; }
  void OnMouseExit() { isAbove = false; }

  void OnMouseOver() {
    if (selectionLocked || !Input.GetButtonDown("Fire1") || Piece.Color != TurnManager.Turn) return;

    SetSelection(!Piece.IsSelected);
    if (!Piece.IsSelected) HidePlaceholders();
  }

  #region Events

  public void LaserHit(Vector3 point, Vector3 normal) {
    Vector3 temp = transform.InverseTransformPoint(point);

    willDestroyOnLaser = Piece.HandleLaser(transform, ref temp, ref normal);
  }

  public void PieceMoved(PieceColor color, Point point) {
    SetSelection(false);
    StartCoroutine(Move(color, BasePiece.ParsePosition(point)));
  }

  public void PieceRotated(Quaternion rotation) {
    SetSelection(false);
    StartCoroutine(Rotate(rotation));
  }

  public void SetSelection(bool selection) {
    Piece.IsSelected = selection;
  }

  #endregion

  #region Coroutines

  private IEnumerator Move(PieceColor changeTurnTo, Vector3 position) {
    HidePlaceholders();
    selectionLocked = true;

    if (Piece.Color != changeTurnTo)
      TurnManager.WaitTurn();

    while (transform.position != position) {
      transform.position = Vector3.Lerp(transform.position, position, Time.time * multiplier);
      yield return null;
    }

    if (Piece.Color != changeTurnTo)
      TurnManager.EndTurn();

    selectionLocked = false;
  }

  private IEnumerator Rotate(Quaternion rotation) {
    if (Piece.Color != TurnManager.Turn) yield break;

    HidePlaceholders();
    selectionLocked = true;

    TurnManager.WaitTurn();

    while (transform.rotation != rotation) {
      transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 5);
      yield return null;
    }

    TurnManager.EndTurn();
    selectionLocked = false;
  }

  #endregion

  #region Utility

  private void ShowPlaceholders() {
    if (Piece.PieceType == PieceTypes.Sphynx) return;

    Point[] points = Piece.GetAvailablePositions();
    if (points == null) return;

    Vector3[] positions = BasePiece.ParsePositions(points);
    HidePlaceholders();

    for (int i = 0; i < positions.Length; i++) {
      movementPH.Add(Instantiate(placeholderGO, positions[i], Piece.Rotation) as GameObject);
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

  private bool Contains<T>(T[] array, T equal) where T : IComparable {
    for (int i = 0; i < array.Length; i++) {
      if (!array[i].Equals(equal)) continue;
      return true;
    }

    return false;
  }

  #endregion
}