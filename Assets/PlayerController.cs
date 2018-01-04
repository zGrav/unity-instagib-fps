using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]


public class PlayerController : MonoBehaviour {

	[SerializeField]
	private float speed = 5f;

	[SerializeField]
	private float lookSensitivity = 3f;

	private PlayerMotor motor;

	void Start() {
		motor = GetComponent<PlayerMotor> ();
	}

	void Update() {
		// calc mov velocity as Vector3

		float xMovement = Input.GetAxisRaw ("Horizontal");
		float zMovement = Input.GetAxisRaw ("Vertical");

		Vector3 horizontalMovement = transform.right * xMovement;
		Vector3 verticalMovement = transform.forward * zMovement;

		Vector3 velocity = (horizontalMovement + verticalMovement).normalized * speed;

		motor.Move (velocity);

		// calc rotation as Vector3 for turning

		float yRotation = Input.GetAxisRaw ("Mouse X");

		Vector3 rotation = new Vector3 (0f, yRotation, 0f) * lookSensitivity;

		motor.Rotate (rotation);

		// calc camera rotation as Vector3 for turning

		float xRotation = Input.GetAxisRaw ("Mouse Y");

		float cameraRotationX = xRotation * lookSensitivity;

		motor.RotateCamera (cameraRotationX);
	}
}
