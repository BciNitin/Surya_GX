using ELog.Core.Authorization;
using System.ComponentModel.DataAnnotations.Schema;


namespace ELog.Core.Entities
{

    [Table("CageLabelPrinting")]
    public class CageLabelPrinting : PMMSFullAudit
    {
        //[ForeignKey("DispensingDetails")]
        public int DispensingId { get; set; }
        public string DispensingBarcode { get; set; }
        public int ProductID { get; set; }
        public string ProductCode { get; set; }
        //[ForeignKey("ProcessOrders")]
        public int ProcessorderID { get; set; }
        //[ForeignKey("CubicleMaster")]
        public int CubicleID { get; set; }
        public string CubcileCode { get; set; }
        public int? NoOfContainer { get; set; }
        public string CageLabelBarcode { get; set; }
        public int? PrintCount { get; set; }
        public int? PrinterID { get; set; }
        public bool IsActive { get; set; }




    }
}
