using UnityEngine;

public class Move {
    private readonly int fromIndex;
    private readonly int toIndex;

    public Move(int from, int to) {
        Debug.Assert(from >= 0);
        Debug.Assert(to >= 0);

        fromIndex = from;
        toIndex = to;
    }

    public int GetFrom() {
        return fromIndex;
    }

    public int GetTo() {
        return toIndex;
    }
}