using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game Settings")]
    public int maxAttempts = 7;
    [SerializeField] private int currentAttempt = 1;
    private bool gameOver = false;

    [Header("References")]
    public FeedBackManager feedbackManager; // Αναφορά στον FeedbackManager για να μπορούμε να εμφανίζουμε μηνύματα στον παίκτη
    public SoundManager soundManager; // Αναφορά στον SoundManager για να ρυθμίζουμε τον ήχο ανά περιπτώσεις
    private Player player; // Αναφορά στον Player για να μπορούμε να διαχειριστούμε την κατάσταση του παίκτη
    private GridManager gridManager; // Αναφορά στον GridManager για να μπορούμε να διαχειριστούμε το grid
    public Mouse mouse; // Αναφορά στο Mouse για να μπορούμε να το ελέγχουμε από το GameManager
    public GameObject background;

    public GameObject skeletonPrefab;
    private List<Vector3> deathPoints = new List<Vector3>(); // Λίστα για να αποθηκεύουμε τα σημεία θανάτου του παίκτη

    [Header("UI Reference")]
    public TextMeshProUGUI attemptsText;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        background.SetActive(true); // Ενεργοποιούμε το background στην αρχή του παιχνιδιού
        player = FindObjectOfType<Player>(); // Βρίσκουμε τον Player στο σκηνικό
        gridManager = FindObjectOfType<GridManager>(); // Βρίσκουμε τον GridManager στο σκηνικό

        if (player == null) Debug.LogError("Player not found in the scene! Please ensure there is a Player object.");
        if (gridManager == null) Debug.LogError("GridManager not found in the scene! Please ensure there is a GridManager object.");
        
        StartCoroutine(RoundSequence()); // Ξεκινάμε την ακολουθία του πρώτου γύρου
        UpdateAttempText();
    }

    // Kαλείται από το ExitTile όταν ο παίκτης φτάσει στο tile εξόδου.
    public void OnPlayerReachedExit(bool isPlayerAlive)
    {
        if (gameOver) return; // Αν το παιχνίδι έχει τελειώσει, δεν κάνουμε τίποτα

        SpawnSkeletonsAtDeathPoints(); // Δημιουργούμε skeletons στα σημεία θανάτου του παίκτη

        if (isPlayerAlive) WinGame();
        else StartCoroutine(FailAttemptProcessing());
    }

    public void WinGame()
    {
        gameOver = true; // Ορίζουμε το παιχνίδι ως τελειωμένο
        player.canMove = false; // Απενεργοποιούμε την κίνηση του παίκτη
        Debug.Log("Player has won the game!");
        feedbackManager.ShowWinMessage(currentAttempt); // Εμφανίζουμε το μήνυμα νίκης ανάλογα με την τρέχουσα προσπάθεια
        PlayWinMusic(); // Παίζουμε τη μουσική νίκης
    }

    public void LoseGame()
    {
        gameOver = true; // Ορίζουμε το παιχνίδι ως τελειωμένο
        Debug.Log("Game Over! Player has failed all attempts.");
        feedbackManager.ShowFailAttemptMessage(currentAttempt); // Εμφανίζουμε το μήνυμα αποτυχίας ανάλογα με την τρέχουσα προσπάθεια
    }
    
    private IEnumerator FailAttemptProcessing()
    {
        Debug.Log($"Player failed attempt {currentAttempt}. Processing feedback...");
        // Εμφανίζουμε το κατάλληλο μήνυμα αποτυχίας στον παίκτη ανάλογα με την τρέχουσα προσπάθεια
        if(currentAttempt > 1){soundManager.currentAmbient.setParameterByName("Cat ambience", 1);} //Αλλάζω παράμετρο στο FMOD 
        PlayVacantBox();
        feedbackManager.ShowFailAttemptMessage(currentAttempt);

        Debug.Log("Waiting for feedback dialogue to complete...");

         yield return new WaitForSeconds(2f); // Προσωρινή αναμονή για να δώσουμε χρόνο στον παίκτη να διαβάσει το μήνυμα (μπορεί να αφαιρεθεί όταν έχουμε έτοιμο το σύστημα διαλόγων)


        while (feedbackManager.dialogueRunner.IsDialogueRunning)
        {
            // Περιμένουμε μέχρι να τελειώσει ο διάλογος
            yield return null;
        }

       
        // Aφου τελειώσει ο διάλογος, ελέγχουμε αν έχουμε φτάσει στο μέγιστο αριθμό προσπαθειών
        if (currentAttempt < maxAttempts)
        {
            currentAttempt++;
            NextAttempt();
        } 
        else
        {
            LoseGame();
        }
    }

    private void NextAttempt()
    {
        Debug.Log($"Preparing for attempt {currentAttempt}...");
        player.canMove = false; // Απενεργοποιούμε την κίνηση του παίκτη κατά τη διάρκεια της προετοιμασίας για τον επόμενο γύρο
        YarnDeathOnceFalse(); //Αρχικοποιώ μεταβλητή στο Yarn

        ClearSkeletons(); // Καθαρίζουμε τα skeletons sprites από την προηγούμενη προσπάθεια
        deathPoints.Clear(); // Καθαρίζουμε τα σημεία θανάτου από την προηγούμενη προσπάθεια
        if(currentAttempt < maxAttempts) UpdateAttempText();
        if (gridManager != null)
        {
            gridManager.ResetGrid(); // Επαναφορά του grid στην αρχική κατάσταση
            gridManager.AssignItemsToTiles();
            StartCoroutine(RoundSequence());
        
        }
        if(currentAttempt == 2)
        {
            PlayMusic2ndRound();
            PlayAmbienceLab();
        }
        if(currentAttempt > 1){soundManager.currentAmbient.setParameterByName("Cat ambience", 0);} 
    }

    private IEnumerator RoundSequence()
    {
        // H γατα δεν κουνιέται
        player.canMove = false;
        player.isAlive = true; // Ο παίκτης είναι ζωντανός στην αρχή κάθε γύρου

        SpriteRenderer playerSprite = player.GetComponent<SpriteRenderer>();
        if (playerSprite != null) playerSprite.enabled = false;

        if (feedbackManager != null && feedbackManager.dialogueRunner != null)
        {
            yield return null;
            while (feedbackManager.dialogueRunner.IsDialogueRunning)
            {
                yield return null; // Περίμενε το επόμενο frame
            }
        }

        // To ποντίκι ξεκινάει την διαδρομή του
        BaseTile startTile = gridManager.GetStartTile();
        BaseTile exitTile = FindAnyObjectByType<ExitTile>(); // Λαμβάνουμε το tile εξόδου από τη σκηνή

        if(mouse != null && startTile != null && exitTile != null)
        {
            Vector3 startPosition = new Vector3(startTile.transform.position.x, startTile.transform.position.y, -1f); // Θέτουμε το z σε -1 για να είναι πάνω από τα tiles
            Vector3 exitPosition = new Vector3(exitTile.transform.position.x, exitTile.transform.position.y, -1f); // Θέτουμε το z σε -1 για να είναι πάνω από τα tiles
            PlayCatRobotMouseMovement();

            yield return StartCoroutine(mouse.MoveMouse(startPosition, exitPosition)); // Ξεκινάμε την κίνηση του ποντικιού
        }

        // Aφού τελειώσει η κίνηση του ποντικιού, ξεκινάει η κίνηση της γατας προς το startTile
        if (startTile != null)
        {
            PlayCatEntrance();
            yield return StartCoroutine(player.MoveToStartTile(startTile));
        }

        yield return new WaitForSeconds(0.5f); // Μικρή καθυστέρηση πριν ενεργοποιηθεί η κίνηση του παίκτη
        player.canMove = true; // Ενεργοποιούμε την κίνηση του παίκτη
       
    }

    //SOUNDMANAGER SOUND CALLS
    public void PlayCatEntrance()
    {
        soundManager.PlayCatEntrance();
    }
    public void PlayCatRobotMouseMovement()
    {
        soundManager.PlayCatRobotMouseMovement();
    }
    public void PlayBoxMovement()
    {
        soundManager.PlayBoxMovement();
    }
    public void PlayBoxOpen()
    {
        soundManager.PlayBoxOpen();
    }
    public void PlayVacantBox()
    {
        soundManager.PlayVacantBox();
    }
    public void PlayGas()
    {
        soundManager.PlayGas();
    }
    public void PlayFindFish()
    {
        soundManager.PlayFindFish();
    }
    public void PlayEatFish()
    {
        soundManager.PlayEatFish();
    }
    public void PlayAmbienceLab()
    {
        soundManager.PlayAmbienceLab();
    }
    public void PlayMusic2ndRound()
    {
        soundManager.PlayMusic2ndRound();
    }

    //YARN VARIABLE CALLS
    public void YarnDeathOnceTrue()
    {
        feedbackManager.YarnDeathOnceTrue();
    }

    public void YarnDeathOnceFalse()
    {
        feedbackManager.YarnDeathOnceFalse();
    }

    public void YarnFoundFishTrue()
    {
        feedbackManager.YarnFoundFishTrue();
    }

    public void YarnFoundFishFalse()
    {
        feedbackManager.YarnFoundFishFalse();
    }


    // Restart game
    public void ReloadGame()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);    
    }
    
    public void PlayWinMusic()
    {
        // soundManager.PlayMusic(soundManager.musicLibrary.win, 0.5f);
    }

    public void RecordDeathPoint(Vector3 position)
    {
        deathPoints.Add(position); // Προσθέτουμε το σημείο θανάτου στη λίστα
    }

    private void SpawnSkeletonsAtDeathPoints()
    {
        foreach (Vector3 deathPoint in deathPoints)
        {
            Vector3 spawnDeathPoint = new Vector3(deathPoint.x, deathPoint.y, -1f); // Θέτουμε το z σε -1 για να είναι πάνω από τα tiles
            Instantiate(skeletonPrefab, spawnDeathPoint, Quaternion.identity); // Δημιουργούμε ένα skeleton prefab σε κάθε σημείο θανάτου
        }
    }

    private void ClearSkeletons()
    {
        // Βρίσκει όλα τα GameObjects στη σκηνή που έχουν το Tag "Skeleton"
        GameObject[] skeletons = GameObject.FindGameObjectsWithTag("Skeleton");
    
        foreach (GameObject skeleton in skeletons)
        {
            Destroy(skeleton); // Τα διαγράφει από τη σκηνή
        }
    }

    private void UpdateAttempText()
    {
        if (attemptsText != null) attemptsText.text = $"{currentAttempt} | {maxAttempts}";
    }
}





