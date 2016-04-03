using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Data;

namespace CompareDirectories
{
    public class MainViewModel : ObservableObject
    {
        #region Fields

        private bool isRecursiveScan;
        private string pathFirstDir;
        private string pathSecondDir;
        private bool equalDirectories;
        private Filter selectedFilter;
        

        #endregion

        #region Properties

        /// <summary>
        /// Gets/Sets the FilterViewModel instance.
        /// </summary>
        public FilterViewModel FilterViewModel { get; set; }

        /// <summary>
        /// Gets/Sets the ViewModelDG1 instance.
        /// </summary>
        public ViewModelDG1 ViewModelFirstDatagrid { get; set; }

        /// <summary>
        /// Gets/Sets the ViewModelDG2 instance.
        /// </summary>
        public ViewModelDG2 ViewModelSecondDatagrid { get; set; }

        /// <summary>
        /// Gets/Sets the IOUtilities instance.
        /// </summary>
        public IOUtilities IOUtilities { get; set; }

        //è meglio considerare un enum per i filtri? Occhio alla property GetFilter di main window..

        /// <summary>
        /// Gets the filters list.
        /// </summary>
        public List<Filter> FiltersList
        {
            get
            {
                return FilterViewModel.FiltersList;
            }
        }

        /// <summary>
        /// Gets the selected file filter.
        /// </summary>
        public Filter SelectedFilter
        {
            get
            {
                return this.selectedFilter;
            }
            set
            {
                if (value != this.selectedFilter)
                {
                    this.selectedFilter = value;
                    RaisePropertyChanged("SelectedFilter");
                }
            }
        }

        /// <summary>
        /// Gets/Sets the recursive scan check.
        /// </summary>
        public bool IsRecursiveScan
        {
            get
            {
                return this.isRecursiveScan;
            }

            set
            {
                if (value != this.isRecursiveScan)
                {
                    this.isRecursiveScan = value;
                    RaisePropertyChanged("IsRecursiveScan");
                    GetFilesAndDirectoriesDatagrids();
                }
            }
        }

        /// <summary>
        /// Gets/Sets the path of the first directory to compare.
        /// </summary>
        public string PathFirstDir
        {
            get
            {
                return this.pathFirstDir;
            }
            set
            {
                if (value != this.pathFirstDir)
                {
                    this.pathFirstDir = value;
                    RaisePropertyChanged("PathFirstDir");
                    this.ViewModelFirstDatagrid.SelectedDirectory = value;
                }
            }
        }

        /// <summary>
        /// Gets/Sets the path of the second directory to compare.
        /// </summary>
        public string PathSecondDir
        {
            get
            {
                return this.pathSecondDir;
            }
            set
            {
                if (value != this.pathSecondDir)
                {
                    this.pathSecondDir = value;
                    RaisePropertyChanged("PathSecondDir");
                    this.ViewModelSecondDatagrid.SelectedDirectory = value;
                }
            }
        }

        /// <summary>
        /// Gets/Sets the result of contents check of the directories.
        /// </summary>
        public bool EqualDirectories
        {
            get
            {
                return this.equalDirectories;
            }
            set
            {
                if (value != this.equalDirectories)
                {
                    this.equalDirectories = value;
                    RaisePropertyChanged("EqualDirectories");
                }
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Main Constructor.
        /// </summary>
        public MainViewModel()
        {
            this.ViewModelFirstDatagrid = new ViewModelDG1();
            this.ViewModelSecondDatagrid = new ViewModelDG2();
            this.IOUtilities = new IOUtilities(this);
            this.FilterViewModel = new FilterViewModel();
            this.SelectedFilter = FiltersList.First();
            this.PathFirstDir = String.Empty;
            this.PathSecondDir = String.Empty;
        }



        #endregion

        #region Members

        /// <summary>
        /// Gets the list of the files and directories existing in the selected directory of ViewModel instance.
        /// </summary>
        /// <param name="viewModel">The ViewModel instance that invoked the update of its properties.</param>
        public void GetFilesAndDirectories(IViewModelDG viewModel, bool isResursiveSearch)
        {
            int filesNumber = 0;
            int subDirectoriesNumber = 0;
            var context = TaskScheduler.FromCurrentSynchronizationContext();
            var task = new Task<ObservableCollection<DataItem>>(() =>
                {
                    var res = IOUtilities.GetDirectoryElements(viewModel.SelectedDirectory, isResursiveSearch, out filesNumber, out subDirectoriesNumber);
                    return res;
                });

            task.ContinueWith(t =>
                {
                    UpdateDatagrid(viewModel, t.Result, filesNumber, subDirectoriesNumber);
                },
                System.Threading.CancellationToken.None, TaskContinuationOptions.OnlyOnRanToCompletion, context);

            task.ContinueWith(t =>
                {
                    AggregateException aggregateException = t.Exception;
                    aggregateException.Handle(exception => true);
                    Console.WriteLine("The task GetFilesAndDirectories was canceled");
                },
                System.Threading.CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, context);

            task.Start();
        }

        /// <summary>
        /// Updates the properties related to the first datagrid, once the search algorithm has completed.
        /// </summary>
        /// <param name="viewModel">ViewModel instance that invoked the update of its properties.</param>
        /// <param name="directoryItems">Collection binded to the ItemsSource property of the datagrid.</param>
        /// <param name="filesNumber">Number of files found.</param>
        /// <param name="subDirectoriesNumber">Number of subdirectories found.</param>
        private void UpdateDatagrid(IViewModelDG viewModel, ObservableCollection<DataItem> directoryItems, int filesNumber, int subDirectoriesNumber)
        {
            if (viewModel.DirectoryItems != null && viewModel.DirectoryItems.Any())
            {
                viewModel.DirectoryItems.Clear();
            }

            viewModel.DirectoryItems = directoryItems;
            viewModel.FilesNumber = filesNumber;
            viewModel.DirectoriesNumber = subDirectoriesNumber;
            UpdateDirectoriesEquality();
        }

        /// <summary>
        /// Starts the search of the items for the given directories.
        /// </summary>
        private void GetFilesAndDirectoriesDatagrids()
        {
            if ((!String.IsNullOrEmpty(this.PathFirstDir)))
            {
                GetFilesAndDirectories(ViewModelFirstDatagrid, IsRecursiveScan);
            }

            if ((!String.IsNullOrEmpty(this.PathSecondDir)))
            {
                GetFilesAndDirectories(ViewModelSecondDatagrid, IsRecursiveScan);
            }
        }

        /// <summary>
        /// Updates the check for the equality of the contents of the directories.
        /// </summary>
        private void UpdateDirectoriesEquality()
        {
            if (!String.IsNullOrEmpty(this.PathFirstDir) && !String.IsNullOrEmpty(this.PathSecondDir))
            {
                this.IOUtilities.CheckDirectoriesEquality();
                //var items = this.IOUtilities.DifferencesList;
                this.EqualDirectories = this.IOUtilities.EqualDirectories;
            }
        }

        #endregion
    }
}
