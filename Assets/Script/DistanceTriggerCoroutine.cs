using UnityEngine;
using System.Collections;

public class DistanceTriggerCoroutine : MonoBehaviour
{
    [SerializeField] private GameObject Panel; // Panel déjà dans la scène
    [SerializeField] private ParticleSystem particleSystem;
    [SerializeField] private Transform player;
    [SerializeField] private Transform targetPoint;
    public GameObject lunettes;
    public GameObject masque;
    public GameObject gants;

    public float triggerDistance = 2f;
    public float checkInterval = 0.2f;

    private Animator animator;
    private bool triggered = false;
    private bool alreadyDone = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(CheckDistanceRoutine());
        StartCoroutine(CheckObjectsLoop());
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

    void FaceTarget()
    {
        Vector3 direction = targetPoint.position - transform.position;
        direction.y = 0f; // ignore la rotation verticale

        if (direction.sqrMagnitude > 0.001f)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    IEnumerator CheckObjectsLoop()
    {
        yield return new WaitUntil(() => GameManager.IsHere);

        Debug.Log("IsHere = TRUE → démarrage de la boucle");

        while (!alreadyDone)
        {
            Debug.Log("OUIIIII");

            if (!lunettes.activeSelf &&
                !masque.activeSelf &&
                !gants.activeSelf)
            {
                FaceTarget();
                alreadyDone = true;

                if (animator != null)
                    animator.SetTrigger("Walk");

                yield break; // stop la coroutine
            }

            yield return new WaitForSeconds(0.4f);
        }
    }
}