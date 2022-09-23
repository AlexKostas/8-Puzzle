using System;
using System.Collections.Generic;
using UnityEngine;

public class Board {
    private readonly int numberOfTiles;
    private readonly int dimension;
    private readonly List<int> boardList;

    public Board(int dimensions) {
        Debug.Assert(dimensions >= 2);
        
        numberOfTiles = dimensions*dimensions;
        dimension = dimensions;
        
        boardList = new List<int>(numberOfTiles);
        for (int i = 0; i < numberOfTiles - 1; i++)
            boardList.Add(i+1);

        boardList.Add(0);
        
        ShuffleTileOrder();
    }

    public int GetTileValue(int tileIndex) {
        Debug.Assert(tileIndex >= 0);
        Debug.Assert(tileIndex < numberOfTiles);

        return boardList[tileIndex];
    }

    // Returns true if the tile clicked can move otherwise returns false
    public bool OnTileClicked(int tileIndex) {
        Debug.Assert(tileIndex >= 0);
        Debug.Assert(tileIndex < numberOfTiles);
        
        int row = tileIndex / 3;
        int column = tileIndex % 3;

        int emptyTileIndex = findEmptyTile();
        Debug.Assert(emptyTileIndex >= 0 && emptyTileIndex < numberOfTiles);

        int emptyTileRow = emptyTileIndex / 3;
        int emptyTileColumn = emptyTileIndex % 3;

        int manhattanDistance = Utils.ManhattanDistance(column, emptyTileColumn, row, emptyTileRow);
        Debug.Assert(manhattanDistance >= 1);

        if (manhattanDistance > 1) return false;

        // Swapping value via deconstruction
        (boardList[tileIndex], boardList[emptyTileIndex]) = (boardList[emptyTileIndex], boardList[tileIndex]);
        return true;
    }

    public List<Board> GetSuccessorStates() {
        int emptyTileIndex = findEmptyTile();
        Debug.Assert(emptyTileIndex >= 0);
        List<Board> successors = new List<Board>();

        for (int i = 0; i < numberOfTiles; i++) {
            if(i == emptyTileIndex) continue;
            
            int row = i / 3;
            int column = i % 3;
            
            int emptyTileRow = emptyTileIndex / 3;
            int emptyTileColumn = emptyTileIndex % 3;
            
            if(Utils.ManhattanDistance(column, emptyTileColumn, row, emptyTileRow) > 1) continue;

            Board newBoard = cloneBoard(this);
            
            // Swapping value via deconstruction
            (newBoard.boardList[i], newBoard.boardList[emptyTileIndex]) = 
                (newBoard.boardList[emptyTileIndex], newBoard.boardList[i]);
            
            successors.Add(newBoard);
        }

        return successors;
    }

    private static Board cloneBoard(Board referenceBoard) {
        Board newBoard = new Board(referenceBoard.dimension);

        for (int i = 0; i < referenceBoard.numberOfTiles; i++) 
            newBoard.boardList[i] = referenceBoard.boardList[i];
        
        return newBoard;
    }

    public int GetNumberOfTiles() {
        return numberOfTiles;
    }

    public void ShuffleTileOrder() {
        var randomNumberGenerator = new System.Random();
        int n = boardList.Count;  
        while (n > 1) {  
            n--;  
            int k = randomNumberGenerator.Next(n + 1);  
            (boardList[k], boardList[n]) = (boardList[n], boardList[k]);
        }  
    }
    
    private int matchCoordinatesToInternalIndex(int row, int col) {
        Debug.Assert(row >= 0);
        Debug.Assert(row < dimension);
        Debug.Assert(col >= 0);
        Debug.Assert(col < dimension);

        return row * dimension + col;
    }

    private int findEmptyTile() {
        for (int i = 0; i < boardList.Count; i++) 
            if (boardList[i] == 0) return i;

        return -1;
    }
}