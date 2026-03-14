using UnityEngine;

public class Player : MonoBehaviour
{   
    [Header("References")]
    public GridManager gridManager; // Αναφορά στον GridManager για να μπορούμε να αλληλεπιδράσουμε με τα tiles
    private Vector3 targetPosition; // Η θέση που θέλουμε να μετακινηθεί ο παίκτης
    public GameManager gameManager; // Αναφορά στον GameManager για να μπορούμε να διαχειριστούμε την κατάσταση του παιχνιδιού

    [Header("Player Stats")]
    public int x, y; // Συντεταγμένες του παίκτη στον πίνακα
    public int maxHealth = 100; // Υγεία του παίκτη (έβαλα 100 αυθαίρετα, το βλέπουμε)
    public int currentHealth;
    public bool isAlive = true; // Κατάσταση ζωής του παίκτη
    
    void Start()
    {
        currentHealth = maxHealth; // Αρχικοποίηση της τρέχουσας υγείας με τη μέγιστη υγεία
        targetPosition = transform.position; // Αρχικά, η τρέχουσα θέση είναι και o στόχος

        BaseTile startTile = gridManager.GetStartTile(); // Λαμβάνουμε το αρχικό tile από τον GridManager
        if (startTile != null)
        {
            x = startTile.x; // Ορίζουμε τις συντεταγμένες του παίκτη σύμφωνα με το αρχικό tile
            y = startTile.y;
            transform.position = new Vector3(startTile.transform.position.x, startTile.transform.position.y, -1f); // Τοποθετούμε τον παίκτη στη θέση του αρχικού tile
            gridManager.UpdateGridVisibility(x, y); // Αποκαλύπτουμε το tile που βρίσκεται στις συντεταγμένες του παίκτη
        }
        else
        {
            Debug.LogError("No starting tile found! Please ensure the GridManager has a valid starting tile.");
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Όταν ο χρήστης κάνει κλικ με το ποντίκι
        {
           HandleMovement();
        }
    }

    // Μέθοδος για να χειριστούμε την κίνηση του παίκτη προς το tile που κλικάραμε
    private void HandleMovement()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); // Μετατροπή της θέσης του ποντικιού σε συντεταγμένες κόσμου
        mousePosition.z = 0; // Διασφαλίζουμε ότι ο στόχος είναι στο ίδιο επίπεδο με τον παίκτη

        BaseTile clickedTile = gridManager.GetTileAtPosition(mousePosition); // Λαμβάνουμε το tile που βρίσκεται στη θέση του κλικ του ποντικιού μέσω του GridManager

        BaseTile currentTile = gridManager.GetTileAtPosition(transform.position); // Λαμβάνουμε το tile που βρίσκεται ο παίκτης αυτή τη στιγμή
        if (clickedTile != null && currentTile != null)
        {
            // Ελέγχουμε αν το tile που κλικάραμε είναι γειτονικό με το tile που βρίσκεται ο παίκτης αυτή τη στιγμή
            if (currentTile.neighbors.Contains(clickedTile))
            {
                targetPosition = clickedTile.transform.position; // Ορίζουμε τη νέα θέση στόχο ως τη θέση του tile που κλικάραμε
                transform.position = new Vector3(targetPosition.x, targetPosition.y, transform.position.z); // Μετακίνηση του παίκτη στη νέα θέση
                x = clickedTile.x; // Ενημέρωση των συντεταγμένων του παίκτη
                y = clickedTile.y;
                Debug.Log($"Player moved to tile at ({clickedTile.name}) with coordinates ({clickedTile.x}, {clickedTile.y})");
        
                gridManager.UpdateGridVisibility(x, y); // Αποκαλύπτουμε το tile που βρίσκεται στις συντεταγμένες του παίκτη

            }
        }
    }

    // Μέθοδος για να λαμβάνει ζημιά ο παίκτης
    public void TakeDamage(int damage)
    {
        currentHealth -= damage; // Μείωση της τρέχουσας υγείας κατά το ποσό της ζημιάς
        Debug.Log($"Player took {damage} damage. Current health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die(); // Αν η υγεία πέσει στο μηδέν ή κάτω, ο παίκτης πεθαίνει
        }
    }

    // Μέθοδος για να χειριστούμε το θάνατο του παίκτη
    private void Die()
    {
        isAlive = false; // Ο παίκτης δεν είναι πλέον ζωντανός
        Debug.Log("Player has died!");
        // Εδώ μπορούμε να προσθέσουμε λογική για το τι συμβαίνει όταν ο παίκτης πεθαίνει 
        // π.χ. να αλλάζει το sprite της γάτας σε σκελετό.
        // Οτι αλλο γινεται αν δεν βγει από τον γύρο ζωντανή.
    }

    // Μέθοδος για να θεραπεύεται ο παίκτης
    public void Heal(int amount)
    {
        currentHealth += amount; // Αύξηση της τρέχουσας υγείας κατά το ποσό της θεραπείας
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth; // Διασφάλιση ότι η υγεία δεν υπερβαίνει τη μέγιστη υγεία
        }
        Debug.Log($"Player healed by {amount}. Current health: {currentHealth}");
    }

}
