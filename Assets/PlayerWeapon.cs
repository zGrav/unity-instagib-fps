using UnityEngine;

[System.Serializable]
public class PlayerWeapon: MonoBehaviour {
	
	[SerializeField]
	public string weaponName = "UBER-OMEGA-LEET-MLG-RAYGUN";

	public int weaponDamage = 100;

	public float weaponRange = 300f;
}
