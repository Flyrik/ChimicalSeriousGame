using UnityEngine;

public class BacDisappearAlways : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Vérifie que l'objet a un Rigidbody (utile pour VR)
        if (other.attachedRigidbody == null)
            return;

        // Détruire l'objet immédiatement
        Destroy(other.gameObject);

        // Optionnel : juste pour debug
        
    }
}
