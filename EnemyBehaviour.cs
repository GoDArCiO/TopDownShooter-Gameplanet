using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [Header("settings")]
    [SerializeField]
    private int enemyType = 0;// 0-heavy 1-light
    [SerializeField]
    private int maxHealth = 0, dmg = 0, points = 0, speed = 0;

    [Header("References")]
    [SerializeField]
    private UnityEngine.AI.NavMeshAgent agent = null;
    [SerializeField]
    private GameObject MockFight = null;
    private GameObject player;

    [Header("Info")]
    [SerializeField]
    private int health=0;

    // set default values and find player
    void Start()
    {
        health = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player");
        agent.speed = speed;
    }

    // run to player, if is deal dmg and die if hp<1 die
    void FixedUpdate()
    {
        //pretend to be killed by player when in menu
        if (ManageUI.GameState == ManageUI.GameStates.Menu)
        {
            if (Vector3.Distance(player.transform.position, transform.position) < 5.5f)//if close to player spawn mockfightFX and destroy myself
            {
                Instantiate(MockFight);
                Die();
                return;
            }
        }
        //in GameState.Playing
        if (health < 1)
        {
            ManageUI.score += points;
            if (ManageUI.style == 3)// if style S add my health to players health (vampirism)
            {
                ManageUI.health += maxHealth;
            }
            ManageUI.killedEnemies++;
            if (ManageUI.style==1) ManageUI.score += points;
            Die();
        }
        else
        {
            if (Vector3.Distance(player.transform.position, transform.position)<1.5f)//if close to player deal dmg to him and die
            {
                ManageUI.health -= dmg;
                Die();
            }
            else
            {
                agent.SetDestination(player.transform.position);
            }
        }
    }

    void Die()
    {
        SpawnOutCamera.numberOfEnemies--;
        Destroy(gameObject);
    }

    public void TakeDmg(int dmg)
    {
        health -= dmg;
        if (ManageUI.doubleDmgL && enemyType == 1) health -= dmg;
        if (ManageUI.doubleDmgH && enemyType == 0) health -= dmg;
    }

}
