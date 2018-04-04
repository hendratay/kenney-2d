using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using UnityEngine.UI;

public class Network : MonoBehaviour {

	static SocketIOComponent socket;
	public GameObject currentPlayer;
	public Spawner spawner;
    public InputField playerName;

    void Start()
    {
        socket = GetComponent<SocketIOComponent>();
        socket.On("register", OnRegister);
        socket.On("spawn", OnSpawn);
        socket.On("move", OnMove);
        socket.On("shoot", OnShoot);
        socket.On("disconnected", OnDisconnected);
    }

    public void JoinGame()
    {
        string name = playerName.text.ToString();
        float health = currentPlayer.GetComponent<PlayerHealth>().currentHealth;
        socket.Emit("join", DataToJson(name, health));
    }

    public void OnRegister(SocketIOEvent obj)
    {
        Debug.Log("registered id = " + obj.data);
        spawner.AddPlayer(obj.data["id"].str, currentPlayer);
    }
    
    private void OnSpawn(SocketIOEvent obj)
    {
        Debug.Log("Spawn " + obj.data);
        var player = spawner.SpawnPlayer(obj.data["id"].str);
        PlayerController pc = player.GetComponent<PlayerController>();
        var move = GetVectorFromJson(obj);
        player.transform.position = move;
    }

    private void OnMove(SocketIOEvent obj)
    {
        var position = GetVectorFromJson(obj);
        var player = spawner.GetPlayer(obj.data["id"].str);
        player.transform.position = position;
    }

    private void OnShoot(SocketIOEvent obj)
    {
		var player = spawner.GetPlayer(obj.data["id"].str);
		PlayerController pc = player.GetComponent<PlayerController>();
        pc.Fire();
    }

    private void OnDisconnected(SocketIOEvent obj)
    {
        var disconnectedId = obj.data["id"].str;
        spawner.Remove(disconnectedId);
    }
    
    private static Vector2 GetVectorFromJson(SocketIOEvent obj)
    {
        return new Vector2(obj.data["x"].n, obj.data["y"].n);
    }

    public static JSONObject VectorToJson(Vector2 vector)
    {
        JSONObject jsonObject = new JSONObject(JSONObject.Type.OBJECT);
        jsonObject.AddField("x", vector.x);
        jsonObject.AddField("y", vector.y);
        return jsonObject;
    }

    public static JSONObject DataToJson(string name, float health)
    {
        JSONObject jsonObject = new JSONObject(JSONObject.Type.OBJECT);
        jsonObject.AddField("name", name);
        jsonObject.AddField("health", health);
        return jsonObject;
    }

    public static void Move(Vector3 current, Vector3 destination)
    {
		Debug.Log("send moving to node " + Network.VectorToJson(destination));

		JSONObject jsonObject = new JSONObject(JSONObject.Type.OBJECT);
		jsonObject.AddField("c", Network.VectorToJson(current));
		jsonObject.AddField("d", Network.VectorToJson(destination));
		socket.Emit("move", jsonObject);
    }

    public static void Shoot(Vector3 current)
    {
		JSONObject jsonObject = new JSONObject(JSONObject.Type.OBJECT);
		jsonObject.AddField("shoot", Network.VectorToJson(current));
        socket.Emit("shoot", jsonObject);
    }
}
