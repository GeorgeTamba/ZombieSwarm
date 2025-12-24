using UnityEngine;

public class ZombieAnimationRelay : MonoBehaviour
{
    // Reference to the main script on the parent
    private Zombie parentZombieScript;

    void Start()
    {
        // Automatically find the script on the parent object
        parentZombieScript = GetComponentInParent<Zombie>();
    }

    // THIS is the function you will select in the Animation Window
    public void OnAttackHit()
    {
        if (parentZombieScript != null)
        {
            // Forward the message to the main script
            parentZombieScript.DealDamageEvent();
        }
    }
}