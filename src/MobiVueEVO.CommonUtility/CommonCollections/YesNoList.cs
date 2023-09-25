using System.Collections.Generic;

namespace MobiVUE.Utility.CommonCollections
{
    public class YesNoListCollection : List<KeyValuePair<int, string>>
    {
        public YesNoListCollection()
        {
            this.Add(new KeyValuePair<int, string>(0, "No"));
            this.Add(new KeyValuePair<int, string>(1, "Yes"));
            this.Add(new KeyValuePair<int, string>(-1, "All"));
        }
    }
}