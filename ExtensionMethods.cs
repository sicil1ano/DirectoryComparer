using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace CompareDirectories
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Adds the items of another collection to an observable collection.
        /// </summary>
        /// <typeparam name="T">Generic type.</typeparam>
        /// <param name="observableCollection">The observable collection to which we want to add other items.</param>
        /// <param name="rangeList">The collection of items we want to add to the observable collection.</param>
        public static void AddRange<T>(this ObservableCollection<T> observableCollection, IEnumerable<T> rangeList)
        {
            foreach (T item in rangeList)
            {
                observableCollection.Add(item);
            }
        }
    }
}
