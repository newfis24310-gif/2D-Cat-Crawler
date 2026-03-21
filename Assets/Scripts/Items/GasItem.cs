using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class GasItem : Item
{
    public override void OnInteract(Player player)
    {
        StartCoroutine(PlayGasAnimation(player));
    }

    public override void Reveal()
    {
        base.Reveal();
        Debug.Log("Gas item revealed! Be careful, collecting 2 will be fatal for the player.");
        
        Animator animator = GetComponent<Animator>();
        if(animator != null) animator.Play("Gas_animetion", 0, 0f); // Παίζουμε την animation του gas item όταν αποκαλύπτεται
        GameManager.Instance.PlayGas();
    }

    private IEnumerator PlayGasAnimation(Player player)
    {
        player.canMove = false; // Απενεργοποιούμε την κίνηση του παίκτη κατά τη διάρκεια της animation
        player.gasItemCount++; // Αυξάνουμε τον μετρητή των gas items του παίκτη κατά 1

        yield return new WaitForSeconds(1.5f); // Περιμένουμε για τη διάρκεια της animation (προσαρμόστε το χρόνο ανάλογα με τη διάρκεια της animation)
        
        if (player.gasItemCount >= 2)
        {
            player.Die(); // Ο παίκτης πεθαίνει αν συλλέξει 2 ή περισσότερα gas items
            Debug.Log("Player has collected 2 or more gas items and has died!");
        }
        
        player.canMove = true; // Ενεργοποιούμε ξανά την κίνηση του παίκτη μετά την animation
        Debug.Log($"Player has collected {player.gasItemCount} gas item(s).");
    
    }
    
}
