using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace HidDisplayDnc.Mvvm
{
    public class SortedObservableList<T> : ICollection<T>, IList<T>, INotifyCollectionChanged, INotifyPropertyChanged
    {
        private const string CountString = "Count";

        // This must agree with Binding.IndexerName.  It is declared separately
        // here so as to avoid a dependency on PresentationFramework.dll.
        private const string IndexerName = "Item[]";

        private SimpleMonitor _monitor = new SimpleMonitor();

        private List<T> _items;
        private Func<T, T, int> _itemCompareFunction;
        private IComparer<T> _icomparer;

        /// <summary>
        /// Initializes a new instance of the <see cref="SortedObservableList{T}"/> class.
        /// </summary>
        /// <param name="itemCompareFunction">
        /// Function which compares two elements in the collection
        /// and returns -1 if the first comes before the second,
        /// 0 if they are equivalent,
        /// and 1 if the first comes after the seccond.
        /// </param>
        public SortedObservableList(Func<T, T, int> itemCompareFunction)
        {
            _items = new List<T>();
            _itemCompareFunction = itemCompareFunction;
            _icomparer = Comparer<T>.Create(new Comparison<T>(_itemCompareFunction));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SortedObservableList{T}"/> class.
        /// </summary>
        public SortedObservableList()
        {
            _items = new List<T>();
            _icomparer = Comparer<T>.Default;
            _itemCompareFunction = _icomparer.Compare;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SortedObservableList{T}"/> class.
        /// </summary>
        /// <param name="comparer">
        /// Comparer, returns -1 if the first comes before the second,
        /// 0 if they are equivalent,
        /// and 1 if the first comes after the seccond.
        /// </param>
        public SortedObservableList(IComparer<T> comparer)
        {
            _items = new List<T>();
            _icomparer = comparer;
            _itemCompareFunction = comparer.Compare;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SortedObservableList{T}"/>
        /// class that contains elements copied from the specified collection.
        /// </summary>
        /// <param name="collection">The collection from which the elements are copied.</param>
        /// <param name="itemCompareFunction">
        /// Function which compares two elements in the collection
        /// and returns -1 if the first comes before the second,
        /// 0 if they are equivalent,
        /// and 1 if the first comes after the seccond.
        /// </param>
        public SortedObservableList(IEnumerable<T> collection, Func<T, T, int> itemCompareFunction)
        {
            _items = new List<T>();
            CopyFrom(collection);
            _itemCompareFunction = itemCompareFunction;
            _icomparer = Comparer<T>.Create(new Comparison<T>(_itemCompareFunction));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SortedObservableList{T}"/>
        /// class that contains elements copied from the specified collection.
        /// </summary>
        /// <param name="collection">The collection from which the elements are copied.</param>
        /// <param name="comparer">
        /// Comparer, returns -1 if the first comes before the second,
        /// 0 if they are equivalent,
        /// and 1 if the first comes after the seccond.
        /// </param>
        public SortedObservableList(IEnumerable<T> collection, IComparer<T> comparer)
        {
            _items = new List<T>();
            CopyFrom(collection);
            _icomparer = comparer;
            _itemCompareFunction = comparer.Compare;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SortedObservableList{T}"/>
        /// class that contains elements copied from the specified list.
        /// </summary>
        /// <param name="list">The list from which the elements are copied.</param>
        /// <param name="itemCompareFunction">
        /// Function which compares two elements in the collection
        /// and returns -1 if the first comes before the second,
        /// 0 if they are equivalent,
        /// and 1 if the first comes after the seccond.
        /// </param>
        public SortedObservableList(List<T> list, Func<T, T, int> itemCompareFunction)
        {
            _items = new List<T>();
            CopyFrom(list);
            _itemCompareFunction = itemCompareFunction;
            _icomparer = Comparer<T>.Create(new Comparison<T>(_itemCompareFunction));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SortedObservableList{T}"/>
        /// class that contains elements copied from the specified list.
        /// </summary>
        /// <param name="list">The list from which the elements are copied.</param>
        /// <param name="comparer">
        /// Comparer, returns -1 if the first comes before the second,
        /// 0 if they are equivalent,
        /// and 1 if the first comes after the seccond.
        /// </param>
        public SortedObservableList(List<T> list, IComparer<T> comparer)
        {
            _items = new List<T>();
            CopyFrom(list);
            _icomparer = comparer;
            _itemCompareFunction = comparer.Compare;
        }

        /// <summary>
        /// Gets the item at the specified index.
        /// </summary>
        /// <param name="index">Index to get item from.</param>
        /// <returns>The item.</returns>
        public T this[int index]
        {
            get
            {
                return _items[index];
            }

            set
            {
                throw new NotSupportedException("Cannot specify index in sorted list. Used Add instead.");
            }
        }

        /// <summary>
        /// Number of items in the collection.
        /// </summary>
        public int Count
        {
            get
            {
                return _items.Count;
            }
        }

        /// <summary>
        /// Gets readonly (always false).
        /// </summary>
        public bool IsReadOnly => false;

        /// <summary>
        /// Occurs when an item is added, removed, changed, moved, or the entire list is
        /// refreshed.
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        protected virtual event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// PropertyChanged event (per <see cref="INotifyPropertyChanged" />).
        /// </summary>
        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add
            {
                PropertyChanged += value;
            }
            remove
            {
                PropertyChanged -= value;
            }
        }

        /// <summary>
        /// Adds an item at the correct index based on the sort method.
        /// </summary>
        /// <param name="item">Item to add.</param>
        public void Add(T item)
        {
            CheckReentrancy();

            int index = 0;
            int max = _items.Count;

            for (index = 0; index < max; index++)
            {
                if (_itemCompareFunction(item, _items[index]) == -1)
                {
                    _items.Insert(index, item);
                    max++;
                    return;
                }
            }

            _items.Insert(index, item);

            OnPropertyChanged(CountString);
            OnPropertyChanged(IndexerName);
            OnCollectionChanged(NotifyCollectionChangedAction.Add, item, index);
        }

        /// <summary>
        /// Adds a collection of items at the correct index based on the sort method.
        /// </summary>
        /// <param name="collection">Items to add.</param>
        public void AddRange(IEnumerable<T> collection)
        {
            if (object.ReferenceEquals(null, collection))
            {
                return;
            }

            if (!collection.Any())
            {
                return;
            }
            CheckReentrancy();

            var toAddList = new List<T>(collection);
            toAddList.Sort(_icomparer);

            int toAddIndex = 0;
            int toAddMax = toAddList.Count;
            int itemIndex = 0;
            int itemIndexMax = _items.Count;

            while (true)
            {
                var compare = _itemCompareFunction(toAddList[toAddIndex], _items[itemIndex]);
                if (compare == -1 || compare == 0)
                {
                    _items.Insert(itemIndex, toAddList[toAddIndex]);
                    toAddIndex++;
                    itemIndexMax++;
                }
                else
                {
                    itemIndex++;
                }

                if (itemIndex >= itemIndexMax
                    || toAddIndex >= toAddMax)
                {
                    break;
                }
            }

            while (toAddIndex < toAddMax)
            {
                _items.Add(toAddList[toAddIndex]);
                toAddIndex++;
            }

            OnPropertyChanged(CountString);
            OnPropertyChanged(IndexerName);
            OnCollectionChanged(NotifyCollectionChangedAction.Add, toAddList);
        }

        /// <summary>
        /// Removes all items from the collection.
        /// </summary>
        public void Clear()
        {
            CheckReentrancy();
            _items.Clear();
            OnPropertyChanged(CountString);
            OnPropertyChanged(IndexerName);
            OnCollectionReset();
        }

        /// <summary>
        /// Determines whether an element is in the collection.
        /// </summary>
        /// <param name="item">The object to locate in the collection. The value can 
        /// be null for reference types.</param>
        /// <returns>true if item is found in the collection; otherwise, false.</returns>
        public bool Contains(T item)
        {
            return _items.Contains(item);
        }

        /// <summary>
        /// Copies the entire collection to a compatible one-dimensional
        /// array, starting at the specified index of the target array.
        /// </summary>
        /// <param name="array">The one-dimensional System.Array that is the
        /// destination of the elements copied from. The System.Array must
        /// have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            _items.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>Enumerator.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the first
        /// occurrence within the entire collection.
        /// </summary>
        /// <param name="item">The object to locate in the collection. The value can
        /// be null for reference types.</param>
        /// <returns>The zero-based index of the first occurrence of item within the entire collection,
        /// if found; otherwise, -1.</returns>
        public int IndexOf(T item)
        {
            return _items.IndexOf(item);
        }

        /// <summary>
        /// NotSupported.
        /// </summary>
        /// <param name="index">NotSupported1.</param>
        /// <param name="item">NotSupported2.</param>
        public void Insert(int index, T item)
        {
            throw new NotSupportedException("Cannot specify index in sorted list. Used Add instead.");
        }

        /// <summary>
        /// Removes the first occurrence of a specific object.
        /// </summary>
        /// <param name="item">The object to remove from the collection. The value can
        /// be null for reference types.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns
        /// false if item was not found in the collection.</returns>
        public bool Remove(T item)
        {
            int index = IndexOf(item);
            if (index < 0)
            {
                return false;
            }

            CheckReentrancy();

            T removedItem = _items[index];

            _items.RemoveAt(index);

            OnPropertyChanged(CountString);
            OnPropertyChanged(IndexerName);
            OnCollectionChanged(NotifyCollectionChangedAction.Remove, removedItem, index);

            return true;
        }

        /// <summary>
        /// Removes the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to remove.</param>
        public void RemoveAt(int index)
        {
            if (index < 0)
            {
                return;
            }

            CheckReentrancy();

            T removedItem = _items[index];

            _items.RemoveAt(index);

            OnPropertyChanged(CountString);
            OnPropertyChanged(IndexerName);
            OnCollectionChanged(NotifyCollectionChangedAction.Remove, removedItem, index);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>Enumerator.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        /// <summary>
        /// Converts the current collection to a list.
        /// </summary>
        /// <returns>List of items.</returns>
        public List<T> ToNewList()
        {
            return new List<T>(_items);
        }

        /// <summary>
        /// Disallow reentrant attempts to change this collection. E.g. a event handler
        /// of the CollectionChanged event is not allowed to make changes to this collection.
        /// </summary>
        /// <remarks>
        /// typical usage is to wrap e.g. a OnCollectionChanged call with a using() scope:
        /// <code>
        ///         using (BlockReentrancy())
        ///         {
        ///             CollectionChanged(this, new NotifyCollectionChangedEventArgs(action, item, index));
        ///         }
        /// </code>
        /// </remarks>
        protected IDisposable BlockReentrancy()
        {
            _monitor.Enter();
            return _monitor;
        }

        /// <summary> Check and assert for reentrant attempts to change this collection. </summary>
        /// <exception cref="InvalidOperationException"> raised when changing the collection
        /// while another collection change is still being notified to other listeners </exception>
        protected void CheckReentrancy()
        {
            if (_monitor.Busy)
            {
                // we can allow changes if there's only one listener - the problem
                // only arises if reentrant changes make the original event args
                // invalid for later listeners.  This keeps existing code working
                // (e.g. Selector.SelectedItems).
                if ((CollectionChanged != null) && (CollectionChanged.GetInvocationList().Length > 1))
                    throw new InvalidOperationException("ObservableCollectionReentrancyNotAllowed");
            }
        }

        /// <summary>
        /// Raise CollectionChanged event to any listeners.
        /// Properties/methods modifying this ObservableCollection will raise
        /// a collection changed event through this virtual method.
        /// </summary>
        /// <remarks>
        /// When overriding this method, either call its base implementation
        /// or call <see cref="BlockReentrancy"/> to guard against reentrant collection changes.
        /// </remarks>
        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (CollectionChanged != null)
            {
                using (BlockReentrancy())
                {
                    CollectionChanged(this, e);
                }
            }
        }

        /// <summary>
        /// Raises a PropertyChanged event (per <see cref="INotifyPropertyChanged" />).
        /// </summary>
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        /// <summary>
        /// Helper to raise a PropertyChanged event  />).
        /// </summary>
        private void OnPropertyChanged(string propertyName)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Helper to raise CollectionChanged event to any listeners
        /// </summary>
        private void OnCollectionChanged(NotifyCollectionChangedAction action, object item, int index)
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, item, index));
        }

        /// <summary>
        /// Helper to raise CollectionChanged event to any listeners
        /// </summary>
        private void OnCollectionChanged(NotifyCollectionChangedAction action, IList newItems)
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, newItems));
        }

        /// <summary>
        /// Helper to raise CollectionChanged event with action == Reset to any listeners
        /// </summary>
        private void OnCollectionReset()
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        private void CopyFrom(IEnumerable<T> collection)
        {
            if (collection != null && _items != null)
            {
                using (IEnumerator<T> enumerator = collection.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        Add(enumerator.Current);
                    }
                }
            }
        }

        /// <summary>
        /// This class helps prevent reentrant calls.
        /// </summary>
        /// <remarks>
        /// https://referencesource.microsoft.com/#system/compmod/system/collections/objectmodel/observablecollection.cs,b11e7ea64c231ef2
        /// </remarks>
        private class SimpleMonitor : IDisposable
        {
            public void Enter()
            {
                ++_busyCount;
            }

            public void Dispose()
            {
                --_busyCount;
            }

            public bool Busy { get { return _busyCount > 0; } }

            int _busyCount;
        }
    }
}
