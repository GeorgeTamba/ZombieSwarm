using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform cam;

    void Start()
    {
        cam = Camera.main.transform;
    }

    void LateUpdate()
    {
        // Force the canvas to look exactly the same direction as the camera
        // This keeps it flat relative to your screen
        transform.forward = cam.forward;
    }
}