using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ObjectExplorer
{
    public static class TUNING
    {
        public static class CONTROLS
        {
            public static float REFRESH_RATE = .1F;

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
                public static KeyCode debugEnable = KeyCode.BackQuote;
            }
        }
    }
}
