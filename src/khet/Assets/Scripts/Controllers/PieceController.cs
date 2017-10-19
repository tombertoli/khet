using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof(Renderer), typeof(Collider))]
public class PieceController : MonoBehaviour {
  [SerializeField] private float multiplier = 5f;

  #pragma warning disable 0649
  [SerializeField] private Material silverMaterial, redMaterial;
  #pragma warning restore 0649

  public IGamePiece Piece { get; set; }

  private Renderer r;
  private PlaceholderManager placeholderManager;
  private bool isAbove = false;
  private static bool selectionLocked = false;

	void Start() {
    //TurnManager.IsSinglePlayer = true;
    Piece.Moved += MovePiece;
    Piece.Rotated += RotatePiece;
    
    r = GetComponent<Renderer>();
    placeholderManager = transform.parent.GetComponent<PlaceholderManager>();
    
    if (Piece.Color == PieceColor.Red)         r.material = redMaterial;
    else if (Piece.Color == PieceColor.Silver) r.material = silverMaterial;
  }

  void Update() {
    if (!TurnManager.IsSinglePlayer && Piece.Color != NetworkController.Color) return;

    if (!Piece.IsSelected) {
      if (Input.GetButtonDown("Fire1") && PlaceholderManager.Active) placeholderManager.HidePlaceholders();
      return;
    }

    if (Input.GetButtonDown("Fire1")) {
      if (!PlaceholderManager.Active) placeholderManager.ShowPlaceholders();

      if (!selectionLocked && !isAbove && !Movement.mouseAbove) {
        SetSelection(false);
        placeholderManager.HidePlaceholders();
      }
    }

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
    if (!Piece.IsSelected) placeholderManager.HidePlaceholders();
  }

  #region Events

  public void LaserHit(Vector3 point, Vector3 normal) {
    point = transform.parent.InverseTransformPoint(point);

    if (!Piece.WillDie(transform.parent, ref point, ref normal)) return;
    
    LaserController.Hit += Die;
  }

  private void Die() {
    LaserController.Hit -= Die;

    if (Piece.Type == PieceTypes.Pharaoh)
      NetworkController.EndGame(Piece.Color == PieceColor.Red ? PieceColor.Silver : PieceColor.Red);

    Destroy(transform.parent.gameObject);
  }

  private void MovePiece(PieceColor color, Point point) {
    SetSelection(false);

    Vector3 temp = BasePiece.ParsePosition(transform.parent, point);
    temp.y = transform.parent.position.y;
    StartCoroutine(Move(color, temp));
  }

  private void RotatePiece(Quaternion rotation) {
    SetSelection(false);
    StartCoroutine(Rotate(rotation));
  }

  public void SetSelection(bool selection) {
    Piece.IsSelected = selection;
  }

  #endregion

  #region Coroutines

  private IEnumerator Move(PieceColor changeTurnTo, Vector3 position) {
    placeholderManager.HidePlaceholders();
    selectionLocked = true;

    if (Piece.Color != changeTurnTo) TurnManager.Wait();

    while (transform.parent.position != position) {
      transform.parent.position = Vector3.Slerp(transform.parent.position, position, Time.deltaTime * multiplier);
      yield return null;
    }

    if (Piece.Color != changeTurnTo) TurnManager.End();

    selectionLocked = false;
  }

  private IEnumerator Rotate(Quaternion rotation) {
    if (Piece.Color != TurnManager.Turn) yield break;

    placeholderManager.HidePlaceholders();
    selectionLocked = true;

    TurnManager.Wait();

    while (transform.parent.rotation != rotation) {
      transform.parent.rotation = Quaternion.RotateTowards(transform.parent.rotation, rotation, multiplier);
      yield return null;
    }

    TurnManager.End();
    selectionLocked = false;
  }

  #endregion

  #region Utility
  
  private bool Contains<T>(T[] array, T equal) where T : IComparable {
    for (int i = 0; i < array.Length; i++) {
      if (!array[i].Equals(equal)) continue;
      return true;
    }

    return false;
  }

  #endregion
}