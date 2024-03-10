using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
   private float timer = 0f;
    private bool hited = false;

    /*
    // bullet controll
    private void OnCollisionEnter(Collision collision)
    {

        //to prevent double hurt player;
        if (collision.transform.CompareTag("Player") && !hited)
        {
            Debug.Log("hit");
            collision.transform.GetComponent<PlayerInteraction>().TakeDamage(PlayerInteraction.bulletDamage);
            hited = true;
            Destroy(gameObject);
        }
    }
    */
    private void OnTriggerEnter(Collider collision)
    {
        //to prevent double hurt player;
        if (collision.transform.CompareTag("Player") && !hited)
        {
            Debug.Log("hit");
            collision.transform.GetComponent<PlayerInteraction>().TakeDamage(PlayerInteraction.bulletDamage);

            //when bullet hit player , cam shake
            //CameraRequst.instance.RequestShake(1f);

            hited = true;
            Destroy(gameObject);
        }
    }

    //destroy after 8s
    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > 6f)
        {
            Destroy(gameObject);
        }
    }
}
