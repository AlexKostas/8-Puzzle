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
}