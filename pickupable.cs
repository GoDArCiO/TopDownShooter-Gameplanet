using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickupable : MonoBehaviour
{
    [SerializeField]
    private int item=0;// 0 - ammo 1 - doubleDmg

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (item == 0)
            {
                ManageUI.ammo += 20;
            }
            else
            {
                int random = Random.Range(0, 2);
                ManageUI.DoubleDamageStart(random);//random is 0 - bonus light enemies, when its 1 bonus heavy
            }
            AudioManager.instance.Play("pickup");
            gameObject.transform.parent.GetComponent<spawnPickups>().Spawn();//send msg to spawner to spawn another pickup
            gameObject.SetActive(false);//disable pickup
        }
    }
}
