using System;
using System.Collections.Generic;

namespace MobiVUE.Utility
{
    public class BusinessCollectionChangeEventArgs<T> : EventArgs
    {
        public OperationType Operation { get; set; }

        public List<T> Objects { get; set; }
    }

    public enum OperationType
    {
        Added,
        Removed
    }
}