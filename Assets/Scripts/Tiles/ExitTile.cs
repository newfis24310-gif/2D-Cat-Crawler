using UnityEngine;

/*
 * Κλάση για το tile εξόδου.
 * Όταν ο παίκτης φτάσει εδώ, ελέγχεται αν είναι ζωντανός
 * ώστε να ολοκληρωθεί το παιχνίδι με νίκη ή ήττα.
 */
public class ExitTile : BaseTile
{
    // Καλείται όταν ο παίκτης εισέρχεται στο tile εξόδου
    public override void OnPlayerEnter()
    {
        Debug.Log($"Player entered ExitTile at ({x}, {y})");

        // Βρίσκουμε αναφορά στον παίκτη και στον GameManager μέσα στη σκηνή
        Player player = FindObjectOfType<Player>();
        GameManager gameManager = FindObjectOfType<GameManager>();

        // Έλεγχος αν βρέθηκε ο παίκτης
        if (player == null)
        {
            Debug.LogError("Player not found in scene.");
            return;
        }
        // Έλεγχος αν βρέθηκε ο GameManager
        if (gameManager == null)
        {
            Debug.LogError("GameManager not found in scene.");
            return;
        }
        // Αν ο παίκτης είναι ζωντανός, κερδίζει το παιχνίδι
        if (player.isAlive)
        {
            Debug.Log("Player reached the exit alive!");
            gameManager.WinGame();
        }
        else
        {
            // Αν έχει πεθάνει, χάνει το παιχνίδι
            Debug.Log("Player reached the exit but is dead.");
            gameManager.LoseGame();
        }
    }
}