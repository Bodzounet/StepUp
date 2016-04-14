using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class JoyStickManager : MonoBehaviour
{
    public enum e_XBoxControllerAxis
    {
        Horizontal,
        Vertical,
        HCross,
        VCross,
        HJoyStick,
        VJoyStick
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
            setMaps(value.ToString());
        }
    }

    private Dictionary<e_XBoxControllerAxis, string> _givenNameToAxis;
    private Dictionary<e_XBoxControllerButtons, string> _givenNameToButton;

    public void Reset(int id)
    {
        _givenNameToAxis = new Dictionary<e_XBoxControllerAxis, string>();
        _givenNameToButton = new Dictionary<e_XBoxControllerButtons, string>();
        JoystickId = id;
    }

    /// <summary>
    /// Axis are : 
    /// Horizontal (left joystick, left/right)
    /// Vertical (left joystick, up/down)
    /// HCross (cross, left/right)
    /// VCross (cross, up/down)
    /// HJoystick (right joystick, left/right)
    /// VJoystick (right joystick, up/down)
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public float GetAxis(e_XBoxControllerAxis axis)
    {       
        return Input.GetAxis(_givenNameToAxis[axis]);
    }

    /// <summary>
    /// the same as GetAxis, but instead of returning a value clamped in [-1;1], it only returns {-1, 0, 1}
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public int GetAxisClamped(e_XBoxControllerAxis axis)
    {
        var val = Input.GetAxis(_givenNameToAxis[axis]);
        if (val < 0)
            return -1;
        else if (val > 0)
            return 1;
        return 0;
    }

    /// <summary>
    /// names are : 
    /// A, B, Y, X, Start, Back, RB, RT, LB, LT, LeftJSClick, RightJSClick
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public bool GetButtonDown(e_XBoxControllerButtons btn)
    {
        if (btn == e_XBoxControllerButtons.RT || btn == e_XBoxControllerButtons.LT)
            return Input.GetAxis(_givenNameToButton[btn]) != 0;
        return Input.GetButtonDown(_givenNameToButton[btn]);
    }

    public bool GetButtonUp(e_XBoxControllerButtons btn)
    {
        if (btn == e_XBoxControllerButtons.RT || btn == e_XBoxControllerButtons.LT)
            return Input.GetAxis(_givenNameToButton[btn]) == 0;

        return Input.GetButtonUp(_givenNameToButton[btn]);
    }

    private void setMaps(string id)
    {
        _givenNameToAxis[e_XBoxControllerAxis.Horizontal] = "Horizontal" + id;
        _givenNameToAxis[e_XBoxControllerAxis.Vertical] = "Vertical" + id;
        _givenNameToAxis[e_XBoxControllerAxis.HCross] = "HCross" + id;
        _givenNameToAxis[e_XBoxControllerAxis.VCross] = "VCross" + id;
        _givenNameToAxis[e_XBoxControllerAxis.HJoyStick] = "HJoystick" + id;
        _givenNameToAxis[e_XBoxControllerAxis.VJoyStick] = "VJoystick" + id;

        _givenNameToButton[e_XBoxControllerButtons.A] = "A" + id;
        _givenNameToButton[e_XBoxControllerButtons.B] = "B" + id;
        _givenNameToButton[e_XBoxControllerButtons.Y] = "Y" + id;
        _givenNameToButton[e_XBoxControllerButtons.X] = "X" + id;
        _givenNameToButton[e_XBoxControllerButtons.Start] = "Start" + id;
        _givenNameToButton[e_XBoxControllerButtons.Back] = "Back" + id;
        _givenNameToButton[e_XBoxControllerButtons.RB] = "RB" + id;
        _givenNameToButton[e_XBoxControllerButtons.LB] = "LB" + id;
        _givenNameToButton[e_XBoxControllerButtons.LeftJSClick] = "LeftJSClick" + id;
        _givenNameToButton[e_XBoxControllerButtons.RightJSClick] = "RightJSCLick" + id;
        _givenNameToButton[e_XBoxControllerButtons.RT] = "RT" + id;
        _givenNameToButton[e_XBoxControllerButtons.LT] = "LT" + id;
    }
}
