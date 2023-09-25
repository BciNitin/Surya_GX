using MobiVUE.Utility;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace MobiVueEVO.BO
{

    [DataContract]

    public class Menu : BusinessBase
    {

        private short _id;

        [DataMember]
        public short Id
        {
            get { return _id; }
            set { SetValue<short>(ref _id, value, () => this.Id); }
        }

        private string _module;

        [Required(AllowEmptyStrings = false, ErrorMessage = "Module is mandatory.")]
        [DataMember]
        public string Module
        {
            get { return _module; }
            set { SetValue<string>(ref _module, value, () => this.Module); }
        }

        private string _submodule;

        [Required(AllowEmptyStrings = false, ErrorMessage = "SubModule is mandatory.")]
        [DataMember]
        public string SubModule
        {
            get { return _submodule; }
            set { SetValue<string>(ref _submodule, value, () => this.SubModule); }
        }

        private string _path;

        [Required(AllowEmptyStrings = false, ErrorMessage = "Path is mandatory.")]
        [DataMember]
        public string Path
        {
            get { return _path; }
            set { SetValue<string>(ref _path, value, () => this.Path); }
        }


        private string _displayName;

        [DataMember]
        public string DisplayName
        {
            get { return _displayName; }
            set { SetValue<string>(ref _displayName, value, () => this.DisplayName); }
        }

        private string _componentName;

        [DataMember]
        public string ComponentName
        {
            get { return _componentName; }
            set { SetValue<string>(ref _componentName, value, () => this.ComponentName); }
        }

        private string _permissionName;

        [DataMember]
        public string PermissionName
        {
            get { return _permissionName; }
            set { SetValue<string>(ref _permissionName, value, () => this.PermissionName); }
        }


        private KeyValue<long, string> _createdBy;

        [DataMember]
        public KeyValue<long, string> CreatedBy
        {
            get { return _createdBy; }
            set { SetValue<KeyValue<long, string>>(ref _createdBy, value, () => this.CreatedBy); }
        }

        private SmartDateTime _createdOn;

        [DataMember]
        public SmartDateTime CreatedOn
        {
            get { return _createdOn; }
            set { SetValue<SmartDateTime>(ref _createdOn, value, () => this.CreatedOn); }
        }






    }
    [DataContract]
    public class MenuSearchCriteria
    {
        [DataMember]
        public int UserId { get; set; } = 0;

        //[DataMember]
        //public long SiteId { get; set; } = 0;

        //[DataMember]
        //public int Status { get; set; } = -1;

        //[DataMember]
        //public string Module { get; set; } = "";

        //[DataMember]
        //public string Name { get; set; } = "";

        //[DataMember]
        //public string SubModule { get; set; } = "";

        //[DataMember]
        //public long TotalRowCount { get; set; } = 0;

        //[DataMember]
        //public int PageSize { get; set; } = 100;

        //[DataMember]
        //public int PageNumber { get; set; } = 1;
    }
}
