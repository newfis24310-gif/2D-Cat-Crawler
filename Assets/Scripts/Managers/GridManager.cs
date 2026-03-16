using System.Collections.Generic;
using UnityEngine;
/*
* Κλάση που θα διαχειρίζεται το grid και τα tiles. 
*/
public class GridManager : MonoBehaviour
{
    [Header("Grid Settings")]
    public int width; // Πλάτος του grid (πόσα tiles οριζόντια)
    public int height; // Ύψος του grid (πόσα tiles κάθετα)
    public float tileSpacing = 5.5f; // Η απόσταση μεταξύ των tiles, προσαρμοσμένη για να ταιριάζει με το μέγεθος των sprites
    
    [Header("Tile Prefabs")]
    public BaseTile emptyTilePrefab; // Το prefab που θα χρησιμοποιηθεί για τη δημιουργία των tiles
    public BaseTile exitTilePrefab; // Το prefab για το tile εξόδου
    public BaseTile actionTilePrefab; // Το prefab για τα tiles με ενέργεια/αντικείμενο
    
    private BaseTile[,] grid; // Διδιάστατος πίνακας για να αποθηκεύουμε τις αναφορές στα tiles, ώστε να μπορούμε εύκολα να τα διαχειριστούμε
    float startX, startY; // Μεταβλητές για να κρατάμε τις αρχικές συντεταγμένες του grid για τον υπολογισμό των θέσεων των tiles
    

    
    private Vector2Int startTilePosition; // Η θέση του αρχικού tile στο grid, για να μπορούμε να τοποθετήσουμε τον παίκτη εκεί στην αρχή του παιχνιδιού
    
    void Awake()
    {
        CalculateGridOffsets();
        GenerateGrid();
        SetUpNeighbors();
        
    }

