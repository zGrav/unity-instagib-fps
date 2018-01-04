using UnityEngine;
using UnityEngine.Networking;

public class PlayerSetup : NetworkBehaviour {

	[SerializeField]
	Behaviour[] disableComponents;

	Camera sceneCamera;

	void Start() {
		if (!isLocalPlayer) {
			foreach (Behaviour b in disableComponents) {
				b.enabled = false;
			}
		} else {
			sceneCamera = Camera.main;
			if (sceneCamera != null) {
				sceneCamera.gameObject.SetActive (false);
			}
		}
	}

	void onDisable() {
		if (sceneCamera != null) {
			sceneCamera.gameObject.SetActive (true);
		}
	}
}
