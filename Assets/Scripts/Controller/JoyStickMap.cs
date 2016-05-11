using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class JoyStickMap : Singleton<JoyStickMap>
{
    private Dictionary<string, KeyCode> _map = new Dictionary<string, KeyCode>() {
        { "A1", KeyCode.Joystick1Button0 },
        { "B1", KeyCode.Joystick1Button1 },
        { "X1", KeyCode.Joystick1Button2 },
        { "Y1", KeyCode.Joystick1Button3 },
        { "LB1", KeyCode.Joystick1Button4 },
        { "RB1", KeyCode.Joystick1Button5 },
        { "Back1", KeyCode.Joystick1Button6 },
        { "Start1", KeyCode.Joystick1Button7 },
        { "LeftJSClick1", KeyCode.Joystick1Button8 },
        { "RightJSCLick1", KeyCode.Joystick1Button9 },

        { "A2", KeyCode.Joystick2Button0 },
        { "B2", KeyCode.Joystick2Button1 },
        { "X2", KeyCode.Joystick2Button2 },
        { "Y2", KeyCode.Joystick2Button3 },
        { "LB2", KeyCode.Joystick2Button4 },
        { "RB2", KeyCode.Joystick2Button5 },
        { "Back2", KeyCode.Joystick2Button6 },
        { "Start2", KeyCode.Joystick2Button7 },
        { "LeftJSClick2", KeyCode.Joystick2Button8 },
        { "RightJSCLick2", KeyCode.Joystick2Button9 },

        { "A3", KeyCode.Joystick3Button0 },
        { "B3", KeyCode.Joystick3Button1 },
        { "X3", KeyCode.Joystick3Button2 },
        { "Y3", KeyCode.Joystick3Button3 },
        { "LB3", KeyCode.Joystick3Button4 },
        { "RB3", KeyCode.Joystick3Button5 },
        { "Back3", KeyCode.Joystick3Button6 },
        { "Start3", KeyCode.Joystick3Button7 },
        { "LeftJSClick3", KeyCode.Joystick3Button8 },
        { "RightJSCLick3", KeyCode.Joystick3Button9 },

        { "A4", KeyCode.Joystick4Button0 },
        { "B4", KeyCode.Joystick4Button1 },
        { "X4", KeyCode.Joystick4Button2 },
        { "Y4", KeyCode.Joystick4Button3 },
        { "LB4", KeyCode.Joystick4Button4 },
        { "RB4", KeyCode.Joystick4Button5 },
        { "Back4", KeyCode.Joystick4Button6 },
        { "Start4", KeyCode.Joystick4Button7 },
        { "LeftJSClick4", KeyCode.Joystick4Button8 },
        { "RightJSCLick4", KeyCode.Joystick4Button9 },

        { "A5", KeyCode.Joystick5Button0 },
        { "B5", KeyCode.Joystick5Button1 },
        { "X5", KeyCode.Joystick5Button2 },
        { "Y5", KeyCode.Joystick5Button3 },
        { "LB5", KeyCode.Joystick5Button4 },
        { "RB5", KeyCode.Joystick5Button5 },
        { "Back5", KeyCode.Joystick5Button6 },
        { "Start5", KeyCode.Joystick5Button7 },
        { "LeftJSClick5", KeyCode.Joystick5Button8 },
        { "RightJSCLick5", KeyCode.Joystick5Button9 },

        { "A6", KeyCode.Joystick6Button0 },
        { "B6", KeyCode.Joystick6Button1 },
        { "X6", KeyCode.Joystick6Button2 },
        { "Y6", KeyCode.Joystick6Button3 },
        { "LB6", KeyCode.Joystick6Button4 },
        { "RB6", KeyCode.Joystick6Button5 },
        { "Back6", KeyCode.Joystick6Button6 },
        { "Start6", KeyCode.Joystick6Button7 },
        { "LeftJSClick6", KeyCode.Joystick6Button8 },
        { "RightJSCLick6", KeyCode.Joystick6Button9 },

        { "A7", KeyCode.Joystick7Button0 },
        { "B7", KeyCode.Joystick7Button1 },
        { "X7", KeyCode.Joystick7Button2 },
        { "Y7", KeyCode.Joystick7Button3 },
        { "LB7", KeyCode.Joystick7Button4 },
        { "RB7", KeyCode.Joystick7Button5 },
        { "Back7", KeyCode.Joystick7Button6 },
        { "Start7", KeyCode.Joystick7Button7 },
        { "LeftJSClick7", KeyCode.Joystick7Button8 },
        { "RightJSCLick7", KeyCode.Joystick7Button9 },

        { "A8", KeyCode.Joystick8Button0 },
        { "B8", KeyCode.Joystick8Button1 },
        { "X8", KeyCode.Joystick8Button2 },
        { "Y8", KeyCode.Joystick8Button3 },
        { "LB8", KeyCode.Joystick8Button4 },
        { "RB8", KeyCode.Joystick8Button5 },
        { "Back8", KeyCode.Joystick8Button6 },
        { "Start8", KeyCode.Joystick8Button7 },
        { "LeftJSClick8", KeyCode.Joystick8Button8 },
        { "RightJSCLick8", KeyCode.Joystick8Button9 }
    };

    public Dictionary<string, KeyCode> Map
    {
        get { return _map; }
        //set { _map = value; }
    }

    //public void ResetGameConfig()
    //{
    //    _map = null;
    //}
}
