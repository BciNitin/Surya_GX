using System.Collections.Generic;

namespace ELog.Core.Interfaces
{
    public interface ILDAPClient
    {
        List<Dictionary<string, string>> search(string baseDn, string ldapFilter);
        bool validateUserByBind(string username, string password);
        void CreateLdapConnection(string username, string password, string url);
    }
}
