using UnityEngine;
using System.Collections;

public struct Point {
  public int x;
  public int y;

  public Point(int x, int y) {
    this.x = x;
    this.y = y;
  }

  public override string ToString() {
    return string.Format("[{0}, {1}]", x, y);
  }

  public static bool operator ==(Point left, Point right) {
    if (left.x == right.x && left.y == right.y) return true;
    else return false;
  }

  public static bool operator !=(Point left, Point right) {
    if (left.x != right.x && left.y != right.y) return true;
    else return false;
  }

  public override bool Equals(object obj) {
    Point point;
    
    if (obj is Point) point = (Point)obj;
    else return false;
    
    if (point == this) return true;
    else return false;
  }

  public override int GetHashCode() {
    return x.GetHashCode() + y.GetHashCode();
  }
}