using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Threading;


using Tizen.Sensor;
using Tizen;

namespace MyWatchCal
{
    

    public class SensorsCollector
    {
        public Accelerometer accelerometer;
        /*public ProximitySensor proximitySensor;*/
        private bool hasStartedApp = false;

        private string header = "X,Y,Z,Proximity,Timestamp";
        private List<string> timeSeriesData = new List<string>();

        public string appDataDirectory = @"/home/owner/data/archery-helper/";
        private string tsDataDirectory = @"/home/owner/data/archery-helper/time-series/";

        private long appStartTime;

        public SensorsCollector()
        {

            if (Accelerometer.IsSupported)
            {
                accelerometer = new Accelerometer
                {
                    Interval = 1000,
                    PausePolicy = SensorPausePolicy.None
                };
                accelerometer.DataUpdated += UpdateData;
            }

            /*if (ProximitySensor.IsSupported)
            {
                proximitySensor = new ProximitySensor();
                proximitySensor.DataUpdated += UpdateData;
            }*/

            if (Directory.Exists(appDataDirectory))
            {
                if (!Directory.Exists(tsDataDirectory))
                {
                    Directory.CreateDirectory(tsDataDirectory);
                }
            }
            else
            {
                Directory.CreateDirectory(appDataDirectory);
                Directory.CreateDirectory(tsDataDirectory);
            }

            timeSeriesData.Add(header);
        }

        public double Rounding(double number)
        {
            return Math.Round(number, 2);
        }

        public double CalculateAcceleration(float X, float Y, float Z)
        {
            double a = Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2) + Math.Pow(Z, 2));
            return a;
        }

        void UpdateData(object s, EventArgs e)
        {
            if (hasStartedApp)
            {
                timeSeriesData.Add(
                    accelerometer.X.ToString() + ',' +
                    accelerometer.Y.ToString() + ',' +
                    accelerometer.Z.ToString() + ',' +
                    /*proximitySensor.Proximity.ToString() + ',' +*/
                    DateTimeOffset.Now.ToUnixTimeSeconds().ToString()
                );
            }
        }

        private async Task WriteDataToDisk(string name)
        {
            string filePath = Path.Combine(tsDataDirectory, name);
            using StreamWriter file = new StreamWriter(filePath);

            foreach (string line in timeSeriesData)
            {
                await file.WriteLineAsync(line);
            }
        }

        public void PauseForAMoment(int miliseconds)
        {
            Thread.Sleep(miliseconds);
        }

        public async void Start()
        {
            if (!hasStartedApp)
            {
                accelerometer?.Start();
                /*proximitySensor?.Start();*/
                hasStartedApp = true;
                appStartTime = DateTimeOffset.Now.ToUnixTimeSeconds();
            }
        }

        public async void Stop()
        {
            if (hasStartedApp)
            {
                accelerometer?.Stop();
                /*proximitySensor?.Stop();*/
                hasStartedApp = false;

                long timeNow = DateTimeOffset.Now.ToUnixTimeSeconds();

                if (timeNow - appStartTime >= 1)
                {
                    string fileName = String.Format("sensor-timeseries-{0}.txt", timeNow);
                    await WriteDataToDisk(fileName);
                    appStartTime = timeNow;
                }

                timeSeriesData.Clear();
                timeSeriesData.Add(header);
            }
        }
    }
}
