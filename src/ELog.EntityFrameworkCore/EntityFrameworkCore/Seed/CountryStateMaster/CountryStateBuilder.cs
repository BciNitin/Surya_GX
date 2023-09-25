using ELog.Core;
using ELog.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace ELog.EntityFrameworkCore.EntityFrameworkCore.Seed.CountryStateMaster
{
    [ExcludeFromCodeCoverage]
    public class CountryStateBuilder
    {
        private readonly PMMSDbContext _context;
        private readonly int _tenantId;

        public CountryStateBuilder(PMMSDbContext context, int tenantId)
        {
            _context = context;
            _tenantId = tenantId;
        }

        public void Create()
        {
            var existingCountries = _context.CountryMaster.IgnoreQueryFilters().Select(x => x.CountryName).ToList() ?? default;
            var existingStates = _context.StateMaster.IgnoreQueryFilters().Select(x => x.StateName).ToList() ?? default;
            var countries = SeedHelper.SeedEntityData<CountryStateDto>(PMMSConsts.CountryStateSeedFilePath).Where(x => !existingCountries.Contains(x.CountryName));
            foreach (CountryStateDto countryDto in countries)
            {
                var country = new CountryMaster
                {
                    CountryName = countryDto.CountryName,
                    CreationTime = DateTime.UtcNow,
                    IsDeleted = false
                };
                _context.CountryMaster.Add(country);
                _context.SaveChanges();
                foreach (var stateDto in countryDto.States.Where(x => !existingStates.Contains(x)))
                {
                    var state = new StateMaster
                    {
                        CountryId = country.Id,
                        StateName = stateDto,
                        CreationTime = DateTime.UtcNow,
                        IsDeleted = false
                    };
                    _context.StateMaster.Add(state);
                }
            }
            _context.SaveChanges();
        }
    }
}