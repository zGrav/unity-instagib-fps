using UnityEngine;

[System.Serializable]
public class PlayerWeapon: MonoBehaviour {
	
	[SerializeField]
	public string weaponName = "UBER-OMEGA-LEET-MLG-RAYGUN";

	public float weaponDamage = 10f;
	public float weaponRange = 300f;
}
