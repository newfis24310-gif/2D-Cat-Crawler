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
        Player player = FindAnyObjectByType<Player>();
        GameManager gameManager = FindAnyObjectByType<GameManager>();

        if (player != null && gameManager != null)
        {
            gameManager.OnPlayerReachedExit(player.isAlive); // Ενημερώνουμε τον GameManager ότι ο παίκτης έφτασε στο tile εξόδου
        }
    }
}