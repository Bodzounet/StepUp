using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using XInputDotNetPure;

public class JoystickManagerMini
{
    public enum e_XBoxControllerAxis
    {
        Horizontal,
        Vertical,
        HCross,
        VCross,
        HJoyStick,
        VJoyStick,
        RT,
        LT
    }

    public enum e_XBoxControllerButtons
    {
        A,
        B,
        Y,
        X,
        Start,
        Back,
        RB,
        LB,
        LeftJSClick,
        RightJSClick,
        RT,
        LT
    }

    [SerializeField]
    private int _joystickId;
    public int JoystickId
    {
        get { return _joystickId; }
        set
        {
            _joystickId = value;
        }
    }
    

    private bool _isVibrate;
    public bool isVibrate { get { return _isVibrate; } }

    GamePadState state;
    GamePadState prevState;

    public JoystickManagerMini(int id)
    {
        Reset(id);
    }

    public void Reset(int id)
    {
        JoystickId = id;
    }

    public float GetAxis(e_XBoxControllerAxis axis)
    {
        return GetAxisState(axis);
    }

    public float GetAxisAngle()
    {
        // minus -> trigo rotation
        return -Mathf.Atan2(GetAxis(e_XBoxControllerAxis.Vertical), GetAxis(e_XBoxControllerAxis.Horizontal)) * Mathf.Rad2Deg;
    }


    public bool GetButtonDown(e_XBoxControllerButtons btn)
    {
        if (btn == e_XBoxControllerButtons.RT || btn == e_XBoxControllerButtons.LT)
        {
            float res = 0.0f;

            if (btn == e_XBoxControllerButtons.RT)
                res = GetAxis(e_XBoxControllerAxis.RT);
            else
                res = GetAxis(e_XBoxControllerAxis.LT);

            return res != 0;
        }
        return GetBtnState(btn) == ButtonState.Pressed && GetBtnPrevState(btn) == ButtonState.Released;
    }

    public bool GetButton(e_XBoxControllerButtons btn)
    {
        if (btn == e_XBoxControllerButtons.RT || btn == e_XBoxControllerButtons.LT)
        {
            float res = 0.0f;

            if (btn == e_XBoxControllerButtons.RT)
                res = GetAxis(e_XBoxControllerAxis.RT);
            else
                res = GetAxis(e_XBoxControllerAxis.LT);

            return res != 0;
        }
        return GetBtnState(btn) == ButtonState.Pressed;
    }

    public bool GetButtonUp(e_XBoxControllerButtons btn)
    {
        if (btn == e_XBoxControllerButtons.RT || btn == e_XBoxControllerButtons.LT)
        {
            float res = 0.0f;

            if (btn == e_XBoxControllerButtons.RT)
                res = GetAxis(e_XBoxControllerAxis.RT);
            else
                res = GetAxis(e_XBoxControllerAxis.LT);

            return res == 0;
        }
        return GetBtnState(btn) == ButtonState.Released && GetBtnPrevState(btn) == ButtonState.Pressed;
    }

    private ButtonState GetBtnState(e_XBoxControllerButtons btn)
    {
        if (!state.IsConnected)
            return ButtonState.Released;

        if (btn == e_XBoxControllerButtons.A)
            return state.Buttons.A;
        if (btn == e_XBoxControllerButtons.B)
            return state.Buttons.B;
        if (btn == e_XBoxControllerButtons.X)
            return state.Buttons.X;
        if (btn == e_XBoxControllerButtons.Y)
            return state.Buttons.Y;
        if (btn == e_XBoxControllerButtons.Start)
            return state.Buttons.Start;
        if (btn == e_XBoxControllerButtons.Back)
            return state.Buttons.Back;
        if (btn == e_XBoxControllerButtons.RB)
            return state.Buttons.RightShoulder;
        if (btn == e_XBoxControllerButtons.LB)
            return state.Buttons.LeftShoulder;
        if (btn == e_XBoxControllerButtons.RightJSClick)
            return state.Buttons.RightStick;
        if (btn == e_XBoxControllerButtons.LeftJSClick)
            return state.Buttons.LeftStick;


        return ButtonState.Released;
    }

    private ButtonState GetBtnPrevState(e_XBoxControllerButtons btn)
    {
        if (!prevState.IsConnected)
            return ButtonState.Released;

        if (btn == e_XBoxControllerButtons.A)
            return prevState.Buttons.A;
        if (btn == e_XBoxControllerButtons.B)
            return prevState.Buttons.B;
        if (btn == e_XBoxControllerButtons.X)
            return prevState.Buttons.X;
        if (btn == e_XBoxControllerButtons.Y)
            return prevState.Buttons.Y;
        if (btn == e_XBoxControllerButtons.Start)
            return prevState.Buttons.Start;
        if (btn == e_XBoxControllerButtons.Back)
            return prevState.Buttons.Back;
        if (btn == e_XBoxControllerButtons.RB)
            return prevState.Buttons.RightShoulder;
        if (btn == e_XBoxControllerButtons.LB)
            return prevState.Buttons.LeftShoulder;
        if (btn == e_XBoxControllerButtons.RightJSClick)
            return prevState.Buttons.RightStick;
        if (btn == e_XBoxControllerButtons.LeftJSClick)
            return prevState.Buttons.LeftStick;


        return ButtonState.Released;
    }

    private float GetAxisState(e_XBoxControllerAxis axis)
    {
        if (!state.IsConnected)
            return .0f;

        if (axis == e_XBoxControllerAxis.Horizontal)
            return state.ThumbSticks.Left.X;
        if (axis == e_XBoxControllerAxis.Vertical)
            return state.ThumbSticks.Left.Y;
        if (axis == e_XBoxControllerAxis.HCross)
            return state.DPad.Left == ButtonState.Pressed ? -1.0f : (state.DPad.Right == ButtonState.Pressed ? 1.0f : 0.0f);
        if (axis == e_XBoxControllerAxis.VCross)
            return state.DPad.Down == ButtonState.Pressed ? -1.0f : (state.DPad.Up == ButtonState.Pressed ? 1.0f : 0.0f);
        if (axis == e_XBoxControllerAxis.HJoyStick)
            return state.ThumbSticks.Right.X;
        if (axis == e_XBoxControllerAxis.VJoyStick)
            return state.ThumbSticks.Right.Y;
        if (axis == e_XBoxControllerAxis.RT)
            return state.Triggers.Right;
        if (axis == e_XBoxControllerAxis.LT)
            return state.Triggers.Left;

        return .0f;
    }

    public void Update()
    {
        prevState = state;
        state = GamePad.GetState((PlayerIndex)_joystickId);
    }

    public void SetVibration(float left, float right)
    {
        GamePad.SetVibration((PlayerIndex)_joystickId, left, right);

        if (left == .0f && right == .0f)
            _isVibrate = false;
        else
            _isVibrate = true;
    }
}