using UnityEngine;

public class MobileFPS : MonoBehaviour
{
    void Awake()
    {
        // --- FIX 2: REFRESH RATE ---
        // -1 lets Unity automatically match the device's screen Hz (60, 90, 120).
        // This fixes the "Jitter" on your Xiaomi Note 11.
        Application.targetFrameRate = -1; 
        
        // VSync must be off for targetFrameRate to work, 
        // though -1 usually overrides it anyway. Safer to disable.
        QualitySettings.vSyncCount = 0;

        // --- FIX 3: RESOLUTION (Heat/Lag prevention) ---
        // Force the game to render at 720p height to keep it fast on low-end phones.
        int targetHeight = 720;
        
        // Only lower the resolution if the phone is trying to do 1080p or 2K
        if (Screen.height > targetHeight)
        {
            float ratio = (float)Screen.currentResolution.width / (float)Screen.currentResolution.height;
            int targetWidth = Mathf.RoundToInt(targetHeight * ratio);
            
            // Apply the new resolution
            Screen.SetResolution(targetWidth, targetHeight, true);
        }
    }
}