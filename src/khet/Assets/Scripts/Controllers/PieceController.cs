﻿using UnityEngine;
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

  private static List<ReflectionProbe> rps = new List<ReflectionProbe>();
  private PlaceholderManager placeholderManager;
  private bool isAbove = false;
  private static bool selectionLocked = false;

	void Start() {
    Piece.Moved += MovePiece;
    Piece.Rotated += RotatePiece;
    
    placeholderManager = transform.parent.GetComponent<PlaceholderManager>();

    ReflectionProbe rp = transform.parent.GetComponentInChildren<ReflectionProbe>();

    if (rp != null) {
      rps.Add(rp);
      rp.RenderProbe();
    }

    Renderer r = GetComponent<Renderer>();
    
    if      (Piece.Color == PieceColor.Red)    r.material = redMaterial;
    else if (Piece.Color == PieceColor.Silver) r.material = silverMaterial;
  }

  void Update() {
    if (!TurnManager.IsSinglePlayer && Piece.Color != NetworkController.Color) return;

    if (placeholderManager != null && !Piece.IsSelected) {
      if (Input.GetButtonDown("Fire1") && PlaceholderManager.Active) SetSelection(false);
      return;
    }

    if (Input.GetButtonDown("Fire1")) {
      if (placeholderManager != null && !PlaceholderManager.Active) SetSelection(true);

      if (!selectionLocked && !isAbove && !Movement.mouseAbove)
        SetSelection(false);
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
    if (placeholderManager == null) return;

    if (!Piece.IsSelected) SetSelection(false);
    else SetSelection(true);
  }

  #region Events

  public void LaserHit(Vector3 point, Vector3 normal) {
    point = transform.parent.InverseTransformPoint(point);

    if (!Piece.WillDie(transform.parent, point, normal)) return;
    
    LaserController.Hit += Die;
  }

  private void Die() {
    LaserController.Hit -= Die;

    if (Piece.Type == PieceTypes.Pharaoh)
      NetworkController.EndGame(Piece.Color == PieceColor.Red ? PieceColor.Silver : PieceColor.Red);

    Destroy(transform.parent.gameObject);
  }

  private void MovePiece(PieceColor color, Point point) {
    Vector3 temp = BasePiece.ParsePosition(transform.parent, point);
    temp.y = transform.parent.position.y;
    StartCoroutine(Move(color, temp));
  }

  private void RotatePiece(Quaternion rotation) {
    StartCoroutine(Rotate(rotation));
  }

  public void SetSelection(bool selection) {
    Piece.IsSelected = selection;
    
    if (placeholderManager == null) return;
    placeholderManager.SetPlaceholders(selection);
  }

  #endregion

  #region Coroutines

  private IEnumerator Move(PieceColor changeTurnTo, Vector3 position) {
    SetSelection(false);
    selectionLocked = true;

    if (Piece.Color != changeTurnTo) TurnManager.Wait();

    while (transform.parent.position != position) {
      transform.parent.position = Vector3.Lerp(transform.parent.position, position, Time.deltaTime * multiplier);
      
      UpdateProbes();

      yield return null;
    }

    if (Piece.Color != changeTurnTo) TurnManager.End();

    selectionLocked = false;
  }

  private IEnumerator Rotate(Quaternion rotation) {
    if (Piece.Color != TurnManager.Turn) yield break;

    SetSelection(false);
    selectionLocked = true;

    TurnManager.Wait();

    while (transform.parent.rotation != rotation) {
      transform.parent.rotation = Quaternion.RotateTowards(transform.parent.rotation, rotation, multiplier);

      UpdateProbes();

      yield return null;
    }

    TurnManager.End();
    selectionLocked = false;
  }

  #endregion

  #region Utility
  
  public static void UpdateProbes() {
    for (int i = 0; i < rps.Count; i++) {
      if (rps[i] != null) rps[i].RenderProbe();
      else rps.Remove(rps[i]);
    }
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