using ELog.Core.Authorization;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("MaterialTransferTypeMaster")]
    public class MaterialTransferTypeMaster : PMMSFullAudit
    {
        [StringLength(PMMSConsts.Medium)]
        public string TransferType { get; set; }

        [ForeignKey("MaterialTransferTypeId")]
        public ICollection<PutAwayBinToBinTransfer> PutAwayBinToBinTransfers { get; set; }
    }
}