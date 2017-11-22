using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof(Collider), typeof(Renderer))]
public class Glow : MonoBehaviour {
  [SerializeField] private string key, button = "Fire1";
  [SerializeField] private float outlineWidth;

  private PieceColor pieceColor;
  private Renderer r;
  private bool permanent, over;

  void Start() {
    TurnManager.TurnFinished += () => SetRange(false, GameObject.FindObjectsOfType<Glow>());

    r = GetComponent<Renderer>();
    pieceColor = GetComponent<PieceController>().Piece.Color;
  }

  void Update() { 
    if (!NetworkController.AllPlayersConnected) return;
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
    if (!TurnManager.IsLocalGame && pieceColor != NetworkController.Color) return;
    if (!NetworkController.AllPlayersConnected) return;

    SetOutline(true);
  }

  void OnMouseOver() {
    if (!over) over = true;

    if (permanent) return;
    if (pieceColor != TurnManager.Turn) return;
    if (!TurnManager.IsLocalGame && pieceColor != NetworkController.Color) return;
    if (!NetworkController.AllPlayersConnected) return;

    SetOutline(true);
  }

  void OnMouseExit() { 
    over = false; 

    if (permanent) return;
    if (pieceColor != TurnManager.Turn) return;
    if (!TurnManager.IsLocalGame && pieceColor != NetworkController.Color) return;
    if (!NetworkController.AllPlayersConnected) return;

    SetOutline(false);
  }

  public static void SetRange(bool setTo, Glow[] gos) {
    for (int i = 0; i < gos.Length; i++)
      gos[i].SetOutline(setTo);
  }
  
  public void SetOutline(bool state) {
    r.material.SetFloat(key, state ? outlineWidth : 0f);
  }
}
