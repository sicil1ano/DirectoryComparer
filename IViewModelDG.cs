using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace CompareDirectories
{
    /// <summary>
    /// Main ViewModelDG interface.
    /// </summary>
    public interface IViewModelDG
    {
        #region Properties

        /// <summary>
        /// Gets/Sets the number of files found.
        /// </summary>
        int FilesNumber { get; set; }

        /// <summary>
        /// Gets/Sets the number of subdirectories found.
        /// </summary>
        int DirectoriesNumber { get; set; }

        /// <summary>
        /// Gets/Sets the list of items existing in the directory selected.
        /// </summary>
        ObservableCollection<DataItem> DirectoryItems { get; set; }
        
        /// <summary>
        /// Gets/Sets the path of directory currently selected for the scan.
        /// </summary>
        string SelectedDirectory { get; set; }

        #endregion
    }
}
