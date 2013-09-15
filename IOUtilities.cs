using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections.ObjectModel;

namespace CompareDirectories
{
    public class IOUtilities
    {
        #region Fields

        /// <summary>
        /// ObservableCollection of files and directories found during the search.
        /// </summary>
        private ObservableCollection<DataItem> _filesAndDirectoriesList;

        #endregion

        #region Properties

        /// <summary>
        /// Gets/Sets the MainViewModel instance.
        /// </summary>
        public MainViewModel MainViewModel { get; set; }

        /// <summary>
        /// Gets the list of items of the given directory.
        /// </summary>
        public ObservableCollection<DataItem> FilesAndDirectoriesList 
        {
            get
            {
                return _filesAndDirectoriesList;
            }
        }

        /// <summary>
        /// Gets/Sets the list of differences found between the two directories.
        /// </summary>
        public List<DataItem> DifferencesList
        {
            get
            {
                return GetDifferencesList();
            }
        }

        /// <summary>
        /// Returns true if the directories are equal.
        /// </summary>
        public bool EqualsDirectories
        {
            get;

            private set;
        }

        /// <summary>
        /// The number of files found during the search.
        /// </summary>
        private int filesFoundNumber { get; set; }

        /// <summary>
        /// The number of subdirectories found during the search.
        /// </summary>
        private int subDirectoriesFoundNumber { get; set; }

        #endregion

        #region Members

        /// <summary>
        /// Main Constructor.
        /// </summary>
        /// <param name="mainViewModel"></param>
        public IOUtilities(MainViewModel mainViewModel)
        {
            MainViewModel = mainViewModel;
            _filesAndDirectoriesList = new ObservableCollection<DataItem>();
        }

        /// <summary>
        /// Gets the list of files and directories for the path selected.
        /// </summary>
        /// <param name="directoryPath">Path of directory used to get files and directories.</param>
        private void SearchFilesAndDirectories(string directoryPath)
        {
            ObservableCollection<DataItem> tempDirectoryItems = new ObservableCollection<DataItem>();
            int tempFilesNumber = 0;
            int tempSubDirNumber = 0;
            DirectoryInfo dirInfo = new DirectoryInfo(directoryPath);

            var files = dirInfo.EnumerateFiles();
            tempFilesNumber = files.Count();
            

            foreach (var file in files)
	        {
                if (file != null)
                {
                    tempDirectoryItems.Add(new DataItem()
                    {
                        IsSubFolder = false,
                        ItemName = file.Name,
                        ItemPath = file.FullName,
                        ItemFileExtension = file.Extension,
                        ItemCreatedDate = file.CreationTimeUtc,
                        ItemModifiedDate = file.LastWriteTimeUtc
                    });
                }
            }

            var directories = dirInfo.EnumerateDirectories();
            tempSubDirNumber = directories.Count();
            UpdateTempVariables(tempDirectoryItems, tempFilesNumber, tempSubDirNumber);

            foreach (var directory in directories)
	        {
                tempDirectoryItems.Clear();
                if (directory != null)
                {
                    tempDirectoryItems.Add(new DataItem()
                    {
                        IsSubFolder = true,
                        ItemName = directory.Name,
                        ItemPath = directory.FullName,
                        ItemFileExtension = null,
                        ItemCreatedDate = directory.CreationTimeUtc,
                        ItemModifiedDate = directory.LastWriteTimeUtc
                    });

                    UpdateTempVariables(tempDirectoryItems, 0, 0);

                    if (MainViewModel.RecursiveScan)
                    {
                        SearchFilesAndDirectories(directory.FullName);
                    }
                }
            }
        }

        /// <summary>
        /// Get the list of differences existing between the two directories selected.
        /// </summary>
        /// <returns></returns>
        private List<DataItem> GetDifferencesList()
        {
            ObservableCollection<DataItem> itemsFirstPath = MainViewModel.ViewModelFirstDatagrid.DirectoryItems;
            ObservableCollection<DataItem> itemsSecondPath = MainViewModel.ViewModelSecondDatagrid.DirectoryItems;

            if (itemsFirstPath != null && itemsSecondPath != null)
            {
                var differencesList = itemsFirstPath.Where(x => !itemsSecondPath.Any(x1 => x1.ItemName == x.ItemName))
                     .Union(itemsSecondPath.Where(x => !itemsFirstPath.Any(x1 => x1.ItemName == x.ItemName))).ToList<CompareDirectories.DataItem>();


                //DifferencesList = differencesList;
                EqualsDirectories = DirectoriesAreEquals();

                return differencesList;
            }
            else
            {
                return new List<DataItem>();
            }
        }

        /// <summary>
        /// Checks if the directories selected contain the same items.
        /// </summary>
        /// <returns></returns>
        private bool DirectoriesAreEquals()
        {
            int filesFirstPath = MainViewModel.ViewModelFirstDatagrid.FilesNumber;
            int filesSecondPath = MainViewModel.ViewModelSecondDatagrid.FilesNumber;

            int subDirectoriesFirstPath = MainViewModel.ViewModelFirstDatagrid.SubDirectoriesNumber;
            int subDirectoriesSecondPath = MainViewModel.ViewModelSecondDatagrid.SubDirectoriesNumber;

            bool sameNumberOfFiles = filesFirstPath == filesSecondPath;
            bool sameNumberofSubDirectories = subDirectoriesFirstPath == subDirectoriesSecondPath;

            if (sameNumberOfFiles && sameNumberofSubDirectories)
            {
                if (DifferencesList.Count == 0)
                {
                    return true;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Clear the parameters of the search.
        /// </summary>
        private void ClearParameters()
        {
            FilesAndDirectoriesList.Clear();
            filesFoundNumber = 0;
            subDirectoriesFoundNumber = 0;
        }

        /// <summary>
        /// Updates the temporary variables used in the recursive search algorithm.
        /// </summary>
        /// <param name="directoryItems">The temporary collection given by the search algorithm.</param>
        /// <param name="filesNumber">The temporary number of files found by the search algorithm.</param>
        /// <param name="subDirectoriesNumber">The temporary number of subdirectories found by the search algorithm.</param>
        private void UpdateTempVariables(ObservableCollection<DataItem> directoryItems, int filesNumber, int subDirectoriesNumber)
        {
            _filesAndDirectoriesList.AddRange(directoryItems);
            filesFoundNumber += filesNumber;
            subDirectoriesFoundNumber += subDirectoriesNumber;
        }

        /// <summary>
        /// Returns all the elements for the given directory.
        /// </summary>
        /// <param name="directoryPath">The path of the directory where the search will start.</param>
        /// <param name="filesNumber">The number of files found in the given directory.</param>
        /// <param name="subDirectoriesNumber">The number of subdirectories found in the given directory.</param>
        /// <returns></returns>
        public ObservableCollection<DataItem> GetDirectoryElements(string directoryPath, out int filesNumber, out int subDirectoriesNumber)
        {
            ClearParameters();
            if (!String.IsNullOrEmpty(directoryPath))
            {
                SearchFilesAndDirectories(directoryPath);
            }
            filesNumber = filesFoundNumber;
            subDirectoriesNumber = subDirectoriesFoundNumber;
            return FilesAndDirectoriesList;
        }

        #endregion
    }
}
