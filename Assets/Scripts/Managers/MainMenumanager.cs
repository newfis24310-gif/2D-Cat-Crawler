using UnityEngine;
using UnityEngine.SceneManagement;  

public class MainMenumanager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void StartGame()
    {
        SceneManager.LoadScene("SampleScene"); // Φορτώνουμε τη σκηνή του παιχνιδιού όταν ο παίκτης επιλέξει "Start Game"
    }

    public void QuitGame()
    {
        Application.Quit(); // Κλείνουμε την εφαρμογή όταν ο παίκτης επιλέξει "Quit Game"
        Debug.Log("Quit Game called. If running in the editor, this will not close the application.");
    }

}
