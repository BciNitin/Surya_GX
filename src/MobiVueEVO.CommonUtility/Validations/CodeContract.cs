using System;
using System.Diagnostics;

namespace MobiVUE
{
    public class CodeContract
    {
        public static void Required<TException>(bool Predicate, string Message)
            where TException : Exception, new()
        {
            if (!Predicate)
            {
                Debug.WriteLine(Message);
                throw (TException)Activator.CreateInstance(typeof(TException), Message);
            }
        }
    }
}