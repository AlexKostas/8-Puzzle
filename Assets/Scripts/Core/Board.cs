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
    }

    public int GetTileValue(int tileIndex) {
        Debug.Assert(tileIndex >= 0);
        Debug.Assert(tileIndex < numberOfTiles);

        return boardList[tileIndex];
    }

    public void OnTileClicked(int tileIndex) {
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

        if (manhattanDistance != 1) return;

        // Swapping value via deconstruction
        (boardList[tileIndex], boardList[emptyTileIndex]) = (boardList[emptyTileIndex], boardList[tileIndex]);
    }

    public int GetNumberOfTiles() {
        return numberOfTiles;
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