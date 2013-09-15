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
        private string _directorySelected;

        #endregion

        #region Properties

        /// <summary>
        /// Gets/Sets the number of files found.
        /// </summary>
        public int FilesNumber
        {
            get
            {
                return _filesNumber;
            }
            set
            {
                if (value != _filesNumber)
                {
                    _filesNumber = value;
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
                return _subDirectoriesNumber;
            }
            set
            {
                if (value != _subDirectoriesNumber)
                {
                    _subDirectoriesNumber = value;
                    RaisePropertyChanged("SubDirectoriesNumber");
                }
            }

        }

        /// <summary>
        /// Gets/Sets the path of the directory currently selected for the scan.
        /// </summary>
        public string DirectorySelected 
        {
            get
            {
                return _directorySelected;
            }
            set
            {
                if (value != _directorySelected)
                {
                    _directorySelected = value;
                    RaisePropertyChanged("DirectorySelected");
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
                return _directoryItems;
            }
            set
            {
                if (value != null)
                {
                    _directoryItems = value;
                    RaisePropertyChanged("DirectoryItems");
                }
            }
        }

        /// <summary>
        /// Gets/Sets the current instance of MainViewModel.
        /// </summary>
        public MainViewModel MainViewModel { get; set; }

        #endregion

        #region Members

        /// <summary>
        /// Main Constructor
        /// </summary>
        /// <param name="mainViewModel">Reference to the MainViewModel.</param>
        public ViewModelDG1(MainViewModel mainViewModel)
        {
            MainViewModel = mainViewModel;
        }

        #endregion
    }
}
