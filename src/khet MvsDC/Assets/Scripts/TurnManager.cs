using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class TurnManager {
  public delegate void TurnEvent();
  public static event TurnEvent TurnFinished;
  public static Transform Red, Silver;

  private static PieceColor turn = PieceColor.Silver;
  private static bool waiting = false;

  public static void WaitTurn() {
    waiting = true;
  }

  public static void EndWaitTurn() {
    waiting = false;
  }

  public static void EndTurn () {
    TurnFinished();

    Transform temp = turn == PieceColor.Red ? Red : Silver;
    PieceColor tempColor = turn == PieceColor.Red ? PieceColor.Silver : PieceColor.Red;

    LaserPointer.FireLaser(temp.position, temp.forward);
    turn = tempColor;
  }  

  public static PieceColor GetTurn() {
    return waiting ? PieceColor.None : turn;
  }
}

