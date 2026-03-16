using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game Settings")]
    public int maxAttempts = 7;
    private int currentAttempt = 1;
    private bool gameOver = false;

    [Header("References")]
    //public FeebackManager feebackManager; // Αναφορά στον FeedbackManager για να μπορούμε να εμφανίζουμε μηνύματα στον παίκτη

    private Player player; // Αναφορά στον Player για να μπορούμε να διαχειριστούμε την κατάσταση του παίκτη
    private GridManager gridManager; // Αναφορά στον GridManager για να μπορούμε να διαχειριστούμε το grid

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        player = FindObjectOfType<Player>(); // Βρίσκουμε τον Player στο σκηνικό
        gridManager = FindObjectOfType<GridManager>(); // Βρίσκουμε τον GridManager στο σκηνικό

        if (player == null) Debug.LogError("Player not found in the scene! Please ensure there is a Player object.");
        if (gridManager == null) Debug.LogError("GridManager not found in the scene! Please ensure there is a GridManager object.");
    }

    // Kαλείται από το ExitTile όταν ο παίκτης φτάσει στο tile εξόδου.
    public void OnPlayerReachedExit(bool isPlayerAlive)
    {
        if (gameOver) return; // Αν το παιχνίδι έχει τελειώσει, δεν κάνουμε τίποτα

        if (isPlayerAlive) WinGame();
        else FailAttemptProcessing();
    }

    public void WinGame()
    {
        gameOver = true; // Ορίζουμε το παιχνίδι ως τελειωμένο
        //feebackManager.ShowWinMessage(); // Εμφανίζουμε μήνυμα νίκης στον παίκτη
        Debug.Log("Player has won the game!");
    }

    public void LoseGame()
    {
        gameOver = true; // Ορίζουμε το παιχνίδι ως τελειωμένο
        //feebackManager.ShowGameOverMessage(); // Εμφανίζουμε μήνυμα ήττας στον παίκτη
        Debug.Log("Game Over! Player has failed all attempts.");
    }
    
    private void FailAttemptProcessing()
    {
        //if (feebackManager != null) feebackManager.ShowFailAttemptMessage(currentAttempt); // Εμφανίζουμε μήνυμα αποτυχίας για την τρέχουσα προσπάθεια
        Debug.Log($"Player failed attempt {currentAttempt}.");

        if (currentAttempt < maxAttempts)
        {
            currentAttempt++; // Αύξηση του αριθμού των προσπαθειών
            //NextAttempt(); // Προετοιμασία για την επόμενη προσπάθεια
            StartCoroutine(NextAttemptCoroutine()); // Χρήση Coroutine για να έχουμε μια μικρή καθυστέρηση πριν την επόμενη προσπάθεια
        }
        else
        {
            LoseGame(); // Αν ο παίκτης έχει εξαντλήσει όλες τις προσπάθειες, τελειώνει το παιχνίδι
        }
    }

    private void NextAttempt()
    {
        Debug.Log($"Preparing for attempt {currentAttempt}...");
        if (gridManager != null)
        {
            gridManager.ResetGrid(); // Επαναφορά του grid στην αρχική κατάσταση
            BaseTile newStartTile = gridManager.GetStartTile(); // Λαμβάνουμε το αρχικό tile από τον GridManager
            player.ResetPlayer(newStartTile);
            newStartTile.RevealTile(true); // Αποκαλύπτουμε το tile που βρίσκεται στις συντεταγμένες του παίκτη
            player.canMove = true; // Ενεργοποιούμε ξανά την κίνηση του παίκτη
        }
    }

    private IEnumerator NextAttemptCoroutine()
    {
        player.canMove = false; // Απενεργοποιούμε την κίνηση του παίκτη κατά τη διάρκεια της μετάβασης
        yield return new WaitForSeconds(2f); // Μικρή καθυστέρηση πριν την επόμενη προσπάθεια
        NextAttempt();
    }
}


