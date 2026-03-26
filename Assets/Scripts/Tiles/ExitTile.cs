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

        SetBoxOpen(false); // Ξεκινάμε με το κουτί ανοιχτό
        SetMouseVisible(FindAnyObjectByType<Mouse>(), false); // Το ποντίκι είναι κρυφό στην αρχή
    }

    // Καλείται όταν ο παίκτης μπαίνει στο tile εξόδου
    public override void OnPlayerEnter()
    {
        if (sequenceStarted) return;
        StartCoroutine(PlayExitSequence(GameManager.Instance.player));
    }

    /*
     * Η βασική ακολουθία εξόδου:
     * - εμφανίζεται το ποντίκι
     * - κλείνει το κουτί
     * - ξανανοίγει το κουτί
     * - εμφανίζεται μόνο η γάτα ή μόνο το ποντίκι
     * - στο τέλος ενημερώνεται ο GameManager
     */
    private IEnumerator PlayExitSequence(Player player)
    {
        sequenceStarted = true;
        player.canMove = false;

        Mouse mouse = FindAnyObjectByType<Mouse>();
        SpriteRenderer playerRenderer = player.GetComponent<SpriteRenderer>();

        // 1. Εμφάνιση ποντικιού πάνω στο exit
        SetBoxOpen(true);
        SetMouseVisible(mouse, true);
        yield return new WaitForSeconds(mouseVisibleTime);

        // 2. Κλείσιμο κουτιού
        SetBoxOpen(false);
        SetMouseVisible(mouse, false);
        // Όσο το κουτί είναι κλειστό, κρύβουμε τη γάτα
        if (playerRenderer != null)
        {
            playerRenderer.enabled = false;
        }
        yield return new WaitForSeconds(boxClosedTime);

        // 3. Αν η γάτα ζει, κρύβουμε το ποντίκι πριν ξανανοίξει το κουτί
        SetBoxOpen(true);
        yield return new WaitForSeconds(reopenDelay);

        if (player.isAlive)
        {
            if (playerRenderer != null) playerRenderer.enabled = true; 
            SetMouseVisible(mouse, false);
        }
        else
        {
            if (playerRenderer != null) playerRenderer.enabled = false; 
            SetMouseVisible(mouse, true);
        }

        yield return new WaitForSeconds(reopenDelay);
        GameManager.Instance.OnPlayerReachedExit(player.isAlive);

    }

    private void SetBoxOpen(bool isOpen)
    {
        if (tileRenderer == null) return;

        tileRenderer.sprite = isOpen ? openBoxSprite : closedBoxSprite;
        Debug.Log(isOpen ? "Box is now open." : "Box is now closed.");
    }

    private void SetMouseVisible(Mouse mouse, bool isVisible)
    {
        if (mouse == null) return;

        // Μεταφέρουμε το ποντίκι στο exitTile αν πρόκειται να φανεί
        if (isVisible)
        {
            mouse.transform.position = new Vector3(transform.position.x, transform.position.y, -1f);
        }

        SpriteRenderer mouseRenderer = mouse.GetComponent<SpriteRenderer>();
        if (mouseRenderer != null)
        {
            mouseRenderer.enabled = isVisible;
            Debug.Log(isVisible ? "Mouse is now visible." : "Mouse is now hidden.");
        }

        
    }
}