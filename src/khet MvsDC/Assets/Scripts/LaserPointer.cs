using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[RequireComponent (typeof(LineRenderer))]
public class LaserPointer : MonoBehaviour {
  [SerializeField] private static float seconds = 2;
  public static LineRenderer line;

  private static LaserPointer reference;  
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
    Debug.Log(hitInfo.collider.gameObject);

    PieceSetup ps = hitInfo.collider.gameObject.GetComponent<PieceSetup>();
    ps.OnLaserHit(hitInfo.point, hitInfo.normal);
  }

  private static IEnumerator TurnOff() {
    yield return new WaitForSeconds(seconds);
    line.enabled = false;

    PieceSetup[] pss = FindObjectsOfType<PieceSetup>();

    for(int i = 0; i < pss.Length; i++) {
      if (pss[i].willDestroyOnLaser) {
        Destroy(pss[i].gameObject);
        break;
      }
    }
  }
	
  public static void FireLaser(Vector3 position, Vector3 direction) {
    if (line.enabled) return;

    TargetChanged();
    AddPosition(position, direction);
    line.SetVertexCount(points.Count);
    line.SetPositions(points.ToArray());
    line.enabled = true;

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
