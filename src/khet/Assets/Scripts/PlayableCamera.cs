using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class PlayableCamera : MonoBehaviour {
	[SerializeField] private float movementSpeed = 3, rotationSpeed = 1;
	private float yaw, pitch, size;
	private new Camera camera;

	public static bool IsControllable { get; set; }

	void Start() {
		camera = GetComponent<Camera>();
	}

	void Update() {
		if (!Input.GetButtonDown("Control")) return;
		
		IsControllable = !IsControllable;
		yaw = transform.eulerAngles.y;
		pitch = transform.eulerAngles.x;
	}

	void FixedUpdate () {
		if (!IsControllable) return;

		transform.Translate(transform.right * Input.GetAxis("Horizontal") * movementSpeed);

		yaw += rotationSpeed * Input.GetAxis("Mouse X");
		pitch -= rotationSpeed * Input.GetAxis("Mouse Y");
		
		transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);

		size += movementSpeed * -Input.GetAxis("Vertical");
		camera.orthographicSize = Mathf.Clamp(size, 1, 5);
	}
}
