using UnityEngine;

public class Player : MonoBehaviour
{
    public GridManager gridManager; // Αναφορά στον GridManager για να μπορούμε να αλληλεπιδράσουμε με τα tiles
    private Vector3 targetPosition; // Η θέση που θέλουμε να μετακινηθεί ο παίκτης

    void Start()
    {
        targetPosition = transform.position; // Αρχικά, η τρέχουσα θέση είναι και o στόχος
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Όταν ο χρήστης κάνει κλικ με το ποντίκι
        {
           HandleMovement();
        }
    }

    private void HandleMovement()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); // Μετατροπή της θέσης του ποντικιού σε συντεταγμένες κόσμου
        mousePosition.z = 0; // Διασφαλίζουμε ότι ο στόχος είναι στο ίδιο επίπεδο με τον παίκτη

        BaseTile clickedTile = gridManager.GetTileAtPosition(mousePosition); // Λαμβάνουμε το tile που βρίσκεται στη θέση του κλικ του ποντικιού μέσω του GridManager

        if (clickedTile != null)
        {
            targetPosition = clickedTile.transform.position; // Ορίζουμε τη νέα θέση στόχο ως τη θέση του tile που κλικάραμε
            transform.position = new Vector3(targetPosition.x, targetPosition.y, transform.position.z); // Μετακίνηση του παίκτη στη νέα θέση
            Debug.Log($"Player moved to tile at ({clickedTile.name}) with coordinates ({clickedTile.x}, {clickedTile.y})");
        }
    }

}
