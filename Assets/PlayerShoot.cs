using UnityEngine;
using UnityEngine.Networking;

public class PlayerShoot : NetworkBehaviour {

	public PlayerWeapon weapon;

	[SerializeField]
	private Camera cam;

	[SerializeField]
	private LayerMask mask;

	[SerializeField]
	private GameObject shootEffect;

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

		GameObject gfxIns = (GameObject)Instantiate (shootEffect, cam.transform.position, cam.transform.rotation, cam.transform);

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
