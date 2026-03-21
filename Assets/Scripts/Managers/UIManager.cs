using UnityEngine;
using Yarn.Unity;

public class UIManager : MonoBehaviour
{
    public GameObject winLuck, winSkill, lose, resetButton, background;
    
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
}
