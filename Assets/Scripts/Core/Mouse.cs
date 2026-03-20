using System.Collections;
using UnityEngine;

public class Mouse : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false; // Στην αρχή, το ποντίκι είναι κρυφό
    }

    public void StartMouse(Vector3 startPosition, Vector3 exitPosition)
    {
        StartCoroutine(MoveMouse(startPosition, exitPosition));
    }

    public IEnumerator MoveMouse(Vector3 startPosition, Vector3 exitPosition)
    {

        Vector3 offGridStart = startPosition + new Vector3(-4f, -1f, 0f);
        transform.position = new Vector3(offGridStart.x, offGridStart.y, -1f); // Τοποθετούμε το ποντίκι εκτός grid κάτω από το startTile
        //Εμφανίζουμε το ποντίκι εκτός του grid
        spriteRenderer.enabled = true;
        yield return new WaitForSeconds(1f); // Μικρή καθυστέρηση πριν ξεκινήσει η κίνηση

        // Κίνησ προς το startTile
        float elapsedTime = 0f;
        float moveDuration = 2f; // Διάρκεια κίνησης προς το startTile
        Vector3 initialPosition = transform.position;

        while (elapsedTime < moveDuration)
        {
            transform.position = Vector3.Lerp(new Vector3(offGridStart.x, offGridStart.y, -1f), startPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = startPosition; // Βεβαιωνόμαστε ότι φτάσαμε ακριβώς στο startTile

        // Εξαφανίζουμε το ποντίκι μετά από λίγο
        yield return new WaitForSeconds(1f); // Μικρή καθυστέρηση πριν εξαφανιστεί το ποντίκι
        spriteRenderer.enabled = false;

        // Περιμένουμε λίγο πριν το εμφανίζουμε στο exitTile
        yield return new WaitForSeconds(1f); // Μικρή καθυστέρηση πριν εμφανιστεί στο exitTile

        // Εμφανίζουμε το ποντίκι στο exitTile
        transform.position = exitPosition;
        spriteRenderer.enabled = true;
        
    }
}
