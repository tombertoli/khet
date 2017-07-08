using UnityEngine;
using System.Collections;

public class KeyListener : MonoBehaviour {
  [SerializeField] private KeyCode turnBoardRight, turnBoardLeft;
  [SerializeField] private Transform board;
  [SerializeField] private float turnSpeed;

	// Update is called once per frame
	void Update () {
	   if (Input.GetKeyDown(turnBoardRight)) {
		 Quaternion finalRot = Quaternion.Euler(0, 90, 0);
         StartCoroutine(DoRotation(board, finalRot, turnSpeed));
       }

	   if (Input.GetKeyDown(turnBoardLeft)) {
	     Quaternion finalRot = Quaternion.Euler(0, -90, 0);
	     StartCoroutine(DoRotation(board, finalRot, turnSpeed));
	   }
	}

	IEnumerator DoRotation(Transform rotator, Quaternion addRotation, float turnSpeed) {
	  Quaternion sumRotation = rotator.rotation * addRotation;
		Debug.Log(sumRotation.eulerAngles);

      while (rotator.rotation != sumRotation) {
		rotator.rotation = Quaternion.LerpUnclamped(rotator.rotation, sumRotation, turnSpeed * Time.deltaTime);
        yield return null;
      }
  }
}
