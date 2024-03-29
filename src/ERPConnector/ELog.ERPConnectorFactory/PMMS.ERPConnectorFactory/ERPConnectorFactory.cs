﻿using Abp.Dependency;

using ELog.ERPConnector;

using System;

using static ELog.Core.PMMSEnums;

namespace ELog.ERPConnectorFactory
{
    public class ERPConnectorFactory
    {
        public IConnector GetERPConnector(ERPConnectorType connectorType)
        {
            try
            {
                switch (connectorType)
                {
                    case ERPConnectorType.SAPAjanta:
                    default:
                        return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}