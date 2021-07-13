using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MyWatchCal
{
    class RepCounter
    {
        bool repStarted = false;
        int holdSeconds = 10;
        int relaxSeconds = 5;
        int repsPerSession = 10;
        int sessionPerExercies = 5;

        /*Timer timer = new Timer;*/

        public void StartSession()
        {
            int repsCounted = 0;
            while (repsCounted < repsPerSession)
            {
                for (int i = 0; i < holdSeconds; i++)
                {
                
                }
                repsCounted += 1;
            }
        }
    }
}
