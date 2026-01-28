using UnityEngine;
using UnityEngine.SceneManagement;

public class PlatformLoader : MonoBehaviour
{
    void Start()
    {
#if UNITY_ANDROID
        // Casque autonome (Quest standalone)
        SceneManager.LoadScene("MainAutonome");
#else
        // PC (PCVR)
        SceneManager.LoadScene("MainPCVR");
#endif
    }
}