using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SocketIO;

public class NetworkManager : MonoBehaviour {

	public static NetworkManager instance;
	public SocketIOComponent socket;
	public InputField playerNameInput;
	public GameObject player;

	void Awake()
	{
		if(instance == null)
			instance = this;
		else if (instance != this)
			Destroy(gameObject);
		DontDestroyOnLoad(gameObject);
	}

	public void JoinGame()
	{
		socket.On("user connected", OnUserConnected);
		socket.On("play", OnUserPlay);
		StartCoroutine(ConnectToServer());
	}

	IEnumerator ConnectToServer()
	{
		yield return new WaitForSeconds(1f);

		socket.Emit("user connect");

		yield return new WaitForSeconds(1f);
		
		List<SpawnPoint> playerSpawnPoints  = GetComponent<PlayerSpawner>().playerSpawnPoint;

		string playerName = playerNameInput.text.ToString();;
		PlayerJSON playerJSON = new PlayerJSON(playerName, playerSpawnPoints);
		string data = JsonUtility.ToJson(playerJSON);

		socket.Emit("play", new JSONObject(data));
	}

	private void OnUserConnected(SocketIOEvent obj) {
		Debug.Log("Get the message from the server is " + obj.data + "OnUserConnected");
		String data = obj.data.ToString();
		UserJSON userJSON = UserJSON.CreateFromJSON(data);
		Vector2 position = new Vector2(userJSON.position[0], userJSON.position[1]);
		//Vector2 position = new Vector2(8, -2.5f);
		GameObject newPlayer = Instantiate(player, position, Quaternion.identity) as GameObject;
	}

	private void OnUserPlay(SocketIOEvent obj) {
		Debug.Log("Get the message from the server is " + obj.data + "OnUserPlay");
	}

	[Serializable]
	public class PlayerJSON
	{
		public string name;
		public List<PointJSON> playerSpawnPoints;
		public PlayerJSON(string _name, List<SpawnPoint> _playerSpawnPoint)
		{
			playerSpawnPoints = new List<PointJSON>();
			name = _name;
			foreach(SpawnPoint playerSpawnPoint in _playerSpawnPoint)
			{
				PointJSON pointJSON = new PointJSON(playerSpawnPoint);
				playerSpawnPoints.Add(pointJSON);
			}
		}
	}

	[Serializable]
	public class PointJSON
	{
		public float[] position;
		public PointJSON(SpawnPoint spawnPoint)
		{
			position = new float[] {
				spawnPoint.transform.position.x,
				spawnPoint.transform.position.y,
				spawnPoint.transform.position.z
			};
		}
	}

	[Serializable]
	public class UserJSON
	{
		public string name;
		public float[] position;
		public static UserJSON CreateFromJSON(string data)
		{
			return JsonUtility.FromJson<UserJSON>(data);
		}
	}

	[Serializable]
	public class EnemyJSON
	{
		public string name;
		public List<UserJSON> enemies;
		public static EnemyJSON CreateFromJSON(string data)
		{
			return JsonUtility.FromJson<EnemyJSON>(data);
		}
	}
}
