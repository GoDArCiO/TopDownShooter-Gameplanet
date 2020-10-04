using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MockFight : MonoBehaviour
{
    [Header("settings")]
    [SerializeField]
    private float fireRate=0f;
    [SerializeField]
    private int ammo=0;

    [Header("references")]
    [SerializeField]
    private GameObject bulletFX=null;
    [SerializeField]
    private string[] soundsHit = null;

    private float fireRateTimer;

    void Update()
    {
        if (fireRateTimer > 0) fireRateTimer -= Time.deltaTime;
        if (fireRateTimer <= 0)
        {
            fireRateTimer = fireRate;//reset timer
            AudioManager.instance.Play("shoot");
            Instantiate(bulletFX, transform.position, transform.rotation);
            ammo--;
            float random = Random.Range(0.05f, 0.3f);
            Invoke("MockHitSound",random);//play the sound of bullet hit or miss after random time
        }
        if (ammo <= 0)//if ammo is used up destroy mockfight
        {
            Destroy(gameObject);
        }
    }

    void MockHitSound()
    {
        int random = Random.Range(0, 7);
        AudioManager.instance.Play(soundsHit[random]);
    }
}
