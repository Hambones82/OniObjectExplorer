using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ObjectExplorer
{
    //all this stuff needs to be imported from the scriptable object... otherwise, we need to update styling in two places...
    public static class TUNING
    {
        public static class CONTROLS
        {
            public static class NONEDITABLE
            {
                public static Color textColor = Color.grey;
            }

            public static class EDITABLE
            {
                public static Color textColor = new Color(0, 255, 0, 255);
            }
        }

        public static class INPUT
        {
            public static class KEYS
            {
                public static KeyCode debugEnable = KeyCode.Menu;
            }
        }
    }
}
