using UnityEngine;
using System.Collections;

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
    pieceColor = GetComponent<PieceSetup>().Piece.Color;
  }

  void Update() { 
    if (!Input.GetButtonDown(button)) return;
    
    if (over) permanent = !permanent;
    else { 
      permanent = false;
      SetOutline(false);
    }
  }

  void OnMouseEnter() { 
    if (pieceColor == TurnManager.Turn && !permanent) SetOutline(true);
    over = true; 
  }

  void OnMouseOver() {
    if (over) return;
    over = true;
  }

  void OnMouseExit() { 
    if (pieceColor == TurnManager.Turn && !permanent) SetOutline(false);
    over = false; 
  }

  public static void SetRange(bool setTo, GameObject[] gos) {
    for (int i = 0; i < gos.Length; i++) {
      Glow glow = gos[i].GetComponent<Glow>();

      if (glow == null) {
        Debug.Log("nulleadisimo");
        continue;
      }

      glow.SetOutline(setTo);
    }
  }
  
  public void SetOutline(bool state) {
    r.material.SetFloat(key, state ? outlineWidth : 0f);
  }
}
