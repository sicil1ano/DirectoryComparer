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
                    OnPropertyChanged("RecursiveScan");
                    GetFilesAndDirectoriesDatagrids();
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
            else if ((!String.IsNullOrEmpty(PathFirstDir)))
            {
                GetFilesAndDirectories(1, PathFirstDir);
            }

            else if ((!String.IsNullOrEmpty(PathSecondDir)))
            {
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
        public MainViewModel()
        {
            ViewModelFirstDatagrid = new ViewModelDG1(this);
            ViewModelSecondDatagrid = new ViewModelDG2(this);
            IOUtilities = new IOUtilities(this);
            PathFirstDir = String.Empty;
            PathSecondDir = String.Empty;
        }

        /// <summary>
        /// Gets the list of the files and directories existing in the selected directory.
        /// </summary>
        /// <param name="datagridNumber">The datagrid we want to populate.</param>
        /// <param name="directoryPath">The path of directory in which we want to search.</param>
        public void GetFilesAndDirectories(int datagridNumber, string directoryPath)
        {
            int filesNumber = 0;
            int subDirectoriesNumber = 0;
            if (datagridNumber == 1)
            {
                ViewModelFirstDatagrid.DirectoryItems = IOUtilities.GetDirectoryElements(directoryPath, out filesNumber, out subDirectoriesNumber);
                ViewModelFirstDatagrid.FilesNumber = filesNumber;
                ViewModelFirstDatagrid.SubDirectoriesNumber = subDirectoriesNumber;
            }
            else if (datagridNumber == 2)
            {
                ViewModelSecondDatagrid.DirectoryItems = IOUtilities.GetDirectoryElements(directoryPath, out filesNumber, out subDirectoriesNumber);
                ViewModelSecondDatagrid.FilesNumber = filesNumber;
                ViewModelSecondDatagrid.SubDirectoriesNumber = subDirectoriesNumber;
            }

            //((datagridNumber == 1) ? new Do(UpdateFirstDatagrid) : new Do(UpdateSecondDatagrid))();
        }

        private void UpdateFirstDatagrid()
        { 
        
        }

        private void UpdateSecondDatagrid()
        { 
        
        }

        #endregion
    }
}