    // Μέθοδος για τον υπολογισμό των αρχικών θέσεων
    private void CalculateGridOffsets()
    {
        Vector3 worldCenter = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 0)); // Υπολογίζουμε το κέντρο του κόσμου με βάση το κέντρο της οθόνης
        worldCenter.z = 0; // Θέτουμε το z στο 0 για να δουλεύουμε σε 2D χώρο

        float gridWidth = width * tileSpacing; // Υπολογίζουμε το συνολικό πλάτος του grid
        float gridHeight = height * tileSpacing; // Υπολογίζουμε το συνολικό ύψος του grid

        startX = worldCenter.x - (gridWidth / 2) + (tileSpacing / 2); // Υπολογίζουμε την αρχική x θέση για το πρώτο tile, προσθέτοντας tileSpacing/2 για να ξεκινάμε από το κέντρο του πρώτου tile
        startY = worldCenter.y - (gridHeight / 2) + (tileSpacing / 2); // Υπολογίζουμε την αρχική y θέση για το πρώτο tile, προσθέτοντας tileSpacing/2 για να ξεκινάμε από το κέντρο του πρώτου tile
    }

    // Βασική μέθοδος για τη δημιουργία του grid.
    void GenerateGrid()
    {
        // Δημιουργία πίνακα για να κρατάμε τις αναφορές στα tiles,
        // ώστε να μπορούμε εύκολα να τα διαχειριστούμε (π.χ. να αποκαλύπτουμε το tile που βρίσκεται ο παίκτης)
        grid = new BaseTile[width, height];

        // Ορισμός συντεταγμένων για τις θέσεις του αρχικού tile και του tile εξόδου
        startTilePosition = new Vector2Int(0, Random.Range(0, height)); // Επιλέγουμε τυχαία μια θέση στην πρώτη στήλη 
        Vector2Int exitTilePosition = new Vector2Int(width - 1, Random.Range(0, height)); // Επιλέγουμε τυχαία μια θέση στην τελευταία στήλη για το tile εξόδου

        // Δημιουργία λίστας με όλες τι διαθέσιμες θέσεις για τα action tiles.
        List<Vector2Int> actionTilePositions = GetRandomActionPositions(startTilePosition, exitTilePosition);
        
        // Δημιοιυργία των tiles 
        SpawnAllTiles(actionTilePositions, exitTilePosition); // Δημιουργούμε όλα τα tiles στο grid, περνώντας τις θέσεις των action tiles και του tile εξόδου για να καθορίσουμε ποιο prefab θα χρησιμοποιηθεί για κάθε tile
    }

    private List<Vector2Int> GetRandomActionPositions(Vector2Int startTilePosition, Vector2Int exitTilePosition)
    {
        List<Vector2Int> availablePositions = new List<Vector2Int>();

        // Προσθέτουμε όλες τις θέσεις εκτός από το αρχικό tile και το tile εξόδου στη λίστα των διαθέσιμων θέσεων για τα action tiles
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2Int pos = new Vector2Int(x, y);
                if (pos != startTilePosition && pos != exitTilePosition)
                {
                    availablePositions.Add(pos);
                }
            }
        }

        // Επιλέγουμε τυχαία 5 θέσεις για τα action tiles από τη λίστα των διαθέσιμων θέσεων
        return RandomPositions(availablePositions, 5); // Επιστρέφουμε τη λίστα με τις επιλεγμένες θέσεις για τα action tiles
    }

    // Μέθοδος για την τυχαία επιλογή θέσεων για τα action tiles
    private List<Vector2Int> RandomPositions(List<Vector2Int> availablePositions, int count)
    {
        List<Vector2Int> selectedPositions = new List<Vector2Int>();

        for (int i = 0; i < count; i++)
        {
            if (availablePositions.Count == 0) break; // Αν δεν υπάρχουν διαθέσιμες θέσεις, σταματάμε

            int randomIndex = Random.Range(0, availablePositions.Count); // Επιλέγουμε τυχαία ένα index από τις διαθέσιμες θέσεις
            selectedPositions.Add(availablePositions[randomIndex]); // Προσθέτουμε τη θέση στη λίστα των επιλεγμένων
            availablePositions.RemoveAt(randomIndex); // Αφαιρούμε τη θέση από τις διαθέσιμες για να μην επιλεγεί ξανά
        }
        return selectedPositions;
    }

    // Mέθοδος για την δημιουργία των tiles στις κατάλληλες θέσεις. 
    private void SpawnAllTiles(List<Vector2Int> actionTilePositions, Vector2Int exitTilePosition)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                 // Δημιουργούμε ένα tile για κάθε θέση στο grid,
                 // περνώντας τις θέσεις των action tiles και του tile εξόδου για να καθορίσουμε ποιο prefab θα χρησιμοποιηθεί
                CreateTileAt(x, y, actionTilePositions, exitTilePosition);
            }
        }
    }

    // Μέθοδος για τη δημιουργία ενός tile στη θέση (x, y) και την τοποθέτηση του στο grid
    private void CreateTileAt(int x, int y, List<Vector2Int> actionTilePositions, Vector2Int exitTilePosition)
    {
        BaseTile tileToInstantiate;

        // Επιλογή prefab για το tile που θα δημιουργηθεί, με βάση τις θέσεις των action tiles και του tile εξόδου
        if (x == exitTilePosition.x && y == exitTilePosition.y) // Ελέγχουμε αν η τρέχουσα θέση είναι για το tile εξόδου
        {
            tileToInstantiate = exitTilePrefab; // Αν ναι, χρησιμοποιούμε το prefab του tile εξόδου
        }
        else if (actionTilePositions.Exists(pos => pos.x == x && pos.y == y)) // Ελέγχουμε αν η τρέχουσα θέση είναι για ένα action tile
        {
            tileToInstantiate = actionTilePrefab; // Αν ναι, χρησιμοποιούμε το prefab του action tile
        }
        else
        {
            tileToInstantiate = emptyTilePrefab; // Αν όχι, χρησιμοποιούμε το prefab του κενό tile
        }

        // Τοποθέτηση του tile στη σκηνή με βάση τις υπολογισμένες αρχικές συντεταγμένες και την απόσταση μεταξύ των tiles
        Vector3 position = new Vector3(startX + (x * tileSpacing), startY + (y * tileSpacing), 0); // Υπολογίζουμε τη θέση του tile με βάση τις αρχικές συντεταγμένες και την απόσταση μεταξύ των tiles
        BaseTile newTile = Instantiate(tileToInstantiate, position, Quaternion.identity); // Δημιουργούμε το tile στη σκηνή
        newTile.name = $"Tile_{x}_{y}"; // Ονομάζουμε το tile για ευκολότερη αναγνώριση στην ιεραρχία
        newTile.transform.parent =transform; // Ορίζουμε το GridManager ως γονέα του tile για καλύτερη οργάνωση στην ιεραρχία
        newTile.SetUp(x,y); // Ρυθμίζουμε τις συντεταγμένες του tile για να μπορεί να γνωρίζει τη θέση του στο grid
        grid[x, y] = newTile; // Αποθηκεύουμε την αναφορά στο νέο tile στον πίνακα grid
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
    public void UpdateGridVisibility(int x, int y)
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
        return grid[startTilePosition.x, startTilePosition.y]; // Επιστρέφουμε το tile που βρίσκεται στη θέση του αρχικού tile
    }
}
