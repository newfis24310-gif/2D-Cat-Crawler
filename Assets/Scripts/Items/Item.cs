using UnityEngine;

/*
 * Αφηρημένη κλάση για όλα τα αντικείμενα/δράσεις που μπορούν
 * να ενεργοποιηθούν πάνω σε ένα ActionTile.
 * 
 * Κάθε παιδί της κλάσης Item πρέπει να υλοποιεί τη μέθοδο OnInteract,
 * δηλαδή τι συμβαίνει όταν αλληλεπιδρά ο παίκτης με αυτό το αντικείμενο.
 */
public abstract class Item : MonoBehaviour
{
    // Αφηρημένη μέθοδος αλληλεπίδρασης με τον παίκτη
    public abstract void OnInteract(Player player);
}