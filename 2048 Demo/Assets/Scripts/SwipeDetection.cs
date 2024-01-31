using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeDetection : MonoBehaviour
{
    private static Vector2 _touchStartPos;
    private static Vector2 _touchEndPos;
    private static bool _isSwiping = false;

    private static float _swipeThresholdPercentage = 0.05f;

    public static bool CheckSwipe(string inputDirection)
    {
        float swipeThreshold = Screen.width * _swipeThresholdPercentage;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    _touchStartPos = touch.position;
                    _isSwiping = true;
                    break;

                case TouchPhase.Moved:
                    _touchEndPos = touch.position;
                    break;

                case TouchPhase.Ended:
                    if (_isSwiping)
                    {
                        float swipeDistance = Vector2.Distance(_touchStartPos, _touchEndPos);

                        if (swipeDistance > swipeThreshold)
                        {
                            Vector2 swipeDirection = _touchEndPos - _touchStartPos;
                            swipeDirection.Normalize();

                            if (Mathf.Abs(swipeDirection.x) > Mathf.Abs(swipeDirection.y))
                            {
                                if (swipeDirection.x > 0)
                                    return inputDirection.Equals("Right");
                                else
                                    return inputDirection.Equals("Left");
                            }
                            else
                            {
                                if (swipeDirection.y > 0)
                                    return inputDirection.Equals("Up");
                                else
                                    return inputDirection.Equals("Down");
                            }
                        }
                    }

                    _isSwiping = false;
                    break;
            }
        }
        return false;
    }
}
