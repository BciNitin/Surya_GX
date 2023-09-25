using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MobiVUE.Utility
{
    public class BusinessListBase<T> : List<T> where T : BusinessBase
    {
        public event EventHandler<BusinessCollectionChangeEventArgs<T>> CollectionChanged;

        public bool IsDirty
        {
            get
            {
                foreach (T item in this)
                {
                    if (item.IsDirty)
                        return true;
                }
                return false;
            }
        }

        public bool IsValid(out List<ValidationResult> validationResults)
        {
            bool isValid = true;
            validationResults = new List<ValidationResult>();
            foreach (var businessObject in this)
            {
                List<ValidationResult> results = new List<ValidationResult>();
                isValid = isValid && businessObject.IsValid(out results);
                if (results.Count > 0)
                    validationResults.AddRange(results);
            }
            return isValid;
        }

        public void MarkAsOld()
        {
            foreach (T item in this)
            {
                item.MarkAsOld();
            }
        }

        public void Validate()
        {
            List<ValidationResult> validationResults = new List<ValidationResult>();
            IsValid(out validationResults);
            if (validationResults.Count > 0)
            {
                throw new MobiVUEException(String.Join("\n", validationResults));
            }
        }

        #region Remove Methods

        public new bool Remove(T item)
        {
            if (base.Remove(item))
            {
                CollectionChanged?.Invoke(this, new BusinessCollectionChangeEventArgs<T>() { Operation = OperationType.Removed, Objects = new List<T> { item } });
                return true;
            }
            else
            {
                return false;
            }
        }

        public new int RemoveAll(Predicate<T> match)
        {
            var itemsToRemove = this.FindAll(match);
            var removedItems = base.RemoveAll(match);
            if (removedItems > 0)
            {
                CollectionChanged?.Invoke(this, new BusinessCollectionChangeEventArgs<T>() { Operation = OperationType.Removed, Objects = itemsToRemove });
            }
            return removedItems;
        }

        public new void RemoveAt(int index)
        {
            var item = this[index];
            base.RemoveAt(index);
            CollectionChanged?.Invoke(this, new BusinessCollectionChangeEventArgs<T>() { Operation = OperationType.Removed, Objects = new List<T> { item } });
        }

        public new void RemoveRange(int index, int count)
        {
            var itemsToRemove = this.GetRange(index, count);
            base.RemoveRange(index, count);
            CollectionChanged?.Invoke(this, new BusinessCollectionChangeEventArgs<T>() { Operation = OperationType.Removed, Objects = itemsToRemove });
        }

        #endregion Remove Methods

        #region AddMethod

        public new void Add(T item)
        {
            base.Add(item);
            CollectionChanged?.Invoke(this, new BusinessCollectionChangeEventArgs<T>() { Operation = OperationType.Added, Objects = new List<T> { item } });
        }

        public new void AddRange(IEnumerable<T> collection)
        {
            base.AddRange(collection);
            CollectionChanged?.Invoke(this, new BusinessCollectionChangeEventArgs<T>() { Operation = OperationType.Added, Objects = collection.ToList() });
        }

        public new void Insert(int index, T item)
        {
            base.Insert(index, item);
            CollectionChanged?.Invoke(this, new BusinessCollectionChangeEventArgs<T>() { Operation = OperationType.Added, Objects = new List<T> { item } });
        }

        public new void InsertRange(int index, IEnumerable<T> collection)
        {
            base.InsertRange(index, collection);
            CollectionChanged?.Invoke(this, new BusinessCollectionChangeEventArgs<T>() { Operation = OperationType.Added, Objects = collection.ToList() });
        }

        #endregion AddMethod
    }
}