using System.Collections.Generic;

namespace ELog.EntityFrameworkCore.EntityFrameworkCore.Seed.CountryStateMaster
{
    public class CountryStateDto
    {
        public string CountryName { get; set; }
        public IEnumerable<string> States { get; set; }
    }
}