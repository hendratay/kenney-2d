using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class Network : MonoBehaviour {

	static SocketIOComponent socket;

	void Start()
	{
		socket = GetComponent<SocketIOComponent>();
		socket.On("user connected", OnUserConnected);
		socket.On("play", OnUserPlay);

		StartCoroutine(ConnectToServer());
	}

	IEnumerator ConnectToServer()
	{
		yield return new WaitForSeconds(0.5f);
		socket.Emit("user connect");

		yield return new WaitForSeconds(1f);
		Dictionary<string, string> data = new Dictionary<string, string>();
		data["name"] = "tay";
		Vector2 position = new Vector2(0, 0);
		data["position"] = position.x + "," + position.y;
		socket.Emit("play", new JSONObject(data));
	}

	private void OnUserConnected(SocketIOEvent obj) {
		Debug.Log("Get the message from the server is " + obj.data + "OnUserConnected");
	}

	private void OnUserPlay(SocketIOEvent obj) {
		Debug.Log("Get the message from the server is " + obj.data + "OnUserPlay");
	}

}
