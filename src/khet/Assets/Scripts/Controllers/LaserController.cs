using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LaserController : MonoBehaviour {
  private static float waitTimeInSeconds = 2;
  
  #pragma warning disable 0649
  [SerializeField] private GameObject prefab;
  #pragma warning restore 0649

  private static bool isFiring = false;
  public static bool IsFiring { 
    get { return isFiring; }
    private set { 
      isFiring = value;
      for (int i = 0; i < lines.Count; i++)
        if (lines[i] != null)
          lines[i].enabled = value;
     }
  }

  public delegate void LaserHit();
  public static event LaserHit Hit;

  private static LaserController instance;
  private static Dictionary<LineRenderer, List<Vector3>> positions = new Dictionary<LineRenderer, List<Vector3>>();
  private static List<LineRenderer> lines = new List<LineRenderer>();

  void Start() {
    instance = this;
  }

  public static void AddPosition(Vector3 position, Vector3 direction) {
    if (IsFiring) return;

    LineRenderer current = GetRenderer();

    Ray ray = new Ray(position, direction);
    RaycastHit hitInfo;

    Physics.Raycast(ray, out hitInfo, 50);

    positions[current].Add(ray.origin);
    current.SetPosition(0, ray.origin);

    Vector3 endPoint = hitInfo.point == Vector3.zero ? ray.GetPoint(50) : hitInfo.point;

    if (hitInfo.collider == null) { 
      positions[current].Add(ray.origin);
      current.SetPosition(1, endPoint);
      return;
    }

    PieceController ps = hitInfo.collider.gameObject.GetComponent<PieceController>();

    float hitY = endPoint.y;

    endPoint = ps.transform.TransformPoint(0, 0, 0);
    endPoint.y = hitY;

    positions[current].Add(ray.origin);
    current.SetPosition(1, endPoint);

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
    IsFiring = true;

    PieceController.UpdateProbes();
    instance.StartCoroutine(TurnOff());
  }

  private static void TargetChanged() {
    if (IsFiring) {
      instance.StartCoroutine(WaitForOff());
      return;
    }

    for (int i = 0; i < lines.Count; i++) {
      if (lines[i] == null) {
        lines.Remove(lines[i]);
        continue; 
      }
      
      Destroy(lines[i].gameObject);
    }

    lines.Clear();
    positions.Clear();
  }

  private static IEnumerator WaitForOff() {
    while (IsFiring) yield return null;

    TargetChanged();
  }

  private static LineRenderer GetRenderer() {
    if (lines.Count != 0 && positions[lines[lines.Count - 1]].Count < 2) return lines[lines.Count - 1];

    GameObject lr = Instantiate(instance.prefab) as GameObject;
    LineRenderer renderer = lr.GetComponent<LineRenderer>();
    lr.transform.parent = instance.transform;

    lines.Add(renderer);
    positions.Add(lines[lines.Count - 1], new List<Vector3>());

    return renderer;
  }
}