using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(PlayerManager))]

public class PlayerSetup : NetworkBehaviour {

	[SerializeField]
	private Behaviour[] disableComponents;

	[SerializeField]
	private string remoteLayerName = "RemotePlayer";

	private Camera sceneCamera;

	void Start() {
		if (!isLocalPlayer) {
			doDisableComponents ();
			doAssignRemoteLayer ();
		} else {
			sceneCamera = Camera.main;
			if (sceneCamera) {
				sceneCamera.gameObject.SetActive (false);
			}
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
		if (sceneCamera) {
			sceneCamera.gameObject.SetActive (true);
		}

		GameManager.deletePlayer (transform.name);
	}
}
