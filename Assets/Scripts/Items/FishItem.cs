using UnityEngine;

public class FishItem : Item
{

    public override void OnInteract(Player player)
    {
        Debug.Log("The cat found the fish");
        // Λογικη θα προστεθει αργοτερα
        player.fishItem = this; // Αποθηκεύουμε την αναφορά στο αντικείμενο του ψαριού που βρήκε ο παίκτης
    }

    public void EatFish(Player player)
    {
        ActionTile parentTile = GetComponentInParent<ActionTile>(); // Προσπαθούμε να βρούμε το ActionTile που είναι γονέας αυτού του αντικειμένου
        if (parentTile != null) parentTile.actionDone = true; // Αν βρούμε το ActionTile, ορίζουμε ότι η ενέργεια του έχει ολοκληρωθεί
        
        if (player.isAlive)
        {
            player.Die(); // Ο παίκτης πεθαίνει όταν τρώει το ψάρι
        } 
        else
        {
            player.Heal();
        }
        Destroy(gameObject); // Καταστρέφουμε το αντικείμενο του ψαριού αφού ο παίκτης το φάει
    }
}
