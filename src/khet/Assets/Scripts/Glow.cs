using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof(Collider), typeof(Renderer))]
public class Glow : MonoBehaviour {
  [SerializeField] private string key, button = "Fire1", pieceTag = "Piece";
  [SerializeField] private float outlineWidth;

  private PieceColor pieceColor;
  private Renderer r;
  private bool permanent, over;

  void Start() {
    TurnManager.TurnFinished += () => SetRange(false, GameObject.FindGameObjectsWithTag(pieceTag));

    r = GetComponent<Renderer>();
    pieceColor = GetComponent<PieceController>().Piece.Color;
  }

  void Update() { 
    if (!Input.GetButtonDown(button)) return;
    
    if (over) { 
      permanent = !permanent;
      return;
    }

    permanent = false;
    SetOutline(false);
  }

  void OnMouseEnter() {
    over = true; 

    if (permanent) return;
    if (pieceColor != TurnManager.Turn) return;
    if (!TurnManager.IsSinglePlayer && pieceColor != NetworkHandler.Color) return;

    SetOutline(true);
  }

  void OnMouseOver() {
    if (!over) over = true;

    if (permanent) return;
    if (pieceColor != TurnManager.Turn) return;
    if (!TurnManager.IsSinglePlayer && pieceColor != NetworkHandler.Color) return;

    SetOutline(true);
  }

  void OnMouseExit() { 
    over = false; 

    if (permanent) return;
    if (pieceColor != TurnManager.Turn) return;
    if (!TurnManager.IsSinglePlayer && pieceColor != NetworkHandler.Color) return;

    SetOutline(false);
  }

  public static void SetRange(bool setTo, GameObject[] gos) {
    for (int i = 0; i < gos.Length; i++) {
      if (!gos[i].CompareTag("Piece")) Debug.Log("Se hace el piola");
      Glow glow = gos[i].GetComponent<Glow>();

      if (glow == null) {
        Debug.Log("nulleadisimo; que carajo pasa?");
        continue;
      }

      glow.SetOutline(setTo);
    }
  }
  
  public void SetOutline(bool state) {
    r.material.SetFloat(key, state ? outlineWidth : 0f);
  }
}
