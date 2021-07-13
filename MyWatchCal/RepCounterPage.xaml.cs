using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using Tizen.Sensor;
using Tizen.Multimedia;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Platform.Tizen;


namespace MyWatchCal
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RepCounterPage : ContentPage
    {

        SensorsCollector sensorCollector = new SensorsCollector();
        Player player = new Player();
        
        private string header = "X,Y,Z,Timestamp";
        private List<string> timeSeriesData = new List<string>();

        Stopwatch stopWatch = new Stopwatch();

        bool repStarted = false;
        int holdSeconds = 10;
        int relaxSeconds = 5;
        int repsPerSession = 10;
        int sessionPerExercies = 5;

        private long appStartTime;
        private bool appStarted = false;

        public RepCounterPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();

            player.SetSource(new MediaUriSource("/shared/media/start.mp3"));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

        public async Task<int> StartSession()
        {
            int repsCounted = 0;
            while (repsCounted < repsPerSession)
            {
                for (int i = 0; i < holdSeconds; i++)
                {
                    await Task.Delay(500);
                }
                repsCounted += 1;
                Log.Tag = "XamrinApplication";
                Log.Warn(stopWatch.Elapsed.Seconds.ToString() + " seconds has passed");
                countLabel.Text = "Count: " + repsCounted.ToString();
            }

            return repsCounted;
        }

        private async void OnButtonClicked(object sender, EventArgs args)
        {
            /*await player.PrepareAsync();*/
            /*player.Start();*/

            executeButton.IsVisible = false;
            sensorCollector.Start();
            await StartSession();

            executeButton.IsVisible = true;
            sensorCollector.Stop();

        }
    }
}
