using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ObjectExplorer
{
    public delegate void timerCallback();

    public class Timer : MonoBehaviour
    {
        public float period;
        private float counter = 0;

        public timerCallback timerCallback;

        public void Update()
        {
            counter += Time.unscaledDeltaTime;
            while(counter > period)
            {
                counter -= period;
                timerCallback.Invoke();
            }
            
        }
    }
}
