using UnityEngine;
using System.Collections;

public class BoardKeyListener : MonoBehaviour {
  [SerializeField] private KeyCode turnBoardRight = KeyCode.E, turnBoardLeft = KeyCode.Q;
  [SerializeField] private float turnDelta = 5;
  private bool started;

	// Update is called once per frame
	void Update () {
    if (!started && (Input.GetKeyDown(turnBoardRight) || Input.GetKeyDown(turnBoardLeft))) {
      started = true;

      Quaternion finalRot = Input.GetKeyDown(turnBoardRight) ? Quaternion.Euler(0, 90, 0) : Quaternion.Euler(0, -90, 0);
      StartCoroutine(DoRotation(transform, finalRot, turnDelta));
    }
  }

  /// <summary>
  /// Hace una rotacion desde la base del transform hasta eso + addRotation,
  ///  teninedo como maximo turnDelta grados por frame
  /// </summary>
  /// <param name="rotator">El transform que se esta rotando.</param>
  /// <param name="addRotation">La rotacion que se quiere agregar.</param>
  /// <param name="turnDelta">Maximo de grados por frame.</param>
  IEnumerator DoRotation(Transform rotator, Quaternion addRotation, float turnDelta) {
    Quaternion sumRotation = rotator.rotation * addRotation;

    while (rotator.rotation != sumRotation) {
      rotator.rotation = Quaternion.RotateTowards(rotator.rotation, sumRotation, turnDelta * Time.deltaTime);

      yield return null;
    }

    started = false;
  }
}
