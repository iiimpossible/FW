using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



namespace GraphyFW
{
    namespace Common
    {
        public class Timer
        {
            public int counter;
            public int delta;
            public string log;
            public Timer(string log = "", int c = 0, int d = 0)
            {
                this.counter = c;
                this.delta = d;
                this.log = log;
            }
        }

        public class TimerHandle
        {
            public int handle { get; private set; }
            public TimerHandle(int h)
            {
                handle = h;
            }
        }


        /// <summary>
        /// µ÷ÊÔ¼ÆÊ±
        /// </summary>
        public class DebugTime
        {
            public static int time { get; private set; }
            public static int delta { get; private set; }

            private static List<Timer> handles = new List<Timer>();

            private static int index = 0;

            public static void StartTimer()
            {
                time = System.Environment.TickCount;
                Debug.Log("[DebugTime]---------->" + time);
            }

            public static void EndTimer()
            {
                delta = System.Environment.TickCount - time;
                Debug.Log("[DebugTime]delta---------->" + delta * 0.001 + "second");
            }


            /// <summary>
            /// ????????????
            /// </summary>
            /// <param name="timer"></param>
            public static void StartTimer(Timer timer)
            {
                timer.counter = System.Environment.TickCount;                
            }

            /// <summary>
            /// ????????????
            /// </summary>
            /// <param name="timer"></param>
            public static void EndTimer(Timer timer)
            {
                timer.delta = System.Environment.TickCount - timer.counter;                     
                Debug.Log($"[DebugTime]{timer.log}---------->" + timer.delta + "ms");
            }


            public static TimerHandle CreateTimer(string log)
            {
                TimerHandle h = new TimerHandle(index);
                Timer t = new Timer(log);
                handles.Add(t);
                index++;
                return h;
            }

           public  Timer this[TimerHandle i]
            { get
                {
                    Timer t = handles[i.handle];
                    t.counter = System.Environment.TickCount;
                    return t;
                }
              private set { }
            }
        }
    }


}
