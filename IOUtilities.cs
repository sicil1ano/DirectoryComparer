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

        private List<DataItem> _differencesList;

        #endregion

        #region Properties

        /// <summary>
        /// Gets/Sets the MainViewModel instance.
        /// </summary>
        public MainViewModel MainViewModel { get; set; }

        /// <summary>
        /// Gets/Sets the list of differences found between the two directories.
        /// </summary>
        public List<DataItem> DifferencesList
        {
            get
            {
                return this._differencesList;
            }
        }

        /// <summary>
        /// Returns true if the directories are equal.
        /// </summary>
        public bool EqualDirectories
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

        #region Constructors

        /// <summary>
        /// Main Constructor.
        /// </summary>
        /// <param name="mainViewModel"></param>
        public IOUtilities(MainViewModel mainViewModel)
        {
            this.MainViewModel = mainViewModel;
        }

        #endregion

        #region Members

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

                    UpdateTempVariables(tempDirectoryItems);

                    if (this.MainViewModel.RecursiveScanEnabled)
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
        private void GetDifferencesList()
        {
            //the algorithm could be improved?
            ObservableCollection<DataItem> itemsFirstPath = this.MainViewModel.ViewModelFirstDatagrid.DirectoryItems;
            ObservableCollection<DataItem> itemsSecondPath = this.MainViewModel.ViewModelSecondDatagrid.DirectoryItems;

            if (itemsFirstPath != null && itemsSecondPath != null)
            {
                this._differencesList = itemsFirstPath.Where(x => !itemsSecondPath.Any(x1 => x1.ItemName == x.ItemName))
                     .Union(itemsSecondPath.Where(x => !itemsFirstPath.Any(x1 => x1.ItemName == x.ItemName))).ToList<CompareDirectories.DataItem>();


                this.EqualDirectories = DirectoriesAreEquals();
            }
        }

        /// <summary>
        /// Checks if the directories selected contain the same items.
        /// </summary>
        /// <returns></returns>
        private bool DirectoriesAreEquals()
        {
            bool sameNumberOfFiles = this.MainViewModel.ViewModelFirstDatagrid.FilesNumber == this.MainViewModel.ViewModelSecondDatagrid.FilesNumber;
            bool sameNumberofSubDirectories = this.MainViewModel.ViewModelFirstDatagrid.SubDirectoriesNumber == this.MainViewModel.ViewModelSecondDatagrid.SubDirectoriesNumber;

            if (sameNumberOfFiles && sameNumberofSubDirectories && DifferencesList.Count == 0)
            {
                return true;
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
            this._filesAndDirectoriesList = new ObservableCollection<DataItem>();
            this.filesFoundNumber = 0;
            this.subDirectoriesFoundNumber = 0;
        }

        /// <summary>
        /// Updates the temporary variables used in the recursive search algorithm.
        /// </summary>
        /// <param name="directoryItems">The temporary collection given by the search algorithm.</param>
        /// <param name="filesNumber">The temporary number of files found by the search algorithm.</param>
        /// <param name="subDirectoriesNumber">The temporary number of subdirectories found by the search algorithm.</param>
        private void UpdateTempVariables(ObservableCollection<DataItem> directoryItems, int filesNumber = 0, int subDirectoriesNumber = 0)
        {
            this._filesAndDirectoriesList.AddRange(directoryItems);
            this.filesFoundNumber += filesNumber;
            this.subDirectoriesFoundNumber += subDirectoriesNumber;
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

            return this._filesAndDirectoriesList;
        }

        /// <summary>
        /// Check the equality of the contents of the directories.
        /// </summary>
        public void CheckDirectoriesEquality()
        {
            this._differencesList = new List<DataItem>();
            GetDifferencesList();
        }

        #endregion
    }
}
