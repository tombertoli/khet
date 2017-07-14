using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

  private static IEnumerator TurnOff() {
    yield return new WaitForSeconds(seconds);
    line.enabled = false;
  }
  
  public static void AddPosition(Vector3 position, Vector3 direction) {
    Ray ray = new Ray(position, direction);
    RaycastHit hitInfo;

    Physics.Raycast(ray, out hitInfo, 50);
    points.Add(ray.origin);
    
    Vector3 endPoint = hitInfo.point == Vector3.zero ? ray.GetPoint(50) : hitInfo.point;
    points.Add(endPoint);

    if (hitInfo.collider == null) return;

    PieceSetup ps = hitInfo.collider.gameObject.GetComponent<PieceSetup>();
    ps.OnLaserHit(hitInfo.point, hitInfo.normal);
  }
	
  public static void FireLaser(Vector3 position, Vector3 direction) {
    line.SetVertexCount(points.Count);
    line.SetPositions(points.ToArray());
    line.enabled = true;
    reference.StartCoroutine(TurnOff());
  }

  public static void TargetChanged() {
    points.Clear();
    line.SetVertexCount(points.Count);
    line.SetPositions(new Vector3[] { });
  }
}
