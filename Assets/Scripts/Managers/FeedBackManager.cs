using UnityEngine;
using Yarn.Unity;

public class FeedBackManager : MonoBehaviour
{   
    public DialogueRunner dialogueRunner;
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


}
