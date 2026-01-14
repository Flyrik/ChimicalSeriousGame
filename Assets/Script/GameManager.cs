using UnityEngine;

public class GameManager : MonoBehaviour
{

    public GameObject Mask;
    public GameObject Gloves;
    public GameObject Glasses;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject PanelWelecome;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ActivateEPI()
    {
        Mask.SetActive(true);
        Gloves.SetActive(true);
        Glasses.SetActive(true);
        PanelWelecome.SetActive(false);
    }
}