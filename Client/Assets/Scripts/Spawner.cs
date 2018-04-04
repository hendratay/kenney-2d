using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class Spawner : MonoBehaviour {

    public GameObject currentPlayer;
	public GameObject networkPlayer;
    public SocketIOComponent socket;
    private Dictionary<string, GameObject> players = new Dictionary<string, GameObject>();

    public GameObject SpawnPlayer(string id)
    {
        GameObject player = Instantiate(networkPlayer, Vector2.zero, Quaternion.identity) as GameObject;
        AddPlayer(id, player);
        return player;
    }

    public GameObject GetPlayer(string id)
    {
        return players[id];
    }

    public void AddPlayer(string id, GameObject player)
    {
        players.Add(id, player);
    }

    public void Remove(string id)
    {
        var player = players[id];
        Destroy(player);
        players.Remove(id);
	}
}
