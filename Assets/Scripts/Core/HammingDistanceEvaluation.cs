public class HammingDistanceEvaluation : IEvaluate{
    // Evaluates the state using the Hamming Distance function (more in the README)
    public int EvaluateState(Board currentState, Board targetState) {
        int piecesOutOfPosition = 0;

        // Counts how many pieces are not in their correct position
        for (int i = 0; i < currentState.GetNumberOfTiles(); i++) {
            if (currentState.GetTileValue(i) != targetState.GetTileValue(i))
                piecesOutOfPosition++;
        }
        
        // The evaluation is twice the pieces out of positions (more
        // gravity to getting the pieces in the right place) plus the
        // cost of reaching current state (number of moves)
        return 2 * piecesOutOfPosition + currentState.GetNumberOfMoves();
    }
}