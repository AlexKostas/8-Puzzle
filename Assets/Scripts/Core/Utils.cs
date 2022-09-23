using System;
using UnityEngine;

public static class Utils {
        public static int ManhattanDistance(int x1, int x2, int y1, int y2) { 
                return Math.Abs(x1 - x2) + Math.Abs(y1 - y2);
        }

        // Assumes the states given have the same dimensions
        public static bool StatesEqual(Board state1, Board state2) {
                Debug.Assert(state1.GetNumberOfTiles() == state2.GetNumberOfTiles());

                for (int i = 0; i < state1.GetNumberOfTiles(); i++) 
                        if (state1.GetTileValue(i) !=  state2.GetTileValue(i)) return false;

                return true;
        }
}