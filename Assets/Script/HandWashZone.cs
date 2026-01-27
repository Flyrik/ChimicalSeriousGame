using UnityEngine;
using Unity.XR.CoreUtils; // pour détecter XROrigin

public class HandWashZone : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip washHandsClip;

    [SerializeField] private ParticleSystem waterFx;

    private bool done = false;

    private void OnTriggerEnter(Collider other)
    {
        if (done) return;
        if (TrainingManager.Instance == null || !TrainingManager.Instance.NeedHandWash) return;

        // Détecte l’utilisateur via XROrigin (plus robuste qu’un tag)
        var xrOrigin = other.GetComponentInParent<XROrigin>();
        if (xrOrigin == null) return;

        done = true;

        StartWater();

        if (audioSource != null && washHandsClip != null)
            audioSource.PlayOneShot(washHandsClip);

        TrainingManager.Instance.ConfirmHandWash();
    }

    private void OnTriggerExit(Collider other)
    {
        StopWater();
        audioSource.Stop(); 
    }

    private void StartWater()
    {
        if (waterFx != null) waterFx.Play();
    }

    private void StopWater()
    {
        if (waterFx != null) waterFx.Stop(true, ParticleSystemStopBehavior.StopEmitting);
    }
}
