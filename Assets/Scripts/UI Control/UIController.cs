using System;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour {
    
    
    [HideInInspector] public GameManager gameManager;
    
    [SerializeField] private GameObject tileButton;
    [SerializeField] private GameObject buttonsParent;

    private int numberOfTiles;
    private int lastDisabledButtonIndex;
    private readonly List<ButtonController> buttons = new();

    public void SetupUI(int tilesCount) {
        numberOfTiles = tilesCount;

        for (int i = 0; i < numberOfTiles; i++) {
            var newButton = Instantiate(tileButton, buttonsParent.transform, true);
            newButton.GetComponent<ButtonController>().SetupButton(i+1, i);
            buttons.Add(newButton.GetComponent<ButtonController>());
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
