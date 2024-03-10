using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.AI;
using System;
using Cinemachine;
using UnityEngine.SceneManagement;

public class SceneTimeline : MonoBehaviour
{
    // TODO: 关卡加载 灯塔附近一圈不动，灯塔旁边有可交互按钮，场景变量，按钮跳出，其他地板向下移动，上层同时下来，，跳字幕。 

    /*
     * yield return null; —— 暂停一帧。
       yield return new WaitForSeconds(n); —— 暂停n秒。
       yield return StartCoroutine(AnotherCoroutine()); —— 等待另一个协程完成。
       yield break; —— 终止协程执行。
     */

    public static SceneTimeline instance;
    public CinemachineVirtualCamera cam;

    public bool buttonRaised = false;
    public Transform zhuXiTai;
    private float zhuXiTaiUp;
    private float zhuXiTaiDown;
    private PlayerInteraction player;

    //light controll
    public GameObject towerLight;
    public GameObject totalLight;
    public float spotAngle = 109f;
    public float lightUpIntensity;
    public float curTime = 0f;
    public float lerpDuration;

    //level0
    public bool G = false;

    public bool G1 = false;
    public bool G2 = false;

    public bool U = false;
    public bool I = false;
    public bool D = false;
    public bool E = false;
    public bool startAdjustment = true; // 控制是否开始调整

    private Quaternion targetRotation = Quaternion.Euler(37.32f, -90f, 0f); // 目标旋转
    private float targetOrthoSize = 10.08f; // 目标Ortho Size
    private Vector3 targetScale = new Vector3(1.1160f, 1.1160f, 1.1160f);
    private float adjustmentDuration = 1f; // 调整过程时长，5秒

    //level0
    public Transform Level0GameObeject;
    private float Level0ToBottom;

    //Level1
    public Transform Level1GameObeject;
    private float Level1ToBottom;


    /*
    //hk
    public GameObject Level1Enemy;
    public GameObject tower;
    */

    //Level2
    public Transform Level2GameObeject;
    private float Level2To0;

    //Level2
    public Transform Level3GameObeject;
    private float Level3To0;


    //UI
    public TextMeshProUGUI buttonText;
    public TextMeshProUGUI levelText;
    public float nf = 8f;

    


    //用枚举来记录所有的关卡流程
    public enum LevelFlowEnmu
    {
        level0,
        level0end,
        level1before,
        level1,
        level1end,
        level2before,
        level2,
        level2end,
        
        level3,
        
    }
    public LevelFlowEnmu currentLevelState;
    public bool isLevelStart;
    public float levelTimer;

    public AudioSource up;
    public AudioClip upp;


    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        zhuXiTai.gameObject.SetActive(false);
        player = GameObject.FindWithTag("Player").GetComponent<PlayerInteraction>();
        buttonText.gameObject.SetActive(false);
        currentLevelState = LevelFlowEnmu.level0;
        zhuXiTaiUp = zhuXiTai.position.y + 2f;
        zhuXiTaiDown = zhuXiTai.position.y ;
        Level2To0 = Level2GameObeject.position.y - 70f;
        Level1ToBottom = Level1GameObeject.position.y - 70f;
        Level0ToBottom = Level0GameObeject.position.y - 70f;
        Level3To0 = Level3GameObeject.position.y - 70f;
        for (int i = 0; i < EnemyController.Level1Enemys.Count; i++)
        {
            EnemyController.Level1Enemys[i].GetComponent<NavMeshAgent>().enabled = false;
            EnemyController.Level1Enemys[i].GetComponent<EnemyStateMachine>().enabled = false;
            //            Debug.Log(EnemyController.Level2Enemys.Count);
            //EnemyController.Level2Enemys[i].GetComponent<Enemy>().enabled = false;

        }
        for (int i = 0; i < EnemyController.Level2Enemys.Count; i++)
        {
            EnemyController.Level2Enemys[i].GetComponent<NavMeshAgent>().enabled = false;
            EnemyController.Level2Enemys[i].GetComponent<EnemyStateMachine>().enabled = false;
//            Debug.Log(EnemyController.Level2Enemys.Count);
            //EnemyController.Level2Enemys[i].GetComponent<Enemy>().enabled = false;
          
        }

    }

