using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompareDirectories
{
    public class DataItem : ObservableObject, IEquatable<DataItem>
    {
        #region Fields

        private string itemName;
        private bool isFolder;
        private DateTime itemCreatedDate;
        private DateTime itemModifiedDate;
        private string itemPath;
        private string itemFileExtension;

        #endregion

        #region Properties

        /// <summary>
        /// True if the directory selected is a sub folder.
        /// </summary>
        public bool IsFolder
        {
            get
            {
                return isFolder;
            }
            set
            {
                if (value != isFolder)
                {
                    isFolder = value;
                    RaisePropertyChanged("IsSubFolder");
                }
            }
        }

        /// <summary>
        /// Gets/Sets the name of the file/directory selected.
        /// </summary>
        public string Name
        {
            get
            {
                return itemName;
            }

            set
            {
                if (value != null)
                {
                    itemName = value;
                    RaisePropertyChanged("Name");
                }
            }
        }

        /// <summary>
        /// Gets/Sets the path of the file/directory selected.
        /// </summary>
        public string Path
        {
            get
            {
                return itemPath;
            }
            set
            {
                if (value != null)
                {
                    itemPath = value;
                    RaisePropertyChanged("Path");
                }
            }
        }

        /// <summary>
        /// Gets/Sets the creation date of the selected file/directory.
        /// </summary>
        public DateTime CreatedDate
        {
            get
            {
                return itemCreatedDate;
            }

            set
            {
                if (value != null)
                {
                    itemCreatedDate = value;
                    RaisePropertyChanged("CreatedDate");
                }
            }
        }

        /// <summary>
        /// Gets/Sets the modification date of the selected file/directory.
        /// </summary>
        public DateTime ModifiedDate
        {
            get
            {
                return itemModifiedDate;
            }

            set
            {
                if (value != null)
                {
                    itemModifiedDate = value;
                    RaisePropertyChanged("ModifiedDate");
                }
            }
        }

        /// <summary>
        /// Gets/Sets the file extension of the selected file.
        /// </summary>
        public string FileExtension 
        {
            get
            {
                return itemFileExtension;
            }
            set
            {
                if (value != null)
                {
                    itemFileExtension = value;
                    RaisePropertyChanged("FileExtension");
                }
            }
        
        }

        #endregion

        #region Members

        /// <summary>
        /// Checks if the item is equal to another item.
        /// </summary>
        /// <param name="otherItem">The other item to compare.</param>
        /// <returns></returns>
        public bool Equals(DataItem otherItem)
        {
            if (Object.ReferenceEquals(otherItem, null)) return false;

            if (Object.ReferenceEquals(this, otherItem)) return true;

            return Name.Equals(otherItem.Name) && Path.Equals(otherItem.Path) && CreatedDate.Equals(otherItem.CreatedDate) && ModifiedDate.Equals(otherItem.ModifiedDate);
        }

        /// <summary>
        /// Gets the hash code of the item.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            int hashInfoElementName = Name == null ? 0 : Name.GetHashCode();

            int hashInfoElementPath = Path == null ? 0 : Path.GetHashCode();

            int hashInfoElementCreatedDate = CreatedDate == null ? 0 : CreatedDate.GetHashCode();

            int hashInfoElementeModifiedDate = ModifiedDate == null ? 0 : ModifiedDate.GetHashCode();

            return hashInfoElementName ^ hashInfoElementPath ^ hashInfoElementCreatedDate ^ hashInfoElementeModifiedDate;
        }

        #endregion
    }
}
