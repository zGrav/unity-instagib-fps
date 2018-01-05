using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

[RequireComponent(typeof(PlayerSetup))]
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

	[SerializeField]
	private GameObject[] disableGameObjectsOnRIP;

	[SerializeField]
	private GameObject deathEffect;

	[SerializeField]
	private GameObject spawnEffect;

	private bool isFirstSetup = true;

	public void Setup() {
		if (isLocalPlayer) {
			GameManager.instance.setSceneCameraActive (false);
			GetComponent<PlayerSetup> ().playerUIInstance.SetActive (true);
		}

		CmdNewPlayer ();
	}

	[Command]
	private void CmdNewPlayer() {
		RpcNewPlayer ();
	}

	[ClientRpc]
	private void RpcNewPlayer() {

		if (isFirstSetup) {
			wasEnabledAtTimeOfRIP = new bool[disableOnRIP.Length];

			for (int i = 0; i < wasEnabledAtTimeOfRIP.Length; i++) {
				wasEnabledAtTimeOfRIP [i] = disableOnRIP [i].enabled;
			}

			isFirstSetup = false;
		}

		setDefaults ();
	}

	public void setDefaults() {
		RIP = false;

		currentHealth = health;

		for (int i = 0; i < disableOnRIP.Length; i++) {
			disableOnRIP [i].enabled = wasEnabledAtTimeOfRIP [i];
		}

		for (int i = 0; i < disableGameObjectsOnRIP.Length; i++) {
			disableGameObjectsOnRIP [i].SetActive (true);
		}

		Collider c = GetComponent<Collider> ();

		if (!c) {
			c.enabled = true;
		}

		GameObject gfxIns = (GameObject)Instantiate (spawnEffect, transform.position, Quaternion.identity);
		Destroy (gfxIns, 1f);
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

		for (int i = 0; i < disableGameObjectsOnRIP.Length; i++) {
			disableGameObjectsOnRIP [i].SetActive (false);
		}

		Collider c = GetComponent<Collider> ();

		if (!c) {
			c.enabled = false;
		}

		GameObject gfxIns = (GameObject)Instantiate (deathEffect, transform.position, Quaternion.identity);
		Destroy (gfxIns, 3f);

		if (isLocalPlayer) {
			GameManager.instance.setSceneCameraActive (true);
			GetComponent<PlayerSetup> ().playerUIInstance.SetActive (false);
		}

		Debug.Log (transform.name + " is RIP!");

		StartCoroutine (respawn ());
	}

	private IEnumerator respawn() {
		yield return new WaitForSeconds (GameManager.instance.matchSettings.respawnTime);

		Transform spawnPoint = NetworkManager.singleton.GetStartPosition ();
		transform.position = spawnPoint.position;
		transform.rotation = spawnPoint.rotation;

		yield return new WaitForSeconds (1f);

		Setup();

		Debug.Log (transform.name + " - RESPAWN!");
	}
}
