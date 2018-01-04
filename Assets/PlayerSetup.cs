using UnityEngine;
using UnityEngine.Networking;

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
			if (sceneCamera != null) {
				sceneCamera.gameObject.SetActive (false);
			}
		}

		setPlayerName ();
	}

	void setPlayerName() {
		string ID = "Player " + GetComponent<NetworkIdentity> ().netId;
		transform.name = ID;
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
		if (sceneCamera != null) {
			sceneCamera.gameObject.SetActive (true);
		}
	}
}
