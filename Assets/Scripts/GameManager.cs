using System;
using UnityEngine;

public class GameManager : MonoBehaviour {
    [SerializeField] private int boardDimensions;
    [SerializeField] private UIController controller;

    private void Start() {
        Board board = new Board(boardDimensions);
        controller.SetupUI(boardDimensions*boardDimensions);
        controller.UpdateBoardUI(board);
    }
}