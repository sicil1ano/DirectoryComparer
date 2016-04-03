using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace CompareDirectories
{
    public class FilterViewModel
    {
        #region Fields

        private List<Filter> filtersList;

        #endregion

        #region Properties

        /// <summary>
        /// Gets/Sets the filters list.
        /// </summary>
        public List<Filter> FiltersList
        {
            get 
            { 
                return this.filtersList; 
            }
        }
        

        #endregion

        #region Constructor

        public FilterViewModel()
        {
            LoadFilters();
        }

        #endregion

        #region Members

        /// <summary>
        /// Adds filters to the FiltersList property.
        /// </summary>
        private void LoadFilters()
        {
            if (this.filtersList == null)
            {
                this.filtersList = new List<Filter>();
                string[] array = new string[] { "*.*", "*.pdf", "*.txt", "*.config", "*.dll", "*.zip", "*.rar", "*.exe" };
                foreach (string filter in array)
                {
                    filtersList.Add(new Filter()
                    {
                        Name = filter
                    });
                }
            }
        }

        #endregion
    }

    public class Filter
    {
        private string filterName;

        /// <summary>
        /// Gets/Sets the name of the filter.
        /// </summary>
        public string Name
        {
            get
            {
                return this.filterName;
            }
            set
            {
                if (value != null)
                {
                    this.filterName = value;
                }
            }
        }
    }
}
