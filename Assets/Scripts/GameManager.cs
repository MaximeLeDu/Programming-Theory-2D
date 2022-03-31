using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<Enemy> enemyPrefabs;

    public Transform allExceptPlayer;

    private float spawnRate = 2;

    private float rangeX = 20;
    private float rangeY = 12;

    private float currentTime = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if(currentTime >= spawnRate)
        {
            currentTime = 0;
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        int enemyIndex = Random.Range(0, enemyPrefabs.Count);

        Instantiate(enemyPrefabs[enemyIndex], SpawnPosition(), enemyPrefabs[enemyIndex].transform.rotation, allExceptPlayer);
    }

    private Vector2 SpawnPosition()
    {
        //The characters can appear from any 4 sides of the map
        int side = Random.Range(0, 4);
        Vector2 position = Vector2.zero;
        switch (side)
        {
            case 0:
                position = new Vector2(rangeX, Random.Range(-rangeY, rangeY));
                break;
            case 1:
                position = new Vector2(-rangeX, Random.Range(-rangeY, rangeY));
                break;

            case 2:
                position = new Vector2(Random.Range(-rangeX, rangeX), rangeY);
                break;

            case 3:
                position = new Vector2(Random.Range(-rangeX, rangeX), -rangeY);
                break;
        }
        return position;
    }
}
