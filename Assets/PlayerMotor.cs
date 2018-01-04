using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class PlayerMotor : MonoBehaviour {

	[SerializeField]
	private Camera cam;

	private Vector3 velocity = Vector3.zero;
	private Vector3 rotation = Vector3.zero;

	private float cameraRotationX = 0f;
	private float currentCameraRotationX = 0f;

	[SerializeField]
	private float cameraRotationLimit = 85f;

	private Rigidbody rigidBody;

	void Start() {
		rigidBody = GetComponent<Rigidbody>();
	}

	public void Move(Vector3 vel) {
		// grab vel from PlayerController
		velocity = vel;
	}

	public void Rotate(Vector3 rot) {
		// grab rot from PlayerController
		rotation = rot;
	}

	public void RotateCamera(float camRotX) {
		// grab camRotX from PlayerController
		cameraRotationX = camRotX;
	}

	void FixedUpdate() {
		// run every physics iteration

		PerformMovement ();
		PerformRotation ();
	}

	void PerformMovement() {
		// move according to velocity

		if (velocity != Vector3.zero) {
			rigidBody.MovePosition (rigidBody.position + velocity * Time.fixedDeltaTime);
		}
	}

	void PerformRotation() {
		rigidBody.MoveRotation (rigidBody.rotation * Quaternion.Euler(rotation));

		if (cam) {
			currentCameraRotationX -= cameraRotationX;

			currentCameraRotationX = Mathf.Clamp (currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

			cam.transform.localEulerAngles = new Vector3 (currentCameraRotationX, 0f, 0f);
		}
	}
}
