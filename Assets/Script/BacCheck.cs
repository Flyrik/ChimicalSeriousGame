using UnityEngine;
using TMPro; 
public class BacCheck : MonoBehaviour
{
    [Header("Tag de l'objet correct")]
    public string tagCorrect;

    [Header("Points à ajouter ou retirer")]
    public int pointsCorrect = 1; // points si bon objet
    public int pointsWrong = -1;  // points si mauvais objet
    public AudioClip reussiteClip;
    public AudioClip echecClip;
    public AudioClip warningClip;
    public AudioSource audioSource;

   
    public static int totalScore = 0;


    public TextMeshProUGUI scoreText; 

    private void OnTriggerEnter(Collider other)
    {
       
        if (other.attachedRigidbody == null)
            return;

        if (other.CompareTag(tagCorrect))
        {
            totalScore += pointsCorrect;
            Debug.Log("✅ Objet correct ! Score: " + totalScore);
            audioSource.clip = reussiteClip;
            audioSource.Play();
        }
        else
        {
            totalScore += pointsWrong;
            Debug.Log("❌ Mauvais objet ! Score: " + totalScore);
            audioSource.clip = echecClip;
            audioSource.Play();
            audioSource.clip = warningClip;
            audioSource.Play();
        }

        UpdateScoreText();

        TrainingManager.Instance?.RegisterBarrelTreated();



        other.gameObject.SetActive(false);
    }

       private void UpdateScoreText()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + totalScore;
    }
}
