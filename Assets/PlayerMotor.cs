using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class PlayerMotor : MonoBehaviour {

	[SerializeField]
	private Camera cam;

	private Vector3 velocity = Vector3.zero;
	private Vector3 rotation = Vector3.zero;
	private Vector3 cameraRotation = Vector3.zero;

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

	public void RotateCamera(Vector3 camRot) {
		// grab camRot from PlayerController
		cameraRotation = camRot;
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

		if (cam != null) {
			cam.transform.Rotate (-cameraRotation);
		}
	}
}
