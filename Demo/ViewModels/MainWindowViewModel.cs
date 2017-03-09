using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Demo.Models;
using Newtonsoft.Json;
using static System.Reflection.Assembly;

namespace Demo.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private List<LaborData> _laborData;

        public MainWindowViewModel()
        {
            ReloadDataCommand = new DelegateCommand(ReloadData);
            PopupJsonCommand = new DelegateCommand<LaborData>(PopupJson);
        }

        private void PopupJson(LaborData obj)
        {
            MessageBox.Show(JsonConvert.SerializeObject(obj, Formatting.Indented));
        }

        private void ReloadData()
        {
            LaborData = null;
            var resourceStream = GetExecutingAssembly().GetManifestResourceStream("Demo.Data.data.json");
            if (resourceStream == null) return;

            var sr = new StreamReader(resourceStream);
            var data = sr.ReadToEnd();
            LaborData = JsonConvert.DeserializeObject<List<LaborData>>(data);
        }

        public List<LaborData> LaborData
        {
            get { return _laborData; }
            set { SetProperty(ref _laborData, value); }
        }


        private DelegateCommand _reloadDataCommand;

        public DelegateCommand ReloadDataCommand
        {
            get { return _reloadDataCommand; }
            set { SetProperty(ref _reloadDataCommand, value); }
        }
        

        private DelegateCommand<LaborData> _popupJsonCommand;

        public DelegateCommand<LaborData> PopupJsonCommand
        {
            get { return _popupJsonCommand; }
            set { SetProperty(ref _popupJsonCommand, value); }
        }

    }
}
