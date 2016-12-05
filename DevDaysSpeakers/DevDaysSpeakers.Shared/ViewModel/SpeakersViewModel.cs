using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using DevDaysSpeakers.Model;
using DevDaysSpeakers.Services;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

namespace DevDaysSpeakers.ViewModel
{
    public class SpeakersViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        // プロパティ
        public ObservableCollection<Speaker> Speakers { get; set; }
        public Command GetSpeakersCommand { get; set; }
        public bool IsBusy
        {
            get { return busy; }
            set
            {
                busy = value;
                OnPropatyChanged();
                // Update the can execute
                GetSpeakersCommand.ChangeCanExecute();
            }
        }

        // コンストラクタ
        public SpeakersViewModel()
        {
            Speakers = new ObservableCollection<Speaker>();
            GetSpeakersCommand = new Command(
                               async () => await GetSpeakers(),
                               () => !IsBusy);
        }


        void OnPropatyChanged([CallerMemberName] String name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        bool busy;


        // メソッド
        async Task GetSpeakers()
        {
            if (IsBusy)
                return;

            Exception error = null;
            try
            {
                IsBusy = true;

                //using(var client = new HttpClient())
                //{
                //    // サーバーから json を取得します
                //    var json = await client.GetStringAsync("http://demo4404797.mockable.io/speakers");
                //    // json をデシリアライズします
                //    var items = JsonConvert.DeserializeObject<List<Speaker>>(json);
                //    // リストをSpeakersに読みこませます
                //    Speakers.Clear();
                //    foreach (var item in items)
                //        Speakers.Add(item);
                //}
                var service = DependencyService.Get<AzureService>();
                var items = await service.GetSpeakers();

                Speakers.Clear();
                foreach (var item in items)
                    Speakers.Add(item);
            }
            catch(Exception ex)
            {
                Debug.WriteLine("Error: " + ex);
                error = ex;
            }
            finally
            {
                IsBusy = false;
            }
            if (error != null)
                await Application.Current.MainPage.DisplayAlert("Error!", error.Message, "OK");

        }
        public async Task UpdateSpeaker(Speaker speaker)
        {
            var service = DependencyService.Get<AzureService>();
            await service.UpdateSpeaker(speaker);
            await GetSpeakers();
        }
    }
}
