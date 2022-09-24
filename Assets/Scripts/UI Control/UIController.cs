using System;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour {
    [SerializeField] private List<ButtonController> buttons = new();

    private int numberOfTiles;
    private int lastDisabledButtonIndex;

    public void SetupUI(int tilesCount) {
        numberOfTiles = tilesCount;

        for (int i = 0; i < numberOfTiles; i++) {
            var button = buttons[i];
            button.GetComponent<ButtonController>().SetupButton(i+1, i);
        }
        
        buttons[numberOfTiles-1].DisableButton();
        lastDisabledButtonIndex = numberOfTiles - 1;
    }
    
    public void UpdateBoardUI(Board board) {
        for (int i = 0; i < board.GetNumberOfTiles(); i++) {
            int tileValue = board.GetTileValue(i);
            
            buttons[i].ChangeButtonNumber(tileValue);

            if (tileValue != 0) continue;
            
            buttons[lastDisabledButtonIndex].EnableButton();
            buttons[i].DisableButton();
            lastDisabledButtonIndex = i;
        }
    }
}
