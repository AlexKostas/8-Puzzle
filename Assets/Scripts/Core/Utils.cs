using System;

public static class Utils {
        public static int ManhattanDistance(int x1, int x2, int y1, int y2) {
                return Math.Abs(x1 - x2) + Math.Abs(y1 - y2);
        }
}