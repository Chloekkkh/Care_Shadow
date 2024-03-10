using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRequst : MonoBehaviour
{
    public static CameraRequst instance;

    public  float _shakeAmount;
    public float _shakeTime;

   [ SerializeField]
    private Camera _shaker;


    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RequestShake(float shakeTime)
    {
        StartCoroutine(coRequestShake(shakeTime));
    }

    public IEnumerator coRequestShake(float shakeTime)
    {
        _shaker.RequestShake(_shakeAmount, _shakeTime);

        yield return new WaitForSeconds(shakeTime);

        _shaker.RequestShake(0f, _shakeTime);
        yield return null;
    }
    
}
