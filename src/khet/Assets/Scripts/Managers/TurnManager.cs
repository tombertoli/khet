using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class TurnManager {
  public delegate void TurnEvent();
  public static event TurnEvent TurnFinished;

  public static Transform Red, Silver;
  public static PieceColor Turn { get { return waiting ? PieceColor.None : turn; } }
  public static bool IsLocalGame { get; set; }

  private static PieceColor turn = PieceColor.Silver;
  private static bool waiting = false;

  public static void Wait() {
    waiting = true;
  }

  public static void EndWait() {
    waiting = false;
  }

  public static void End() {
    TurnFinished();
    
    Transform temp = turn == PieceColor.Red ? Red : Silver;
    PieceColor tempColor = turn == PieceColor.Red ? PieceColor.Silver : PieceColor.Red;

    LaserController.Fire(temp.position, temp.forward);
    turn = tempColor;
  }

  public static void Reset() {
    turn = PieceColor.Silver;
  }
}

