using UnityEngine;
using TMPro;

public class TrainingManager : MonoBehaviour
{
    public static TrainingManager Instance { get; private set; }

    [Header("Réfs")]
    [SerializeField] private BarrelSpawnerPool spawner;       // pour lire maxSpawns
    [SerializeField] private GameObject endPanel;             // UI panel (optionnel)
    [SerializeField] private TextMeshProUGUI endText;         // texte de fin (optionnel)

    [Header("Seuil de réussite")]
    [Tooltip("Si true: seuil = moyenne entre score min et max possibles")]
    [SerializeField] private bool useAverageOfRange = true;

    [Tooltip("Utilisé si useAverageOfRange = false")]
    [SerializeField] private int manualThreshold = 0;

    [Header("Audio (message fin / allez à l'évier)")]
    [SerializeField] private AudioSource announcerSource;
    [SerializeField] private AudioClip trainingFinishedClip; // “Formation terminée... allez à l’évier”

    private int treatedCount = 0;
    private bool finished = false;
    private bool needHandWash = false;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    public bool NeedHandWash => needHandWash;

    public void RegisterBarrelTreated()
    {
        if (finished) return;

        treatedCount++;

        int maxSpawns = (spawner != null) ? spawner.maxSpawns : treatedCount; // fallback
        if (treatedCount >= maxSpawns)
            FinishTraining(maxSpawns);
    }

    private void FinishTraining(int maxSpawns)
    {
        finished = true;
        needHandWash = true;

        int score = BacCheck.totalScore;

        // “moyenne” = moyenne entre score min et max possibles (générique)
        // min = maxSpawns * pointsWrong, max = maxSpawns * pointsCorrect
        // Dans votre BacCheck: pointsCorrect=1, pointsWrong=-1 par défaut :contentReference[oaicite:4]{index=4}
        int pointsCorrect = 1;
        int pointsWrong = -1;

        int threshold = useAverageOfRange
            ? Mathf.RoundToInt(maxSpawns * (pointsCorrect + pointsWrong) / 2f)
            : manualThreshold;

        bool success = score > threshold; // “supérieur à la moyenne”

        if (endPanel != null) endPanel.SetActive(true);
        if (endText != null)
        {
            endText.text =
                "Formation terminée !\n" +
                $"Barils traités : {treatedCount}/{maxSpawns}\n" +
                $"Score : {score}\n" +
                $"Résultat : {(success ? "RÉUSSITE " : "ÉCHEC ")}\n\n" +
                "Rendez-vous à l’évier pour vous laver les mains.";
        }

        if (announcerSource != null && trainingFinishedClip != null)
            announcerSource.PlayOneShot(trainingFinishedClip);
    }

    public void ConfirmHandWash()
    {
        if (!needHandWash) return;
        needHandWash = false;

        if (endText != null)
            endText.text += "\n\n Mains lavées. Fin de la formation.";
    }
}
