using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    public bool GetInput(string direction)
    {
#if UNITY_EDITOR
        return Input.GetButtonDown(direction);
#else
        return SwipeDetection.CheckSwipe(direction);
#endif
    }
}
