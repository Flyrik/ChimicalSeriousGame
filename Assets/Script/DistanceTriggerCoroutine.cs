using UnityEngine;
using System.Collections;

public class DistanceTriggerCoroutine : MonoBehaviour
{
    [SerializeField] private GameObject Panel; // Panel déjà dans la scène
    [SerializeField] private ParticleSystem particleSystem;
    [SerializeField] private Transform player;

    public float triggerDistance = 2f;
    public float checkInterval = 0.2f;

    private Animator animator;
    private bool triggered = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(CheckDistanceRoutine());
    }

    IEnumerator CheckDistanceRoutine()
    {
        while (!triggered)
        {
            float distance = Vector3.Distance(player.position, transform.position);

            if (distance <= triggerDistance)
            {
                triggered = true;


                if (Panel != null)
                    Panel.SetActive(true);


                if (animator != null)
                    animator.SetTrigger("Talk");


                if (particleSystem != null)
                    particleSystem.gameObject.SetActive(false);


            }

            yield return new WaitForSeconds(checkInterval);
        }
    }
}