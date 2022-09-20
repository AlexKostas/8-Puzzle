using System;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager instance;
    
    [SerializeField] private int boardDimensions;
    [SerializeField] private UIController controller;
    [SerializeField] private AudioClip failSound;
    [SerializeField] private AudioClip tileMovedSound;

    private Board board;
    private AudioSource audioSource;

    private void Awake() {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start() {
        board = new Board(boardDimensions);

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
}