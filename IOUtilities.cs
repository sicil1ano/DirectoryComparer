using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CompareDirectories
{
    public class IOUtilities
    {
        #region Fields

        /// <summary>
        /// List of files and directories found during the search.
        /// </summary>
        private List<DataItem> _filesAndDirectoriesList;

        #endregion

        #region Properties

        /// <summary>
        /// Gets/Sets the MainViewModel instance.
        /// </summary>
        public MainViewModel MainViewModel { get; set; }

        public List<DataItem> FilesAndDirectoriesList 
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
        public bool EqualDirectories
        {
            get;

            private set;
        }

        /// <summary>
        /// The number of files found during the search.
        /// </summary>
        private int FilesFoundNumber { get; set; }

        /// <summary>
        /// The number of subdirectories found during the search.
        /// </summary>
        private int SubDirectoriesFoundNumber { get; set; }

        #endregion

        #region Members

        /// <summary>
        /// Main Constructor.
        /// </summary>
        /// <param name="mainViewModel"></param>
        public IOUtilities(MainViewModel mainViewModel)
        {
            MainViewModel = mainViewModel;
            _filesAndDirectoriesList = new List<DataItem>();
        }

        /// <summary>
        /// Gets the list of files and directories for the path selected.
        /// </summary>
        /// <param name="directoryPath">Path of directory used to get files and directories.</param>
        private void SearchFilesAndDirectories(string directoryPath)
        {
            List<DataItem> tempDirectoryItems = new List<DataItem>();
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
            List<DataItem> itemsFirstPath = MainViewModel.ViewModelFirstDatagrid.DirectoryItems;
            List<DataItem> itemsSecondPath = MainViewModel.ViewModelSecondDatagrid.DirectoryItems;

            if (itemsFirstPath != null && itemsSecondPath != null)
            {
                var differencesList = itemsFirstPath.Where(x => !itemsSecondPath.Any(x1 => x1.ItemName == x.ItemName))
                     .Union(itemsSecondPath.Where(x => !itemsFirstPath.Any(x1 => x1.ItemName == x.ItemName))).ToList<CompareDirectories.DataItem>();


                //DifferencesList = differencesList;
                EqualDirectories = DirectoriesAreEqual();

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
        private bool DirectoriesAreEqual()
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
        //public static void ClearParameters()
        //{
        //    _filesAndDirectoriesList.Clear();
        //    FilesFoundNumber = 0;
        //    SubDirectoriesFoundNumber = 0;
        //}

        private void UpdateTempVariables(List<DataItem> directoryItems, int filesNumber, int subDirectoriesNumber)
        {
            _filesAndDirectoriesList.AddRange(directoryItems);
            FilesFoundNumber += filesNumber;
            SubDirectoriesFoundNumber += subDirectoriesNumber;
        }

        public List<DataItem> GetDirectoryElements(string directoryPath, out int filesNumber, out int subDirectoriesNumber)
        {
            FilesAndDirectoriesList.Clear();
            FilesFoundNumber = 0;
            SubDirectoriesFoundNumber = 0;
            SearchFilesAndDirectories(directoryPath);
            filesNumber = FilesFoundNumber;
            subDirectoriesNumber = SubDirectoriesFoundNumber;
            return FilesAndDirectoriesList;
        }

        #endregion
    }
}
