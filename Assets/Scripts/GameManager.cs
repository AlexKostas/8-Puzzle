using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager instance;
    
    [SerializeField] private int boardDimensions;
    [SerializeField] private float moveDelay = 1.0f;
    [SerializeField] private UIController controller;
    [SerializeField] private AudioClip failSound;
    [SerializeField] private AudioClip tileMovedSound;

    private Board board;
    private Board targetState;
    private AudioSource audioSource;

    private void Awake() {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start() {
        board = new Board(boardDimensions);
        targetState = new Board(boardDimensions, false);

        controller.gameManager = this;
        controller.SetupUI(boardDimensions*boardDimensions);
        controller.UpdateBoardUI(board);

        audioSource = GetComponent<AudioSource>();
    }

    public void OnButtonClicked(int buttonIndex) {
        bool status = board.OnTileClicked(buttonIndex);
        
        // If the tile moved
        if (status) {
            controller.UpdateBoardUI(board);
            audioSource.PlayOneShot(tileMovedSound);
        }
        else 
            audioSource.PlayOneShot(failSound);
    }

    public void OnShuffleButtonClicked() {
        board.ShuffleTileOrder();
        audioSource.PlayOneShot(tileMovedSound);
        controller.UpdateBoardUI(board);
    }

    public void OnSolveButtonClicked() {
        try {
            var moves = AI.SolvePuzzle(board, targetState, new ManhattanDistanceEvaluation());
            StartCoroutine(displayMoves(moves));
        }
        catch (ApplicationException) {
            audioSource.PlayOneShot(failSound);
        }
    }

    public void OnGetMoveButtonClicked() {
        try {
            var moves = AI.SolvePuzzle(board, targetState, new ManhattanDistanceEvaluation());
            if (moves.Count <= 0) return;
            
            board.ExecuteMove(moves[0]);
            
            audioSource.PlayOneShot(tileMovedSound);
            controller.UpdateBoardUI(board);
        }
        catch (ApplicationException) {
            audioSource.PlayOneShot(failSound);
        }
    }

    private IEnumerator displayMoves(List<Move> moves) {
        while (moves.Count > 0) {
            board.ExecuteMove(moves[0]);
            moves.RemoveAt(0);
            
            audioSource.PlayOneShot(tileMovedSound);
            controller.UpdateBoardUI(board);

            yield return new WaitForSeconds(moveDelay);
        }
    }
}