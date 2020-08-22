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
            Debug.Log($"delta time: {Time.unscaledDeltaTime}");
            Debug.Log($"timer value is: {counter}");
            counter += Time.unscaledDeltaTime;
            while(counter > period)
            {
                Debug.Log("invoking callbacks for timer");
                counter -= period;
                timerCallback.Invoke();
            }
            
        }
    }
}
