using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Utilities;

namespace NmtExplorer.Business
{
    public class DirectoryListEntryCollection : List<DirectoryListEntry>
    {
        private object m_syncRoot = new Object();
		
		public DirectoryListEntryCollection()
		{
		}

		public void Add(DirectoryListEntryCollection collection)
		{
			this.AddRange(collection);
		}
		
		public void AddInterlaced(DirectoryListEntryCollection collection)
        {
            int index = 1;
            foreach (DirectoryListEntry entity in collection)
            {
                if (this.Count > index)
                {
                    this.Insert(index, entity);
                }
                else
                {
                    this.Add(entity);
                }
                index = index + 2;
            }
        }
		
		public void Remove(DirectoryListEntryCollection collection)
		{
			foreach (DirectoryListEntry entity in collection)
			{
				if (Contains(entity))
				{
					Remove(entity);
				}
			}
		}
		
		public DirectoryListEntryCollection GetPage(int pageIndex, int pageSize)
		{
			return GetPage(this, pageIndex, pageSize);
		}
		
		public DirectoryListEntryCollection GetRandom(int count)
		{
			return GetRandom(this, count);
		}
		
		public void Shuffle()
		{
			DirectoryListEntryCollection randomizedCollection = GetRandom(this.Count);
			this.Clear();
            Add(randomizedCollection);
		}
		/*
		public void Intersect(DirectoryListEntryCollection collection)
		{
			DirectoryListEntryCollection intersection = GetIntersection(this, collection);
			this.Clear();
            Add(intersection);
		}
		*/
        
		public DirectoryListEntryCollection GetReversed()
		{
			return GetReversed(this);
		}
		
		
		public void Sort(DirectoryListEntryColumnName sortByColumn, ListSortDirection sortDirection)
        {
			Sort(sortByColumn.ToString(), sortDirection);
        }
		
		public void Sort(string sortByColumn, ListSortDirection sortDirection)
        {
			DirectoryListEntryCollection sortedCollection = GetSorted(this, sortByColumn, sortDirection);
            this.Clear();
			Add(sortedCollection);
        }
		
		public void Sort(List<KeyValuePair<string, ListSortDirection>> sortBy)
        {
            DirectoryListEntryCollection sortedCollection = GetSorted(this, sortBy);
            this.Clear();
            Add(sortedCollection);
        }
		
		public DirectoryListEntry FirstItem
		{
			get
			{
				if (this.Count > 0)
				{
					return this[0];
				}
				else return null;
			}
		}
		
		public DirectoryListEntryCollection Clone()
        {
			DirectoryListEntryCollection result = new DirectoryListEntryCollection();
			//ArrayList innerList = (ArrayList)this.InnerList.Clone();
			foreach(DirectoryListEntry entity in this)
			{
				result.Add(entity.Clone());
			}
            return result;
        }
				
		public bool Contains(DirectoryListEntryColumnName columnName, object value)
		{
			foreach(DirectoryListEntry entity in this)
			{
				if (entity[columnName.ToString()].Equals(value))
				{
					return true;
				}
			}
			return false;	
		}
		
		public int IndexOf(DirectoryListEntryColumnName columnName, object value)
		{
			foreach(DirectoryListEntry entity in this)
			{
				if (entity[columnName.ToString()].Equals(value))
				{
					return IndexOf(entity);
				}
			}
			return -1;	
		}
		
