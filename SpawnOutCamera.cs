using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SpawnOutCamera : MonoBehaviour
{
    [Header("references")]
    [SerializeField]
    private Camera cam = null;
    [SerializeField]
    private GameObject[] EnemyPrefabs = null;

    [Header("settings")]
    [SerializeField]
    private float spawnTime = 0f;
    [SerializeField]
    private int maxNumberOfEnemies=0;
    [SerializeField]
    private int startNumberOfEnemies=0;

    private float timer;
    public static int numberOfEnemies;
    public static bool onlyOnceOnPlay;
    private Vector3 pos;

    void FixedUpdate()
    {
        //start the game with startNumberOfEnemies
        if (ManageUI.GameState == ManageUI.GameStates.Playing && onlyOnceOnPlay)
        {
            while (numberOfEnemies < startNumberOfEnemies)
            {
                SpawnEnemy();
            }
            onlyOnceOnPlay = false;
        }

        if (timer< spawnTime)
        {
            timer += Time.deltaTime;
        }
        else
        {
            timer = 0;
            if (numberOfEnemies < maxNumberOfEnemies)
            {
                SpawnEnemy();
            }
        }
    }

    void SpawnEnemy()
    {
            numberOfEnemies++;
            
            float distance = 30.0f;//hardcoded distance from camera so enemy spawn outside of player vision
            //try to get a position that will be far from camera 
            do
            {
                pos = new Vector3(Random.Range(-21, 21), 0, Random.Range(-21, 21));

            } while (Vector3.Distance(gameObject.transform.position + pos, cam.transform.position) < distance);

        int random = Random.Range(0, 2);
        Instantiate(EnemyPrefabs[random], pos, transform.rotation);
    }
}


