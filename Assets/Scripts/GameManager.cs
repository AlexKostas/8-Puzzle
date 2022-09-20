using System;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager instance;
    
    [SerializeField] private int boardDimensions;
    [SerializeField] private UIController controller;

    private Board board;

    private void Awake() {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start() {
        board = new Board(boardDimensions);

        controller.gameManager = this;
        controller.SetupUI(boardDimensions*boardDimensions);
        controller.UpdateBoardUI(board);
    }

    public void OnButtonClicked(int buttonIndex) {
        board.OnTileClicked(buttonIndex);
        controller.UpdateBoardUI(board);
    }
}