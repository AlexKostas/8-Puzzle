using System;
using System.Collections.Generic;
using Priority_Queue;

public static class AI {
    private const int MAX_LOOPS = 100000;

    public static List<Move> SolvePuzzle(Board startingState, Board targetState, IEvaluate evaluationFunction) {
        // Initialize a priority queue with the starting state as its only element
        SimplePriorityQueue<Board> pq = new SimplePriorityQueue<Board>();
        pq.Enqueue(startingState, 0);

        Board currentBoard;
        // We need to keep track of the previous state to avoid
        // adding the same states to the queue multiple times
        Board previousBoard = null;
        
        // We are counting the loops to ensure the program does not crash
        int numberOfLoops = 0;
        
        // Remove the state with the lowest evaluation
        // (closer to target) from the Queue until we
        // reach the target state
        while (!Utils.StatesEqual(currentBoard = pq.Dequeue(), targetState)) {
            //Exiting with an Exception after a fixed number of loops
            if (numberOfLoops >= MAX_LOOPS) throw new ApplicationException("Solving the puzzle failed");
            
            // Get successor states
            List<Board> successors = currentBoard.GetSuccessorStates();

            foreach (Board successor in successors) {
                // Calculate the evaluation of the state. Stored inside the successor object
                successor.EvaluateState(evaluationFunction, targetState);
                
                // Don't add a state to the queue if it is the
                // same as its grandparent (optimization)
                if (previousBoard == null)
                    pq.Enqueue(successor, successor.GetEvaluation());
                else {
                    if (!Utils.StatesEqual(previousBoard, successor))  
                        pq.Enqueue(successor, successor.GetEvaluation());
                }
            }
            
            // Update previous state
            previousBoard = currentBoard;
            
            // Updating loop count
            numberOfLoops++;
        }
        
        // Immediately after the loop's exit we know that 'currentState' contains the targetState along with the history
        // of how it was reached
        return currentBoard.GetMovesHistory();
    }
}