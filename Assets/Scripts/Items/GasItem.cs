using Unity.VisualScripting;
using UnityEngine;

public class GasItem : Item
{
    public override void OnInteract(Player player)
    {
        Debug.Log("Cat found the gas");
        player.gasItemCount++; // Αυξάνουμε τον μετρητή των gas items του παίκτη κατά 1
        Debug.Log($"Player has collected {player.gasItemCount} gas item(s).");

        if(player.gasItemCount >= 2) 
        {
            player.Die();
        }
        else
        {
            // Ισως βαλουμε καποιο εφε δεν ξερω
        }
        Destroy(gameObject,2f); // Καταστρέφουμε το αντικείμενο του gas item αφού ο παίκτης το συλλέξει
    }
}
