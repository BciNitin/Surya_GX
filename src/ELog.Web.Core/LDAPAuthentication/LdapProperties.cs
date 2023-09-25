using System.Collections.Generic;
using System.Linq;

namespace ELog.Web.Core.LDAPAuthentication
{
    public class LdapProperties
    {
        public LdapProperties(Dictionary<string, string> dictLdapProperties)
        {
            DN = dictLdapProperties.TryGetValue("DN", out string DummyDN) ? DummyDN : null;
            Uid = dictLdapProperties.TryGetValue("uid", out string Dummyuid) ? Dummyuid : null;
            Givenname = dictLdapProperties.TryGetValue("givenname", out string Dummygivenname) ? Dummygivenname : null;
            Displayname = dictLdapProperties.TryGetValue("displayname", out string Dummydisplayname) ? Dummydisplayname : null;
            Loginshell = dictLdapProperties.TryGetValue("loginshell", out string Dummyloginshell) ? Dummyloginshell : null;
            Homedirectory = dictLdapProperties.TryGetValue("homedirectory", out string Dummyhomedirectory) ? Dummyhomedirectory : null;
            Departmentnumber = dictLdapProperties.TryGetValue("departmentnumber", out string Dummydepartmentnumber) ? Dummydepartmentnumber : null;
            Sn = dictLdapProperties.TryGetValue("sn", out string Dummysn) ? Dummysn : null;
            Employeenumber = dictLdapProperties.TryGetValue("employeenumber", out string Dummyemployeenumber) ? Dummyemployeenumber : null;
            Memberof = dictLdapProperties.TryGetValue("memberof", out string Dummymemberof) ? Dummymemberof : null;
            Objectclass = dictLdapProperties.TryGetValue("objectclass", out string Dummyobjectclass) ? Dummyobjectclass : null;
            Postaladdress = dictLdapProperties.TryGetValue("postaladdress", out string Dummypostaladdress) ? Dummypostaladdress : null;
            Cn = dictLdapProperties.TryGetValue("cn", out string Dummycn) ? Dummycn : null;
            Employeetype = dictLdapProperties.TryGetValue("employeetype", out string Dummyemployeetype) ? Dummyemployeetype : null;
            Physicaldeliveryofficename = dictLdapProperties.TryGetValue("physicaldeliveryofficename", out string Dummyphysicaldeliveryofficename) ? Dummyphysicaldeliveryofficename : null;
            Mail = dictLdapProperties.TryGetValue("mail", out string Dummymail) ? Dummymail : null;
            Homepostaladdress = dictLdapProperties.TryGetValue("homepostaladdress", out string Dummyhomepostaladdress) ? Dummyhomepostaladdress : null;
            Gidnumber = dictLdapProperties.TryGetValue("gidnumber", out string Dummygidnumber) ? Dummygidnumber : null;
            Uidnumber = dictLdapProperties.TryGetValue("uidnumber", out string Dummyuidnumber) ? Dummyuidnumber : null;
            Title = dictLdapProperties.TryGetValue("title", out string Dummytitle) ? Dummytitle : null;
            Company = dictLdapProperties.TryGetValue("company", out string Dummycompany) ? Dummycompany : null;
        }

        public string DN { get; set; }
        public string Uid { get; set; }
        public string Givenname { get; set; }
        public string Displayname { get; set; }
        public string Loginshell { get; set; }
        public string Homedirectory { get; set; }
        public string Departmentnumber { get; set; }
        public string Sn { get; set; }
        public string Employeenumber { get; set; }
        public string Memberof { get; set; }
        public string Objectclass { get; set; }
        public string Postaladdress { get; set; }
        public string Cn { get; set; }
        public string Employeetype { get; set; }
        public string Physicaldeliveryofficename { get; set; }
        public string Mail { get; set; }
        public string Homepostaladdress { get; set; }
        public string Gidnumber { get; set; }
        public string Uidnumber { get; set; }
        public string Title { get; set; }
        public string Company { get; set; }

        public List<string> GetRoles()
        {
            if (Memberof == null)
            {
                return default(List<string>);
            }
            List<string> roles = new List<string>();
            var roleess = Memberof.Split(",").Where(x => x.Contains("cn"));
            foreach (var rolecn in roleess)
            {
                roles.Add(rolecn.Split("=")[1]?.ToLower());
            }
            return roles;
        }
    }
}