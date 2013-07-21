﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CompareDirectories
{
    public static class IOUtilities
    {
        #region Fields

        /// <summary>
        /// List of files and directories found during the search.
        /// </summary>
        private static List<DataItem> _filesAndDirectoriesList = new List<DataItem>();

        #endregion

        #region Properties

        /// <summary>
        /// Gets/Sets the MainViewModel instance.
        /// </summary>
        public static MainViewModel MainViewModel { get; set; }

        public static List<DataItem> FilesAndDirectoriesList 
        {
            get
            {
                return _filesAndDirectoriesList;
            }
        }

        /// <summary>
        /// Gets/Sets the list of differences found between the two directories.
        /// </summary>
        public static List<DataItem> DifferencesList
        {
            get
            {
                return GetDifferencesList();
            }
        }

        /// <summary>
        /// Returns true if the directories are equal.
        /// </summary>
        public static bool EqualDirectories
        {
            get;

            private set;
        }

        /// <summary>
        /// The number of files found during the search.
        /// </summary>
        public static int FilesFoundNumber { get; set; }

        /// <summary>
        /// The number of subdirectories found during the search.
        /// </summary>
        public static int SubDirectoriesFoundNumber { get; set; }

        #endregion

        #region Members

        /// <summary>
        /// Gets the list of files and directories for the path selected.
        /// </summary>
        /// <param name="directoryPath">Path of directory used to get files and directories.</param>
        public static void SearchFilesAndDirectories(string directoryPath)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(directoryPath);

            var files = dirInfo.EnumerateFiles();
            FilesFoundNumber += files.Count();
            

            foreach (var file in files)
	        {
                if (file != null)
                {
                    _filesAndDirectoriesList.Add(new DataItem()
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
            SubDirectoriesFoundNumber += directories.Count();

            foreach (var directory in directories)
	        {
                if (directory != null)
                {
                    _filesAndDirectoriesList.Add(new DataItem()
                    {
                        IsSubFolder = true,
                        ItemName = directory.Name,
                        ItemPath = directory.FullName,
                        ItemFileExtension = null,
                        ItemCreatedDate = directory.CreationTimeUtc,
                        ItemModifiedDate = directory.LastWriteTimeUtc
                    });

                    //if (MainViewModel.RecursiveScan)
                    //{
                    //    SearchFilesAndDirectories(directory.FullName);
                    //}
                }
            }
        }

        /// <summary>
        /// Get the list of differences existing between the two directories selected.
        /// </summary>
        /// <returns></returns>
        private static List<DataItem> GetDifferencesList()
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
        private static bool DirectoriesAreEqual()
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
        public static void ClearParameters()
        {
            _filesAndDirectoriesList.Clear();
            FilesFoundNumber = 0;
            SubDirectoriesFoundNumber = 0;
        }

        #endregion
    }
}