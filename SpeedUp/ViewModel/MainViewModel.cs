using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using SpeedUP.DAL;
using SpeedUP.DAL.Domain;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Timers;
using System.Windows;
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
        public ObservableCollection<Car> cars = new ObservableCollection<Car>();
        private string readtimeElapsed;
        private string saveTimeElapsed;
        private string copyTimeElapsed;
        public int CarCount { get; set; }        

        public ObservableCollection<Car> Cars
        {
            get
            {
                return cars;
            }
            set
            {
                cars = value;
                RaisePropertyChanged(nameof(Cars));
            }
        }

        public string ReadTimeElapsed
        {
            get { return readtimeElapsed; }
            set
            {
                readtimeElapsed = value;
                RaisePropertyChanged(nameof(ReadTimeElapsed));
            }
        }
                
        public string SaveTimeElapsed
        {
            get { return saveTimeElapsed; }
            set
            {
                saveTimeElapsed = value;
                RaisePropertyChanged(nameof(SaveTimeElapsed));
            }
        }

        public string CopyTimeElapsed
        {
            get { return copyTimeElapsed; }
            set
            {
                copyTimeElapsed = value;
                RaisePropertyChanged(nameof(CopyTimeElapsed));
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
                    watch.Reset();
                    watch.Start();
                    IList<Car> readCars = await DataAccessManager.ReadCarsAsync();
                    watch.Stop();
                    ReadTimeElapsed = watch.ElapsedMilliseconds.ToString();

                    watch.Restart();
                    Cars = new ObservableCollection<Car>(readCars);
                    watch.Stop();
                    CopyTimeElapsed = watch.ElapsedMilliseconds.ToString();
                    //foreach (Car car in readCars)
                    //{
                    //    Cars.Add(car);
                    //}
                });
            }
        } 

        public ICommand Save
        {
            get
            {
                return new RelayCommand(async () => 
                {
                    SaveTimeElapsed = await DataAccessManager.SaveCarsAsync(CarCount);
                });
            }
        }

        public ICommand ClearDatabase
        {
            get
            {
                return new RelayCommand(() => 
                {
                    if (MessageBox.Show("Are you sure?", "Clear Database", MessageBoxButton.YesNo,
                        MessageBoxImage.Exclamation, MessageBoxResult.No) == MessageBoxResult.Yes)
                    {
                        DataAccessManager.ClearCars();
                    }
                });
            }
        }
        #endregion

    }
}