using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioDOntDestroy : MonoBehaviour
{
    void Awake()
    {
        // This makes the game object not be destroyed automatically when loading a new scene.
        DontDestroyOnLoad(gameObject);
    }
}
