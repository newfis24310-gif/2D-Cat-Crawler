using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    public Player player; // Αναφορά στον παίκτη για να μπορούμε να διαχειριστούμε την κατάσταση του παιχνιδιού
    public GridManager gridManager; // Αναφορά στον GridManager για να μπορούμε να διαχειριστούμε το grid και τα tiles

    [Header("Game Settings")]
    public int maxTurns = 7; // Μέγιστος αριθμός γύρων
    private int currentTurn = 0; // Τρέχων γύρος

    

}
