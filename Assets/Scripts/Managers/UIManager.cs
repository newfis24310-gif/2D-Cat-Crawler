using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject winLuck, winSkill, lose, resetButton;
    
    public void WinLuckGame()
    {
        winLuck.SetActive(true);
        resetButton.SetActive(true);
    }

    public void WinSkillGame()
    {
        winSkill.SetActive(true);
        resetButton.SetActive(true);
    }

    public void LoseGame()
    {
        lose.SetActive(true);
        resetButton.SetActive(true);
    }
}
