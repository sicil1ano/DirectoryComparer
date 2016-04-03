using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace CompareDirectories
{
    public class ViewModelDG2 : ObservableObject, IViewModelDG
    {
        #region Fields
        
        private int filesNumber;
        private int directoriesNumber;
        private ObservableCollection<DataItem> directoryItems;
        private string selectedDirectory;

        #endregion

        #region Properties

        /// <summary>
        /// Gets/Sets the number of files found.
        /// </summary>
        public int FilesNumber 
        {
            get
            {
                return this.filesNumber;
            }
            set
            {
                if (value != this.filesNumber)
                {
                    this.filesNumber = value;
                    RaisePropertyChanged("FilesNumber");
                }
            }
        }
        
        /// <summary>
        /// Gets/Sets the number of subdirectories found.
        /// </summary>
        public int DirectoriesNumber
        {
            get
            {
                return this.directoriesNumber;
            }
            set
            {
                if (value != this.directoriesNumber)
                {
                    this.directoriesNumber = value;
                    RaisePropertyChanged("DirectoriesNumber");
                }
            }
        
        }

        /// <summary>
        /// Path of current directory selected for the scan.
        /// </summary>
        public string SelectedDirectory
        {
            get
            {
                return this.selectedDirectory;
            }
            set
            {
                if (value != this.selectedDirectory)
                {
                    this.selectedDirectory = value;
                    RaisePropertyChanged("SelectedDirectory");
                }
            }
        }

        /// <summary>
        /// Gets/Sets the list of items existing in the directory selected.
        /// </summary>
        public ObservableCollection<DataItem> DirectoryItems 
        {
            get
            {
                return this.directoryItems;
            }
            set
            {
                if (value != null)
                {
                    this.directoryItems = value;
                    RaisePropertyChanged("DirectoryItems");
                }
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Main Constructor.
        /// </summary>
        public ViewModelDG2()
        {
        }

        #endregion
    }
}
