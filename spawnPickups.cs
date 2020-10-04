using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnPickups : MonoBehaviour
{

    [Header("references")]
    [SerializeField]
    private GameObject ammoPickup = null;
    [SerializeField]
    private GameObject dmgPickup = null;

    [Header("settings")]
    [SerializeField]
    private float MaxTimer=0f;
    private float timer = 0.0f;
    private bool empty = true;

    void FixedUpdate()
    {
        if(ManageUI.style == 2)//if we are at style A, spawn pickups twice as fast
        {
            MaxTimer = 2.5f;
        }
        else
        {
            MaxTimer = 5.0f;
        }

        if (empty)//if spawner is empty, run timer to spawn
        {
            timer -= Time.deltaTime;
        }
        
        if (timer <= 0&& empty)
        {
            int random = Random.Range(0, 2);
            if (random == 1) 
            {
                ammoPickup.SetActive(true);
            } 
            else
            {
                dmgPickup.SetActive(true);
            }
            empty = false;
        }
    }

    public void Spawn()//public function called when previous item is picked up
    {
        empty = true;
        timer = MaxTimer;
    }
}
