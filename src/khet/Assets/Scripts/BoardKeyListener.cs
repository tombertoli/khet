using UnityEngine;
using System.Collections;

public class BoardKeyListener : MonoBehaviour {
  [SerializeField] private KeyCode turnBoardRight = KeyCode.E, turnBoardLeft = KeyCode.Q;
  [SerializeField] private float turnDelta = 5;
  private bool started;

	void Update () {
    if (!started && (Input.GetKeyDown(turnBoardRight) || Input.GetKeyDown(turnBoardLeft)) && !LaserController.IsFiring) {
      started = true;

      float finalRot = Input.GetKeyDown(turnBoardRight) ? -90 : 90;
      RotateCam(finalRot, turnDelta);
    }
  }

  private void RotateCam(float amount, float turnDelta) {
    if (Camera.main == null) return;

    Transform rotator = Camera.main.transform;
    rotator.RotateAround(Vector3.zero, Vector3.up, amount);

    started = false;
  }
}
