using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(PlayerManager))]

public class PlayerSetup : NetworkBehaviour {

	[SerializeField]
	private Behaviour[] disableComponents;

	[SerializeField]
	private string remoteLayerName = "RemotePlayer";

	[SerializeField]
	private GameObject playerUIPrefab;
	[HideInInspector]
	public GameObject playerUIInstance;

	void Start() {
		if (!isLocalPlayer) {
			doDisableComponents ();
			doAssignRemoteLayer ();
		} else {
			playerUIInstance = Instantiate (playerUIPrefab);
			playerUIInstance.name = playerUIPrefab.name;
		}

		GetComponent<PlayerManager> ().Setup ();
	}

	public override void OnStartClient() {
		base.OnStartClient();

		string netID = GetComponent<NetworkIdentity> ().netId.ToString();
		PlayerManager player = GetComponent<PlayerManager> ();

		GameManager.addPlayer (netID, player);
	}

	void doDisableComponents() {
		foreach (Behaviour b in disableComponents) {
			b.enabled = false;
		}
	}

	void doAssignRemoteLayer() {
		gameObject.layer = LayerMask.NameToLayer (remoteLayerName);
	}

	void onDisable() {
		Destroy (playerUIInstance);

		GameManager.instance.setSceneCameraActive (true);

		GameManager.deletePlayer (transform.name);
	}
}
