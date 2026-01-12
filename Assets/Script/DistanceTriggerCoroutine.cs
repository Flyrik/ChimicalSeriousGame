using UnityEngine;
using System.Collections;

public class DistanceTriggerCoroutine : MonoBehaviour
{
    public Transform player;            
    public float triggerDistance = 2f;  
    public GameObject Panel; 
    public float checkInterval = 0.2f;  
    private Animator animator;
    private bool triggered = false;
    public ParticleSystem particleSystem;

    void Start()
    {
        StartCoroutine(CheckDistanceRoutine());
         animator = GetComponent<Animator>();
    }

    IEnumerator CheckDistanceRoutine()
    {
        while (!triggered)
        {
            float distance = Vector3.Distance(player.position, transform.position);

            if (distance <= triggerDistance)
            {
                triggered = true;
                Panel.SetActive(true); // Déclenchement de l’action
                 animator.SetTrigger("Talk");
                particleSystem.gameObject.SetActive(false);


                Debug.Log("Distance reached! Action triggered.");
            }

            yield return new WaitForSeconds(checkInterval); 
        }
    }
}
