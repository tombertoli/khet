using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//[RequireComponent(typeof(LineRenderer))]
public class LaserController : MonoBehaviour {
  [SerializeField] private static float waitTimeInSeconds = 2;
  public static bool IsFiring { 
    get { return IsFiring; }
    private set { 
        for (int i = 0; i < lines.Count; i++)
          lines[i].enabled = false;
     }
  }
  //public static LineRenderer line;

  public delegate void LaserHit();
  public static event LaserHit Hit;

  private static LaserController instance;
  private static List<Vector3> points = new List<Vector3>();
  private static List<LineRenderer> lines = new List<LineRenderer>();

  void Start() {
    //line = GetComponent<LineRenderer>();
    //line.enabled = false;
    instance = this;
  }

  public static void AddPosition(Vector3 position, Vector3 direction) {
    if (IsFiring) return;

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
    IsFiring = false;

    PieceController.UpdateProbes();

    if (Hit != null) Hit();
    TurnManager.EndWait();
  }

  public static void Fire(Vector3 position, Vector3 direction) {
    if (IsFiring) return;

    TargetChanged();
    AddPosition(position, direction);
    //line.SetVertexCount(points.Count);
    //line.SetPositions(points.ToArray());
    IsFiring = true;

    PieceController.UpdateProbes();

    instance.StartCoroutine(TurnOff());
  }

  private static void TargetChanged() {
    if (IsFiring) {
      instance.StartCoroutine(WaitForOff());
      return;
    }

    points.Clear();
    //line.SetVertexCount(points.Count);
    //line.SetPositions(points.ToArray());
  }

  private static IEnumerator WaitForOff() {
    while (IsFiring) yield return null;

    TargetChanged();
  }
}