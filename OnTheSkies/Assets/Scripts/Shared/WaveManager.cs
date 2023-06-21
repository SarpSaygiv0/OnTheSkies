using System.Collections;
using UnityEngine;

// Handles spawning of enemies in in packs
public class WaveManager : MonoBehaviour
{
	public Transform[] spawnPoints;
	public GameObject[] enemies;

	public Vector2 spawnTimeRange = new Vector2(5f, 10f);   // after spawn will occur
	public int initialUnit = 2;                             //  units to spawn
	public int incremental = 1;                             // increase in units count after spawn

	private void Start()
	{
		StartCoroutine(SpawnerCoroutine());
	}

	IEnumerator SpawnerCoroutine()
	{
		while (true)    // infinite loop to keep spawning while playing
		{
			yield return new WaitForSeconds(Random.Range(spawnTimeRange.x, spawnTimeRange.y));

			// spawning random enemie at random spawn point until packs full
			for (int i = 0; i < initialUnit; i++)
			{
				Instantiate(enemies[Random.Range(0, enemies.Length)],
				spawnPoints[Random.Range(0, spawnPoints.Length)].position,
				Quaternion.identity);

				// adding to stop spawning on each othrt
				yield return new WaitForSeconds(1f);
			}

			// making more difficult
			initialUnit += incremental;
		}
	}
}
