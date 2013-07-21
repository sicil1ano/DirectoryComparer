using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompareDirectories
{
    public class DataItem : IEquatable<DataItem>
    {
        #region Fields

        private string _itemName;
        private DateTime _itemCreatedDate;
        private DateTime _itemModifiedDate;
        private string _itemPath;

        #endregion

        #region Properties

        /// <summary>
        /// True if the directory selected is a sub folder.
        /// </summary>
        public bool IsSubFolder
        { get; set; }

        /// <summary>
        /// Gets/Sets the name of the file/directory selected.
        /// </summary>
        public string ItemName
        {
            get
            {
                return _itemName;
            }

            set
            {
                _itemName = value;
            }
        }

        /// <summary>
        /// Gets/Sets the path of the file/directory selected.
        /// </summary>
        public string ItemPath
        {
            get
            {
                return _itemPath;
            }
            set
            {
                _itemPath = value;
            }
        }

        /// <summary>
        /// Gets/Sets the creation date of the selected file/directory.
        /// </summary>
        public DateTime ItemCreatedDate
        {
            get
            {
                return _itemCreatedDate;
            }

            set
            {
                _itemCreatedDate = value;
            }
        }

        /// <summary>
        /// Gets/Sets the modification date of the selected file/directory.
        /// </summary>
        public DateTime ItemModifiedDate
        {
            get
            {
                return _itemModifiedDate;
            }

            set
            {
                _itemModifiedDate = value;
            }
        }

        /// <summary>
        /// Gets/Sets the file extension of the selected file.
        /// </summary>
        public string ItemFileExtension { get; set; }

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

            return ItemName.Equals(otherItem.ItemName) && ItemPath.Equals(otherItem.ItemPath) && ItemCreatedDate.Equals(otherItem.ItemCreatedDate) && ItemModifiedDate.Equals(otherItem.ItemModifiedDate);
        }

        /// <summary>
        /// Gets the hash code of the item.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            int hashInfoElementName = ItemName == null ? 0 : ItemName.GetHashCode();

            int hashInfoElementPath = ItemPath == null ? 0 : ItemPath.GetHashCode();

            int hashInfoElementCreatedDate = ItemCreatedDate == null ? 0 : ItemCreatedDate.GetHashCode();

            int hashInfoElementeModifiedDate = ItemModifiedDate == null ? 0 : ItemModifiedDate.GetHashCode();

            return hashInfoElementName ^ hashInfoElementPath ^ hashInfoElementCreatedDate ^ hashInfoElementeModifiedDate;
        }

        #endregion
    }
}
