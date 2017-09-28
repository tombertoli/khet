using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class TurnManager {
  public static PieceColor turn = PieceColor.Silver;// = PïeceColor.None;
  public static Transform Red, Silver;

  public static void EndTurn () {
    Debug.Log("xD");
    
    if (turn == PieceColor.Red) {
      LaserPointer.FireLaser(Red.position, Red.forward);
      turn = PieceColor.Silver;
    } else if (turn == PieceColor.Silver) {
      LaserPointer.FireLaser(Silver.position, Silver.forward);
      turn = PieceColor.Red;
    }
  }
}

