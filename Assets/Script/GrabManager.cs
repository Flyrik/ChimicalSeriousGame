using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DisableOnRelease : MonoBehaviour
{
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grab;

    void Awake()
    {
        grab = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        grab.selectExited.AddListener(OnRelease);
    }

    void OnRelease(SelectExitEventArgs args)
    {
        // Désactive l'objet après qu'il ait été relâché
        gameObject.SetActive(false);
    }
}
