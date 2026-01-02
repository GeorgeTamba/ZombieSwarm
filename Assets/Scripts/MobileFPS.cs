using UnityEngine;

public class MobileFPS : MonoBehaviour
{
    void Awake()
    {
        // CHANGE 1: Force 60 FPS specifically
        // -1 often defaults to 30 on Android. 
        // 60 is the "Gold Standard" for smooth mobile games.
        Application.targetFrameRate = 60; 
        
        // Disable VSync so it obeys our targetFrameRate
        QualitySettings.vSyncCount = 0;

        // --- RESOLUTION SCALING (Keep this!) ---
        int targetHeight = 720;
        
        if (Screen.height > targetHeight)
        {
            float ratio = (float)Screen.currentResolution.width / (float)Screen.currentResolution.height;
            int targetWidth = Mathf.RoundToInt(targetHeight * ratio);
            Screen.SetResolution(targetWidth, targetHeight, true);
        }
    }
}