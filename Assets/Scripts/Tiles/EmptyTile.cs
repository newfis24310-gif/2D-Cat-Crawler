using UnityEngine;
/*
 * Κλάση για ένα απλό, κενό tile.
 * Δεν έχει κάποιο ειδικό effect πάνω στον παίκτη.
 * Απλώς καταγράφουμε ότι ο παίκτης μπήκε σε αυτό.
 */
public class EmptyTile : BaseTile
{
    // Καλείται όταν ο παίκτης εισέρχεται στο συγκεκριμένο tile
        public override void OnPlayerEnter()
    {
        Debug.Log($"Player entered EmptyTile at ({x}, {y})");
    }
}