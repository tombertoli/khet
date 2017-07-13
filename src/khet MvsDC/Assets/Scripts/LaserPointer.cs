using UnityEngine;
using System.Collections;

public class LaserPointer : MonoBehaviour {
  public LineRenderer line;

	// Use this for initialization
	void Start () {
    line.enabled = false;
	}

  void Update() {
    if (Input.GetMouseButtonDown(0)) {
      StartCoroutine(Fire());
    }
  }

  private IEnumerator Fire() {
    FireLaser();
    yield return new WaitForSeconds(2);
    TurnOff();
  }
	
  public void FireLaser() {
    Ray ray = new Ray(transform.position, transform.forward);
    RaycastHit hitInfo;

    Physics.Raycast(ray, out hitInfo, 50);


    line.SetPosition(0, ray.origin);
    line.SetPosition(1, hitInfo.point);
  }

  public void TurnOff() {
    line.enabled = false;
  }
}
