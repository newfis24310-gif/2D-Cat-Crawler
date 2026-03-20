using System.Collections;
using UnityEngine;

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
        
        StartCoroutine(RoundSequence()); // Ξεκινάμε την ακολουθία του πρώτου γύρου
    }

    // Kαλείται από το ExitTile όταν ο παίκτης φτάσει στο tile εξόδου.
    public void OnPlayerReachedExit(bool isPlayerAlive)
    {
        if (gameOver) return; // Αν το παιχνίδι έχει τελειώσει, δεν κάνουμε τίποτα

        if (isPlayerAlive) WinGame();
        else StartCoroutine(FailAttemptProcessing());
    }

    public void WinGame()
    {
        gameOver = true; // Ορίζουμε το παιχνίδι ως τελειωμένο
        player.canMove = false; // Απενεργοποιούμε την κίνηση του παίκτη
        Debug.Log("Player has won the game!");
    }

    public void LoseGame()
    {
        gameOver = true; // Ορίζουμε το παιχνίδι ως τελειωμένο
        Debug.Log("Game Over! Player has failed all attempts.");
      
    }
    
    private IEnumerator FailAttemptProcessing()
    {
        Debug.Log($"Player failed attempt {currentAttempt}. Processing feedback...");
        // Εμφανίζουμε το κατάλληλο μήνυμα αποτυχίας στον παίκτη ανάλογα με την τρέχουσα προσπάθεια
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
        if (gridManager != null)
        {
            gridManager.ResetGrid(); // Επαναφορά του grid στην αρχική κατάσταση
            gridManager.AssignItemsToTiles();
            //BaseTile newStartTile = gridManager.GetStartTile(); // Λαμβάνουμε το αρχικό tile από τον GridManager
            //player.ResetPlayer(newStartTile);
            //newStartTile.RevealTile(true); // Αποκαλύπτουμε το tile που βρίσκεται στις συντεταγμένες του παίκτη
            //player.canMove = true; // Ενεργοποιούμε ξανά την κίνηση του παίκτη
            StartCoroutine(RoundSequence());
        
        }
        if(currentAttempt == 2) soundManager.PlayMusic2ndRound();
    }

    private IEnumerator RoundSequence()
    {
        // H γατα δεν κουνιέται
        player.canMove = false;
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

            yield return StartCoroutine(mouse.MoveMouse(startPosition, exitPosition)); // Ξεκινάμε την κίνηση του ποντικιού
        }

        // Aφού τελειώσει η κίνηση του ποντικιού, ξεκινάει η κίνηση της γατας προς το startTile
        if (startTile != null)
        {
            yield return StartCoroutine(player.MoveToStartTile(startTile));
        }
       
    }

    public void PlayBoxOpen()
    {
        soundManager.PlayBoxOpen();
    }
 


}


