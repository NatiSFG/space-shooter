using UnityEngine;

public  class LevelBounds : MonoBehaviour {
    public int leftBound = -9;
    public int rightBound = 9;

    [Space(20)]
    public int topBound = 9;
    public int bottomBound = -8;

    public int LeftBound => leftBound;
    public int RightBound => rightBound;
    public int TopBound => topBound;
    public int BottomBound => bottomBound;
}
