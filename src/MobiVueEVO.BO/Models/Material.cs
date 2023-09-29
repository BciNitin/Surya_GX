using System;
using System.Collections.Generic;
using System.Text;

namespace MobiVueEVO.BO.Models
{
    public class Material
    {
       
            public string MaterialCode { get; set; }
            public string Description { get; set; }
            public int PackSize { get; set; }
            public string UOM { get; set; }
            public int UnitWeight { get; set; }
            public int SelfLife { get; set; }
            public bool Active { get; set; }

    }
}
