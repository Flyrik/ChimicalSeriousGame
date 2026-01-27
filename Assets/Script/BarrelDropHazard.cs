using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class BarrelDropHazard : MonoBehaviour
{
    [Header("Detection sol")]
    [SerializeField] private string floorTag = "HazardFloor";
    [SerializeField] private float releaseWindowSeconds = 2.0f;

    [Header("Type / Prefabs")]
    [SerializeField] private GameObject flammableZonePrefab;
    [SerializeField] private GameObject corrosiveZonePrefab;

    [Header("Alerte audio")]
    [SerializeField] private AudioClip warningClip;
    [SerializeField] private float warningVolume = 1f;

    private XRGrabInteractable grab;
    private bool wasEverGrabbed = false;
    private float lastReleaseTime = -999f;
    private bool triggered = false;

    private void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();
        if (grab != null)
        {
            grab.selectEntered.AddListener(_ => wasEverGrabbed = true);
            grab.selectExited.AddListener(_ => lastReleaseTime = Time.time);
        }
    }

    private void OnDestroy()
    {
        if (grab != null)
        {
            grab.selectEntered.RemoveAllListeners();
            grab.selectExited.RemoveAllListeners();
        }
    }

    private void OnEnable()
    {
        // important si vos barils sont réutilisés via pool
        triggered = false;
        wasEverGrabbed = false;
        lastReleaseTime = -999f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (triggered) return;
        if (!collision.collider.CompareTag(floorTag)) return;

        // On ne veut l'erreur que si le joueur l'a manipulé puis lâché
        if (!wasEverGrabbed) return;
        if (Time.time - lastReleaseTime > releaseWindowSeconds) return;

        triggered = true;

        Vector3 spawnPos = collision.contacts.Length > 0 ? collision.contacts[0].point : transform.position;
        SpawnHazardZone(spawnPos);
        PlayWarning(spawnPos);
    }

    private void SpawnHazardZone(Vector3 pos)
    {
        GameObject prefab = null;

        if (CompareTag("Inflammable")) prefab = flammableZonePrefab;
        else if (CompareTag("Corrosif")) prefab = corrosiveZonePrefab;

        if (prefab != null)
            Instantiate(prefab, pos, Quaternion.identity);
    }

    private void PlayWarning(Vector3 pos)
    {
        if (warningClip != null)
            AudioSource.PlayClipAtPoint(warningClip, pos, warningVolume);
    }
}
