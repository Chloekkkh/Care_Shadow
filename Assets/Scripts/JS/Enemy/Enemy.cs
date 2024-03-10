using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    // NPC navigation settings
    [Header("Basic Setting")]
    [HideInInspector]
    public GameObject player;
    private EnemyStateMachine enemy_StateMachine;
    private NavMeshAgent navMeshAgent;
    public NavMeshAgent NavMeshAgent { get => navMeshAgent; }
    [SerializeField]
    private string curActionState;
    public EnemyPath path;
    private Transform parent;
    private Transform grandParent;
    private Vector3 diePos;
    //Enemy View
    [Header("View Setting")]
    public float sightDistance = 10f;
    public float fieldOfView = 30f;
    public LayerMask playerLayerMask;


    //patrol setting
    [Header("patrol setting")]
    public float patrolWaitingTime = 3f;

    

    //Enemy Shoot
    [Header("Shoot Setting")]
    public float fireRate = 2f;
   [ HideInInspector]
    public Transform firePosition;
    public float fireForce = 30f;


    //Enemy Hurt
    [Header("Health")]
    public float maxHealth = 30f;  
    public float damagePerSecond = 10f;  
    public float currentHealth;

    //Enemy UI
    private Transform UIPos;
    public GameObject healthPrefab;
    private Canvas UIcanvas;
    private Transform camPos;
    private Transform UIbar;
    private Image healthSlider;
    public bool CanSeeUI ;
    private GameObject darkUI;
    private GameObject redEyes;
    private GameObject greenEyes;
    private float darkTime;


    //animation
    private Animator animator;

    //audio
    private float lastHealth; // 用于存储上一次的生命值
    private float timeSinceLastChange = 0f; // 自上次生命值变化以来的时间
    private float bleedStopTime = 0.5f;
    public AudioSource damageAudioSource;

    // Start is called before the first frame update
    void Start()
    {
        parent = transform.parent.transform;
        grandParent = parent.transform.parent.transform;
        //enemy state
        navMeshAgent = transform.GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player");
        enemy_StateMachine = GetComponent<EnemyStateMachine>();
        enemy_StateMachine.Initialise();
        firePosition = transform.Find("FirePosition").transform;

        //health bar
        currentHealth = maxHealth;
        camPos = GameObject.FindWithTag("MainCamera").transform;
        UIcanvas = GameObject.FindWithTag("UIcanvas").GetComponent<Canvas>();
        UIPos = transform.Find("UIpos").transform;
        InitialUI();

        /*
        //eyes
        darkUI = transform.Find("DarkUI").gameObject;
        redEyes = darkUI.transform.Find("Red").gameObject;
        greenEyes = darkUI.transform.Find("Green").gameObject;

        darkUI.SetActive(false);

        */

        //animation
        animator = GetComponent<Animator>();
        lastHealth = currentHealth;

        

    }

    void OnEnable()
    {
        if (transform.parent.tag == "Level1Enemy" )
        {
            // when enemy is onenable,add to proper 
            EnemyController.Level1Enemys.Add(gameObject);
        }
        if (transform.parent.tag == "Level2Enemy")
        {
            // when enemy is onenable,add to proper 
            EnemyController.Level2Enemys.Add(gameObject);
        }

    }

    void OnDisable()
    {
        if (transform.parent.tag == "Level1Enemy")
        {
            // when enemy is onenable,add to proper 
            EnemyController.Level1Enemys.Remove(gameObject);
        }
        if (transform.parent.tag == "Level2Enemy")
        {
            // when enemy is onenable,add to proper 
            EnemyController.Level2Enemys.Remove(gameObject);
        }

    }

    //initial UI for each enemy prefab;
    public void InitialUI()
    {
        UIbar = Instantiate(healthPrefab, UIcanvas.transform).transform;
        healthSlider = UIbar.GetChild(0).GetComponent<Image>();
        
    }
    // Update is called once per frame
    void Update()
    {
        //Navigation();
        CanSeePlayer();
        curActionState = enemy_StateMachine.activeState.ToString();
        //can see health bar only when enemy in light
        UpdateHealthBar();
        //DarkUIManager();
        AudioPlay();
       
    }

    public void Navigation()
    {
        //enemy.SetDestination(player.transform.position);
    }


    //used for further Enemy action
    public bool CanSeePlayer()
    {
        //Distance restriction
        if (Vector3.Distance(transform.position, player.transform.position) < sightDistance)
        {
            
            Vector3 targetDirection = new Vector3(player.transform.position.x, player.transform.position.y + 0.7f, player.transform.position.z) - transform.position;
            
            //HK
            //turn to player and find player animation
          //   transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetDirection), 0.1f);
            



            //angle restriction
            float angleToPlayer = Vector3.Angle(targetDirection, transform.forward);
            if (angleToPlayer >= -fieldOfView && angleToPlayer <= fieldOfView)
            {
                animator.SetTrigger("FindPlayer");
//                Debug.Log(angleToPlayer.ToString());
                Ray ray = new Ray(transform.position, targetDirection);
                RaycastHit raycastHit1;
                if (Physics.Raycast(ray, out raycastHit1, sightDistance, playerLayerMask))
                {
//                    Debug.Log("hh");
                    //debug raycast if player is detected
                    if (raycastHit1.transform.gameObject == player)
                    {
                        Debug.DrawRay(ray.origin, ray.direction * sightDistance);
                        return true;
                    }
                }
            }
        }
        //HK
        //if enemy cant see player
        // else
        // {
        //     animator.SetBool("FindPlayer", false);
        //     return false;
        // }
        return false;
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
            StartCoroutine( Die());  
        }
    }

    public void UpdateHealthBar()
    {
        if (UIbar != null)
        {
            UIbar.gameObject.SetActive(CanSeeUI);
            UIbar.position = UIPos.position;
            UIbar.forward = -camPos.forward;
            if (healthSlider != null)
            {
                //update health 
                healthSlider.fillAmount = currentHealth / maxHealth;
            }
            if (CheckInLight.instance.IsInLight(transform))
            {
                CanSeeUI = true;
            }
            if (!CheckInLight.instance.IsInLight(transform))
            {

                CanSeeUI = false;

            }
        }
        
    }

    private IEnumerator Die()
    {

        diePos = transform.position;
        animator.SetTrigger("Die");


        yield return new WaitForSeconds(1.5f);
        Destroy(UIbar.gameObject);
        Destroy(gameObject);
        
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
