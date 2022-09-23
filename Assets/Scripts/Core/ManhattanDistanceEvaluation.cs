public class ManhattanDistanceEvaluation : IEvaluate {
    // Evaluates a state using the manhattan distance function (more in the README)
    public int EvaluateState(Board currentState, Board targetState) {
        // Accumulator for the sum of the manhattan distances
        int sum = 0;
        
        for (int i = 0; i < currentState.GetNumberOfTiles(); i++) {
            int currentPiece = currentState.GetTileValue(i);
            // Skip empty piece
            if(currentPiece == 0) continue;

            // Find the position of current piece in the target state
            int positionInTarget = targetState.FindTile(currentPiece);

            // Calculate row and column values for the two pieces' positions
            int curPieceRow = i / 3;
            int curPieceCol = i % 3;
            
            int targetPieceRow = positionInTarget / 3;
            int targetPieceCol = positionInTarget % 3;

            // Calculate and add to the sum the manhattan distance between the two positions
            sum += Utils.ManhattanDistance(curPieceCol, targetPieceCol, curPieceRow, targetPieceRow);
        }
        
        // The evaluation is twice the total manhattan distance (more gravity to getting the pieces in
        // the right place) plus the cost of reaching current state (number of moves)
        return 2 * sum + currentState.GetNumberOfMoves();
    }
}