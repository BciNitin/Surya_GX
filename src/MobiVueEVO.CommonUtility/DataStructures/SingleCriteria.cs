namespace MobiVUE.Utility
{
    public class SingleCriteria<T>
    {
        private T _value = default(T);

        public T Value
        {
            get
            {
                return _value;
            }
        }
    }
}