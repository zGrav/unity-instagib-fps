using UnityEngine;
using UnityEngine.Networking;

public class PlayerShoot : NetworkBehaviour {

	public PlayerWeapon weapon;

	[SerializeField]
	private Camera cam;

	[SerializeField]
	private LayerMask mask;

	private const string PLAYER = "Player";

	void Start() {
		if (cam == null) {
			Debug.LogError ("PlayerShoot: No cam!");
			this.enabled = false;
		}
	}

	void Update() {
		if (Input.GetButtonDown ("Fire1")) {
			Fire ();
		}
	}

	[Client]
	void Fire() {
		RaycastHit hit;

		if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, weapon.weaponRange, mask)) {
			if (hit.collider.tag == PLAYER) {
				CmdPlayerHit (hit.collider.name);
			}
		}
	}

	[Command]
	void CmdPlayerHit(string playerName) {
		Debug.Log (playerName + " has been hit");
	}
}
