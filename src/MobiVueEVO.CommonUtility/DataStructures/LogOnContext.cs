using MobiVUE.Utility;
using System;
using System.Collections.Generic;

namespace MobiVUE
{
    public sealed class LogOnContext
    {
        public static event EventHandler<KeyValuePair<int, string>> LogOnLocationChangedEvent;

        public static event EventHandler<KeyValuePair<int, string>> LogOnSiteChangedEvent;

        private static KeyValuePair<int, string> _Site = new KeyValuePair<int, string>(0, "");

        public static KeyValuePair<int, string> Site
        {
            get { return _Site; }
            set
            {
                _Site = value;
                LogOnSiteChangedEvent?.Invoke(null, value);
            }
        }

        private static KeyValuePair<int, string> _Location = new KeyValuePair<int, string>(0, "");

        public static KeyValuePair<int, string> Location
        {
            get { return _Location; }
            set
            {
                _Location = value;
                LogOnLocationChangedEvent?.Invoke(null, value);
            }
        }

        //public static IUser LogOnUser { get; set; }

        private static IUser _user;

        public static IUser LogOnUser
        {
            get
            {
                if (_user.IsNull() || _user.ApplicationType == AppType.Web)
                {
                    _user = TypeFactory.GetObject<IUser>("logOnContextType");
                }
                return _user;
            }
            set
            {
                _user = value;
            }
        }

        public static void Reset()
        {
            Site = new KeyValuePair<int, string>(0, "");
            Location = new KeyValuePair<int, string>(0, "");
            // LogOnUser = new User();
        }
    }

    public class LogOnUser : IUser
    {
        public AppType ApplicationType { get; set; } = AppType.Windows;
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string UserToken { get; set; }
        public bool IsSupperUser { get; set; }
        public int EmployeeId { get; set; }
        public int CompanyId { get; set; }
    }

    //public class LogOnUser1 : IUser
    //{
    //    IUser _iser;
    //    public LogOnUser1() {
    //        _iser = session[];
    //    }
    //    public AppType ApplicationType { get; set; } = AppType.Windows;
    //    public string UserId {
    //        get {
    //            return _iser.UserId;
    //        }
    //        set { }
    //    }

    //    public string UserName { get; set; }
    //    public string UserToken { get; set; }
    //    public bool IsSupperUser { get; set; }
    //    public int EmployeeId { get; set; }
    //    public int CompanyId { get; set; }
    //}

    public interface IUser
    {
        AppType ApplicationType { get; set; }
        string UserId { get; set; }
        string UserName { get; set; }
        string UserToken { get; set; }
        bool IsSupperUser { get; set; }
        int EmployeeId { get; set; }
        int CompanyId { get; set; }
    }
}