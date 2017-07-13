using UnityEngine;
using System.Collections;

[RequireComponent (typeof(LineRenderer))]
public class LaserPointer : MonoBehaviour {
  [SerializeField] private static float seconds = 2;
  public static LineRenderer line;
  
  private static int index = 0;

	void Start () {
    line = GetComponent<LineRenderer>();
    line.enabled = false;
	}

  public static IEnumerator Fire(Vector3 position, Vector3 direction) {
    FireLaser(position, direction);
    yield return new WaitForSeconds(seconds);
    TurnOff();
  }
  
  public static void AddPosition(Vector3 position, Vector3 direction) {
    Ray ray = new Ray(position, direction);
    RaycastHit hitInfo;

    Debug.Log(Physics.Raycast(ray, out hitInfo, 50));
    Debug.Log(hitInfo.transform);

    line.SetPosition(index, ray.origin);
    
    Vector3 endPoint = hitInfo.point == Vector3.zero ? ray.GetPoint(50) : hitInfo.point;
    Debug.Log(endPoint);
    line.SetPosition(++index, endPoint);
  }
	
  public static void FireLaser(Vector3 position, Vector3 direction) {
    index = 0;
    line.enabled = true;
  }

  public static void TurnOff() {
    line.enabled = false;
  }
}
