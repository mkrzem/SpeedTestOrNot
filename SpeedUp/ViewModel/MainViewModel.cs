using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using SpeedUP.DAL;
using SpeedUP.DAL.Domain;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Timers;
using System.Windows.Input;

namespace SpeedUp.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        public ObservableCollection<Car> Cars { get; set; }
        public int CarCount { get; set; }

        private string timeElapsed;
        public string TimeElapsed
        {
            get { return timeElapsed; }
            set
            {
                timeElapsed = value;
                RaisePropertyChanged(nameof(TimeElapsed));
            }
        }
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            Cars = new ObservableCollection<Car>();
            if (IsInDesignMode)
            {
            }
            else
            {
                DataAccessManager.InitDataService("");                
            }
        }
        #region Commands
        public ICommand Read
        {
            get
            {
                return new RelayCommand(async () => 
                {
                    Cars.Clear();
                    Timer timer = new Timer();
                    Stopwatch watch = new Stopwatch();
                    watch.Start();
                    IList<Car> readCars = await DataAccessManager.ReadCarsAsync();
                    watch.Stop();
                    TimeElapsed = watch.Elapsed.ToString();
                    foreach (Car car in readCars)
                    {
                        Cars.Add(car);
                    }
                });
            }
        } 

        public ICommand Save
        {
            get
            {
                return new RelayCommand(async () => 
                {
                    await DataAccessManager.SaveCarsAsync(CarCount);
                });
            }
        }

        public ICommand ClearDatabase
        {
            get
            {
                return new RelayCommand(() => 
                {
                    DataAccessManager.ClearCars();
                });
            }
        }
        #endregion

    }
}