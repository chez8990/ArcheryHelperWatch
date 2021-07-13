using System;
using Xamarin.Forms;
using Tizen.Wearable.CircularUI.Forms;
using Tizen.Sensor;
using Tizen.Security;
using System.Threading.Tasks;
using System.ServiceProcess;


namespace MyWatchCal
{
    class Program : global::Xamarin.Forms.Platform.Tizen.FormsApplication
    {
        protected override void OnCreate()
        {
            base.OnCreate();
            LoadApplication(new App());
        }

        static void Main(string[] args)
        {
            var app = new Program();
            Forms.Init(app);
            FormsCircularUI.Init();
            app.Run(args);
        }
    }
}
