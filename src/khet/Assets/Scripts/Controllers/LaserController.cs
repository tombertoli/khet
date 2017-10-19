using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[RequireComponent (typeof(LineRenderer))]
public class LaserController : MonoBehaviour {
  [SerializeField] private static float seconds = 2;
  public static LineRenderer line;

  public delegate void LaserHit();
  public static event LaserHit Hit;

  private static LaserController reference;  
  private static List<Vector3> points = new List<Vector3>();

  void Start () {
    line = GetComponent<LineRenderer>();
    line.enabled = false;
    reference = this;
  }
  
public static void AddPosition(Vector3 position, Vector3 direction) {
    if (line.enabled) return;

    Ray ray = new Ray(position, direction);
    RaycastHit hitInfo;

    Physics.Raycast(ray, out hitInfo, 50);
    points.Add(ray.origin);

    Vector3 endPoint = hitInfo.point == Vector3.zero ? ray.GetPoint(50) : hitInfo.point;
    points.Add(endPoint);

    if (hitInfo.collider == null) return;

    PieceController ps = hitInfo.collider.gameObject.GetComponent<PieceController>();
    ps.LaserHit(hitInfo.point, hitInfo.normal);
  }

  public static void AddPositionDirty(Vector3 position) {
    if (line.enabled) return;

    points.Add(position);
  }

  private static IEnumerator TurnOff() {
    yield return new WaitForSeconds(seconds);
    line.enabled = false;

    PieceController.UpdateProbes();

    if (Hit != null) Hit();
    TurnManager.EndWait();
  }
	
  public static void Fire(Vector3 position, Vector3 direction) {
    if (line.enabled) return;

    TargetChanged();
    AddPosition(position, direction);
    line.SetVertexCount(points.Count);
    line.SetPositions(points.ToArray());
    line.enabled = true;

    PieceController.UpdateProbes();

    reference.StartCoroutine(TurnOff());
  }

  private static void TargetChanged() {
    if (line.enabled) {
      reference.StartCoroutine(WaitForOff());
      return;
    }

    points.Clear();
    line.SetVertexCount(points.Count);
    line.SetPositions(points.ToArray());
  }

  private static IEnumerator WaitForOff() {
    while (line.enabled) yield return null;

    TargetChanged();
  }
}
