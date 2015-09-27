using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {

	public float avgTimeBetweenSpawns;
	public float spawnTimeMaxVariation;
	public GameObject[] enemies; 
	public float[]	percentChanceEnemySpawn;
	public Transform[] spawnpoints;
	public float[] percentChanceSpawnpoint;


	private float spawnTimer=0f;
	private float randomTimeVariation;
	private int enemyIndex;
	private int spawnIndex;
	private float randomNumber;
	private float minCheck;
	private float maxCheck;


	void Start(){
		randomTimeVariation = Random.Range (-spawnTimeMaxVariation, spawnTimeMaxVariation);
		spawnTimer = avgTimeBetweenSpawns + spawnTimeMaxVariation;
	}

	void Update () {
		//Increment the spawn timer...
		spawnTimer += Time.deltaTime;

		//Test to see if enough time has passed to spawn enemy...
		if(spawnTimer > (avgTimeBetweenSpawns + randomTimeVariation)){
			SpawnEnemy();
			randomTimeVariation = Random.Range (-spawnTimeMaxVariation, spawnTimeMaxVariation);
			spawnTimer = 0f;
		}
	}


	void SpawnEnemy(){
		//Set enemy...
		if(enemies.Length == 1){
			enemyIndex = 0;
		}
		else{
			//Pick a random enemy from the primary weapon list
			randomNumber = Random.value*100.0f;
			minCheck = 0.0f;
			maxCheck = 0.0f;
			if(randomNumber == 0.0f){
				enemyIndex = 0;
			}
			else if(randomNumber == 1.0f){
				enemyIndex = enemies.Length - 1; 
			}
			else{
				for (int i=0; i < enemies.Length; i++){
					maxCheck = maxCheck + percentChanceEnemySpawn[i];
					if (randomNumber >= minCheck && randomNumber < maxCheck){
						enemyIndex = i;
					}
					minCheck = maxCheck;
				}
			}
		}
		//Set spawn point...
		if (spawnpoints.Length == 1) {
			spawnIndex = 0;
		}
		else{						
			//Pick a random spawnpoint from the primary weapon list
			randomNumber = Random.value*100.0f;
			minCheck = 0.0f;
			maxCheck = 0.0f;
			if(randomNumber == 0.0f){
				spawnIndex = 0;
			}
			else if(randomNumber == 1.0f){
				spawnIndex = spawnpoints.Length - 1; 
			}
			else{
				for (int i=0; i < spawnpoints.Length; i++){
					maxCheck = maxCheck + percentChanceSpawnpoint[i];
					if (randomNumber >= minCheck && randomNumber < maxCheck){
						spawnIndex = i;
					}
					minCheck = maxCheck;
				}
			}
		}
																														
		Instantiate (enemies [enemyIndex], spawnpoints [spawnIndex].position, Quaternion.identity);
	}
}
