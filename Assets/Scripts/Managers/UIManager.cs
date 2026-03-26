using UnityEngine;
using System.Collections;
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
    public CanvasGroup titleCard;
    [SerializeField] float titleAttack = 3f;
    [Range(0f, 1f)]
    [SerializeField] float fadeSpeed = 0.5f;
    [SerializeField] float titleWait = 5f;

    private bool optionsMenuState = false;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleOptionsMenu();
            SoundManager.Instance.PlayVolumeButton();
        }
    }
    
    [YarnCommand("winluck")]
    public void WinLuckGame()
    {
        winLuck.SetActive(true);
        resetButton.SetActive(true);
        background.SetActive(true);
        SoundManager.Instance.PlayWinStinger(); // Παίζουμε τη μουσική νίκης
    }
    
    [YarnCommand("winskill")]
    public void WinSkillGame()
    {
        winSkill.SetActive(true);
        resetButton.SetActive(true);
        background.SetActive(true);
        SoundManager.Instance.PlayWinStinger(); // Παίζουμε τη μουσική νίκης
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

    public IEnumerator TitleCardFade()
    {  
        yield return new WaitForSeconds(titleAttack);
        float t = 0f;

        while(t < 1)
        {
            t += Time.deltaTime * fadeSpeed;
            titleCard.alpha = Mathf.Clamp01(t);
            yield return null;
        }

        yield return new WaitForSeconds(titleWait);

        while(t > 0)
        {
            t -= Time.deltaTime * fadeSpeed;
            titleCard.alpha = Mathf.Clamp01(t);
            yield return null;
        }
    }
}
