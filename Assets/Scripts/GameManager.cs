using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager instance;
    
    [SerializeField] private int boardDimensions;
    [SerializeField] private float moveDelay = 1.0f;
    [SerializeField] private UIController controller;
    [SerializeField] private TextMeshProUGUI movesLabel;
    [SerializeField] private TMP_Dropdown dropdown;
    [SerializeField] private AudioClip failSound;
    [SerializeField] private AudioClip tileMovedSound;

    private Board board;
    private Board targetState;
    private AudioSource audioSource;
    private IEvaluate selectedEvaluationFunction;
    private HammingDistanceEvaluation hammingDistanceEval;
    private ManhattanDistanceEvaluation manhattanDistanceEval;
    private bool isSolvingPuzzle;
    private const string defaultMovesText = "Number of moves: ";

    private void Awake() {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start() {
        board = new Board(boardDimensions);
        targetState = new Board(boardDimensions, false);

        controller.SetupUI(boardDimensions*boardDimensions);
        controller.UpdateBoardUI(board);
        resetMovesLabel();

        hammingDistanceEval = new HammingDistanceEvaluation();
        manhattanDistanceEval = new ManhattanDistanceEvaluation();
        selectedEvaluationFunction = manhattanDistanceEval;

        audioSource = GetComponent<AudioSource>();
    }

    public void OnButtonClicked(int buttonIndex) {
        if (isSolvingPuzzle) return;

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
        if (isSolvingPuzzle) return;

        board.ShuffleTileOrder();
        audioSource.PlayOneShot(tileMovedSound);
        controller.UpdateBoardUI(board);
        resetMovesLabel();
    }

    public void OnSolveButtonClicked() {
        if (isSolvingPuzzle) return;

        try {
            var moves = AI.SolvePuzzle(board, targetState, selectedEvaluationFunction);
            movesLabel.SetText(defaultMovesText + moves.Count);
            StartCoroutine(displayMoves(moves));
        }
        catch (ApplicationException) {
            audioSource.PlayOneShot(failSound);
        }
    }

    public void OnGetMoveButtonClicked() {
        if (isSolvingPuzzle) return;
        
        try {
            var moves = AI.SolvePuzzle(board, targetState, selectedEvaluationFunction);
            if (moves.Count <= 0) return;
            
            board.ExecuteMove(moves[0]);
            
            audioSource.PlayOneShot(tileMovedSound);
            controller.UpdateBoardUI(board);
            
            resetMovesLabel();
        }
        catch (ApplicationException) {
            audioSource.PlayOneShot(failSound);
        }
    }

    public void OnExitButtonClicked() {
        Application.Quit();
    }

    public void OnSelectionValueChanged() {
        int value = dropdown.value;
        Debug.Assert(value is >= 0 and <= 1 );

        if (value == 0) selectedEvaluationFunction = manhattanDistanceEval;
        else  selectedEvaluationFunction = hammingDistanceEval;
    }

    private IEnumerator displayMoves(List<Move> moves) {
        isSolvingPuzzle = true;
        while (moves.Count > 0) {
            board.ExecuteMove(moves[0]);
            moves.RemoveAt(0);
            
            audioSource.PlayOneShot(tileMovedSound);
            controller.UpdateBoardUI(board);

            yield return new WaitForSeconds(moveDelay);
        }

        isSolvingPuzzle = false;
    }

    private void resetMovesLabel() {
        movesLabel.SetText(defaultMovesText);
    }
}