﻿using System.Collections.Generic;

namespace ELog.Application.Sessions.Dto
{
    public class GetCurrentLoginInformationsOutput
    {
        public ApplicationInfoDto Application { get; set; }

        public UserLoginInfoDto User { get; set; }

        public TenantLoginInfoDto Tenant { get; set; }
        public IList<string> Role { get; set; }
    }
}