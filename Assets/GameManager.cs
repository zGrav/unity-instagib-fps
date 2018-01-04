using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static GameManager instance;

	public MatchSettings matchSettings;

	void Awake() {
		if (instance) {
			Debug.LogError ("MORE THAN ONE GAMEMANAGER IN SCENE!");
		}

		instance = this;
	}

	private const string PLAYER = "Player ";

	private static Dictionary<string, PlayerManager> players = new Dictionary<string, PlayerManager>();

	public static void addPlayer(string netID, PlayerManager player) {
		string playerID = PLAYER + netID;

		players.Add (playerID, player);
		player.transform.name = playerID;
	}

	public static void deletePlayer(string playerID) {
		players.Remove (playerID);
	}

	public static PlayerManager getPlayer(string playerID) {
		return players [playerID];
	}
}
