using UnityEngine;
/*
 * Κλάση για tile που περιέχει κάποια ενέργεια ή αντικείμενο.
 * Όταν ο παίκτης μπει σε αυτό το tile, ενεργοποιείται το αντίστοιχο Item.
 * Η δράση μπορεί να γίνει μόνο μία φορά.
 */
public class ActionTile : BaseTile
{
    [Header("Action Tile Settings")]
    public bool actionDone = false; // Δείχνει αν η ενέργεια του tile έχει ήδη εκτελεστεί
    public Item item; // Αναφορά στο αντικείμενο/ενέργεια που σχετίζεται με το tile


    public override void OnPlayerEnter()
    {
        Debug.Log($"Player entered ActionTile at ({x}, {y})");

        // Αν η ενέργεια έχει ήδη γίνει, δεν την ξαναεκτελούμε

        if (actionDone)
        {
            Debug.Log("Action on this tile has already been completed.");
            return;
        }
        // Βρίσκουμε τον παίκτη μέσα στη σκηνή
        Player player = FindObjectOfType<Player>();

        // Έλεγχος αν βρέθηκε ο παίκτης
        if (player == null)
        {
            Debug.LogError("Player not found in scene.");
            return;
        }

        // Αν υπάρχει Item συνδεδεμένο σε αυτό το tile, το ενεργοποιούμε
        if (item != null)
        {
            item.OnInteract(player);  // Εκτέλεση της δράσης πάνω στον παίκτη
            actionDone = true;   // Σημειώνουμε ότι η δράση ολοκληρώθηκε
        }
        else
        {
             // Προειδοποίηση αν δεν έχει οριστεί αντικείμενο στο tile
            Debug.LogWarning("No Item assigned to this ActionTile.");
        }
    }
}