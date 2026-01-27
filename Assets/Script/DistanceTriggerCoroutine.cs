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
    public AudioSource audioSource;
    public GameObject SuiteConteneur;
    public GameObject BoutonIndiqu;
    public float triggerDistance = 2f;
    public float checkInterval = 0.2f;
    public GameObject BoutonOfficiel;
    private Animator animator;
    private bool triggered = false;
    private bool alreadyDone = false;
    public AudioClip audioClip1;
    public AudioClip audioClip2;
    void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(CheckDistanceRoutine());
        StartCoroutine(CheckObjectsLoop());
        if (audioSource != null)
            audioSource.Stop();

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
                if (audioSource != null)
                    audioSource.clip = audioClip1;
                    audioSource.Play();

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
            

            if (!lunettes.activeSelf &&
                !masque.activeSelf &&
                !gants.activeSelf)
            {
                FaceTarget();
                alreadyDone = true;

                if (animator != null)
                    animator.SetTrigger("Walk");
                    StartCoroutine(StopAfterTime(13f));
                    audioSource.clip = audioClip2;
                    audioSource.Play();
                yield break; // stop la coroutine
            }

            yield return new WaitForSeconds(0.4f);
        }
    }

    public void StartBouton()
    {
        SuiteConteneur.SetActive(false);
        BoutonIndiqu.SetActive(true);
        BoutonOfficiel.SetActive(true);
    }

        
    

    IEnumerator StopAfterTime(float time)
{
    yield return new WaitForSeconds(time);
    
    SuiteConteneur.SetActive(true);
    if (animator != null)
        animator.SetTrigger("Idle");
}

}