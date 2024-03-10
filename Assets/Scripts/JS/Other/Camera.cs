using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Linq;

public class Camera : MonoBehaviour
{

    private readonly List<ShakerRequest> _request = new();
    private CinemachineBasicMultiChannelPerlin _noise;

    [SerializeField]
    private float _shakeDecreaseAmount = 10f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_request.Count == 0)
        {
            _noise.m_AmplitudeGain = 0;
            return;
        }

        var strongestShake = _request.Max(shake => shake.ShakeAmount);
        _noise.m_AmplitudeGain = strongestShake;


        for (int i = _request.Count - 1; i >= 0; i--)
        {
            var request = _request[i];
            request.ShakeTime -= Time.deltaTime;
            if (request.ShakeTime <= 0)
            {

                request.ShakeTime = Mathf.Max(0 ,request.ShakeAmount - Time.deltaTime * _shakeDecreaseAmount);
            }
            if (request.ShakeTime == 0)
            {
                _request.Remove(request);
                _noise.m_AmplitudeGain = 0;
            }
        }

    }

    private void Awake()
    {
        _noise = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void RequestShake(float amount)
    {
        RequestShake(amount, time: 0);
    }

    public void RequestShake(float amount,float time)
    {
        _request.Add(new ShakerRequest
        {
            ShakeAmount = amount,
            ShakeTime = time

        })
      ;
    }
    
    private class ShakerRequest
    {
        public float ShakeAmount { get; set; }
        public float ShakeTime { get; set; }

    }


    /*
    public IEnumerator coRequestShake(float shakeTime)
    {
        RequestShake(_shakeAmount, _shakeTime);

        yield return new WaitForSeconds(shakeTime);

        _shaker.RequestShake(0f, _shakeTime);
        yield return null;
    }
    */
}
