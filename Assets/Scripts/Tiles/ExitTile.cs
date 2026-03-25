using System.Collections;
using UnityEngine;

/*
 * Όταν ο παίκτης φτάσει εδώ:
 * 1. εμφανίζεται το ποντίκι πάνω στο exit
 * 2. το κουτί κλείνει??
 * 3. το κουτί ξανανοίγει
 * 4. αν η γάτα είναι ζωντανή, φαίνεται μόνο η γάτα
 * 5. αν η γάτα είναι νεκρή, φαίνεται μόνο το ποντίκι
 */
public class ExitTile : BaseTile
{
    [Header("Exit Tile Sprites")]
    public Sprite openBoxSprite;    // Sprite για το ανοιχτό κουτί
    public Sprite closedBoxSprite;  // Sprite για το κλειστό κουτί???

    [Header("Exit Sequence Timings")]
    public float mouseVisibleTime = 1f; // Πόσο μένει ορατό το ποντίκι πριν κλείσει το κουτί
    public float boxClosedTime = 1f;    // Πόσο μένει κλειστό το κουτί
    public float reopenDelay = 0.4f;    // Μικρή καθυστέρηση πριν ξανανοίξει

    private SpriteRenderer tileRenderer; // Ο renderer του ίδιου του ExitTile
    private bool sequenceStarted = false; // Για να μην τρέξει δύο φορές το ίδιο sequence

    void Start()
    {
        tileRenderer = GetComponent<SpriteRenderer>();

        if (tileRenderer == null)
        {
            Debug.LogError("ExitTile has no SpriteRenderer on the same GameObject.");
            return;
        }

        // Αρχικά το exit tile εμφανίζεται ανοιχτό
        if (openBoxSprite != null)
        {
            tileRenderer.sprite = openBoxSprite;
        }

        // Κρύβουμε το ποντίκι στην αρχή,
        // ώστε να μην φαίνεται πριν φτάσει ο παίκτης στο exit
        Mouse mouse = FindAnyObjectByType<Mouse>();
        if (mouse != null)
        {
            SpriteRenderer mouseRenderer = mouse.GetComponent<SpriteRenderer>();
            if (mouseRenderer != null)
            {
                mouseRenderer.enabled = false;
            }
        }
    }

    // Καλείται όταν ο παίκτης μπαίνει στο tile εξόδου
    public override void OnPlayerEnter()
    {
        if (sequenceStarted) return;

        Player player = FindAnyObjectByType<Player>();
        GameManager gameManager = FindAnyObjectByType<GameManager>();

        if (player == null)
        {
            Debug.LogError("Player not found in scene.");
            return;
        }

        if (gameManager == null)
        {
            Debug.LogError("GameManager not found in scene.");
            return;
        }

        StartCoroutine(PlayExitSequence(player, gameManager));
    }

    /*
     * Η βασική ακολουθία εξόδου:
     * - εμφανίζεται το ποντίκι
     * - κλείνει το κουτί
     * - ξανανοίγει το κουτί
     * - εμφανίζεται μόνο η γάτα ή μόνο το ποντίκι
     * - στο τέλος ενημερώνεται ο GameManager
     */
    private IEnumerator PlayExitSequence(Player player, GameManager gameManager)
    {
        sequenceStarted = true;
        player.canMove = false;

        Mouse mouse = FindAnyObjectByType<Mouse>();
        SpriteRenderer playerRenderer = player.GetComponent<SpriteRenderer>();

        // 1. Εμφάνιση ποντικιού πάνω στο exit
        if (mouse != null)
        {
            ShowMouse(mouse);
        }

        yield return new WaitForSeconds(mouseVisibleTime);

        // 2. Κλείσιμο κουτιού
        CloseBox();

        // Όσο το κουτί είναι κλειστό, κρύβουμε τη γάτα
        if (playerRenderer != null)
        {
            playerRenderer.enabled = false;
        }

        yield return new WaitForSeconds(boxClosedTime);

        // 3. Αν η γάτα ζει, κρύβουμε το ποντίκι πριν ξανανοίξει το κουτί
        if (player.isAlive && mouse != null)
        {
            HideMouse(mouse);
        }

        yield return new WaitForSeconds(reopenDelay);

        // 4. Ξανανοίγουμε το κουτί
        OpenBox();

        // 5. Τελική εικόνα: μόνο γάτα ή μόνο ποντίκι
        if (player.isAlive)
        {
            if (playerRenderer != null)
            {
                playerRenderer.enabled = true; // φαίνεται μόνο η γάτα
            }
        }
        else
        {
            if (playerRenderer != null)
            {
                playerRenderer.enabled = false; // η γάτα δεν φαίνεται
            }
        }

        yield return new WaitForSeconds(0.4f);

        // 6. Ενημέρωση του GameManager για το τελικό αποτέλεσμα.
        // Εδώ θα γίνει και το SpawnSkeletonsAtDeathPoints() από τον GameManager.
        gameManager.OnPlayerReachedExit(player.isAlive);

        sequenceStarted = false;
    }

    private void ShowMouse(Mouse mouse)
    {
        mouse.transform.position = new Vector3(transform.position.x, transform.position.y, -1f);

        SpriteRenderer mouseRenderer = mouse.GetComponent<SpriteRenderer>();
        if (mouseRenderer != null)
        {
            mouseRenderer.enabled = true;
        }

        Debug.Log("Mouse is visible on ExitTile.");
    }

    private void HideMouse(Mouse mouse)
    {
        SpriteRenderer mouseRenderer = mouse.GetComponent<SpriteRenderer>();
        if (mouseRenderer != null)
        {
            mouseRenderer.enabled = false;
        }

        Debug.Log("Mouse is hidden.");
    }

    private void CloseBox()
    {
        if (tileRenderer != null && closedBoxSprite != null)
        {
            tileRenderer.sprite = closedBoxSprite;
            Debug.Log("Exit box CLOSED");
        }
        else
        {
            Debug.LogWarning("CloseBox failed: tileRenderer or closedBoxSprite is missing.");
        }
    }

    private void OpenBox()
    {
        if (tileRenderer != null && openBoxSprite != null)
        {
            tileRenderer.sprite = openBoxSprite;
            Debug.Log("Exit box OPENED");
        }
        else
        {
            Debug.LogWarning("OpenBox failed: tileRenderer or openBoxSprite is missing.");
        }
    }
}