// Update is called once per frame
void Update()
    {
        TimeLine();
        LevelSwichDetection();
    // Debug.Log(EnemyController.Enemys.Count);
    }


	private void TimeLine()
	{
		switch (currentLevelState)
        {
            case LevelFlowEnmu.level0:
                if (!isLevelStart)
                {
                    //todo: 关卡设置前初始化。
                    isLevelStart = true;
                    return;
                }
                if (G  &&  U  && I  && D  && E)
                {
                   
                    SceneSwitch(LevelFlowEnmu.level0, LevelFlowEnmu.level0end);
                }


                break;

            case LevelFlowEnmu.level0end:
                if (!isLevelStart)
                {
                    //todo: 关卡设置前初始化。
                    isLevelStart = true;
                    return;
                }
                levelTimer += Time.deltaTime;
                    Debug.Log("JieSuo");
                    // 检测是否开始调整

                    StartCoroutine(AdjustCamera());


                    StartCoroutine(AnimateArc(cam.transform, new Vector3(20.77f, 17.09f, -0.87f), 4f, 0f));


                zhuXiTai.gameObject.SetActive(true);

                buttonRaised = true;

               
               
                StartCoroutine(AnimateArc(zhuXiTai, new Vector3(zhuXiTai.position.x, zhuXiTaiUp, zhuXiTai.position.z), 1f, 2f));
                // totalLight.SetActive(true);
                // towerLight.SetActive(false);
                //Light Controll after level
                StartCoroutine(EndLevelLightControll());
                AudioManager.instance.LightOnAudio();
                TowerController.instance.rotateSpeed = 0f;




                break;
           


            case LevelFlowEnmu.level1before:
                if (!isLevelStart)
                {
                    //todo: 关卡设置前初始化。
                    isLevelStart = true;
                    return;
                }
                levelTimer += Time.deltaTime;

                player.transform.GetComponent<CharacterController>().enabled = false;

                up.PlayOneShot(upp);
                if (levelTimer > 9)
                {
                    up.Stop();
                }
                // todo:笼子出现 3秒后才上升

                StartCoroutine(AnimateArc(Level0GameObeject, new Vector3(Level0GameObeject.position.x, Level0ToBottom, Level0GameObeject.position.z), 9f, 0f));

                StartCoroutine(AnimateArc(Level1GameObeject, new Vector3(Level1GameObeject.position.x, Level1ToBottom, Level1GameObeject.position.z), 9f, 0f));
                StartCoroutine(AnimateArc(Level2GameObeject, new Vector3(Level2GameObeject.position.x, Level2To0, Level2GameObeject.position.z), 9f, 0f));
                StartCoroutine(AnimateArc(Level0GameObeject, new Vector3(Level3GameObeject.position.x, Level3To0, Level3GameObeject.position.z), 9f, 0f));

                




                if (levelTimer > 0f)
                {
                    AudioManager.instance.LevelUpAudio();

                }
               

                //when upstair achive 0f,
                if (Mathf.Abs(Level1GameObeject.transform.position.y - Level1ToBottom) < 0.2f && levelTimer > nf)
                {
                    StartCoroutine(AnimateArc(zhuXiTai, new Vector3(zhuXiTai.position.x, zhuXiTaiDown, zhuXiTai.position.z), 1f, 2f));
                    buttonRaised = false;
                    levelText.text = "Level1";
                }
                if (levelTimer > nf + 2f)
                {
                    levelText.text = "";
                    player.transform.GetComponent<CharacterController>().enabled = true;
                    // totalLight.SetActive(false);
                    // towerLight.SetActive(true);
                    StartCoroutine(StartLevelLightControll());
                    TowerController.instance.rotateSpeed = 15f;
                    for (int i = 0; i < EnemyController.Level1Enemys.Count; i++)
                    {
                        EnemyController.Level1Enemys[i].GetComponent<NavMeshAgent>().enabled = true;
                        EnemyController.Level1Enemys[i].GetComponent<EnemyStateMachine>().enabled = true;

                        //EnemyController.Level2Enemys[i].GetComponent<Enemy>().enabled = false;

                    }
                    SceneSwitch(LevelFlowEnmu.level1before, LevelFlowEnmu.level1);
                }




                break;

            case LevelFlowEnmu.level1:
                if (!isLevelStart)
                {
                    //todo: 关卡设置前初始化。
                    isLevelStart = true;
                    return;
                }
                break;

            case LevelFlowEnmu.level1end:
                if (!isLevelStart)
                {
                    //todo: 关卡设置前初始化。
                   
                    isLevelStart = true;
                    return;
                }

                zhuXiTai.gameObject.SetActive(true);

                buttonRaised = true;
                
                StartCoroutine(AnimateArc(zhuXiTai, new Vector3(zhuXiTai.position.x, zhuXiTaiUp, zhuXiTai.position.z), 1f, 2f));
                // totalLight.SetActive(true);
                // towerLight.SetActive(false);
                //Light Controll after level
                StartCoroutine(EndLevelLightControll());
                AudioManager.instance.LightOnAudio();
                TowerController.instance.rotateSpeed = 0f;


                break;


            case LevelFlowEnmu.level2before:
                if (!isLevelStart)
                {
                    //todo: 关卡设置前初始化。
                    levelTimer = 0f;
                    isLevelStart = true;
                    return;
                }

                levelTimer += Time.deltaTime;

                player.transform.GetComponent<CharacterController>().enabled = false;

                up.Play();
                if (levelTimer > 9)
                {
                    up.Stop();
                }

                up.PlayOneShot(upp);
                if (levelTimer > 9)
                {
                    up.Stop();
                }
                // todo:笼子出现 3秒后才上升
                StartCoroutine(AnimateArc(Level0GameObeject, new Vector3(Level0GameObeject.position.x, Level0ToBottom - 70f, Level0GameObeject.position.z), 9f, 0f));

                StartCoroutine(AnimateArc(Level1GameObeject, new Vector3(Level1GameObeject.position.x, Level1ToBottom - 70f, Level1GameObeject.position.z), 9f, 0f));
                StartCoroutine(AnimateArc(Level2GameObeject, new Vector3(Level2GameObeject.position.x, Level2To0 - 70f, Level2GameObeject.position.z), 9f, 0f));
                StartCoroutine(AnimateArc(Level3GameObeject, new Vector3(Level3GameObeject.position.x, Level3To0 - 70f, Level3GameObeject.position.z), 9f, 0f));






               
                

                //when upstair achive 0f,
                if (Mathf.Abs(Level2GameObeject.transform.position.y -  Level1ToBottom ) < 0.2f && levelTimer > nf)
                {

                    StartCoroutine(AnimateArc(zhuXiTai, new Vector3(zhuXiTai.position.x, zhuXiTaiDown, zhuXiTai.position.z), 1f, 2f));
                    buttonRaised = false;
                    levelText.text = "Level2";

                }
                if (levelTimer > nf + 2f)
                {
                    levelText.text = "";
                    player.transform.GetComponent<CharacterController>().enabled = true;
                    // totalLight.SetActive(false);
                    // towerLight.SetActive(true);
                    StartCoroutine(StartLevelLightControll());
                    TowerController.instance.rotateSpeed = 15f;
                    for (int i = 0; i < EnemyController.Level2Enemys.Count; i++)
                    {
                        EnemyController.Level2Enemys[i].GetComponent<NavMeshAgent>().enabled = true;
                        EnemyController.Level2Enemys[i].GetComponent<EnemyStateMachine>().enabled = true;

                        //EnemyController.Level2Enemys[i].GetComponent<Enemy>().enabled = false;
                        SceneSwitch(LevelFlowEnmu.level2before, LevelFlowEnmu.level2);
                    }
                }


                break;

            case LevelFlowEnmu.level2:
                if (!isLevelStart)
                {
                    //todo: 关卡设置前初始化。
                    isLevelStart = true;
                    return;
                }
                break;

            case LevelFlowEnmu.level2end:
                if (!isLevelStart)
                {
                    //todo: 关卡设置前初始化。

                    isLevelStart = true;
                    return;
                }

                zhuXiTai.gameObject.SetActive(true);

                buttonRaised = true;

                StartCoroutine(AnimateArc(zhuXiTai, new Vector3(zhuXiTai.position.x, zhuXiTaiUp, zhuXiTai.position.z), 1f, 2f));
                // totalLight.SetActive(true);
                // towerLight.SetActive(false);
                //Light Controll after level
                StartCoroutine(EndLevelLightControll());
                AudioManager.instance.LightOnAudio();
                TowerController.instance.rotateSpeed = 0f;

                break;

            case LevelFlowEnmu.level3:
                if (!isLevelStart)
                {
                    //todo: 关卡设置前初始化。
                    levelTimer = 0f;
                    isLevelStart = true;
                    return;
                }

                levelTimer += Time.deltaTime;

                player.transform.GetComponent<CharacterController>().enabled = false;

                up.Play();
                if (levelTimer > 9)
                {
                    up.Stop();
                }

                up.PlayOneShot(upp);
                if (levelTimer > 9)
                {
                    up.Stop();
                }
                // todo:笼子出现 3秒后才上升
                StartCoroutine(AnimateArc(Level0GameObeject, new Vector3(Level0GameObeject.position.x, Level0ToBottom - 140f, Level0GameObeject.position.z), 9f, 0f));

                StartCoroutine(AnimateArc(Level1GameObeject, new Vector3(Level1GameObeject.position.x, Level1ToBottom - 140f, Level1GameObeject.position.z), 9f, 0f));
                StartCoroutine(AnimateArc(Level2GameObeject, new Vector3(Level2GameObeject.position.x, Level2To0 - 140f, Level2GameObeject.position.z), 9f, 0f));

                StartCoroutine(AnimateArc(Level3GameObeject, new Vector3(Level3GameObeject.position.x, Level3To0 - 140, Level3GameObeject.position.z), 9f, 0f));




                if (levelTimer > 0f)
                {
                    AudioManager.instance.LevelUpAudio();

                }
              

                //when upstair achive 0f,
                if (Mathf.Abs(Level3GameObeject.transform.position.y - Level1ToBottom) < 0.2f && levelTimer > nf)
                {

                    StartCoroutine(AnimateArc(zhuXiTai, new Vector3(zhuXiTai.position.x, zhuXiTaiDown, zhuXiTai.position.z), 1f, 2f));
                    buttonRaised = false;
                    levelText.text = "Congradulation!";

                }
                if (levelTimer > nf + 2f)
                {
                    levelText.text = "";
                    player.transform.GetComponent<CharacterController>().enabled = true;
                    // totalLight.SetActive(false);
                    // towerLight.SetActive(true);
               
                    TowerController.instance.rotateSpeed = 0f;
                   
                }

                if (G1 && G2)
                {
                    SceneManager.LoadScene(0);
                }


                break;

        }
    }

    //to control the whole level switch
    public void SceneSwitch(LevelFlowEnmu oldLevel, LevelFlowEnmu newLevel)
    {

        if (oldLevel != LevelFlowEnmu.level1before)
        {
        }
        isLevelStart = false;
        currentLevelState = newLevel;
        
        player.RestoreHealth(100 - player.health);
        
       
        levelTimer = 0;
    }


    // Detection if all enemy has been slayed;  todo：注意之后每一关的敌人都要在before的时候激活，才不会在monsterDead里检测到。
    public bool Level1AllEnemyDie
    {
        get { return EnemyController.Level1Enemys.Count == 0; }
    }

    public bool Leve2AllEnemyDie
    {
        get { return EnemyController.Level2Enemys.Count == 0; }
    }

    //determine the time need to change level
    public void LevelSwichDetection()
    {
        //n to n_end
        //if there is no enemy survive
        if (Level1AllEnemyDie && !buttonRaised)
        {
            if (currentLevelState == LevelFlowEnmu.level1)
            {
                SceneSwitch(LevelFlowEnmu.level1, LevelFlowEnmu.level1end);
            }

        }
        if (Leve2AllEnemyDie && !buttonRaised)
        {
            if (currentLevelState == LevelFlowEnmu.level2)
            {
                SceneSwitch(LevelFlowEnmu.level2, LevelFlowEnmu.level2end);
            }

        }



        //n_end to n+1_end
        // when the player close to zhuXiTai && player in ciecle plane
        //
        //currentLevelState == LevelFlowEnmu.level1end && 
        if (zhuXiTai.gameObject.activeSelf && Vector3.Distance(zhuXiTai.position,player.transform.position) < 2.5f)
        {

            buttonText.gameObject.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (currentLevelState == LevelFlowEnmu.level0end)
                {
                    SceneSwitch(LevelFlowEnmu.level0end, LevelFlowEnmu.level1before);
                }

                if (currentLevelState == LevelFlowEnmu.level1end)
                {
                    SceneSwitch(LevelFlowEnmu.level1end, LevelFlowEnmu.level2before);
                }

                if (currentLevelState == LevelFlowEnmu.level2end)
                {
                    SceneSwitch(LevelFlowEnmu.level2end, LevelFlowEnmu.level3);
                }

            }
        }
        else
        {

            buttonText.gameObject.SetActive(false);

        }
    }

    //Tower rise
    //enemy appear
    // private IEnumerator Level1Load(float waitTime)
    // {
    //     tower.transform.position = Vector3.Lerp(tower.transform.position, new Vector3(tower.transform.position.x, 0, tower.transform.position.z), 1f);
        
    //     //Level1Enemy.SetActive(true);
    //     yield return new WaitForSeconds(waitTime);
    // }
    //light controll after level
    private IEnumerator EndLevelLightControll()
    {
        curTime += Time.deltaTime;
        if (curTime > lerpDuration)
        {
            curTime = lerpDuration;
        }
        float t = curTime / lerpDuration;
        float totalLightIntensity = totalLight.GetComponent<Light>().intensity;
        totalLightIntensity = Mathf.Lerp(totalLightIntensity, lightUpIntensity, t);
        totalLight.GetComponent<Light>().intensity = totalLightIntensity;

        float towerLightSpotAngle = towerLight.GetComponent<Light>().spotAngle;
        towerLightSpotAngle = Mathf.Lerp(towerLightSpotAngle, 0, t);
        towerLight.GetComponent<Light>().spotAngle = towerLightSpotAngle;
        yield return null;
    }

    private IEnumerator StartLevelLightControll()
    {
        curTime += Time.deltaTime;
        if (curTime > lerpDuration)
        {
            curTime = lerpDuration;
        }
        float t = curTime / lerpDuration;
        float totalLightIntensity = totalLight.GetComponent<Light>().intensity;
        totalLightIntensity = Mathf.Lerp(totalLightIntensity, 0, t);
        totalLight.GetComponent<Light>().intensity = totalLightIntensity;

        float towerLightSpotAngle = towerLight.GetComponent<Light>().spotAngle;
        towerLightSpotAngle = Mathf.Lerp(towerLightSpotAngle, spotAngle, t);
        towerLight.GetComponent<Light>().spotAngle = towerLightSpotAngle;
        yield return null;
    }

    public void ZhuXiRaise()
    {
        
        
    }



    //obj move function
    private IEnumerator AnimateArc(Transform transform, Vector3 destination, float duration , float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Vector3 start = transform.position;
        float startTime = Time.time;

        //duration
        while (Time.time - startTime <= duration)
        {
            if (transform == null)
            {
                yield break;
            }
            float num = (Time.time - startTime) / duration;
            transform.position = Vector3.Lerp(start, destination, num);

            yield return null;
        }
        //move to destination
        transform.position = destination;
        yield break;
    }

    IEnumerator AdjustCamera()
    {
        // 确保cam不是null
        if (cam != null)
        {
            // 获取当前数值
            Quaternion initialRotation = cam.transform.rotation;
            float initialOrthoSize = cam.m_Lens.OrthographicSize;
            
             Vector3 initialScale = player.transform.localScale;
           Debug.Log(initialRotation);

            float elapsedTime = 0f;

            while (elapsedTime < adjustmentDuration)
            {
                Debug.Log("Moving");
                // 计算已过时间的比例
                float t = elapsedTime / adjustmentDuration;

                player.transform.localScale = Vector3.Lerp(initialScale, targetScale, t);

                // 平滑调整旋转
                cam.transform.rotation = Quaternion.Lerp(initialRotation, targetRotation, t);

                // 平滑调整Ortho Size
                cam.m_Lens.OrthographicSize = Mathf.Lerp(initialOrthoSize, targetOrthoSize, t);

                // 更新已过时间
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // 确保最终数值正确
            cam.transform.rotation = targetRotation;
            cam.m_Lens.OrthographicSize = targetOrthoSize;
            player.transform.localScale = targetScale;

        }
    }
}
