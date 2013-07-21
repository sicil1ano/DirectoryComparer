using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompareDirectories
{
    public class MainViewModel : ObservableObject
    {
        #region Fields

        private bool _recursiveScan;
        private string _pathFirstDir;
        private string _pathSecondDir;

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

        public MainWindow MainWindow { get; set; }

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
                    OnPropertyChanged("RecursiveScan");
                    if (RecursiveScan)
                    {
                        GetFilesAndDirectoriesDatagrids();
                    }
                }
            }
        }

        private void GetFilesAndDirectoriesDatagrids()
        {
            if ((!String.IsNullOrEmpty(PathFirstDir)) && (!String.IsNullOrEmpty(PathSecondDir)))
            {
                GetFilesAndDirectories(1, PathFirstDir);
                GetFilesAndDirectories(2, PathSecondDir);
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
                    OnPropertyChanged("PathFirstDir");
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
                    OnPropertyChanged("PathSecondDir");
                }
            }
        }

        #endregion

        #region Members

        /// <summary>
        /// Main Constructor.
        /// </summary>
        public MainViewModel(MainWindow mainWindow)
        {
            ViewModelFirstDatagrid = new ViewModelDG1(this);
            ViewModelSecondDatagrid = new ViewModelDG2(this);
            IOUtilities.MainViewModel = this;
            MainWindow = mainWindow;
        }

        /// <summary>
        /// Gets the list of the files and directories existing in the selected directory.
        /// </summary>
        /// <param name="datagridNumber">The datagrid we want to populate.</param>
        /// <param name="directoryPath">The path of directory in which we want to search.</param>
        public void GetFilesAndDirectories(int datagridNumber, string directoryPath)
        {
            if (datagridNumber == 1)
            {
                IOUtilities.ClearParameters();
                IOUtilities.SearchFilesAndDirectories(directoryPath);
                ViewModelFirstDatagrid.DirectoryItems = IOUtilities.FilesAndDirectoriesList;
                ViewModelFirstDatagrid.FilesNumber = IOUtilities.FilesFoundNumber;
                ViewModelFirstDatagrid.SubDirectoriesNumber = IOUtilities.SubDirectoriesFoundNumber;
                IOUtilities.ClearParameters();
            }
            else if (datagridNumber == 2)
            {
                IOUtilities.ClearParameters();
                IOUtilities.SearchFilesAndDirectories(directoryPath);
                ViewModelSecondDatagrid.DirectoryItems = IOUtilities.FilesAndDirectoriesList;
                ViewModelSecondDatagrid.FilesNumber = IOUtilities.FilesFoundNumber;
                ViewModelSecondDatagrid.SubDirectoriesNumber = IOUtilities.SubDirectoriesFoundNumber;
                //IOUtilities.ClearParameters();
            }
        }

        #endregion
    }
}
