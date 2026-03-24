using UnityEngine;
using Yarn.Unity;

public class UIManager : MonoBehaviour
{
    public GameObject winLuck;
    public GameObject winSkill;
    public GameObject lose;
    public GameObject resetButton;
    public GameObject background;
    public GameObject optionsMessage;
    public GameObject optionsMenu;

    private bool optionsMenuState = false;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleOptionsMenu();
        }
    }
    
    [YarnCommand("winluck")]
    public void WinLuckGame()
    {
        winLuck.SetActive(true);
        resetButton.SetActive(true);
        background.SetActive(true);
    }
    
    [YarnCommand("winskill")]
    public void WinSkillGame()
    {
        winSkill.SetActive(true);
        resetButton.SetActive(true);
        background.SetActive(true);
    }

    [YarnCommand("lose")]
    public void LoseGame()
    {
        lose.SetActive(true);
        resetButton.SetActive(true);
        background.SetActive(true);
    }

    private void ToggleOptionsMenu()
    {
        if(optionsMenuState == false)
        {
            optionsMessage.SetActive(false);
            optionsMenu.SetActive(true);
            optionsMenuState = true;
        }
        else if(optionsMenuState == true)
        {
            optionsMessage.SetActive(true);
            optionsMenu.SetActive(false);
            optionsMenuState = false;
        }
    }
}
