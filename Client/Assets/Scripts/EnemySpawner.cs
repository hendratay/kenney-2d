using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

	public GameObject enemy;
	public GameObject spawnPoint;
	public int numberOfEnemies;

	[HideInInspector]
	public List<SpawnPoint> enemySpawnPoints = new List<SpawnPoint>();

	void Start()
	{
		for(int i = 0; i < numberOfEnemies; i++)
		{
			var spawnPosition = new Vector2(Random.Range(-3f, 3f), -2.5f);
			var spawnRotation = Quaternion.identity;
			SpawnPoint enemySpawnPoint = (Instantiate(spawnPoint, spawnPosition, Quaternion.identity)as GameObject).GetComponent<SpawnPoint>();
			enemySpawnPoints.Add(enemySpawnPoint);
		}
		SpawnEnemies();
	}

	public void SpawnEnemies()
	{
		foreach(SpawnPoint sp in enemySpawnPoints)
		{
			Vector2 position = sp.transform.position;
			Quaternion rotation = sp.transform.rotation;
			GameObject newEnemy = Instantiate(enemy, position, rotation) as GameObject;
		}
		
	}

}
