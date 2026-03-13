using UnityEngine;
/*
* Κλάση που θα διαχειρίζεται το grid και τα tiles. 
*/
public class GridManager : MonoBehaviour
{
    [Header("Grid Settings")]
    public int width; // Πλάτος του grid (πόσα tiles οριζόντια)
    public int height; // Ύψος του grid (πόσα tiles κάθετα)
    public BaseTile tilePrefab; // Το prefab που θα χρησιμοποιηθεί για τη δημιουργία των tiles
    public float tileSpacing = 5.5f; // Η απόσταση μεταξύ των tiles, προσαρμοσμένη για να ταιριάζει με το μέγεθος των sprites
    private BaseTile[,] grid; // Διδιάστατος πίνακας για να αποθηκεύουμε τις αναφορές στα tiles, ώστε να μπορούμε εύκολα να τα διαχειριστούμε

    void Start()
    {
        GenerateGrid();
        SetUpNeighbors();
        GetStartTile();
    }

    // Μέθοδος για τη δημιουργία του grid και την τοποθέτηση των tiles
    // Η μέθοδος υπολογίζει τη θέση κάθε tile με βάση το κέντρο της οθόνης και το μέγεθος του grid, ώστε να είναι κεντραρισμένο
    void GenerateGrid()
    {
        Vector3 worldCenter = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 0)); // Υπολογίζουμε το κέντρο του κόσμου με βάση το κέντρο της οθόνης
        worldCenter.z = 0; 

        // Υπολογισμός του συνολικού πλάτους και ύψους του grid για να το κεντράρουμε σωστά
        float gridWidth = width * tileSpacing;
        float gridHeight = height * tileSpacing;

        float startX = worldCenter.x - (gridWidth / 2) + (tileSpacing / 2); // Προσθέτουμε tileSpacing/2 για να ξεκινάμε από το κέντρο του πρώτου tile
        float startY = worldCenter.y - (gridHeight / 2) + (tileSpacing / 2); // Προσθέτουμε tileSpacing/2 για να ξεκινάμε από το κέντρο του πρώτου tile

        grid = new BaseTile[width, height]; // Αρχικοποιούμε τον πίνακα grid με το σωστό μέγεθος

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // Υπολογισμός της θέσης του tile με βάση το κέντρο του grid
                float posX = startX + x * tileSpacing; // Κεντράρισμα του grid
                float posY = startY + y * tileSpacing; // Κεντράρισμα του grid
                Vector3 tilePosition = new Vector3(posX, posY, 0); // Η θέση του tile στον κόσμο, προσαρμοσμένη για να ταιριάζει με το μέγεθος των sprites

                //Δημιουργία grid
                BaseTile tileObject = Instantiate(tilePrefab, tilePosition, Quaternion.identity); // Δημιουργούμε το tile στη σωστή θέση
                tileObject.name = $"Tile_{x}_{y}"; // Ονομάζουμε το tile για ευκολότερη αναγνώριση στο hierarchy και το debugging
                tileObject.transform.parent = transform; // Ορίζουμε το GridManager ως γονέα για καλύτερη οργάνωση στο hierarchy
                // Ρυθμίζουμε τις συντεταγμένες του tile στον πίνακα μέσω της μεθόδου SetUp του BaseTile
                tileObject.SetUp(x, y); 
                // Αποθήκευση στον πίνακα grid
                grid[x,y] = tileObject;
                
            }
        }
    }

    public BaseTile GetTileAtPosition(Vector3 position)
    {
        float gridWidth = width * tileSpacing;
        float gridHeight = height * tileSpacing;
        Vector3 worldCenter = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 0)); // Υπολογίζουμε το κέντρο του κόσμου με βάση το κέντρο της οθόνης

        float startX = worldCenter.x - (gridWidth / 2) + (tileSpacing / 2); //
        float startY = worldCenter.y - (gridHeight / 2) + (tileSpacing / 2);

        // Υπολογίζουμε τις συντεταγμένες του tile που βρίσκεται στη θέση του κλικ του ποντικιού, προσαρμοσμένες για το κεντράρισμα του grid
        int x = Mathf.RoundToInt((position.x - startX) / tileSpacing); 
        int y = Mathf.RoundToInt((position.y - startY) / tileSpacing);

        if (x >= 0 && x < width && y >= 0 && y < height) // Ελέγχουμε αν οι συντεταγμένες είναι εντός των ορίων του grid
        {
            return grid[x, y]; // Επιστρέφουμε το tile που βρίσκεται στη θέση του κλικ του ποντικιού
        }
        return null; // Επιστρέφει null αν οι συντεταγμένες είναι εκτός ορίων
    }

    // Μέθοδος για να αποκαλύπτουμε το tile που έχει μπει ο  παίκτης/
    //  Θα καλείται από τον Player όταν μετακινείται σε ένα νέο tile;;;;
    public void RevealTile(int x, int y)
    {
        // Έλεγχος οριών πίνακα
        if (x < 00 || x >= width || y < 0 || y >= height)
        {
            Debug.LogWarning($"Attempted to reveal tile at ({x}, {y}), but it's out of bounds.");
            return; // Επιστρέφουμε αν οι συντεταγμένες είναι εκτός ορίων
        }

        grid[x, y].RevealTile(true); // Αποκαλύπτουμε το tile που βρίσκεται στις συντεταγμένες (x, y)


    }

    // Μέθοδος για να ρυθμίσουμε τους γείτονες κάθε tile μετά τη δημιουργία του grid
    private void SetUpNeighbors()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                BaseTile currentTile = grid[x, y];

                // Προσθέτουμε τους γείτονες (πάνω, κάτω, δεξιά, αριστερά) αν είναι εντός ορίων
                if (y < height - 1) currentTile.neighbors.Add(grid[x, y + 1]); // Πάνω
                if (y > 0) currentTile.neighbors.Add(grid[x, y - 1]); // Κάτω
                if (x < width - 1) currentTile.neighbors.Add(grid[x + 1, y]); // Δεξιά
                if (x > 0) currentTile.neighbors.Add(grid[x - 1, y]); // Αριστερά
            }
        }
    }

    //Μέθοδος για τον ορισμό του αρχικού tile
    public BaseTile GetStartTile()
    {
        int randomY = Random.Range(0, 3); // Επιλέγουμε τυχαία μια γραμμή για το αρχικό tile
        return grid[0, randomY]; // Επιστρέφουμε το tile στην πρώτη στήλη και στη τυχαία γραμμή
    }
}
