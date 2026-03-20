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
        
        if (item == null)
        {
            item = GetComponentInChildren<Item>(); // Προσπαθούμε να βρούμε ένα Item που είναι συνδεδεμένο με αυτό το tile
        }

        if (item != null)
        {
            Player player = FindObjectOfType<Player>(); // Βρίσκουμε τον παίκτη στη σκηνή
            if (player != null)
            {
                item.OnInteract(player); // Καλούμε τη μέθοδο αλληλεπίδρασης του αντικειμένου, περνώντας τον παίκτη ως παράμετρο
                actionDone = true;
            }
        }
        else
        {
            Debug.LogWarning("No item found on this ActionTile.");
        }
    }
}