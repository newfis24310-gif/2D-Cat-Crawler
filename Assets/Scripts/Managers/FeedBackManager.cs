using UnityEngine;
using Yarn.Unity;

public class FeedBackManager : MonoBehaviour
{   
    public DialogueRunner dialogueRunner;

    // Aυτα ισως ειναι καλυτερα να πανε στον UIManager αλλα προς το παρον ας μεινουν εδω για να μην μπλεξουμε τα πραγματα
    [Header("Dialogue UI Images")]
    public GameObject ImageSc_1;
    public GameObject ImageSc_2;


    public void ShowFailAttemptMessage(int currentAttempt)
    { 
        string nodeName = "EndRound" + currentAttempt;

        if (dialogueRunner != null)
        {
            dialogueRunner.StartDialogue(nodeName);
        }
        else
        {
            Debug.LogError("DialogueRunner reference is missing in FeedBackManager! Please assign it in the inspector.");
        }
    }

    public void ShowWinMessage(int currentAttempt)
    {
        if(currentAttempt == 1){dialogueRunner.StartDialogue("WinFirstRound");}
        else{dialogueRunner.StartDialogue("Win");}
    }

    public void YarnDeathOnceTrue()
    {
        dialogueRunner.VariableStorage.SetValue("$deathOnce", true);
    }

    public void YarnDeathOnceFalse()
    {
        dialogueRunner.VariableStorage.SetValue("$deathOnce", false);
    }

    public void YarnFoundFishTrue()
    {
        dialogueRunner.VariableStorage.SetValue("$foundFish", true);
    }

    public void YarnFoundFishFalse()
    {
        dialogueRunner.VariableStorage.SetValue("$foundFish", false);

    }

    public void ShowImageSc_1(bool state)
    {
        ImageSc_1.SetActive(state);
    }

    public void ShowImageSc_2(bool state)
    {
        ImageSc_2.SetActive(state);
    }
}