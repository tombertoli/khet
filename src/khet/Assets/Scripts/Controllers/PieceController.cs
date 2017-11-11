using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof(Renderer), typeof(Collider))]
public class PieceController : MonoBehaviour {
  [SerializeField] 
  [Range (1, 10)]
  private float movingMultiplier = 5;

  [SerializeField]
  [Range (10, 50)]
  private float rotationMultiplier = 25;

  [SerializeField] private Vector3 deadDistance = Vector3.zero;

  #pragma warning disable 0649
  [SerializeField] private Material silverMaterial, redMaterial;
  [SerializeField] private AudioClip movingClip;
  #pragma warning restore 0649

  public IPiece Piece { get { return piece; } set { piece = piece == null ? value : piece; } }
  private IPiece piece;

  private static List<ReflectionProbe> rps = new List<ReflectionProbe>();
  private PlaceholderManager placeholderManager;
  private bool isAbove = false;
  private static bool selectionLocked = false;
  private static int deadSilverAmount = 0, deadRedAmount = 0;

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
    if (!Piece.IsSelected || (!TurnManager.IsLocalGame && Piece.Color != NetworkController.Color) || !NetworkController.AllPlayersConnected) return;

    if (Input.GetButtonDown("Fire1") && !selectionLocked && !isAbove && !Movement.mouseAbove)
      SetSelection(false);

    if (Input.GetButtonDown("TurnLeft") && Contains(Piece.GetAvailableRotationsInInt(), -1))
      Piece.Rotate(true, -1);
    else if (Input.GetButtonDown("TurnRight") && Contains(Piece.GetAvailableRotationsInInt(), 1))
      Piece.Rotate(true, 1);
  }
    
  void OnMouseEnter() { isAbove = true; }
  void OnMouseExit() { isAbove = false; }

  void OnMouseOver() {
    if (!TurnManager.IsLocalGame && Piece.Color != NetworkController.Color) return;
    if (selectionLocked || !Input.GetButtonDown("Fire1") || Piece.Color != TurnManager.Turn || !NetworkController.AllPlayersConnected) return;

    SetSelection(!Piece.IsSelected);
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
      EndGame(Piece.Color == PieceColor.Red ? PieceColor.Silver : PieceColor.Red);

    //Destroy(transform.parent.gameObject);
    Vector3 deadPosition = Piece.Color == PieceColor.Silver ? BoardController.silverDead.position : BoardController.redDead.position;

    if (Piece.Color == PieceColor.Silver)
         deadPosition += -deadDistance * deadSilverAmount;
    else deadPosition +=  deadDistance * deadRedAmount;

    transform.parent.position = deadPosition;
    rps.Remove(transform.parent.GetComponentInChildren<ReflectionProbe>());
    
    if (Piece.Color == PieceColor.Silver) deadSilverAmount++;
    else deadRedAmount++;

    Destroy(GetComponent<Glow>());
    Destroy(this);
  }

  private void MovePiece(PieceColor color, IPiece swappedWith, Point point) {
    Vector3 temp = BasePiece.ParsePosition(transform.parent, point);
    temp.y = transform.parent.position.y;
    StartCoroutine(Move(color, swappedWith, temp));
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

  private IEnumerator Move(PieceColor changeTurnTo, IPiece swappedWith, Vector3 position) {
    SetSelection(false);
    selectionLocked = true;

    TurnManager.Wait();

    AudioSource source = Camera.main.GetComponent<AudioSource>();
    source.clip = movingClip;
    source.time = RandomPercent(source.clip, (f) => f - movingMultiplier);
    source.Play();

    while (transform.parent.position != position) {
      transform.parent.position = Vector3.Lerp(transform.parent.position, position, movingMultiplier * Time.deltaTime);
      
      UpdateProbes();
      yield return null;
    }

    source.Stop();

    selectionLocked = false;

    if (Piece.Type == PieceTypes.Scarab && swappedWith.Type != PieceTypes.Empty) yield break;
    TurnManager.End();
  }

  private IEnumerator Rotate(Quaternion rotation) {
    if (Piece.Color != TurnManager.Turn) yield break;

    SetSelection(false);
    selectionLocked = true;

    TurnManager.Wait();

    AudioSource source = Camera.main.GetComponent<AudioSource>();
    source.clip = movingClip;
    source.time = RandomPercent(source.clip, (f) => f - rotationMultiplier / 100);
    source.Play();

    while (transform.parent.rotation != rotation) {
      transform.parent.rotation = Quaternion.RotateTowards(transform.parent.rotation, rotation, rotationMultiplier * Time.deltaTime);

      UpdateProbes();
      yield return null;
    }

    source.Stop();

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
  
  private void EndGame(PieceColor won) {
    if (GameObject.FindObjectsOfType<NetworkController>().Length > 0) {
      NetworkController.EndGame(won);
      return;
    }

    StateController.EndGame(won);
  }

  private delegate float InputFunc(float f);
  private float RandomPercent(AudioClip clip, InputFunc formula) {
    return UnityEngine.Random.Range(0, formula(clip.length)) / clip.length;
  }

  #endregion
}