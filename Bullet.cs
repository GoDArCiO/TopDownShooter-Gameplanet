using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("SoundNames")]
    [SerializeField]
    private string[] soundsHit = null;
    [SerializeField]
    private string[] soundsMiss = null;

    private void OnCollisionEnter(Collision collision)
    {
        int random = Random.Range(0, 7);
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<EnemyBehaviour>().TakeDmg(1);
            AudioManager.instance.Play(soundsHit[random]);
        }
        else
        {
            AudioManager.instance.Play(soundsMiss[random]);
        }
        Destroy(gameObject);//destory myself on collision enter
    }
}
