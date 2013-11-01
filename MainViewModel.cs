using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace CompareDirectories
{
    public class MainViewModel : ObservableObject
    {
        #region Fields

        private bool _recursiveScanEnabled;
        private string _pathFirstDir;
        private string _pathSecondDir;
        private bool _equalDirectories;

        #endregion

        #region Properties

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
        /// Gets/Sets the Filter instance.
        /// </summary>
        public Filter Filter { get; set; }

        /// <summary>
        /// Gets/Sets the recursive scan check.
        /// </summary>
        public bool RecursiveScanEnabled
        {
            get
            {
                return this._recursiveScanEnabled;
            }

            set
            {
                if (value != this._recursiveScanEnabled)
                {
                    this._recursiveScanEnabled = value;
                    RaisePropertyChanged("RecursiveScanEnabled");
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
                return this._pathFirstDir;
            }
            set
            {
                if (value != this._pathFirstDir)
                {
                    this._pathFirstDir = value;
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
                return this._pathSecondDir;
            }
            set
            {
                if (value != this._pathSecondDir)
                {
                    this._pathSecondDir = value;
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
                return this._equalDirectories;
            }
            set
            {
                if (value != this._equalDirectories)
                {
                    this._equalDirectories = value;
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
            this.Filter = new Filter();
            this.PathFirstDir = String.Empty;
            this.PathSecondDir = String.Empty;
        }

        #endregion

        #region Members

        /// <summary>
        /// Gets the list of the files and directories existing in the selected directory of ViewModel instance.
        /// </summary>
        /// <param name="viewModel">The ViewModel instance that invoked the update of its properties.</param>
        public void GetFilesAndDirectories(IViewModelDG viewModel)
        {
            int filesNumber = 0;
            int subDirectoriesNumber = 0;
            ObservableCollection<DataItem> directoryItems;
            Task<ObservableCollection<DataItem>> tsk = new Task<ObservableCollection<DataItem>>(() => IOUtilities.GetDirectoryElements(viewModel.SelectedDirectory, out filesNumber, out subDirectoriesNumber));

            //directoryItems = IOUtilities.GetDirectoryElements(viewModel.SelectedDirectory, out filesNumber, out subDirectoriesNumber);
            tsk.Start();
            try
            {
                directoryItems = tsk.Result;
                UpdateDatagrid(viewModel, directoryItems, filesNumber, subDirectoriesNumber);
            }
            catch (AggregateException ex)
            {
                ex.Handle(e => e is OperationCanceledException);
                Console.WriteLine("The task GetFilesAndDirectories was canceled");
            }
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
            viewModel.DirectoryItems = directoryItems;
            viewModel.FilesNumber = filesNumber;
            viewModel.SubDirectoriesNumber = subDirectoriesNumber;
            UpdateDirectoriesEquality();
        }

        /// <summary>
        /// Starts the search of the items for the given directories.
        /// </summary>
        private void GetFilesAndDirectoriesDatagrids()
        {
            if ((!String.IsNullOrEmpty(this.PathFirstDir)) && (!String.IsNullOrEmpty(this.PathSecondDir)))
            {
                GetFilesAndDirectories(ViewModelFirstDatagrid);
                GetFilesAndDirectories(ViewModelSecondDatagrid);
            }
            else if ((!String.IsNullOrEmpty(this.PathFirstDir)))
            {
                GetFilesAndDirectories(ViewModelFirstDatagrid);
            }

            else if ((!String.IsNullOrEmpty(this.PathSecondDir)))
            {
                GetFilesAndDirectories(ViewModelSecondDatagrid);
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
                var items = this.IOUtilities.DifferencesList;
                this.EqualDirectories = this.IOUtilities.EqualDirectories;
            }
        }

        #endregion
    }
}
