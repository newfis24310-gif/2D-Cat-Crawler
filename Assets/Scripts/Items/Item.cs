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
    protected SpriteRenderer spriteRenderer;

    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        // Ξεκινάει πάντα κρυφό
        if (spriteRenderer != null) spriteRenderer.enabled = false;
    }

    // Μέθοδος για να εμφανίζεται το item
    public void Reveal()
    {
        if (spriteRenderer == null) 
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = true;

        Debug.Log($"REVEALED: {gameObject.name} at {transform.position}");
    }

    // Αφηρημένη μέθοδος αλληλεπίδρασης με τον παίκτη
    public abstract void OnInteract(Player player);
}