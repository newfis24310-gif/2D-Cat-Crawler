using System.Collections.Generic;
using UnityEngine;

public class BaseTile : MonoBehaviour
{
    public int x,y; // Συντεταγμένες του πλακιδίου στον πίνακα
    public bool isRevealed = false; // Κατάσταση αν το πλακίδιο έχει αποκαλυφθεί ή όχι
    public List<BaseTile> neighbors = new List<BaseTile>(); // Λίστα με τα έγκυρα γειτονικά tiles (Πάνω, κάτω, δεξία, αριστερά)
    
    public Sprite hiddenSprite; // Το sprite που θα εμφανίζεται όταν το πλακίδιο δεν είναι αποκαλυμμένο
    public Sprite revealedSprite; // Το sprite που θα εμφανίζεται όταν το πλακίδιο είναι αποκαλυμμένο
    private SpriteRenderer spriteRenderer; // Αναφορά στον SpriteRenderer για την αλλαγή του sprite

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // Αρχικοποίηση της αναφοράς στον SpriteRenderer
        spriteRenderer.sprite = hiddenSprite; // Αρχικά, το πλακίδιο εμφανίζει το κρυφό sprite
    }

    public virtual void OnPlayerEnter() {} // Μέθοδος που θα καλείται όταν ο παίκτης μπει σε αυτό το πλακίδιο, μπορεί να επεκταθεί για να προσθέσει εφέ ή αλληλεπιδράσεις
    public void SetUp(int x, int y)
    {
        // Ορίζουμε τις συντεταγμένες του πλακιδίου στον πίνακα
        this.x = x;
        this.y = y; 
        RevealTile(false); // Αρχικά, το πλακίδιο δεν είναι αποκαλυμμένο
    }

    public void RevealTile(bool state)
    {
        isRevealed = state; // Ενημερώνουμε την κατάσταση αποκαλυμμένου του πλακιδίου

        // Αλλάζουμε το sprite ανάλογα με την κατάσταση αποκαλυμμένου
        spriteRenderer.sprite = isRevealed ? revealedSprite : hiddenSprite;
    }

}
