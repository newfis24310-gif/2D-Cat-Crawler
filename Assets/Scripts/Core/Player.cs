using UnityEngine;
using Yarn.Unity;

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
    public bool canMove = true; // Δυνατότητα κίνησης του παίκτη (μπορεί να απενεργοποιηθεί όταν ο παίκτης πεθάνει)

    public DialogueRunner dialogueRunner; // Αναφορά στον DialogueRunner για να μπορούμε να ξεκινάμε διαλόγους

    void Awake()
    {
        if (dialogueRunner == null)
        {
            dialogueRunner = FindObjectOfType<DialogueRunner>(); // Βρίσκουμε τον DialogueRunner στο σκηνικό αν δεν έχει ανατεθεί
            if (dialogueRunner == null)
            {
                Debug.LogError("No DialogueRunner found in the scene! Please ensure there is a DialogueRunner object.");
            }
        }

        dialogueRunner.AddCommandHandler<string>("notmove", (target) => {
            canMove = false; // Απενεργοποιούμε την κίνηση του παίκτη όταν εκτελείται η εντολή "notmove" στον Yarn
            Debug.Log($"{target} movement has been disabled by Yarn command.");
        });

        dialogueRunner.AddCommandHandler<string>("move", (target) => {
            canMove = true; // Ενεργοποιούμε την κίνηση του παίκτη όταν εκτελείται η εντολή "move" στον Yarn
            Debug.Log($"{target} movement has been enabled by Yarn command.");
        });
    }

    void Start()
    {
        currentHealth = maxHealth; // Αρχικοποίηση της τρέχουσας υγείας με τη μέγιστη υγεία
        targetPosition = transform.position; // Αρχικά, η τρέχουσα θέση είναι και o στόχος
        isAlive = true; //  Ο παίκτης ξεκινάει ζωντανός για να δοκιμάσουμε το σύστημα αλλαγής γύρων

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
        // ΠΡΟΣΩΡΙΝΟ: Πατώντας το πλήκτρο 'K' ξεκλειδώνεις τον παίκτη χειροκίνητα
        if (Input.GetKeyDown(KeyCode.K)) 
        {
            canMove = true;
            Debug.Log("MANUAL UNLOCK: canMove is now TRUE");
        }

        if (Input.GetMouseButtonDown(0)) 
        {
            if (!canMove) {
                Debug.Log("Click ignored because canMove is FALSE");
                return;
            }
            HandleMovement();
        }
    }

    // Μέθοδος για να χειριστούμε την κίνηση του παίκτη προς το tile που κλικάραμε
    private void HandleMovement()
    {
        if (!canMove) return;
        
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

                clickedTile.OnPlayerEnter(); // Καλούμε τη μέθοδο που χειρίζεται την είσοδο του παίκτη στο tile 
                gridManager.UpdateGridVisibility(x, y); // Αποκαλύπτουμε το tile που βρίσκεται στις συντεταγμένες του παίκτη
                
        
            }
        }
    }

    // Μέθοδος για να λαμβάνει ζημιά ο παίκτης
    public void TakeDamage(int damage)
    {
        currentHealth -= damage; // Μείωση της τρέχουσας υγείας κατά το ποσό της ζημιάς
        Debug.Log($"Player took {damage} damage. Current health: {currentHealth}");

        if (currentHealth <= 0) Die(); // Αν η υγεία πέσει στο μηδέν ή κάτω, ο παίκτης πεθαίνει

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
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        Debug.Log($"Player healed by {amount}. Current health: {currentHealth}");
    }

    public void ResetPlayer(BaseTile startTile)
    {
        currentHealth = maxHealth; // Επαναφορά της υγείας στην μέγιστη τιμή
        isAlive = true; // TEST: Ο παίκτης είναι ξανά ζωντανός
        x = startTile.x; // Ενημέρωση των συντεταγμένων του παίκτη σύμφωνα με το αρχικό tile
        y = startTile.y;
        transform.position = new Vector3(startTile.transform.position.x, startTile.transform.position.y, -1f); // Τοποθετούμε τον παίκτη στη θέση του αρχικού tile
        targetPosition = transform.position; // Ενημέρωση της θέσης στόχου στην τρέχουσα θέση
        Debug.Log("Player has been reset to the starting position with full health.");
    }


}
