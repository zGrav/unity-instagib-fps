using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerManager : NetworkBehaviour {

	[SerializeField]
	private int health = 100;

	[SyncVar]
	private int currentHealth;

	[SyncVar]
	private bool RIP = false;
	public bool isRIP {
		get { return RIP; }
		protected set { RIP = value; }
	}

	[SerializeField]
	private Behaviour[] disableOnRIP;
	private bool[] wasEnabledAtTimeOfRIP;

	public void Setup() {
		wasEnabledAtTimeOfRIP = new bool[disableOnRIP.Length];

		for (int i = 0; i < wasEnabledAtTimeOfRIP.Length; i++) {
			wasEnabledAtTimeOfRIP [i] = disableOnRIP [i].enabled;
		}

		setDefaults ();
	}

	public void setDefaults() {
		RIP = false;

		currentHealth = health;

		for (int i = 0; i < disableOnRIP.Length; i++) {
			disableOnRIP [i].enabled = wasEnabledAtTimeOfRIP [i];
		}

		Collider c = GetComponent<Collider> ();

		if (!c) {
			c.enabled = false;
		}
	}

	[ClientRpc]
	public void RpcTakeDamage(int damage) {

		if (RIP)
			return;
		
		currentHealth -= damage;

		Debug.Log(transform.name + " hit for " + damage + " damage, now has " + currentHealth + " health.");

		if (currentHealth <= 0) {
			die ();
		}
	}

	private void die() {
		RIP = true;

		for (int i = 0; i < disableOnRIP.Length; i++) {
			disableOnRIP [i].enabled = false;
		}

		Collider c = GetComponent<Collider> ();

		if (!c) {
			c.enabled = false;
		}

		Debug.Log (transform.name + " is RIP!");

		StartCoroutine (respawn ());
	}

	private IEnumerator respawn() {
		yield return new WaitForSeconds (GameManager.instance.matchSettings.respawnTime);

		setDefaults();

		Transform spawnPoint = NetworkManager.singleton.GetStartPosition ();
		transform.position = spawnPoint.position;
		transform.rotation = spawnPoint.rotation;

		Debug.Log (transform.name + " - RESPAWN!");
	}
}
