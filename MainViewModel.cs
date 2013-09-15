using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace CompareDirectories
{
    public class MainViewModel : ObservableObject
    {
        #region Fields

        private bool _recursiveScan;
        private string _pathFirstDir;
        private string _pathSecondDir;
        //private delegate void Do();

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
        /// Gets/Sets the IOUtiliets instance.
        /// </summary>
        public IOUtilities IOUtilities { get; set; }

        //è meglio considerare un enum per i filtri? Occhio alla property GetFilter di main window..
        public string FilterChosen { get; set; }

        /// <summary>
        /// Gets/Sets the recursive scan check.
        /// </summary>
        public bool RecursiveScan
        {
            get
            {
                return _recursiveScan;
            }

            set
            {
                if (value != _recursiveScan)
                {
                    _recursiveScan = value;
                    RaisePropertyChanged("RecursiveScan");
                    GetFilesAndDirectoriesDatagrids();
                }
            }
        }
        /// <summary>
        /// Starts the search of the items for the given directories.
        /// </summary>
        private void GetFilesAndDirectoriesDatagrids()
        {
            if ((!String.IsNullOrEmpty(PathFirstDir)) && (!String.IsNullOrEmpty(PathSecondDir)))
            {
                GetFilesAndDirectories(ViewModelFirstDatagrid);
                GetFilesAndDirectories(ViewModelSecondDatagrid);
            }
            else if ((!String.IsNullOrEmpty(PathFirstDir)))
            {
                GetFilesAndDirectories(ViewModelFirstDatagrid);
            }

            else if ((!String.IsNullOrEmpty(PathSecondDir)))
            {
                GetFilesAndDirectories(ViewModelSecondDatagrid);
            }
        }

        /// <summary>
        /// Gets/Sets the path of the first directory to compare.
        /// </summary>
        public string PathFirstDir
        {
            get
            {
                return _pathFirstDir;
            }
            set
            {
                if (value != _pathFirstDir)
                {
                    _pathFirstDir = value;
                    RaisePropertyChanged("PathFirstDir");
                    ViewModelFirstDatagrid.DirectorySelected = value;
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
                return _pathSecondDir;
            }
            set
            {
                if (value != _pathSecondDir)
                {
                    _pathSecondDir = value;
                    RaisePropertyChanged("PathSecondDir");
                    ViewModelSecondDatagrid.DirectorySelected = value;
                }
            }
        }

        #endregion

        #region Members

        /// <summary>
        /// Main Constructor.
        /// </summary>
        public MainViewModel()
        {
            ViewModelFirstDatagrid = new ViewModelDG1(this);
            ViewModelSecondDatagrid = new ViewModelDG2(this);
            IOUtilities = new IOUtilities(this);
            PathFirstDir = String.Empty;
            PathSecondDir = String.Empty;
        }

        /// <summary>
        /// Gets the list of the files and directories existing in the selected directory of ViewModel instance.
        /// </summary>
        /// <param name="viewModel">ViewModel instance that invoked the update of its properties.</param>
        public void GetFilesAndDirectories(IViewModelDG viewModel)
        {
            int filesNumber = 0;
            int subDirectoriesNumber = 0;
            ObservableCollection<DataItem> directoryItems;

            directoryItems = IOUtilities.GetDirectoryElements(viewModel.DirectorySelected, out filesNumber, out subDirectoriesNumber);
            UpdateDatagrid(viewModel, directoryItems, filesNumber, subDirectoriesNumber);
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
        }

        #endregion
    }
}