		public object SyncRoot
        {
            get { return m_syncRoot; }
        }
        /*
        public bool Contains(Int32 p_DirectoryListEntryID)
        {
            DirectoryListEntry entity = new DirectoryListEntry();
            entity.DirectoryListEntryID = p_DirectoryListEntryID;
            return this.Contains(entity);	
        }
		
        public int IndexOf(Int32 p_DirectoryListEntryID)
        {
            DirectoryListEntry entity = new DirectoryListEntry();
            entity.DirectoryListEntryID = p_DirectoryListEntryID;
            return this.IndexOf(entity);
        }
		
        public void Remove(Int32 p_DirectoryListEntryID)
        {
            DirectoryListEntry entity = new DirectoryListEntry();
            entity.DirectoryListEntryID = p_DirectoryListEntryID;
            this.Remove(entity);	
        }
         * */
        /*
		#region static methods
		public static DirectoryListEntryCollection GetIntersection(DirectoryListEntryCollection coll1, DirectoryListEntryCollection coll2)
		{
			DirectoryListEntryCollection result = new DirectoryListEntryCollection();
			foreach (DirectoryListEntry entity in coll1)
			{
				if (coll2.Contains(entity))
				{
					result.Add(entity);
				}
			}
			return result;
		}
		*/
		public static DirectoryListEntryCollection GetRandom(DirectoryListEntryCollection collection, int count)
		{
			DirectoryListEntryCollection result = new DirectoryListEntryCollection();
			DirectoryListEntryCollection temp = collection.Clone();
			if (count == 0 || count > temp.Count)
			{
				count = temp.Count;
			}
			for(int index = 0; index<count; index++)
            {
                int randomIndex = new Random().Next(temp.Count);
                result.Add(temp[randomIndex]);
                temp.RemoveAt(randomIndex);
            }
			return result;
		}
		/*
		public static DirectoryListEntryCollection GetFiltered(DirectoryListEntryCollection collection, DirectoryListEntryColumnName columnName, object value)
		{
			return GetFiltered(collection, columnName.ToString(), value);
		}
		
		public static DirectoryListEntryCollection GetFiltered(DirectoryListEntryCollection collection, string columnName, object value)
		{
			DirectoryListEntryCollection result = new DirectoryListEntryCollection();
			
			if (value is bool)
			{
                value = Convert.ToByte(value);
			}
			
			foreach (DirectoryListEntry entity in collection)
			{
				if (entity[columnName].Equals(value))
				{
					result.Add(entity);
				}
			}
			return result;
		}
		
		public static DirectoryListEntryCollection GetFiltered(DirectoryListEntryCollection collection, DirectoryListEntryColumnName columnName, object value, ComparisonOperator comparisonOperator)
		{
			return GetFiltered(collection, columnName.ToString(), value, comparisonOperator);
		}
		
		//Approximately 2.5 Times Slower Than Direct Approach (e.g. value >= entity.DisplayOrder), but still very fast and useful.
		public static DirectoryListEntryCollection GetFiltered(DirectoryListEntryCollection collection, string columnName, object value, ComparisonOperator comparisonOperator)
        {
            DirectoryListEntryCollection result = new DirectoryListEntryCollection();

            if (value is bool)
            {
                value = Convert.ToByte(value);
            }

            foreach (DirectoryListEntry entity in collection)
            {
				int comparisonResult = entity.CompareField(columnName, value);
				if (ComparisonUtils.DoesComparisonMatch(comparisonOperator, comparisonResult))
                {
                    result.Add(entity);
                }
            }
            return result;
        }
		
		public static DirectoryListEntryCollection GetBlocked(DirectoryListEntryCollection collection, DirectoryListEntryColumnName columnName, object value)
		{
			return GetBlocked(collection, columnName.ToString(), value);
		}
		
		public static DirectoryListEntryCollection GetBlocked(DirectoryListEntryCollection collection, string columnName, object value)
		{
			DirectoryListEntryCollection result = new DirectoryListEntryCollection();
			
			if (value is bool)
			{
                value = Convert.ToByte(value);
			}
			
			foreach (DirectoryListEntry entity in collection)
			{
				if (!entity[columnName].Equals(value))
				{
					result.Add(entity);
				}
			}
			return result;
		}
		
		public static int GetCount(DirectoryListEntryCollection collection, DirectoryListEntryColumnName columnName, object value)
		{
			int result = 0;
			
			if (value is bool)
			{
                value = Convert.ToByte(value);
			}
			
			foreach (DirectoryListEntry entity in collection)
			{
				if (entity[columnName.ToString()].Equals(value))
				{
					result++;
				}
			}
			return result;
		}
		*/
		public static DirectoryListEntryCollection GetPage(DirectoryListEntryCollection collection, int pageIndex, int pageSize)
		{
			DirectoryListEntryCollection page = new DirectoryListEntryCollection();
			
			int firstItemIndex = PagingUtils.GetFirstItemIndex(pageIndex, pageSize);
            int lastItemIndex = PagingUtils.GetLastItemIndex(pageIndex, pageSize);
            if (lastItemIndex >= collection.Count)
            {
                lastItemIndex = collection.Count - 1;
            }

            for (int index = firstItemIndex; index < lastItemIndex + 1; index++)
            { 
				page.Add((DirectoryListEntry)collection[index]);
            }
			
			return page;
		}
		
		public static DirectoryListEntryCollection GetSorted(DirectoryListEntryCollection collection, DirectoryListEntryColumnName sortByColumn, ListSortDirection sortDirection)
		{
			return GetSorted(collection, sortByColumn.ToString(), sortDirection);
		}
		
		public static DirectoryListEntryCollection GetSorted(DirectoryListEntryCollection collection, string sortByColumn, ListSortDirection sortDirection)
		{
			bool isReversed = (sortDirection == ListSortDirection.Descending);
            IComparer comparer = new KeyValueCaseInsensitiveComparer(isReversed);
            
            SortedList sortedList = new SortedList(comparer);
			int counter = 0;
            foreach (DirectoryListEntry entity in collection)
            {
                KeyValue key = new KeyValue(counter.ToString(), entity[sortByColumn]);
                sortedList.Add(key, entity);   
				counter++;
            }

            DirectoryListEntryCollection result = new DirectoryListEntryCollection();
			foreach (DirectoryListEntry entity in sortedList.Values)
            {
                   result.Add(entity);
            }
			return result;
		}
		
		public static DirectoryListEntryCollection GetSorted(DirectoryListEntryCollection collection, List<KeyValuePair<string, ListSortDirection>> sortBy)
        {
			// necessary!
			collection = collection.Clone();
			
            for(int index = sortBy.Count - 1; index >-1; index--)
            {
                string columnName = sortBy[index].Key;
                ListSortDirection direction = sortBy[index].Value;
                collection.Sort(columnName, direction);
            }
            return collection;
        }
		/*
		public static DirectoryListEntryCollection GetUnique(DirectoryListEntryCollection collection)
		{
			DirectoryListEntryCollection result = new DirectoryListEntryCollection();
			foreach(DirectoryListEntry entity in collection)
			{
				if(!result.Contains(entity))
				{
					result.Add(entity);
				}
			}
			return result;
		}
		*/
		public static DirectoryListEntryCollection GetReversed(DirectoryListEntryCollection collection)
		{
			DirectoryListEntryCollection result = new DirectoryListEntryCollection();
			for (int index = collection.Count - 1;index > - 1 ; index--)
			{
					result.Add(collection[index]);
			}
			return result;
		}
		/*
		public static DirectoryListEntryCollection GetUnique(DirectoryListEntryCollection collection, DirectoryListEntryColumnName columnName)
		{
			DirectoryListEntryCollection result = new DirectoryListEntryCollection();
			foreach(DirectoryListEntry entity in collection)
			{
				if(!result.Contains(columnName, entity[columnName.ToString()]))
				{
					result.Add(entity);
				}
			}
			return result;
		}
		#endregion */
    }
}
