using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace CompareDirectories
{
    public class ViewModelDG1 : ObservableObject, IViewModelDG
    {
        #region Fields

        private int _filesNumber;
        private int _subDirectoriesNumber;
        private ObservableCollection<DataItem> _directoryItems;
        private string _selectedDirectory;

        #endregion

        #region Properties

        /// <summary>
        /// Gets/Sets the number of files found.
        /// </summary>
        public int FilesNumber
        {
            get
            {
                return this._filesNumber;
            }
            set
            {
                if (value != this._filesNumber)
                {
                    this._filesNumber = value;
                    RaisePropertyChanged("FilesNumber");
                }
            }
        }

        /// <summary>
        /// Gets/Sets the number of subdirectories found.
        /// </summary>
        public int SubDirectoriesNumber
        {
            get
            {
                return this._subDirectoriesNumber;
            }
            set
            {
                if (value != this._subDirectoriesNumber)
                {
                    this._subDirectoriesNumber = value;
                    RaisePropertyChanged("SubDirectoriesNumber");
                }
            }

        }

        /// <summary>
        /// Gets/Sets the path of the directory currently selected for the scan.
        /// </summary>
        public string SelectedDirectory 
        {
            get
            {
                return this._selectedDirectory;
            }
            set
            {
                if (value != this._selectedDirectory)
                {
                    this._selectedDirectory = value;
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
                return this._directoryItems;
            }
            set
            {
                if (value != null)
                {
                    this._directoryItems = value;
                    RaisePropertyChanged("DirectoryItems");
                }
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Main Constructor
        /// </summary>
        public ViewModelDG1()
        {
        }

        #endregion
    }
}
