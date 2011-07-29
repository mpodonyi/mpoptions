using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MPOptions.ElementTree
{
    internal class CollectionAdapter<T> : IMPOptionCollection<T> where T:Element
    {
        internal CollectionAdapter(IDictionary<string,T> collection, string prekey)
        {
            InnerDict = collection;
            Prekey = prekey;
        }


        private IDictionary<string, T> InnerDict;

        private readonly string Prekey;

        protected virtual void InsertItem(T item)
        {
            item.SetReadOnly();
            InnerDict.Add(Prekey + item.Name, item);
        }

        

        #region Implementation of IEnumerable


        public IEnumerator<T> GetEnumerator()
        {
            return (from i in InnerDict
                   where i.Key.StartsWith(Prekey)
                   select i.Value).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region Implementation of ICollection<T>

        public void Add(T item)
        {
            InsertItem(item);
        }

        public void Clear()
        {
            (from i in InnerDict
             where i.Key.StartsWith(Prekey)
             select i.Key).Select((key) => InnerDict.Remove(key));
        }

        public bool Contains(T item)
        {
            return (from i in InnerDict
                    where i.Key.StartsWith(Prekey) && i.Value == item
                    select i).Any();
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            T[] items = (InnerDict.Where(i => i.Key.StartsWith(Prekey)).Select(i => i.Value)).ToArray();
            Array.Copy(items, 0, array, arrayIndex, items.Length);
        }

        public bool Remove(T item)
        {
            var keys = from i in InnerDict
                            where i.Key.StartsWith(Prekey) && i.Value == item
                            select i.Key;
            if(keys.Any())
            {
                foreach (string s in keys)
                {
                    InnerDict.Remove(s);
                }


                return true;
            }

            return false;
        }

        public int Count
        {
            get
            {
                return (from i in InnerDict
                        where i.Key.StartsWith(Prekey) 
                        select i).Count();
            }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        #endregion
    
        public bool Contains(string key)
        {
            return InnerDict.ContainsKey(Prekey+key);
        }

        public bool Remove(string key)
        {
            return InnerDict.Remove(Prekey + key);

        }

        public T this[string key]
        {
            get
            {
                return InnerDict[Prekey+key];
            }
        }


        #region Implementation of ICollection

        void ICollection.CopyTo(Array array, int index)
        {
            this.CopyTo((T[])array, index);
        }

        //int ICollection.Count
        //{
        //    get { return this.Count; }
        //}

        bool ICollection.IsSynchronized
        {
            get { return false; }
        }

        object ICollection.SyncRoot
        {
            get { return null; }
        }

        #endregion
    }

  

    
}
