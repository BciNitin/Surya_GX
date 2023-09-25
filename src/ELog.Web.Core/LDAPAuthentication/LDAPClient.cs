using ELog.Core.Interfaces;

using System;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using System.Net;

namespace ELog.Application.LDAPAuthentication
{
    public class LDAPClient : ILDAPClient
    {

        public void CreateLdapConnection(string username, string password, string url)
        {
            var credentials = new NetworkCredential(username, password);

            connection = new LdapConnection(url)
            {
                AuthType = AuthType.Basic,
                Credential = credentials
            };
            connection.SessionOptions.ProtocolVersion = 3;
            connection.Bind();
        }

        public List<Dictionary<string, string>> search(string baseDn, string ldapFilter)
        {
            var request = new SearchRequest(baseDn, ldapFilter, SearchScope.Subtree, null);
            var response = (SearchResponse)connection.SendRequest(request);

            var result = new List<Dictionary<string, string>>();

            foreach (SearchResultEntry entry in response.Entries)
            {
                var dictSearchResult = new Dictionary<string, string>
                {
                    ["DN"] = entry.DistinguishedName
                };

                foreach (string attrName in entry.Attributes.AttributeNames)
                {
                    //For simplicity, we ignore multi-value attributes
                    dictSearchResult[attrName] = string.Join(",", entry.Attributes[attrName].GetValues(typeof(string)));
                }

                result.Add(dictSearchResult);
            }

            return result;
        }




        public bool validateUserByBind(string username, string password)
        {
            bool result = true;


            var credentials = new NetworkCredential(username, password);

            connection = new LdapConnection(connection.SessionOptions.HostName)
            {
                AuthType = AuthType.Basic,
                Credential = credentials
            };
            connection.SessionOptions.ProtocolVersion = 3;


            try
            {
                connection.Bind();
            }
            catch (Exception)
            {
                result = false;
            }


            return result;
        }

        private LdapConnection connection;
    }
}
