using UnityEngine;

public class HazardZone : MonoBehaviour
{
    [Header("Extinction")]
    [SerializeField] private string extinguisherTag = "Extinguisher";
    [SerializeField] private float requiredTimeInZone = 0.2f; // 0 = instant

    [Header("VFX/SFX")]
    [SerializeField] private ParticleSystem[] vfxToStop;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip extinguishClip;

    private float timeInside = 0f;
    private bool extinguished = false;

    private void Reset()
    {
        // auto-fill pratique
        vfxToStop = GetComponentsInChildren<ParticleSystem>(true);
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (extinguished) return;
        if (!other.CompareTag(extinguisherTag)) return;

        timeInside += Time.deltaTime;
        if (timeInside >= requiredTimeInZone)
            Extinguish();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(extinguisherTag))
            timeInside = 0f;
    }

    private void Extinguish()
    {
        extinguished = true;

        if (vfxToStop != null)
        {
            foreach (var ps in vfxToStop)
                if (ps != null) ps.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }

        if (audioSource != null && extinguishClip != null)
        {
            audioSource.clip = extinguishClip;
            audioSource.Play();
        }

        Destroy(gameObject, 1.0f); // laisse le temps au son de jouer
    }
}
