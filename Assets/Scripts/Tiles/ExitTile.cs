using UnityEngine;

/*
 * Κλάση για το tile εξόδου.
 * Όταν ο παίκτης φτάσει εδώ, ενημερώνεται ο GameManager
 * ώστε να ελεγχθεί αν ο παίκτης ολοκλήρωσε επιτυχώς τον γύρο.
 */
public class ExitTile : BaseTile
{
    // Καλείται όταν ο παίκτης εισέρχεται στο tile εξόδου
    public override void OnPlayerEnter()
    {
        Debug.Log($"Player entered ExitTile at ({x}, {y})");

        // Βρίσκουμε τον παίκτη και τον GameManager στη σκηνή
        Player player = FindAnyObjectByType<Player>();
        GameManager gameManager = FindAnyObjectByType<GameManager>();

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

        // Ενημερώνουμε τον GameManager ότι ο παίκτης έφτασε στο exit tile
        gameManager.OnPlayerReachedExit(player.isAlive);
    }
}