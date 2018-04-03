using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class Network : MonoBehaviour {

	static SocketIOComponent socket;
	public GameObject player;
	public Spawner spawner;

    void Start()
    {
        socket = GetComponent<SocketIOComponent>();
        socket.On("register", OnRegister);
        socket.On("spawn", OnSpawn);
        socket.On("move", OnMove);
        socket.On("disconnected", OnDisconnected);
    }

    private void OnRegister(SocketIOEvent obj)
    {
        Debug.Log("registered id = " + obj.data);
 //       spawner.AddPlayer(obj.data["id"].str, player);
    }
    
    private void OnSpawn(SocketIOEvent obj)
    {
        Debug.Log("Spawn " + obj.data);
        var player = spawner.SpawnPlayer(obj.data["id"].str);
        PlayerController pc = player.GetComponent<PlayerController>();
        pc.LocalPlayer = false;
    }

    private void OnMove(SocketIOEvent obj)
    {
        var position = GetVectorFromJson(obj);
        var player = spawner.GetPlayer(obj.data["id"].str);
        player.transform.position = position;
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

    public static void Move(Vector2 destination)
    {
		Debug.Log("send moving to node " + Network.VectorToJson(destination));

		JSONObject jsonObject = new JSONObject(JSONObject.Type.OBJECT);
		jsonObject.AddField("d", Network.VectorToJson(destination));
		socket.Emit("move", jsonObject);
    }
}
