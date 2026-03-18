using UnityEngine;
using Yarn.Unity;

public class FeedBackManager : MonoBehaviour
{
    public DialogueRunner dialogueRunner;
    public void ShowFailAttemptMessage(int currentAttempt)
    {  
        switch(currentAttempt)
        {
            case 1:
                dialogueRunner.StartDialogue("EndRound1");
                break;
            case 2:
                dialogueRunner.StartDialogue("EndRound2");
                break;
            case 3:
                dialogueRunner.StartDialogue("EndRound3");
                break;
            case 4:
                dialogueRunner.StartDialogue("EndRound4");
                break;
            case 5:
                dialogueRunner.StartDialogue("EndRound5");
                break;
            case 6:
                dialogueRunner.StartDialogue("EndRound6");
                break;
            case 7:
                dialogueRunner.StartDialogue("EndRound7");
                break;
        }
        
    }

    public void ShowWinMessage(int currentAttempt)
    {
        if(currentAttempt == 1){dialogueRunner.StartDialogue("WinFirstRound");}
        else{dialogueRunner.StartDialogue("Win");}
    }


}
