using UnityEngine;
using Yarn.Unity;
using System.Collections;

public class Player : MonoBehaviour
{   
    [Header("References")]
    public GridManager gridManager; // Αναφορά στον GridManager για να μπορούμε να αλληλεπιδράσουμε με τα tiles
    private Vector3 targetPosition; // Η θέση που θέλουμε να μετακινηθεί ο παίκτης
    public GameManager gameManager; // Αναφορά στον GameManager για να μπορούμε να διαχειριστούμε την κατάσταση του παιχνιδιού

    [Header("Player Stats")]
    public int x, y; // Συντεταγμένες του παίκτη στον πίνακα
    public bool isAlive = true; // Κατάσταση ζωής του παίκτη
    public bool canMove = true; // Δυνατότητα κίνησης του παίκτη (μπορεί να απενεργοποιηθεί όταν ο παίκτης πεθάνει)

    public DialogueRunner dialogueRunner; // Αναφορά στον DialogueRunner για να μπορούμε να ξεκινάμε διαλόγους

    public int gasItemCount = 0; // Μετρητής για τα gas items 
    public FishItem fishItem; // Αναφορά στο FishItem για να μπορούμε να το χρησιμοποιήσουμε όταν ο παίκτης το έχει συλλέξει
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
            //canMove = true; // Ενεργοποιούμε την κίνηση του παίκτη όταν εκτελείται η εντολή "move" στον Yarn
            Debug.Log($"{target} movement has been enabled by Yarn command.");
        }); 
    }

    void Start()
    {
        
        /*targetPosition = transform.position; // Αρχικά, η τρέχουσα θέση είναι και o στόχος
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
        }*/
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

        if (Input.GetKeyDown(KeyCode.E)) 
        {
            if (fishItem != null) 
            {
                Debug.Log("Player is using the fish item.");
                fishItem.EatFish(this); // Καλούμε τη μέθοδο αλληλεπίδρασης του ψαριού, περνώντας τον παίκτη ως παράμετρο
                fishItem = null; // Αφαιρούμε την αναφορά στο ψάρι αφού το χρησιμοποιήσουμε
            } 
            else
            {
                Debug.Log("Player tried to use the fish item, but does not have it.");
            }
        }
    }

    // Μέθοδος για να χειριστούμε την κίνηση του παίκτη προς το tile που κλικάραμε
    private void HandleMovement()
    {
        if (!canMove) return;

        // Οταν ο παίκτης κάνει κλικ για να παει σε αλλο tile αφαιρουμε τηην αναφορα στο ψάρι για αν μην μπορεί να το φάει από απόσταση
        fishItem = null;
        
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
                if (!clickedTile.isRevealed) gridManager.UpdateGridVisibility(x, y); // Αποκαλύπτουμε το tile που βρίσκεται στις συντεταγμένες του παίκτη μονο αν δεν είναι ήδη αποκαλυμμένο
                    
                
        
            }
        }
    }
 

    // Μέθοδος για να χειριστούμε το θάνατο του παίκτη
    public void Die()
    {
        if (!isAlive) return; // Αν ο παίκτης είναι ήδη νεκρός, μην κάνεις τίποτα
        isAlive = false; // Ο παίκτης δεν είναι πλέον ζωντανός
        GameManager.Instance.RecordDeathPoint(transform.position); // Καταγράφουμε το σημείο θανάτου του παίκτη στο GameManager
        Debug.Log("Player has died!");
    }

    // Μέθοδος για να θεραπεύεται ο παίκτης
    public void Heal()
    {
        isAlive = true; // Ο παίκτης είναι ξανά ζωντανός
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
       
    }

    public void ResetPlayer(BaseTile startTile)
    {
        
        isAlive = true; // Ο παίκτης είναι ξανά ζωντανός
        gasItemCount = 0; // Επαναφορά του μετρητή gas items
        fishItem = null; // Αφαίρεση της αναφοράς στο ψάρι
        x = startTile.x; // Ενημέρωση των συντεταγμένων του παίκτη σύμφωνα με το αρχικό tile
        y = startTile.y;
        transform.position = new Vector3(startTile.transform.position.x, startTile.transform.position.y, -1f); // Τοποθετούμε τον παίκτη στη θέση του αρχικού tile
        targetPosition = transform.position; // Ενημέρωση της θέσης στόχου στην τρέχουσα θέση
        Debug.Log("Player has been reset to the starting position with full health.");
    }

    public IEnumerator MoveToStartTile(BaseTile startTile)
    {
        // Γατα εκτος grid
        Vector3 offGridPostion = startTile.transform.position + new Vector3(-4f, -1f, 0f); // Θέση εκτός grid κάτω από το startTile
        transform.position = new Vector3(offGridPostion.x, offGridPostion.y, -1f); // Τοποθετούμε τον παίκτη

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null) spriteRenderer.enabled = true; 

        // Kίνηση προς το StartTile
        float elapsedTime = 0f;
        float moveDuration = 2f; // Διάρκεια κίνησης προς το startTile
        Vector3 initialPosition = new Vector3(startTile.transform.position.x, startTile.transform.position.y, -1f); // Θέτουμε το z σε -1 για να είναι πάνω από τα tiles    

        while (elapsedTime < moveDuration)
        {
            transform.position = Vector3.Lerp(offGridPostion, initialPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = initialPosition; // Βεβαιωνόμαστε ότι φτάσαμε ακριβώς στο startTile

        // Ενημέρωση των στοιχείων του παίκτη σύμφωνα με το startTile
        x = startTile.x;
        y = startTile.y;
        isAlive = true; // Ο παίκτης είναι ζωντανός όταν φτάνει στο startTile
        gasItemCount = 0; // Επαναφορά του μετρητή gas items
        fishItem = null; // Αφαίρεση της αναφοράς στο ψάρι

        startTile.RevealTile(true); // Αποκαλύπτουμε το tile που βρίσκεται στις συντεταγμένες του παίκτη
        
        gridManager.UpdateGridVisibility(x, y); // Ενημερώνουμε την ορατότητα του grid με βάση τις νέες συντεταγμένες του παίκτη













    
    }

}
