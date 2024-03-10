using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Cinemachine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Health Bar")]
    public float health;
    public float maxHealth = 100;
    //public List<Image> healthImages = new List<Image>();
    public float damagePerSecond = 10f;
    public float healPerSecond = 10f;
    public Image healthBar;
    public static float bulletDamage = 20f;

    private float inDarkTime;
    private float inLightTime;
    public Enemy enemyInteractWith;
    public Level0 letter;

    private bool hited;

    private CinemachineImpulseSource impulseSource;
    //music
    public AudioClip playerHurtAudio;

    //血条闪烁

   public float blinkInterval = 0.1f;
   public Image image;
   private Color originalColor;
   private bool canBlink = false;




    private void Start()
    {
        health = maxHealth;
        impulseSource = GetComponent<CinemachineImpulseSource>();
        originalColor = image.color;

    }
    

    // Update is called once per frame
    void Update()
    {
        DetectionInDarkTime();
        DetectionInLightTime();
        // show health
        if (healthBar != null)
        {
            healthBar.fillAmount = health / maxHealth;
        }
        if (health <= 0)
        {

            RestartGame();
        }


        if (Input.GetMouseButtonDown(0))
            {

            Debug.Log("1");

                impulseSource.GenerateImpulse();

            }
      
        //HK
        //血量低于30时，血条闪烁
        if (health <= 50 )
        {   canBlink = true;
           StartCoroutine(Blink());
        }
        else
        {
            canBlink = false;
            image.color = originalColor;
        }

}

    
    //EnterShadow Kill enemy
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Shadow")

        {
            
            if (other.transform.parent != null)
            {
//                Debug.Log(enemyInteractWith);
                //HK
                //grandparent is the enemy

                enemyInteractWith = other.transform.parent.parent.GetComponent<Enemy>();
                //every second repeat one time this function
                EnemyTakeDamage();
                // Destroy(other.transform.parent.gameObject);
                
            }


            
            
           // Debug.Log("hhh");
        }

        if (other.transform.parent.parent.tag == "E" || other.transform.parent.parent.tag == "G" || other.transform.parent.parent.tag == "U" || other.transform.parent.parent.tag == "I" || other.transform.parent.parent.tag == "D" || other.transform.parent.parent.tag == "G1" || other.transform.parent.parent.tag == "G2")
        {
//            Debug.Log("yeye");
            if (other.transform.parent != null)
            {
                //                Debug.Log(enemyInteractWith);
                //HK
                //grandparent is the enemy

                letter = other.transform.parent.parent.GetComponent<Level0>();
                //every second repeat one time this function
                LetterTakeDamage();
                // Destroy(other.transform.parent.gameObject);

            }




            // Debug.Log("hhh");
        }
        else
        {
            enemyInteractWith = null;
            letter = null;

        }

    }
    

    /*
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Shadow") && !hited)
        {
            Debug.Log("hit");
           // other.transform.GetComponent<PlayerInteraction>().TakeDamage(PlayerInteraction.bulletDamage);

            //when bullet hit player , cam shake
            //            CameraRequst.instance.RequestShake(1f);

            hited = true;
           // Destroy(gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Shadow") )
        {
            Debug.Log("quit");
            // other.transform.GetComponent<PlayerInteraction>().TakeDamage(PlayerInteraction.bulletDamage);

            //when bullet hit player , cam shake
            //            CameraRequst.instance.RequestShake(1f);

           
            // Destroy(gameObject);
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Shadow")

        {
            Debug.Log("likai");
            CancelInvoke("EnemyTakeDamage");
            enemyInteractWith = null;

        }
    }
    */


    //Hited to takedamage
    public void TakeDamage(float damage)
    {
        
         health -= damage;

        /*
         
        int imageNum;
        imageNum = 3 - health;

        if (healthImages.Count > 0)
        {
            healthImages[healthImages.Count - imageNum].gameObject.SetActive(false);
           // healthImages.RemoveAt(healthImages.Count - 1);
        }

        */
        AudioManager.instance.PlayOneTime(playerHurtAudio);
        impulseSource.GenerateImpulse();


       
    }
    public void DetectionInLightTime()
    {
        //make sure afterandend scene dont hurt player;
        if (SceneTimeline.instance.currentLevelState == SceneTimeline.LevelFlowEnmu.level1 || SceneTimeline.instance.currentLevelState == SceneTimeline.LevelFlowEnmu.level2)
        {
            inLightTime += Time.deltaTime;
            if (!CheckInLight.instance.IsInLight(transform))
            {
                Debug.Log("buzai");
                inLightTime = 0f;
                return;
            }
            if (CheckInLight.instance.IsInLight(transform) )
            {
                Debug.Log(inDarkTime);
                // TakeDamage(1);
                if (health > 0)
                {
                    health -= damagePerSecond * Time.deltaTime;
                    health = Mathf.Min(health, maxHealth); 
                }

               
                inLightTime = 0f;
            }
        }
       

    }

    //
    public void DetectionInDarkTime()
    {
        if (SceneTimeline.instance.currentLevelState == SceneTimeline.LevelFlowEnmu.level1 || SceneTimeline.instance.currentLevelState == SceneTimeline.LevelFlowEnmu.level2)
        {
            inDarkTime += Time.deltaTime;
            if (CheckInLight.instance.IsInLight(transform))
            {
                //Debug.Log("buzai");
                inDarkTime = 0f;
                return;
            }

            if (!CheckInLight.instance.IsInLight(transform) )
            {
                Debug.Log(inDarkTime);
                //
                //RestoreHealth(1);
                if (health < maxHealth)
                {
                    health += healPerSecond * Time.deltaTime;
                    health = Mathf.Min(health, maxHealth); 
                }
                

                inDarkTime = 0f;
            }
        }
        
    }

    public void EnemyTakeDamage()
    {
        if (enemyInteractWith != null)
        {
            enemyInteractWith.TakeDamage();
        }
        
    }

    public void LetterTakeDamage()
    {
        if (letter != null)
        {
            letter.TakeDamage();
        }

    }

    public void RestoreHealth(float healAmount)
    {

        if (health < maxHealth)
        {
            health += healAmount;


            //healthImages[healthImages.Count -(4 - health)].gameObject.SetActive(true);
            // healthImages.RemoveAt(healthImages.Count - 1);
        }


    }

    private void RestartGame()
    {
        // Get the name of the current scene
        string currentSceneName = SceneManager.GetActiveScene().name;

        // Reload the current scene using SceneManager
        SceneManager.LoadScene(currentSceneName);
    }

    //hk
    //血条闪烁
    IEnumerator Blink()
    {
        while (canBlink)
        {
            // Set the color to red
            image.color = Color.red;

            // Wait for a short duration
            yield return new WaitForSeconds(blinkInterval / 2);

            // Restore the original color
            image.color = originalColor;

            // Wait for another short duration
            yield return new WaitForSeconds(blinkInterval / 2);
        }
    }
}
