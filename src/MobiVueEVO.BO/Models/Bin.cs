using System;
using System.Collections.Generic;
using System.Text;

namespace MobiVueEVO.BO.Models
{
    public class Bin
    {
        public int Id { get; set; }
        public string PlantCode { get; set; }
        public string BinCode { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
    }
}
