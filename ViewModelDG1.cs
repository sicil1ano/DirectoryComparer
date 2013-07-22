using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompareDirectories
{
    public class ViewModelDG1 : ObservableObject
    {
        #region Fields

        private int _filesNumber;
        private int _subDirectoriesNumber;
        private List<DataItem> _directoryItems;

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
                    OnPropertyChanged("FilesNumber");
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
                    OnPropertyChanged("SubDirectoriesNumber");
                }
            }

        }

        /// <summary>
        /// Gets/Sets the list of items existing in the directory selected.
        /// </summary>
        public List<DataItem> DirectoryItems 
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
                    OnPropertyChanged("DirectoryItems");
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
