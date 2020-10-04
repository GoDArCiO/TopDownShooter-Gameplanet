using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    private GameObject[] enemies;

    [SerializeField]
    private GameObject explosionFX = null;

    [SerializeField]
    private float exploDistance=0f;

    private void OnCollisionEnter(Collision collision)
    {
        AudioManager.instance.Play("missileHit");
        Instantiate(explosionFX, transform.position, transform.rotation);
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        //deal dmg to every enemy that is closer than explodistance
        foreach (GameObject enemy in enemies)
        {
            if (Vector3.Distance(enemy.transform.position, transform.position) < exploDistance)
            {
                enemy.gameObject.GetComponent<EnemyBehaviour>().TakeDmg(10);
            }
        }
        Destroy(gameObject);//destroy myself
    }
}
