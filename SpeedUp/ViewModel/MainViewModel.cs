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
using System.Windows.Controls;
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
        private string query;
        private bool isBusy;
        private string busyInfo;
        
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

        public string Query
        {
            get
            {
                return query;
            }
            set
            {
                query = value;
                RaisePropertyChanged(nameof(Query));
            }
        }

        public bool IsBusy
        {
            get
            {
                return isBusy;
            }
            set
            {
                isBusy = value;
                RaisePropertyChanged(nameof(IsBusy));
            }
        }

        public string BusyInfo
        {
            get { return busyInfo; }
            set
            {
                busyInfo = value;
                RaisePropertyChanged(nameof(BusyInfo));
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

                    IList<Car> readCars = new List<Car>();
                    Timer timer = new Timer();
                    Stopwatch watch = new Stopwatch();

                    Cars.Clear();
                    watch.Reset();
                    watch.Start();

                    try
                    {
                        BusyInfo = "Reading";
                        IsBusy = true;
                        readCars = await DataAccessManager.ReadCarsAsync(Query);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message, "Error");
                    }
                    finally
                    {
                        IsBusy = false;
                    }

                    watch.Stop();
                    ReadTimeElapsed = watch.ElapsedMilliseconds.ToString();

                    watch.Restart();
                    Cars = new ObservableCollection<Car>(readCars);
                    watch.Stop();
                    CopyTimeElapsed = watch.ElapsedMilliseconds.ToString();
                }, () => !IsBusy);
            }
        } 

        public ICommand Save
        {
            get
            {
                return new RelayCommand(async () => 
                {
                    try
                    {
                        BusyInfo = "Saving";
                        IsBusy = true;
                        SaveTimeElapsed = await DataAccessManager.SaveCarsAsync(CarCount);
                        MessageBox.Show(string.Format("Saved in: {0}!", SaveTimeElapsed), "Information");
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message, "Error");
                    }
                    finally
                    {
                        IsBusy = false;
                    }
                }, () => !IsBusy);
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
                }, () => !IsBusy);
            }
        }

        public ICommand DatabaseChanged
        {
            get
            {
                return new RelayCommand<object>((param) => 
                {
                    var args = param as SelectionChangedEventArgs;
                    if (args?.AddedItems.Count > 0)
                    {
                        DataAccessManager.ChangeDatabase(args?.AddedItems?[0].ToString());
                    }
                });
            }
        }
        #endregion

    }
}