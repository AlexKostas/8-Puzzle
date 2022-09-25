using System.Collections.Generic;
using UnityEngine;

public class Board {
    private int numberOfTiles;
    private int dimension;
    private int evaluation;
    private List<int> boardList;
    private List<Move> movesHistory;

    public Board(int dimensions) {
        initializeBoard(dimensions, true);
    }

    public Board(int dimensions, bool shuffle) {
        initializeBoard(dimensions, shuffle);
    }

    private void initializeBoard(int dimensions, bool shuffle) {
        Debug.Assert(dimensions >= 2);
        
        numberOfTiles = dimensions*dimensions;
        dimension = dimensions;
        
        boardList = new List<int>(numberOfTiles);
        for (int i = 0; i < numberOfTiles - 1; i++)
            boardList.Add(i+1);

        boardList.Add(0);

        movesHistory = new List<Move>();
        
        if(shuffle) ShuffleTileOrder();
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

        int emptyTileIndex = FindTile(0);
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
        int emptyTileIndex = FindTile(0);
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

            Move newMove = new Move(emptyTileIndex, i);
            newBoard.addMoveInHistory(newMove);
            
            successors.Add(newBoard);
        }

        return successors;
    }

    public int GetNumberOfTiles() {
        return numberOfTiles;
    }

    public void ShuffleTileOrder() {
        do {
            shuffleArray();
        } while (!isSolvable());
    }

    private void shuffleArray() {
        var randomNumberGenerator = new System.Random();
        int n = boardList.Count;
        while (n > 1) {
            n--;
            int k = randomNumberGenerator.Next(n + 1);
            (boardList[k], boardList[n]) = (boardList[n], boardList[k]);
        }
    }

    private void addMoveInHistory(Move move) {
        Debug.Assert(move != null);
        movesHistory.Add(move);
    }

    public void ExecuteMove(Move move) {
        (boardList[move.GetFrom()], boardList[move.GetTo()]) = 
            (boardList[move.GetTo()], boardList[move.GetFrom()]);
    }

    public void EvaluateState(IEvaluate evaluationFunction, Board targetState) {
        evaluation = evaluationFunction.EvaluateState(this, targetState);
    }

    public int GetEvaluation() {
        return evaluation;
    }

    public int GetNumberOfMoves() {
        return movesHistory.Count;
    }

    public List<Move> GetMovesHistory() {
        return movesHistory;
    }
    
    public int FindTile(int tileValue) {
        for (int i = 0; i < boardList.Count; i++) 
            if (boardList[i] == tileValue) return i;

        return -1;
    }

    /* This is calculated according to the algorithm and code explained in this website
     https://www.geeksforgeeks.org/check-instance-8-puzzle-solvable/ */
    private bool isSolvable() {
        int inversionCount = 0;
        for(int i = 0; i < numberOfTiles; i++)
            for(int j = i+1; j < numberOfTiles; j++)
                if (boardList[i] > 0 && boardList[j] > 0 && boardList[i] > boardList[j])
                    inversionCount++;

        return inversionCount % 2 == 0;
    }

    private static Board cloneBoard(Board referenceBoard) {
        Board newBoard = new Board(referenceBoard.dimension);

        for (int i = 0; i < referenceBoard.numberOfTiles; i++) 
            newBoard.boardList[i] = referenceBoard.boardList[i];

        foreach (Move move in referenceBoard.movesHistory) 
            newBoard.addMoveInHistory(move);

        return newBoard;
    }
}