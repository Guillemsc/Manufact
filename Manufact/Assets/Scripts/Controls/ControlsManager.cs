using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsManager : Singleton<ControlsManager>
{
    private Lean.Touch.LeanFinger curr_finger = null;

    private void Awake()
    {
        InitInstance(this, gameObject);

        Lean.Touch.LeanTouch.OnFingerDown += OnFingerDown;
        Lean.Touch.LeanTouch.OnFingerUp += OnFingerUp;
        //Lean.Touch.LeanTouch.OnFingerSwipe += OnFingerSwipe;
    }

    private void Update()
    {
        CheckOnFingerDistance();

        if (Input.GetKeyDown("a"))
            OnSwipeLeft();
        if (Input.GetKeyDown("d"))
            OnSwipeRight();
        if (Input.GetKeyDown("w"))
            OnSwipeUp();
        if (Input.GetKeyDown("s"))
            OnSwipeDown();
    }

    private void OnFingerDown(Lean.Touch.LeanFinger finger)
    {
        if (curr_finger == null)
            curr_finger = finger;
    }

    private void OnFingerUp(Lean.Touch.LeanFinger finger)
    {
        if (finger == curr_finger)
            curr_finger = null;
    }

    private void CheckOnFingerDistance()
    {
        if(curr_finger != null)
        {
            Vector2 start = curr_finger.StartScreenPosition;
            Vector2 curr = curr_finger.ScreenPosition;

            if(Mathf.Abs(Vector2.Distance(start, curr)) > 40)
            {
                OnFingerSwipe(curr_finger);
                Debug.Log("swiped");
                curr_finger = null;
            }
        }
    }

    private void OnFingerSwipe(Lean.Touch.LeanFinger finger)
    {
        float angle_threshold = 90;

        float up_angle = 0.0f;
        float right_angle = 90.0f;
        float down_angle = 180.0f;
        float left_angle = 270.0f;

        bool up = false;
        bool right = false;
        bool down = false;
        bool left = false;

        if (finger.StartedOverGui == true)
        {
            return;
        }

        Vector2 swipeDelta = finger.SwipeScreenDelta;

        // Invalid angle?
        float angle = Mathf.Atan2(swipeDelta.x, swipeDelta.y) * Mathf.Rad2Deg;

        // up
        float delta = Mathf.DeltaAngle(angle, up_angle);

        if (delta >= angle_threshold * -0.5f && delta < angle_threshold * 0.5f)
        {
            up = true;
        }

        // right
        delta = Mathf.DeltaAngle(angle, right_angle);

        if (delta >= angle_threshold * -0.5f && delta < angle_threshold * 0.5f)
        {
            right = true;
        }

        // down
        delta = Mathf.DeltaAngle(angle, down_angle);

        if (delta >= angle_threshold * -0.5f && delta < angle_threshold * 0.5f)
        {
            down = true;
        }

        // left
        delta = Mathf.DeltaAngle(angle, left_angle);

        if (delta >= angle_threshold * -0.5f && delta < angle_threshold * 0.5f)
        {
            left = true;
        }

        if (up)
            OnSwipeUp();

        if (right)
            OnSwipeRight();

        if (down)
            OnSwipeDown();

        if (left)
            OnSwipeLeft();
    }

    private void OnSwipeUp()
    {
        EventManager.Event ev = new EventManager.Event(EventManager.EventType.CONTROLS_SWIPE_UP);
        EventManager.Instance.SendEvent(ev);
    }

    private void OnSwipeRight()
    {
        EventManager.Event ev = new EventManager.Event(EventManager.EventType.CONTROLS_SWIPE_RIGHT);
        EventManager.Instance.SendEvent(ev);
    }

    private void OnSwipeDown()
    {
        EventManager.Event ev = new EventManager.Event(EventManager.EventType.CONTROLS_SWIPE_DOWN);
        EventManager.Instance.SendEvent(ev);
    }

    private void OnSwipeLeft()
    {
        EventManager.Event ev = new EventManager.Event(EventManager.EventType.CONTROLS_SWIPE_LEFT);
        EventManager.Instance.SendEvent(ev);
    }
}
