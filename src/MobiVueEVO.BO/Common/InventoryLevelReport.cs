using MobiVUE.Utility;
using System;
using System.Runtime.Serialization;

namespace MobiVueEVO.BO
{
    [DataContract]
    public class InventoryLevelReport : BusinessBase
    {
        private long _inventoryId;

        [DataMember]
        public long InventoryId
        {
            get { return _inventoryId; }
            set { SetValue<long>(ref _inventoryId, value, () => this.InventoryId); }
        }

        private KeyInfo<int, string, string> _material = new KeyInfo<int, string, string>(0, "", "");

        [DataMember]
        public KeyInfo<int, string, string> Material
        {
            get { return _material; }
            set { SetValue<KeyInfo<int, string, string>>(ref _material, value, () => this.Material); }
        }

        private KeyValue<long, string> _location = new KeyValue<long, string>(0, "");

        [DataMember]
        public KeyValue<long, string> Location
        {
            get { return _location; }
            set { SetValue<KeyValue<long, string>>(ref _location, value, () => this.Location); }
        }

        private string _barcode;

        [DataMember]
        public string Barcode
        {
            get { return _barcode; }
            set { SetValue<string>(ref _barcode, value, () => this.Barcode); }
        }

        private decimal _quantity = 1;

        [DataMember]
        public decimal Quantity
        {
            get { return _quantity; }
            set { SetValue<decimal>(ref _quantity, value, () => this.Quantity); }
        }

        private string _batch = "";

        [DataMember]
        public string Batch
        {
            get { return _batch; }
            set { SetValue<string>(ref _batch, value, () => this.Batch); }
        }

        private KeyValue<long, string> _pallet;

        [DataMember]
        public KeyValue<long, string> Pallet
        {
            get { return _pallet; }
            set { SetValue<KeyValue<long, string>>(ref _pallet, value, () => this.Pallet); }
        }

        private KeyValue<long, string> _grnOrderId = new KeyValue<long, string>(0, "");

        [DataMember]
        public KeyValue<long, string> GRNOrder
        {
            get { return _grnOrderId; }
            set { SetValue<KeyValue<long, string>>(ref _grnOrderId, value, () => this.GRNOrder); }
        }

        private long _grnOrderItemId = 0;

        [DataMember]
        public long GRNOrderItemId
        {
            get { return _grnOrderItemId; }
            set { SetValue<long>(ref _grnOrderItemId, value, () => this.GRNOrderItemId); }
        }

        private SmartDateTime _createdDate;

        [DataMember]
        public SmartDateTime CreatedDate
        {
            get { return _createdDate; }
            set { SetValue<SmartDateTime>(ref _createdDate, value, () => this.CreatedDate); }
        }

        private KeyValue<long, string> _createdBy;

        [DataMember]
        public KeyValue<long, string> CreatedBy
        {
            get { return _createdBy; }
            set { SetValue<KeyValue<long, string>>(ref _createdBy, value, () => this.CreatedBy); }
        }

        private SmartDateTime _updatedDate;

        [DataMember]
        public SmartDateTime UpdatedDate
        {
            get { return _updatedDate; }
            set { SetValue<SmartDateTime>(ref _updatedDate, value, () => this.UpdatedDate); }
        }

        private SmartDateTime _manufacturingDate = DateTime.Now;

        [DataMember]
        public SmartDateTime ManufacturingDate
        {
            get { return _manufacturingDate; }
            set { SetValue<SmartDateTime>(ref _manufacturingDate, value, () => this.ManufacturingDate); }
        }

        [DataMember]
        public string MFGDate
        { get { return ManufacturingDate.DateTime.ToString("dd-MM-yyyy"); } }

        private KeyValue<long, string> _updatedBy;

        [DataMember]
        public KeyValue<long, string> UpdatedBy
        {
            get { return _updatedBy; }
            set { SetValue<KeyValue<long, string>>(ref _updatedBy, value, () => this.UpdatedBy); }
        }

        private KeyValue<int, string> _site;

        [DataMember]
        public KeyValue<int, string> Site
        {
            get { return _site; }
            set { SetValue<KeyValue<int, string>>(ref _site, value, () => this.Site); }
        }

        private InventoryStatus _status = InventoryStatus.None;

        [DataMember]
        public InventoryStatus Status
        {
            get { return _status; }
            set { SetValue<InventoryStatus>(ref _status, value, () => this.Status); }
        }

        private SmartDateTime _GRNOn;

        [DataMember]
        public SmartDateTime GRNOn
        {
            get { return _GRNOn; }
            set { SetValue<SmartDateTime>(ref _GRNOn, value, () => this.GRNOn); }
        }

        private SmartDateTime _palletizationOn;

        [DataMember]
        public SmartDateTime PalletizationOn
        {
            get { return _palletizationOn; }
            set { SetValue<SmartDateTime>(ref _palletizationOn, value, () => this.PalletizationOn); }
        }

        private SmartDateTime _putawayOn;

        [DataMember]
        public SmartDateTime PutawayOn
        {
            get { return _putawayOn; }
            set { SetValue<SmartDateTime>(ref _putawayOn, value, () => this.PutawayOn); }
        }

        [DataMember]
        public string Date
        {
            get
            {
                if ((int)Status == 1) return GRNOn.DateTime.ToString("dd-MM-yyyy");
                else if ((int)Status == 3) return PalletizationOn.DateTime.ToString("dd-MM-yyyy");
                else if ((int)Status == 4) return PutawayOn.DateTime.ToString("dd-MM-yyyy");
                else return "";
            }
        }
    }

    [DataContract]
    public class InventoryLevelReportSearchCriteria
    {

        [DataMember]
        public DateTime FromDate { get; set; }

        [DataMember]
        public DateTime ToDate { get; set; }

        [DataMember]
        public string documentNo { get; set; } = "";

        [DataMember]
        public string Barcode { get; set; } = "";

        [DataMember]
        public string LocationCode { get; set; } = "";

        [DataMember]
        public int SiteId { get; set; } = 0;

        [DataMember]
        public int MaterialId { get; set; } = 0;

        [DataMember]
        public string MaterialCode { get; set; } = "";

        [DataMember]
        public long PalletId { get; set; } = 0;

        [DataMember]
        public long GRNId { get; set; } = 0;

        [DataMember]
        public long GRNItemId { get; set; } = 0;

        [DataMember]
        public long LocationId { get; set; } = 0;

        [DataMember]
        public int Status { get; set; } = -1;
    }
}