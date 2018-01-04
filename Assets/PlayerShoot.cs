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
		if (!cam) {
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
				CmdPlayerHit (hit.collider.name, weapon.weaponDamage);
			}
		}
	}

	[Command]
	void CmdPlayerHit(string player, int damage) {
		Debug.Log (player + " has been hit");

		PlayerManager Player = GameManager.getPlayer (player);
		Player.RpcTakeDamage (damage);
	}
}
