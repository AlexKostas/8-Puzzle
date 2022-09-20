using System;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour {
    [SerializeField] private GameObject tileButton;
    [SerializeField] private GameObject buttonsParent;

    private int numberOfTiles;
    private int lastDisabledButtonIndex;
    private List<ButtonController> buttons = new List<ButtonController>();

    private void Start() {
        SetupUI(9);
    }

    public void SetupUI(int tilesCount) {
        numberOfTiles = tilesCount;

        for (int i = 0; i < numberOfTiles; i++) {
            var newButton = Instantiate(tileButton, buttonsParent.transform, true);
            newButton.GetComponent<ButtonController>().SetupButton(i+1);
            buttons.Add(newButton.GetComponent<ButtonController>());
        }
        
        buttons[numberOfTiles-1].DisableButton();
        lastDisabledButtonIndex = numberOfTiles - 1;
    }
    
    public void UpdateBoardUI() {
        
    }
}
