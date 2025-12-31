using UnityEngine;

public class MobileFPS : MonoBehaviour
{
    void Awake()
    {
        Application.targetFrameRate = 60; // Forces phone to use 60Hz
        QualitySettings.vSyncCount = 0;
    }
}