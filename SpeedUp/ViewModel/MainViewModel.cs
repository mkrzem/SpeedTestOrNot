using GalaSoft.MvvmLight;
using SpeedUP.DAL;
using SpeedUP.DAL.Domain;
using System.Collections.ObjectModel;

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
                DataAccessManager.GetDataService("");
                foreach (Car car in DataAccessManager.ReadCars())
                {
                    Cars.Add(car);
                }
            }
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}

        }
    }
}