using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level0 : MonoBehaviour
{
    private Transform UIbar;
    private Transform UIPos;
    public GameObject healthPrefab;
    private Canvas UIcanvas;
    [Header("Health")]
    public float maxHealth = 30f;
    public float damagePerSecond = 10f;
    public float currentHealth;
    private Image healthSlider;
    private Transform camPos;

    //audio
    private float lastHealth; // 用于存储上一次的生命值
    private float timeSinceLastChange = 0f; // 自上次生命值变化以来的时间
    private float bleedStopTime = 0.5f;
    public AudioSource damageAudioSource;
    // Start is called before the first frame update
    void Start()
    {
        //health bar
        currentHealth = maxHealth;
        camPos = GameObject.FindWithTag("MainCamera").transform;
        UIcanvas = GameObject.FindWithTag("UIcanvas").GetComponent<Canvas>();
        UIPos = transform.Find("UIpos").transform;
        InitialUI();
    }

    // Update is called once per frame
    void Update()
    {
        AudioPlay();
        UpdateHealthBar();
    }

    public void InitialUI()
    {
        UIbar = Instantiate(healthPrefab, UIcanvas.transform).transform;
        healthSlider = UIbar.GetChild(0).GetComponent<Image>();

    }



    public void TakeDamage()
    {
        currentHealth -= damagePerSecond;

        if (!damageAudioSource.isPlaying)
        {
            damageAudioSource.Play();
        }

        if (currentHealth <= 0)
        {
            StartCoroutine(Die());
        }
    }

    public void UpdateHealthBar()
    {
        if (UIbar != null)
        {
            UIbar.gameObject.SetActive(true);
            UIbar.position = UIPos.position;
            UIbar.forward = -camPos.forward;
            if (healthSlider != null)
            {
                //update health 
                healthSlider.fillAmount = currentHealth / maxHealth;
            }
            
           
        }

    }

    private IEnumerator Die()
    {

        if (gameObject.tag == "E")
        {
            SceneTimeline.instance.E = true;
        }
        if (gameObject.tag == "D")
        {
            SceneTimeline.instance.D = true;
        }
        if (gameObject.tag == "I")
        {
            SceneTimeline.instance.I = true;
        }
        if (gameObject.tag == "U")
        {
            SceneTimeline.instance.U = true;
        }
        if (gameObject.tag == "G")
        {
            SceneTimeline.instance.G = true;
        }
        if (gameObject.tag == "G1")
        {
            SceneTimeline.instance.G1 = true;
        }
        if (gameObject.tag == "G2")
        {
            SceneTimeline.instance.G2 = true;
        }
        Destroy(UIbar.gameObject);
        Destroy(gameObject);
        return null;

    }


    /*
    //DarkUIcontrol
    public void DarkUIManager()
    {
       
        if (darkUI != null)
        {
            if (CheckInLight.instance.IsInLight(transform))
            {
                darkTime = 0f;
                darkUI.SetActive(false);
            }
            //if in dark ,determine the current enemy state is.
            if (!CheckInLight.instance.IsInLight(transform))
            {
                darkTime += Time.deltaTime;

                //a little delay to make sure UI apprear right after enemy cant see
                if (darkTime > 0.8f)
                {
                    darkUI.SetActive(true);

                    if (curActionState == "GunState")
                    {
                        redEyes.SetActive(true);
                        greenEyes.SetActive(false);
                    }

                    if (curActionState == "PatrolState")
                    {
                        redEyes.SetActive(false);
                        greenEyes.SetActive(true);
                    }
                }
              

            }
        }
        
    }
    */

    public void AudioPlay()
    {
        // 如果当前生命值与上次检测时不同，重置计时器
        if (currentHealth != lastHealth)
        {
            timeSinceLastChange = 0f;
            lastHealth = currentHealth;
        }
        else // 否则，更新计时器
        {
            timeSinceLastChange += Time.deltaTime;
        }

        // 检查是否需要停止掉血音效
        if (timeSinceLastChange >= bleedStopTime && damageAudioSource.isPlaying)
        {
            damageAudioSource.Pause();
        }
    }

}
