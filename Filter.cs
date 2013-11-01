using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace CompareDirectories
{
    public class Filter : ObservableObject
    {
        #region Fields

        private ObservableCollection<Filter> _filtersList;
        private Filter _selectedFilter;
        private string _name;

        #endregion

        #region Properties

        /// <summary>
        /// Gets/Sets the currently selected filter.
        /// </summary>
        public Filter SelectedFilter
        {
            get
            {
                return this._selectedFilter;
            }
            set
            {
                if (value != null)
                {
                    this._selectedFilter = value;
                }
            }
        }

        /// <summary>
        /// Gets/Sets the name of the filter.
        /// </summary>
        public string Name
        {
            get 
            {
                return this._name;
            }
            set
            {
                if (value != null)
                {
                    this._name = value;
                }
            }
        }

        /// <summary>
        /// Gets/Sets the filters list.
        /// </summary>
        public ObservableCollection<Filter> FiltersList
        {
            get 
            { 
                return this._filtersList; 
            }
            set 
            {
                if (value != null)
                {
                    this._filtersList = value;
                }
            }
        }
        

        #endregion

        #region Constructors

        /// <summary>
        /// Main constructor.
        /// </summary>
        public Filter()
        {
            this.FiltersList = new ObservableCollection<Filter>();
            //LoadFilters();
        }

        #endregion

        #region Members

        /// <summary>
        /// Adds filters to the FiltersList property.
        /// </summary>
        private void LoadFilters()
        {
            string[] array = new string[] { "*.*", "*.pdf", "*.txt", "*.config", "*.dll", "*.zip", "*.rar", "*.exe" };
            foreach (string filter in array)
            {
                FiltersList.Add(new Filter()
                {
                    Name = filter
                });
            }
        }

        #endregion
    }
}
