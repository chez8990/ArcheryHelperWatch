using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tizen.Security;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyWatchCal
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // MainPage = new MyWatchCal.MainPage();
            // MainPage = new MyWatchCal.StepCountPage();
            MainPage = new NavigationPage(new MainPage());
#if DEBUG
            TizenHotReloader.HotReloader.Open(this);
#endif
        }

        protected override void OnStart()
        {
            // The app has to request the permission to access health data
            RequestPermissionHealthInfoAsync();
        }
        private async void RequestPermissionHealthInfoAsync()
        {
            var result = await RequestAsync("http://tizen.org/privilege/healthinfo");
            if (!result)
            {
                await MainPage.DisplayAlert("Alert", "This app cannot access your health information. So it will be terminated.", "OK");
                Application.Current.Quit();
            }
        }

        /// <summary>
        /// Asks the user to grant the permission at runtime
        /// </summary>
        /// <param name="privilege">The privilege name to check for</param>
        /// <returns>bool value that indicates whether the permission has been granted or not</returns>
        private async Task<bool> RequestAsync(string privilege)
        {
            // first make sure the user has given permission to use the privacy-related permissions in the app.
            switch (PrivacyPrivilegeManager.CheckPermission(privilege))
            {
                case CheckResult.Allow:
                    // already allowed
                    return true;
                case CheckResult.Deny:
                case CheckResult.Ask:
                    var tcs = new TaskCompletionSource<bool>();
                    var response = PrivacyPrivilegeManager.GetResponseContext(privilege);
                    PrivacyPrivilegeManager.ResponseContext context = null;

                    if (response.TryGetTarget(out context))
                    {
                        context.ResponseFetched += (s, e) =>
                        {
                            bool result = false;
                            if (e.result == RequestResult.AllowForever)
                            {
                                result = true;
                            }

                            tcs.SetResult(result);
                        };
                    }
                    // Ask the user to give the app permission to use the user's personal information
                    PrivacyPrivilegeManager.RequestPermission(privilege);

                    return await tcs.Task;
                default:
                    return false;
            }
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
