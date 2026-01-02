using UnityEngine;
using TMPro; // REQUIRED for TextMeshPro

public class FPSDisplay : MonoBehaviour
{
    [Header("UI Reference")]
    public TextMeshProUGUI fpsText; // Drag your TMP Object here

    [Header("Settings")]
    public float updateInterval = 0.5f; 

    private float accum = 0; 
    private int frames = 0; 
    private float timeleft; 

    void Start()
    {
        if (fpsText == null)
        {
            Debug.LogError("FPSDisplay: No TextMeshPro assigned!");
            enabled = false;
            return;
        }
        timeleft = updateInterval;
    }

    void Update()
    {
        timeleft -= Time.deltaTime;
        accum += Time.timeScale / Time.deltaTime;
        frames++;

        if (timeleft <= 0.0)
        {
            float fps = accum / frames;
            string format = string.Format("{0:F0} FPS", fps); // F0 = No decimals (e.g. "60 FPS")
            
            fpsText.text = format;

            // Color coding
            if(fps < 30)
                fpsText.color = Color.red;
            else if(fps < 50)
                fpsText.color = Color.yellow;
            else
                fpsText.color = Color.green;

            timeleft = updateInterval;
            accum = 0.0f;
            frames = 0;
        }
    }
}