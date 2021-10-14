using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupSpawner : MonoBehaviour {

    [SerializeField]
    public GameObject player; 

    [SerializeField] private GameObject _powerupPrefab;
    public float spawnRadius = 5;

    public float xPos;
    public float yPos;
    public float zPos;


    private void Start()
    {
        InvokeRepeating("spawnPowerup", 10, 10);
    }
    void Update() {
    }

    private void spawnPowerup() {
        xPos = Random.RandomRange(1, spawnRadius);
        yPos = player.transform.position.y;
        zPos = Random.RandomRange(1, spawnRadius);
        

        Vector3 spawnPosition = player.transform.position + new Vector3(xPos, yPos, zPos);
        GameObject powerup = Instantiate(_powerupPrefab, spawnPosition, Quaternion.identity);


    }
}