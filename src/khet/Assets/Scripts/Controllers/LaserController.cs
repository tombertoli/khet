using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LaserController : MonoBehaviour {
  [SerializeField] private static float waitTimeInSeconds = 2;
  public static bool fire = false;

  public delegate void LaserHit();
  public static event LaserHit Hit;

  private static LaserController instance;
  public static List<Vector3> points = new List<Vector3>();

  void Start() {
    instance = this;
  }

  public static void AddPosition(Vector3 position, Vector3 direction) {
    if (fire) return;

    Ray ray = new Ray(position, direction);
    RaycastHit hitInfo;

    Physics.Raycast(ray, out hitInfo, 50);
    points.Add(ray.origin);

    Vector3 endPoint = hitInfo.point == Vector3.zero ? ray.GetPoint(50) : hitInfo.point;

    if (hitInfo.collider == null) { 
      points.Add(endPoint);
      return;
    }

    PieceController ps = hitInfo.collider.gameObject.GetComponent<PieceController>();

    float hitY = endPoint.y;

    endPoint = ps.transform.TransformPoint(0, 0, 0);
    endPoint.y = hitY;

    points.Add(endPoint);

    ps.LaserHit(hitInfo.point, hitInfo.normal);
  }

  private static IEnumerator TurnOff() {
    yield return new WaitForSeconds(waitTimeInSeconds);
    fire = false;

    PieceController.UpdateProbes();

    if (Hit != null) Hit();
    TurnManager.EndWait();
  }

  public static void Fire(Vector3 position, Vector3 direction) {
    if (fire) return;

    TargetChanged();
    AddPosition(position, direction);
    fire = true;

    PieceController.UpdateProbes();

    instance.StartCoroutine(TurnOff());
  }

  private static void TargetChanged() {
    if (fire) {
      instance.StartCoroutine(WaitForOff());
      return;
    }

    points.Clear();
  }

  private static IEnumerator WaitForOff() {
    while (fire) yield return null;

    TargetChanged();
  }  
}
