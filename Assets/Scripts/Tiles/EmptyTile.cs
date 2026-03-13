using UnityEngine;

public class ExitTile : BaseTile
{
    public new void onPlayerEnter()
    {
        RevealTile(true);
        Debug.Log($"Player reached ExitTile at ({x}, {y})");
    }
}