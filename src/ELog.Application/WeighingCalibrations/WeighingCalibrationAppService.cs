using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq;
using Abp.Linq.Extensions;
using ELog.Application.CommonService.Inward;
using ELog.Application.Masters.InspectionChecklists.Dto;
using ELog.Application.Masters.WeighingMachines.Dto;
using ELog.Application.SelectLists.Dto;
using ELog.Application.Sessions;
using ELog.Application.WeighingCalibrations.Dto;
using ELog.Core;
using ELog.Core.Authorization;
using ELog.Core.Entities;
using ELog.Core.Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using static ELog.Core.PMMSEnums;

namespace ELog.Application.WeighingCalibrations
{
    [PMMSAuthorize]
    public class WeighingCalibrationAppService : ApplicationService, IWeighingCalibrationAppService
    {
        private readonly ISessionAppService _sessionAppService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRepository<WMCalibrationHeader> _wmCalibrationHeaderRepository;
        private readonly IRepository<WeighingMachineMaster> _weighingMachineRepository;
        private readonly IRepository<UnitOfMeasurementMaster> _unitOfMeasurementRepository;
        private readonly IRepository<CalibrationFrequencyMaster> _calibrationFrequencyRepository;
        private readonly IRepository<FrequencyTypeMaster> _frequencyTypeRepository;
        private readonly IRepository<StandardWeightBoxMaster> _standardWeightBoxRepository;
        private readonly IRepository<StandardWeightMaster> _standardWeightRepository;
        private readonly IRepository<WMCalibrationDetailWeight> _calibrationStandardWeightRepository;
        private readonly IRepository<WMCalibrationDetail> _wmCalibrationDetailsRepository;
        private readonly IRepository<WMCalibrationEccentricityTest> _wmCalibrationEccentricityTestRepository;
        private readonly IRepository<WMCalibrationLinearityTest> _wmCalibrationLinearityTestRepository;
        private readonly IRepository<WMCalibrationRepeatabilityTest> _wmCalibrationRepeatabilityTestRepository;
        private readonly IRepository<CheckpointMaster> _checkpointRepository;
        private readonly IRepository<WMCalibratedLatestMachineDetail> _wmcalibratedMachineRepository;
        private readonly IRepository<WMCalibrationUncertainityTest> _wmCalibrationUncertainityTestRepository;
        private readonly IRepository<CalibrationStatusMaster> _calibrationStatusRepository;
        private readonly IRepository<WeighingMachineTestConfiguration> _testConfigurationRepository;
        private readonly IWebHostEnvironment _env;
        private readonly IInwardAppService _inwardAppService;
        private readonly IConfiguration _configuration;
        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }

        public WeighingCalibrationAppService(
            IRepository<WeighingMachineMaster> weighingMachineRepository,
            IRepository<WMCalibrationHeader> wmCalibrationHeaderRepository,
            IRepository<UnitOfMeasurementMaster> unitOfMeasurementRepository,
            IRepository<CalibrationFrequencyMaster> calibrationFrequencyRepository,
            IRepository<FrequencyTypeMaster> frequencyTypeRepository,
            IRepository<StandardWeightBoxMaster> standardWeightBoxRepository,
        IRepository<StandardWeightMaster> standardWeightRepository,
        IRepository<WMCalibrationDetailWeight> calibrationStandardWeightRepository,
        IRepository<WMCalibrationDetail> wmCalibrationDetailsRepository,
        IRepository<WMCalibrationEccentricityTest> wmCalibrationEccentricityTestRepository,
        IRepository<WMCalibrationLinearityTest> wmCalibrationLinearityTestRepository,
        IRepository<WMCalibrationRepeatabilityTest> wmCalibrationRepeatabilityTestRepository,
        IRepository<CheckpointMaster> checkpointRepository,
        IRepository<WMCalibratedLatestMachineDetail> wmcalibratedMachineRepository,
        IRepository<WMCalibrationUncertainityTest> wmCalibrationUncertainityTestRepository,
        IRepository<CalibrationStatusMaster> calibrationStatusRepository,
         IRepository<WeighingMachineTestConfiguration> testConfigurationRepository,
        ISessionAppService sessionAppService,
         IHttpContextAccessor httpContextAccessor, IInwardAppService inwardAppService,
          IWebHostEnvironment env, IConfiguration configuration)
        {
            _wmCalibrationHeaderRepository = wmCalibrationHeaderRepository;
            _weighingMachineRepository = weighingMachineRepository;
            _unitOfMeasurementRepository = unitOfMeasurementRepository;
            _calibrationFrequencyRepository = calibrationFrequencyRepository;
            _frequencyTypeRepository = frequencyTypeRepository;
            _standardWeightBoxRepository = standardWeightBoxRepository;
            _standardWeightRepository = standardWeightRepository;
            _calibrationStandardWeightRepository = calibrationStandardWeightRepository;
            _wmCalibrationDetailsRepository = wmCalibrationDetailsRepository;
            _wmCalibrationEccentricityTestRepository = wmCalibrationEccentricityTestRepository;
            _wmCalibrationLinearityTestRepository = wmCalibrationLinearityTestRepository;
            _wmCalibrationRepeatabilityTestRepository = wmCalibrationRepeatabilityTestRepository;
            _checkpointRepository = checkpointRepository;
            _wmcalibratedMachineRepository = wmcalibratedMachineRepository;
            _wmCalibrationUncertainityTestRepository = wmCalibrationUncertainityTestRepository;
            _calibrationStatusRepository = calibrationStatusRepository;
            _testConfigurationRepository = testConfigurationRepository;
            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
            _httpContextAccessor = httpContextAccessor;
            _sessionAppService = sessionAppService;
            _env = env;
            _inwardAppService = inwardAppService;
            _configuration = configuration;
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.WeighingCalibration_SubModule + "." + PMMSPermissionConst.View)]
        public async Task<WeighingCalibrationDto> GetAsync(int weighingMachineId)
        {
            var weighingCalibrationDto = await GetWeighingCalibrationDtoByWeighingMachineId(weighingMachineId);
            weighingCalibrationDto.calibrationFrequencyDtos = await (from weighingMachine in _weighingMachineRepository.GetAll()
                                                                     join calibrationFrequency in _calibrationFrequencyRepository.GetAll()
                                                                     on weighingMachine.Id equals calibrationFrequency.WeighingMachineId
                                                                     join frequencyType in _frequencyTypeRepository.GetAll()
                                                                     on calibrationFrequency.FrequencyTypeId equals frequencyType.Id
                                                                     where weighingMachine.Id == weighingMachineId
                                                                     select new CalibrationFrequencyDto
                                                                     {
                                                                         Id = calibrationFrequency.Id,
                                                                         FrequencyTypeId = calibrationFrequency.FrequencyTypeId,
                                                                         CalibrationLevel = calibrationFrequency.CalibrationLevel,
                                                                         MaximumValue = calibrationFrequency.MaximumValue,
                                                                         MinimumValue = calibrationFrequency.MinimumValue,
                                                                         StandardWeightValue = calibrationFrequency.StandardWeightValue,
                                                                         WeighingMachineId = calibrationFrequency.WeighingMachineId

                                                                     }).AsNoTracking().ToListAsync();
            DateTime today = DateTime.UtcNow;
            var IsCalibrationExistForToday = await _wmCalibrationHeaderRepository.GetAll().Where(x => x.WeighingMachineId == weighingMachineId && x.CalibrationTestDate.Year == today.Year
                          && x.CalibrationTestDate.Month == today.Month
                           && x.CalibrationTestDate.Day == today.Day).AnyAsync();
            //IsCalibrationExistForToday = true;
            var existingFrequencyTypeIds = weighingCalibrationDto.calibrationFrequencyDtos.Select(x => x.FrequencyTypeId)?.ToList();

            int FreqTypeid = 3;

            if (!existingFrequencyTypeIds.Contains(FreqTypeid))
            {
                existingFrequencyTypeIds.Add(FreqTypeid);
            }


            if (existingFrequencyTypeIds != null && existingFrequencyTypeIds.Count > 0)
            {
                if (!IsCalibrationExistForToday)
                {
                    var existingCalibrationDates = await _wmCalibrationHeaderRepository.GetAll().Where(x =>
                                x.WeighingMachineId == weighingMachineId &&
                                x.CalibrationTestDate.Month == today.Month &&
                                x.CalibrationTestDate.Year == today.Year).
                               Select(x => x.CalibrationTestDate).ToListAsync() ?? default;

                    var validSelectedCalibration = PMMSDateTimeHelper.GetDropDownToBindFrequencyTypes(existingCalibrationDates, existingFrequencyTypeIds);
                    weighingCalibrationDto.FrequencyModeld = (int)validSelectedCalibration;
                    weighingCalibrationDto.calibrationFrequencySelectListDtos = new List<SelectListDto>
                    {
                        new SelectListDto { Id = (int)validSelectedCalibration, Value = validSelectedCalibration.GetAttribute<DisplayAttribute>().Name }
                    };
                }
                else
                {
                    var allFrequencies = Enum.GetValues(typeof(WeighingMachineFrequencyType)).Cast<WeighingMachineFrequencyType>().Select(v => new SelectListDto
                    {
                        Id = (int)v,
                        Value = v.GetAttribute<DisplayAttribute>().Name
                    }).AsQueryable().OrderBy(x => x.Value).ToList();
                    weighingCalibrationDto.IsReCalibrated = true;

                    weighingCalibrationDto.calibrationFrequencySelectListDtos = new List<SelectListDto>();

                    foreach (var frequency in allFrequencies)
                    {
                        if (existingFrequencyTypeIds.Contains((int)frequency.Id))
                        {
                            weighingCalibrationDto.FrequencyModeld = (int)frequency.Id;
                            weighingCalibrationDto.calibrationFrequencySelectListDtos.Add(frequency);
                        }
                    }
                }
            }
            weighingCalibrationDto.CalibrationStatusId = (int)CalibrationStatus.In_Progress;
            return weighingCalibrationDto;
        }



        [PMMSAuthorize(Permissions = PMMSPermissionConst.WeighingCalibration_SubModule + "." + PMMSPermissionConst.View)]
        public async Task<bool> isNewCalibration(int weighingMachineId, int? frequencyId)
        {
            dynamic weighingData = null;
            if (frequencyId != null)
            {

                weighingData = await _wmCalibrationHeaderRepository.GetAll()
                                         .Where(x => x.WeighingMachineId == weighingMachineId && x.CalibrationFrequencyId == frequencyId)
                                         .Select(x => new { x.Id, x.WeighingMachineId }).ToListAsync();
            }
            else
            {

                weighingData = await _wmCalibrationHeaderRepository.GetAll()
                                         .Where(x => x.WeighingMachineId == weighingMachineId)
                                         .Select(x => new { x.Id, x.WeighingMachineId }).ToListAsync();
            }

            if (weighingData.Count == 0)
            {
                return false;
            }

            return true;
        }


        [PMMSAuthorize(Permissions = PMMSPermissionConst.WeighingCalibration_SubModule + "." + PMMSPermissionConst.View)]
        public async Task<WeighingCalibrationDto> GetCalibrationAsync(int WeighingCalibrationHeaderId)
        {
            var weighingCalibrationStandardWeightBoxes = await _standardWeightBoxRepository.GetAll().Select(x => new { x.Id, x.StandardWeightBoxId }).AsNoTracking().ToListAsync();
            var wmCalibrationHeader = await _wmCalibrationHeaderRepository.GetAllIncluding(x => x.WMCalibrationDetails, x => x.WMCalibrationCheckpoints,
                x => x.WMCalibrationEccentricityTests, x => x.WMCalibrationLinearityTests,
                x => x.WMCalibrationRepeatabilityTests, x => x.WMCalibrationUncertainityTests).Where(x => x.Id == WeighingCalibrationHeaderId).AsNoTracking().FirstAsync();
            WeighingCalibrationDto calibrationDto = await GetWeighingCalibrationDtoByWeighingMachineId(wmCalibrationHeader.WeighingMachineId.GetValueOrDefault());
            ObjectMapper.Map(wmCalibrationHeader, calibrationDto);

            calibrationDto.FrequencyModeld = wmCalibrationHeader.CalibrationFrequencyId.GetValueOrDefault();
            calibrationDto.UserEnteredFrequencyMode = ((WeighingMachineFrequencyType)calibrationDto.FrequencyModeld).ToString();
            calibrationDto.CalibrationStatus = _calibrationStatusRepository.GetAll().Where(x => x.Id == calibrationDto.CalibrationStatusId).Select(x => x.StatusName).FirstOrDefault();
            calibrationDto.CalibrationTestDate = wmCalibrationHeader.CalibrationTestDate;
            //Get checkpoints
            calibrationDto.WeighingCalibrationCheckpoints = new List<CheckpointDto>();

            var lstCheckpoints = wmCalibrationHeader.WMCalibrationCheckpoints.Select(x => x.CheckPointId)?.ToList();
            if (lstCheckpoints != null && lstCheckpoints.Count > 0)
            {
                var checkpoints = await _checkpointRepository.GetAll().Where(x => lstCheckpoints.Contains(x.Id))
                                    .Select(x => new { x.Id, x.CheckpointName }).ToListAsync();

                foreach (var insertedCheckpoint in wmCalibrationHeader.WMCalibrationCheckpoints)
                {
                    CheckpointDto checkpoint = new CheckpointDto
                    {
                        Id = insertedCheckpoint.Id,
                        Observation = insertedCheckpoint.Observation,
                        DiscrepancyRemark = insertedCheckpoint.DiscrepancyRemark,
                        CheckPointId = insertedCheckpoint.CheckPointId
                    };
                    checkpoint.CheckpointName = checkpoints.First(x => x.Id == checkpoint.CheckPointId).CheckpointName;
                    calibrationDto.WeighingCalibrationCheckpoints.Add(checkpoint);
                }
                calibrationDto.IsCheckpointFinished = true;
            }

            //Get CalibrationDetails
            var commonCalibrationTestDto = await GetCommonTestDto(calibrationDto.WeighingMachineId.GetValueOrDefault(), calibrationDto.FrequencyModeld);
            calibrationDto.AcceptableUncertainityValue = commonCalibrationTestDto.AcceptableUncertainityValue.GetValueOrDefault();
            calibrationDto.lstWeighingCalibrationDetailDto = new List<WeighingCalibrationDetailDto>();
            if (wmCalibrationHeader.WMCalibrationDetails?.Count > 0)
            {
                var lstDetailId = wmCalibrationHeader.WMCalibrationDetails.Select(x => x.Id);
                var lstWeights = await (from calibrationStandardWeight in _calibrationStandardWeightRepository.GetAll()
                                        join standardWeight in _standardWeightRepository.GetAll()
                                        on calibrationStandardWeight.StandardWeightId equals standardWeight.Id
                                        join unitOfMeasurement in _unitOfMeasurementRepository.GetAll()
                                        on standardWeight.UnitOfMeasurementId equals unitOfMeasurement.Id
                                        where lstDetailId.Contains(calibrationStandardWeight.WMCalibrationDetailId.GetValueOrDefault())
                                        select new WMCalibrationInternalStandardWeightDto
                                        {
                                            WMCalibrationDetailTestId = calibrationStandardWeight.WMCalibrationDetailId,
                                            StandardWeightId = calibrationStandardWeight.StandardWeightId,
                                            CapturedWeightKeyTypeId = calibrationStandardWeight.CapturedWeightKeyTypeId,
                                            UserEnteredStandardWeight = standardWeight.StandardWeightId + " - " + standardWeight.Capacity + " " + unitOfMeasurement.UnitOfMeasurement
                                        }).AsNoTracking().ToListAsync() ?? default;

                foreach (var calibrationDetail in wmCalibrationHeader.WMCalibrationDetails)
                {
                    WeighingCalibrationDetailDto detail = ObjectMapper.Map<WeighingCalibrationDetailDto>(calibrationDetail);
                    var currentCalibration = commonCalibrationTestDto.lstCalibrations.First(x => x.CalibrationLevelId == detail.CalibrationLevelId);
                    detail.lstWeightId = lstWeights.Where(x => x.WMCalibrationDetailTestId == detail.Id)
                        .Select(x => x.StandardWeightId.GetValueOrDefault())?.ToList();
                    detail.UserEnteredWeightId = String.Join(",", lstWeights.Where(x => x.WMCalibrationDetailTestId == detail.Id)
                        .Select(x => x.UserEnteredStandardWeight)?.ToList());
                    detail.WeighingMachineId = calibrationDto.WeighingMachineId;
                    detail.DoneBy = calibrationDto.DoneBy;
                    detail.CheckedBy = calibrationDto.CheckedBy;
                    ObjectMapper.Map(currentCalibration, detail);
                    var standardWeightBox = weighingCalibrationStandardWeightBoxes.FirstOrDefault(x => x.Id == detail.StandardWeightBoxId);
                    if (standardWeightBox != null)
                    {
                        detail.UserEnteredWeightBox = standardWeightBox.StandardWeightBoxId;
                    }
                    calibrationDto.lstWeighingCalibrationDetailDto.Add(detail);
                }
            }
            calibrationDto.WeighingCalibrationDetailCurrentDto = new WeighingCalibrationDetailDto();
            var calibrationsNotDone = commonCalibrationTestDto.lstCalibrations.Select(x => x.CalibrationLevelId);
            if (calibrationDto.lstWeighingCalibrationDetailDto != null && calibrationDto.lstWeighingCalibrationDetailDto.Count > 0)
            {
                calibrationsNotDone = commonCalibrationTestDto.lstCalibrations.Select(x => x.CalibrationLevelId)
              .Except(calibrationDto.lstWeighingCalibrationDetailDto.Select(x => x.CalibrationLevelId.GetValueOrDefault()));
            }

            if (calibrationsNotDone?.Count() > 0)
            {
                //Make Calibration First
                var currentCalibration = commonCalibrationTestDto.lstCalibrations.First(x => x.CalibrationLevelId == calibrationsNotDone.First());
                ObjectMapper.Map(currentCalibration, calibrationDto.WeighingCalibrationDetailCurrentDto);
            }
            else
            {
                //Mark IsCalibrationFinished
                calibrationDto.IsAllCalibrationLevelFinished = true;
            }

            //Get Eccentricity Test
            if (wmCalibrationHeader.WMCalibrationEccentricityTests?.Count > 0)
            {
                var eccentricityId = wmCalibrationHeader.WMCalibrationEccentricityTests.Select(x => x.Id).First();
                var lstWeights = await (from calibrationStandardWeight in _calibrationStandardWeightRepository.GetAll()
                                        join standardWeight in _standardWeightRepository.GetAll()
                                        on calibrationStandardWeight.StandardWeightId equals standardWeight.Id
                                        join unitOfMeasurement in _unitOfMeasurementRepository.GetAll()
                                        on standardWeight.UnitOfMeasurementId equals unitOfMeasurement.Id into ps
                                        from calib in ps.DefaultIfEmpty()
                                        where calibrationStandardWeight.WMCalibrationEccentricityTestId == eccentricityId

                                        select new WMCalibrationInternalStandardWeightDto
                                        {
                                            WMCalibrationEccentricityTestId = calibrationStandardWeight.WMCalibrationEccentricityTestId,
                                            StandardWeightId = calibrationStandardWeight.StandardWeightId,
                                            CapturedWeightKeyTypeId = calibrationStandardWeight.CapturedWeightKeyTypeId,
                                            UserEnteredStandardWeight = standardWeight.StandardWeightId + " - " + standardWeight.Capacity + " " + (calib.UnitOfMeasurement == null ? "" : calib.UnitOfMeasurement)
                                        }).AsNoTracking().ToListAsync() ?? default;




                foreach (var eccentricityTest in wmCalibrationHeader.WMCalibrationEccentricityTests)
                {
                    calibrationDto.WeighingCalibrationEccentricityTestDto = ObjectMapper.Map<WeighingCalibrationEccentricityTestDto>(eccentricityTest);
                    ObjectMapper.Map(commonCalibrationTestDto, calibrationDto.WeighingCalibrationEccentricityTestDto);

                    calibrationDto.WeighingCalibrationEccentricityTestDto.lstCValueStandardWeight = lstWeights.Where(x => x.WMCalibrationEccentricityTestId == eccentricityTest.Id
                    && x.CapturedWeightKeyTypeId == (int)WeighingCalibrationCapturedWeightKeyType.CapturedWeightEccentricityCValue)
                        .Select(x => new WMCalibrationStandardWeightDto { StandardWeightId = x.StandardWeightId.GetValueOrDefault(), UserEnteredStandardWeightId = x.UserEnteredStandardWeight })?.ToList();

                    calibrationDto.WeighingCalibrationEccentricityTestDto.lstLBValueStandardWeight = lstWeights.Where(x => x.WMCalibrationEccentricityTestId == eccentricityTest.Id
                    && x.CapturedWeightKeyTypeId == (int)WeighingCalibrationCapturedWeightKeyType.CapturedWeightEccentricityLBValue)
                         .Select(x => new WMCalibrationStandardWeightDto { StandardWeightId = x.StandardWeightId.GetValueOrDefault(), UserEnteredStandardWeightId = x.UserEnteredStandardWeight })?.ToList();

                    calibrationDto.WeighingCalibrationEccentricityTestDto.lstLFValueStandardWeight = lstWeights.Where(x => x.WMCalibrationEccentricityTestId == eccentricityTest.Id
                   && x.CapturedWeightKeyTypeId == (int)WeighingCalibrationCapturedWeightKeyType.CapturedWeightEccentricityLFValue)
                       .Select(x => new WMCalibrationStandardWeightDto { StandardWeightId = x.StandardWeightId.GetValueOrDefault(), UserEnteredStandardWeightId = x.UserEnteredStandardWeight })?.ToList();

                    calibrationDto.WeighingCalibrationEccentricityTestDto.lstRBValueStandardWeight = lstWeights.Where(x => x.WMCalibrationEccentricityTestId == eccentricityTest.Id
                  && x.CapturedWeightKeyTypeId == (int)WeighingCalibrationCapturedWeightKeyType.CapturedWeightEccentricityRBValue)
                      .Select(x => new WMCalibrationStandardWeightDto { StandardWeightId = x.StandardWeightId.GetValueOrDefault(), UserEnteredStandardWeightId = x.UserEnteredStandardWeight })?.ToList();

                    calibrationDto.WeighingCalibrationEccentricityTestDto.lstRFValueStandardWeight = lstWeights.Where(x => x.WMCalibrationEccentricityTestId == eccentricityTest.Id
                  && x.CapturedWeightKeyTypeId == (int)WeighingCalibrationCapturedWeightKeyType.CapturedWeightEccentricityRFValue)
                     .Select(x => new WMCalibrationStandardWeightDto { StandardWeightId = x.StandardWeightId.GetValueOrDefault(), UserEnteredStandardWeightId = x.UserEnteredStandardWeight })?.ToList();

                    calibrationDto.WeighingCalibrationEccentricityTestDto.UserEnteredCValueStandardWeightBox = weighingCalibrationStandardWeightBoxes.FirstOrDefault(x => x.Id == eccentricityTest.CValueStandardWeightBoxId)?.StandardWeightBoxId;
                    calibrationDto.WeighingCalibrationEccentricityTestDto.UserEnteredLFValueStandardWeightBox = weighingCalibrationStandardWeightBoxes.FirstOrDefault(x => x.Id == eccentricityTest.LFValueStandardWeightBoxId)?.StandardWeightBoxId;
                    calibrationDto.WeighingCalibrationEccentricityTestDto.UserEnteredLBValueStandardWeightBox = weighingCalibrationStandardWeightBoxes.FirstOrDefault(x => x.Id == eccentricityTest.LBValueStandardWeightBoxId)?.StandardWeightBoxId;
                    calibrationDto.WeighingCalibrationEccentricityTestDto.UserEnteredRFValueStandardWeightBox = weighingCalibrationStandardWeightBoxes.FirstOrDefault(x => x.Id == eccentricityTest.RFValueStandardWeightBoxId)?.StandardWeightBoxId;
                    calibrationDto.WeighingCalibrationEccentricityTestDto.UserEnteredRBValueStandardWeightBox = weighingCalibrationStandardWeightBoxes.FirstOrDefault(x => x.Id == eccentricityTest.RBValueStandardWeightBoxId)?.StandardWeightBoxId;

                    calibrationDto.WeighingCalibrationEccentricityTestDto.TestResult = ((CalibrationTestStatus)calibrationDto.WeighingCalibrationEccentricityTestDto.TestResultId.GetValueOrDefault()).ToString();
                }
            }
            else
            {
                calibrationDto.WeighingCalibrationEccentricityTestDto = ObjectMapper.Map<WeighingCalibrationEccentricityTestDto>(commonCalibrationTestDto);
                calibrationDto.WeighingCalibrationEccentricityTestDto.WeighingMachineCode = String.Empty;
                calibrationDto.WeighingCalibrationEccentricityTestDto.CalculatedCapacityWeight = Double.Parse(commonCalibrationTestDto.CalculatedCapacityWeight);
                // calibrationDto.WeighingCalibrationEccentricityTestDto.EccentricityInstruction = commonCalibrationTestDto.EccentricityInstruction;
            }

            //Get Linearity Test
            if (wmCalibrationHeader.WMCalibrationLinearityTests?.Count > 0)
            {
                var linearityId = wmCalibrationHeader.WMCalibrationLinearityTests.Select(x => x.Id).First();
                var lstWeights = await (from calibrationStandardWeight in _calibrationStandardWeightRepository.GetAll()
                                        join standardWeight in _standardWeightRepository.GetAll()
                                        on calibrationStandardWeight.StandardWeightId equals standardWeight.Id
                                        join unitOfMeasurement in _unitOfMeasurementRepository.GetAll()
                                       on standardWeight.UnitOfMeasurementId equals unitOfMeasurement.Id
                                        where calibrationStandardWeight.WMCalibrationLinearityTestId == linearityId
                                        select new WMCalibrationInternalStandardWeightDto
                                        {
                                            WMCalibrationLinearityTestId = calibrationStandardWeight.WMCalibrationLinearityTestId,
                                            StandardWeightId = calibrationStandardWeight.StandardWeightId,
                                            CapturedWeightKeyTypeId = calibrationStandardWeight.CapturedWeightKeyTypeId,
                                            UserEnteredStandardWeight = standardWeight.StandardWeightId + " - " + standardWeight.Capacity + " " + (unitOfMeasurement.UnitOfMeasurement == null ? "" : unitOfMeasurement.UnitOfMeasurement)
                                        }).AsNoTracking().ToListAsync() ?? default;
                foreach (var linearityTest in wmCalibrationHeader.WMCalibrationLinearityTests)
                {
                    calibrationDto.WeighingCalibrationLinearityTestDto = ObjectMapper.Map<WeighingCalibrationLinearityTestDto>(linearityTest);
                    ObjectMapper.Map(commonCalibrationTestDto, calibrationDto.WeighingCalibrationLinearityTestDto);

                    calibrationDto.WeighingCalibrationLinearityTestDto.lstWeightValue1StandardWeight = lstWeights.Where(x => x.WMCalibrationLinearityTestId == linearityTest.Id
                   && x.CapturedWeightKeyTypeId == (int)WeighingCalibrationCapturedWeightKeyType.CapturedWeightLinearityWeight1Value)
                       .Select(x => new WMCalibrationStandardWeightDto { StandardWeightId = x.StandardWeightId.GetValueOrDefault(), UserEnteredStandardWeightId = x.UserEnteredStandardWeight })?.ToList();

                    calibrationDto.WeighingCalibrationLinearityTestDto.lstWeightValue2StandardWeight = lstWeights.Where(x => x.WMCalibrationLinearityTestId == linearityTest.Id
                    && x.CapturedWeightKeyTypeId == (int)WeighingCalibrationCapturedWeightKeyType.CapturedWeightLinearityWeight2Value)
                         .Select(x => new WMCalibrationStandardWeightDto { StandardWeightId = x.StandardWeightId.GetValueOrDefault(), UserEnteredStandardWeightId = x.UserEnteredStandardWeight })?.ToList();

                    calibrationDto.WeighingCalibrationLinearityTestDto.lstWeightValue3StandardWeight = lstWeights.Where(x => x.WMCalibrationLinearityTestId == linearityTest.Id
                   && x.CapturedWeightKeyTypeId == (int)WeighingCalibrationCapturedWeightKeyType.CapturedWeightLinearityWeight3Value)
                       .Select(x => new WMCalibrationStandardWeightDto { StandardWeightId = x.StandardWeightId.GetValueOrDefault(), UserEnteredStandardWeightId = x.UserEnteredStandardWeight })?.ToList();

                    calibrationDto.WeighingCalibrationLinearityTestDto.lstWeightValue4StandardWeight = lstWeights.Where(x => x.WMCalibrationLinearityTestId == linearityTest.Id
                  && x.CapturedWeightKeyTypeId == (int)WeighingCalibrationCapturedWeightKeyType.CapturedWeightLinearityWeight4Value)
                      .Select(x => new WMCalibrationStandardWeightDto { StandardWeightId = x.StandardWeightId.GetValueOrDefault(), UserEnteredStandardWeightId = x.UserEnteredStandardWeight })?.ToList();

                    calibrationDto.WeighingCalibrationLinearityTestDto.lstWeightValue5StandardWeight = lstWeights.Where(x => x.WMCalibrationLinearityTestId == linearityTest.Id
                  && x.CapturedWeightKeyTypeId == (int)WeighingCalibrationCapturedWeightKeyType.CapturedWeightLinearityWeight5Value)
                     .Select(x => new WMCalibrationStandardWeightDto { StandardWeightId = x.StandardWeightId.GetValueOrDefault(), UserEnteredStandardWeightId = x.UserEnteredStandardWeight })?.ToList();

                    calibrationDto.WeighingCalibrationLinearityTestDto.UserEnteredWeightValue1StandardWeightBox = weighingCalibrationStandardWeightBoxes.FirstOrDefault(x => x.Id == linearityTest.WeightValue1StandardWeightBoxId)?.StandardWeightBoxId;
                    calibrationDto.WeighingCalibrationLinearityTestDto.UserEnteredWeightValue2StandardWeightBox = weighingCalibrationStandardWeightBoxes.FirstOrDefault(x => x.Id == linearityTest.WeightValue2StandardWeightBoxId)?.StandardWeightBoxId;
                    calibrationDto.WeighingCalibrationLinearityTestDto.UserEnteredWeightValue3StandardWeightBox = weighingCalibrationStandardWeightBoxes.FirstOrDefault(x => x.Id == linearityTest.WeightValue3StandardWeightBoxId)?.StandardWeightBoxId;
                    calibrationDto.WeighingCalibrationLinearityTestDto.UserEnteredWeightValue4StandardWeightBox = weighingCalibrationStandardWeightBoxes.FirstOrDefault(x => x.Id == linearityTest.WeightValue4StandardWeightBoxId)?.StandardWeightBoxId;
                    calibrationDto.WeighingCalibrationLinearityTestDto.UserEnteredWeightValue5StandardWeightBox = weighingCalibrationStandardWeightBoxes.FirstOrDefault(x => x.Id == linearityTest.WeightValue5StandardWeightBoxId)?.StandardWeightBoxId;
                }
            }
            else
            {
                calibrationDto.WeighingCalibrationLinearityTestDto = ObjectMapper.Map<WeighingCalibrationLinearityTestDto>(commonCalibrationTestDto);
                calibrationDto.WeighingCalibrationLinearityTestDto.WeighingMachineCode = String.Empty;
            }

            //Get Repeatability Test
            if (wmCalibrationHeader.WMCalibrationRepeatabilityTests?.Count > 0)
            {
                var repeatabilityId = wmCalibrationHeader.WMCalibrationRepeatabilityTests.Select(x => x.Id).First();
                var lstWeights = await (from calibrationStandardWeight in _calibrationStandardWeightRepository.GetAll()
                                        join standardWeight in _standardWeightRepository.GetAll()
                                        on calibrationStandardWeight.StandardWeightId equals standardWeight.Id
                                        join unitOfMeasurement in _unitOfMeasurementRepository.GetAll()
                                       on standardWeight.UnitOfMeasurementId equals unitOfMeasurement.Id into ps
                                        from calib in ps.DefaultIfEmpty()
                                        where calibrationStandardWeight.WMCalibrationRepeatabilityTestId == repeatabilityId
                                        select new WMCalibrationInternalStandardWeightDto
                                        {
                                            WMCalibrationRepeatabilityTestId = calibrationStandardWeight.WMCalibrationRepeatabilityTestId,
                                            StandardWeightId = calibrationStandardWeight.StandardWeightId,
                                            CapturedWeightKeyTypeId = calibrationStandardWeight.CapturedWeightKeyTypeId,
                                            UserEnteredStandardWeight = standardWeight.StandardWeightId + " - " + standardWeight.Capacity + " " + (calib.UnitOfMeasurement == null ? "" : calib.UnitOfMeasurement)
                                        }).AsNoTracking().ToListAsync() ?? default;

                foreach (var repeatabilityTest in wmCalibrationHeader.WMCalibrationRepeatabilityTests)
                {
                    calibrationDto.WeighingCalibrationRepeatabilityTestDto = ObjectMapper.Map<WeighingCalibrationRepeatabilityTestDto>(repeatabilityTest);
                    calibrationDto.WeighingCalibrationRepeatabilityTestDto.WeighingMachineId = wmCalibrationHeader.WeighingMachineId;
                    ObjectMapper.Map(commonCalibrationTestDto, calibrationDto.WeighingCalibrationRepeatabilityTestDto);

                    calibrationDto.WeighingCalibrationRepeatabilityTestDto.lstWeightValue1StandardWeight = lstWeights.Where(x => x.WMCalibrationRepeatabilityTestId == repeatabilityTest.Id
                  && x.CapturedWeightKeyTypeId == (int)WeighingCalibrationCapturedWeightKeyType.CapturedWeightRepeatabilityWeight1Value)
                      .Select(x => new WMCalibrationStandardWeightDto { StandardWeightId = x.StandardWeightId.GetValueOrDefault(), UserEnteredStandardWeightId = x.UserEnteredStandardWeight })?.ToList();

                    calibrationDto.WeighingCalibrationRepeatabilityTestDto.lstWeightValue2StandardWeight = lstWeights.Where(x => x.WMCalibrationRepeatabilityTestId == repeatabilityTest.Id
                    && x.CapturedWeightKeyTypeId == (int)WeighingCalibrationCapturedWeightKeyType.CapturedWeightRepeatabilityWeight2Value)
                         .Select(x => new WMCalibrationStandardWeightDto { StandardWeightId = x.StandardWeightId.GetValueOrDefault(), UserEnteredStandardWeightId = x.UserEnteredStandardWeight })?.ToList();

                    calibrationDto.WeighingCalibrationRepeatabilityTestDto.lstWeightValue3StandardWeight = lstWeights.Where(x => x.WMCalibrationRepeatabilityTestId == repeatabilityTest.Id
                   && x.CapturedWeightKeyTypeId == (int)WeighingCalibrationCapturedWeightKeyType.CapturedWeightRepeatabilityWeight3Value)
                       .Select(x => new WMCalibrationStandardWeightDto { StandardWeightId = x.StandardWeightId.GetValueOrDefault(), UserEnteredStandardWeightId = x.UserEnteredStandardWeight })?.ToList();

                    calibrationDto.WeighingCalibrationRepeatabilityTestDto.lstWeightValue4StandardWeight = lstWeights.Where(x => x.WMCalibrationRepeatabilityTestId == repeatabilityTest.Id
                  && x.CapturedWeightKeyTypeId == (int)WeighingCalibrationCapturedWeightKeyType.CapturedWeightRepeatabilityWeight4Value)
                      .Select(x => new WMCalibrationStandardWeightDto { StandardWeightId = x.StandardWeightId.GetValueOrDefault(), UserEnteredStandardWeightId = x.UserEnteredStandardWeight })?.ToList();

                    calibrationDto.WeighingCalibrationRepeatabilityTestDto.lstWeightValue5StandardWeight = lstWeights.Where(x => x.WMCalibrationRepeatabilityTestId == repeatabilityTest.Id
                  && x.CapturedWeightKeyTypeId == (int)WeighingCalibrationCapturedWeightKeyType.CapturedWeightRepeatabilityWeight5Value)
                     .Select(x => new WMCalibrationStandardWeightDto { StandardWeightId = x.StandardWeightId.GetValueOrDefault(), UserEnteredStandardWeightId = x.UserEnteredStandardWeight })?.ToList();
                    calibrationDto.WeighingCalibrationRepeatabilityTestDto.lstWeightValue6StandardWeight = lstWeights.Where(x => x.WMCalibrationRepeatabilityTestId == repeatabilityTest.Id
                 && x.CapturedWeightKeyTypeId == (int)WeighingCalibrationCapturedWeightKeyType.CapturedWeightRepeatabilityWeight6Value)
                     .Select(x => new WMCalibrationStandardWeightDto { StandardWeightId = x.StandardWeightId.GetValueOrDefault(), UserEnteredStandardWeightId = x.UserEnteredStandardWeight })?.ToList();

                    calibrationDto.WeighingCalibrationRepeatabilityTestDto.lstWeightValue7StandardWeight = lstWeights.Where(x => x.WMCalibrationRepeatabilityTestId == repeatabilityTest.Id
                    && x.CapturedWeightKeyTypeId == (int)WeighingCalibrationCapturedWeightKeyType.CapturedWeightRepeatabilityWeight7Value)
                         .Select(x => new WMCalibrationStandardWeightDto { StandardWeightId = x.StandardWeightId.GetValueOrDefault(), UserEnteredStandardWeightId = x.UserEnteredStandardWeight })?.ToList();

                    calibrationDto.WeighingCalibrationRepeatabilityTestDto.lstWeightValue8StandardWeight = lstWeights.Where(x => x.WMCalibrationRepeatabilityTestId == repeatabilityTest.Id
                   && x.CapturedWeightKeyTypeId == (int)WeighingCalibrationCapturedWeightKeyType.CapturedWeightRepeatabilityWeight8Value)
                       .Select(x => new WMCalibrationStandardWeightDto { StandardWeightId = x.StandardWeightId.GetValueOrDefault(), UserEnteredStandardWeightId = x.UserEnteredStandardWeight })?.ToList();

                    calibrationDto.WeighingCalibrationRepeatabilityTestDto.lstWeightValue9StandardWeight = lstWeights.Where(x => x.WMCalibrationRepeatabilityTestId == repeatabilityTest.Id
                  && x.CapturedWeightKeyTypeId == (int)WeighingCalibrationCapturedWeightKeyType.CapturedWeightRepeatabilityWeight9Value)
                      .Select(x => new WMCalibrationStandardWeightDto { StandardWeightId = x.StandardWeightId.GetValueOrDefault(), UserEnteredStandardWeightId = x.UserEnteredStandardWeight })?.ToList();

                    calibrationDto.WeighingCalibrationRepeatabilityTestDto.lstWeightValue10StandardWeight = lstWeights.Where(x => x.WMCalibrationRepeatabilityTestId == repeatabilityTest.Id
                  && x.CapturedWeightKeyTypeId == (int)WeighingCalibrationCapturedWeightKeyType.CapturedWeightRepeatabilityWeight10Value)
                     .Select(x => new WMCalibrationStandardWeightDto { StandardWeightId = x.StandardWeightId.GetValueOrDefault(), UserEnteredStandardWeightId = x.UserEnteredStandardWeight })?.ToList();

                    calibrationDto.WeighingCalibrationRepeatabilityTestDto.UserEnteredWeightValue1StandardWeightBox = weighingCalibrationStandardWeightBoxes.FirstOrDefault(x => x.Id == repeatabilityTest.WeightValue1StandardWeightBoxId)?.StandardWeightBoxId;
                    calibrationDto.WeighingCalibrationRepeatabilityTestDto.UserEnteredWeightValue2StandardWeightBox = weighingCalibrationStandardWeightBoxes.FirstOrDefault(x => x.Id == repeatabilityTest.WeightValue2StandardWeightBoxId)?.StandardWeightBoxId;
                    calibrationDto.WeighingCalibrationRepeatabilityTestDto.UserEnteredWeightValue3StandardWeightBox = weighingCalibrationStandardWeightBoxes.FirstOrDefault(x => x.Id == repeatabilityTest.WeightValue3StandardWeightBoxId)?.StandardWeightBoxId;
                    calibrationDto.WeighingCalibrationRepeatabilityTestDto.UserEnteredWeightValue4StandardWeightBox = weighingCalibrationStandardWeightBoxes.FirstOrDefault(x => x.Id == repeatabilityTest.WeightValue4StandardWeightBoxId)?.StandardWeightBoxId;
                    calibrationDto.WeighingCalibrationRepeatabilityTestDto.UserEnteredWeightValue5StandardWeightBox = weighingCalibrationStandardWeightBoxes.FirstOrDefault(x => x.Id == repeatabilityTest.WeightValue5StandardWeightBoxId)?.StandardWeightBoxId;
                    calibrationDto.WeighingCalibrationRepeatabilityTestDto.UserEnteredWeightValue6StandardWeightBox = weighingCalibrationStandardWeightBoxes.FirstOrDefault(x => x.Id == repeatabilityTest.WeightValue6StandardWeightBoxId)?.StandardWeightBoxId;
                    calibrationDto.WeighingCalibrationRepeatabilityTestDto.UserEnteredWeightValue7StandardWeightBox = weighingCalibrationStandardWeightBoxes.FirstOrDefault(x => x.Id == repeatabilityTest.WeightValue7StandardWeightBoxId)?.StandardWeightBoxId;
                    calibrationDto.WeighingCalibrationRepeatabilityTestDto.UserEnteredWeightValue8StandardWeightBox = weighingCalibrationStandardWeightBoxes.FirstOrDefault(x => x.Id == repeatabilityTest.WeightValue8StandardWeightBoxId)?.StandardWeightBoxId;
                    calibrationDto.WeighingCalibrationRepeatabilityTestDto.UserEnteredWeightValue9StandardWeightBox = weighingCalibrationStandardWeightBoxes.FirstOrDefault(x => x.Id == repeatabilityTest.WeightValue9StandardWeightBoxId)?.StandardWeightBoxId;
                    calibrationDto.WeighingCalibrationRepeatabilityTestDto.UserEnteredWeightValue10StandardWeightBox = weighingCalibrationStandardWeightBoxes.FirstOrDefault(x => x.Id == repeatabilityTest.WeightValue10StandardWeightBoxId)?.StandardWeightBoxId;

                    calibrationDto.WeighingCalibrationRepeatabilityTestDto.TestResult = ((CalibrationTestStatus)calibrationDto.WeighingCalibrationRepeatabilityTestDto.TestResultId.GetValueOrDefault()).ToString();
                }
            }
            else
            {
                calibrationDto.WeighingCalibrationRepeatabilityTestDto = ObjectMapper.Map<WeighingCalibrationRepeatabilityTestDto>(commonCalibrationTestDto);
                calibrationDto.WeighingCalibrationRepeatabilityTestDto.WeighingMachineCode = String.Empty;
                calibrationDto.WeighingCalibrationRepeatabilityTestDto.CalculatedCapacityWeight = Double.Parse(commonCalibrationTestDto.CalculatedCapacityWeight);
            }

            //Get Uncertainity Test
            if (wmCalibrationHeader.WMCalibrationUncertainityTests?.Count > 0)
            {
                foreach (var uncertainityTest in wmCalibrationHeader.WMCalibrationUncertainityTests)
                {
                    calibrationDto.UncertainityTestId = uncertainityTest.Id;
                    calibrationDto.UncertainityTestResultId = uncertainityTest.TestResultId;
                    calibrationDto.UncertainityValue = uncertainityTest.UncertainityValue;
                    //calibrationDto.UncertainityInstruction = uncertainityTest.UncertainityInstruction;
                    calibrationDto.UncertainityTestResult = ((CalibrationTestStatus)uncertainityTest.TestResultId.GetValueOrDefault()).ToString();
                }
            }
            return calibrationDto;
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.WeighingCalibration_SubModule + "." + PMMSPermissionConst.View)]
        public async Task<List<SelectListDto>> GetWeighingMachineSelectListAutoComplete(string weighingMachineCode)
        {
            var plantId = _httpContextAccessor.HttpContext.Request.Headers["PlantId"].FirstOrDefault();
            if (!(string.IsNullOrEmpty(weighingMachineCode) || string.IsNullOrWhiteSpace(weighingMachineCode)))
            {
                weighingMachineCode = weighingMachineCode.Trim();
                int approvalStatudId = (int)ApprovalStatus.Approved;
                var weighingMachineQuery = _weighingMachineRepository.GetAll().Where(x => x.WeighingMachineCode == weighingMachineCode && x.ApprovalStatusId == approvalStatudId);
                if (!(string.IsNullOrEmpty(plantId) || string.IsNullOrWhiteSpace(plantId)))
                {
                    weighingMachineQuery.Where(x => x.SubPlantId == Convert.ToInt32(plantId));
                }
                return await weighingMachineQuery.Select(x => new SelectListDto { Id = x.Id, Value = x.WeighingMachineCode }).ToListAsync() ?? default;
            }
            return default;
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.WeighingCalibration_SubModule + "." + PMMSPermissionConst.View)]
        public async Task<List<SelectListDto>> GetAllStandardWeightBoxAutoComplete(string standardWeightBoxId)
        {
            var plantId = _httpContextAccessor.HttpContext.Request.Headers["PlantId"].FirstOrDefault();
            if (!(string.IsNullOrEmpty(standardWeightBoxId) || string.IsNullOrWhiteSpace(standardWeightBoxId)))
            {
                standardWeightBoxId = standardWeightBoxId.Trim();
                int approvalStatudId = (int)ApprovalStatus.Approved;
                var standardWeightBoxQuery = _standardWeightBoxRepository.GetAll().Where(x => x.StandardWeightBoxId == standardWeightBoxId && x.ApprovalStatusId == approvalStatudId);
                if (!(string.IsNullOrEmpty(plantId) || string.IsNullOrWhiteSpace(plantId)))
                {
                    standardWeightBoxQuery = standardWeightBoxQuery.Where(x => x.SubPlantId == Convert.ToInt32(plantId));
                }
                return await standardWeightBoxQuery.OrderBy(x => x.StandardWeightBoxId)
                        .Select(x => new SelectListDto { Id = x.Id, Value = x.StandardWeightBoxId })?
                        .ToListAsync() ?? default;
            }
            return default;
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.WeighingCalibration_SubModule + "." + PMMSPermissionConst.View)]
        public async Task<List<SelectListDto>> GetAllStandardWeightByBoxId(int standardWeightBoxId)
        {
            var plantId = _httpContextAccessor.HttpContext.Request.Headers["PlantId"].FirstOrDefault();
            if (standardWeightBoxId > 0)
            {
                int approvalStatudId = (int)ApprovalStatus.Approved;
                var standardWeightQuery = from standardweight in _standardWeightRepository.GetAll().Where(x => x.StandardWeightBoxMasterId == standardWeightBoxId && x.ApprovalStatusId == approvalStatudId)
                                          join unitOfMeasurement in _unitOfMeasurementRepository.GetAll()
                                          on standardweight.UnitOfMeasurementId equals unitOfMeasurement.Id into unitStandard
                                          from unitOfMeasurement in unitStandard.DefaultIfEmpty()
                                          select new { standardweight.Id, standardweight.SubPlantId, standardweight.StandardWeightId, standardweight.CapacityinDecimal, unitOfMeasurement.UnitOfMeasurement };

                if (!(string.IsNullOrEmpty(plantId) || string.IsNullOrWhiteSpace(plantId)))
                {
                    standardWeightQuery = standardWeightQuery.Where(x => x.SubPlantId == Convert.ToInt32(plantId));
                }
                return await standardWeightQuery.OrderBy(x => x.StandardWeightId)
                        .Select(x => new SelectListDto { Id = x.Id, Value = x.StandardWeightId + " - " + x.CapacityinDecimal + " " + (x.UnitOfMeasurement == null ? "" : x.UnitOfMeasurement) })
                        .ToListAsync() ?? default;
            }
            return default;
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.WeighingCalibration_SubModule + "." + PMMSPermissionConst.View)]
        public async Task<PagedResultDto<WeighingCalibrationListDto>> GetAllAsync(PagedWeighingCalibrationResultRequestDto input)
        {
            var query = CreateUserListFilteredQuery(input);

            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            return new PagedResultDto<WeighingCalibrationListDto>(
                totalCount,
                entities
            );
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.WeighingCalibration_SubModule + "." + PMMSPermissionConst.Add)]
        public async Task<CreateWeighingCalibrationResultDto> CreateAsync(CreateWeighingCalibrationDto input)
        {
            WMCalibrationHeader wmHeader = ObjectMapper.Map<WMCalibrationHeader>(input);
            wmHeader.CalibrationFrequencyId = input.FrequencyModeld;

            if (input.WeighingCalibrationCheckpoints?.Count > 0)
            {
                wmHeader.WMCalibrationCheckpoints = new List<WMCalibrationCheckpoint>();
                foreach (var checkpoint in input.WeighingCalibrationCheckpoints)
                {
                    WMCalibrationCheckpoint checkpointToInsert = new WMCalibrationCheckpoint
                    {
                        CheckPointId = checkpoint.CheckPointId,
                        DiscrepancyRemark = checkpoint.DiscrepancyRemark,
                        Observation = checkpoint.Observation
                    };
                    wmHeader.WMCalibrationCheckpoints.Add(checkpointToInsert);
                }
            }

            wmHeader.CalibrationTestDate = DateTime.UtcNow;
            wmHeader.CalibrationStatusId = IsCheckpointsValid(input) ? (int)CalibrationStatus.In_Progress : (int)CalibrationStatus.Not_Calibrated;
            var headerid = await _wmCalibrationHeaderRepository.InsertAndGetIdAsync(wmHeader);
            return new CreateWeighingCalibrationResultDto { Id = headerid, CalibrationStatusId = wmHeader.CalibrationStatusId.GetValueOrDefault() };
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.WeighingCalibration_SubModule + "." + PMMSPermissionConst.Edit)]
        public async Task<WeighingCalibrationDto> UpdateAsync(WeighingCalibrationDto input)
        {
            var header = await _wmCalibrationHeaderRepository.GetAsync(input.Id);
            //Insert Weighing Calibration Detail for current calibration level selected
            if (input.WeighingCalibrationSaveType == (int)WeighingCalibrationStepType.WeighingCalibrationDetails)
            {
                var wmCalibrationDetailToInsert = ObjectMapper.Map<WMCalibrationDetail>(input.WeighingCalibrationDetailCurrentDto);
                wmCalibrationDetailToInsert.WMCalibrationHeaderId = input.Id;
                wmCalibrationDetailToInsert.WMCalibrationDetailWeights = new List<WMCalibrationDetailWeight>();
                if (input.WeighingCalibrationDetailCurrentDto.lstWeightId?.Count > 0)
                {
                    foreach (var weightId in input.WeighingCalibrationDetailCurrentDto.lstWeightId)
                    {
                        WMCalibrationDetailWeight weight = new WMCalibrationDetailWeight
                        {
                            StandardWeightId = weightId,
                            KeyTypeId = (int)WeighingCalibrationStepType.WeighingCalibrationDetails,
                            CapturedWeightKeyTypeId = (int)WeighingCalibrationCapturedWeightKeyType.CapturedWeightValueCalibration
                        };
                        wmCalibrationDetailToInsert.WMCalibrationDetailWeights.Add(weight);
                    }
                }
                input.WeighingCalibrationDetailCurrentDto.WeighingMachineId = input.WeighingMachineId;

                await _wmCalibrationDetailsRepository.InsertAndGetIdAsync(wmCalibrationDetailToInsert);
                //Refresh List and Get Next Calibration if Exist

                if (input.WeighingCalibrationDetailCurrentDto.CalibrationStatusId != (int)CalibrationStatus.Calibrated)
                {
                    input.CalibrationStatusId = (int)CalibrationStatus.Not_Calibrated;
                }

                await CurrentUnitOfWork.SaveChangesAsync();
                await UpdateCalibrationDetailToCalibrationDto(input);
            }
            else if (input.WeighingCalibrationSaveType == (int)WeighingCalibrationStepType.EccentricityTest)
            {
                //Insert Weighing Calibration Eccentricity Test data
                var wmCalibrationEccentricityTestToInsert = ObjectMapper.Map<WMCalibrationEccentricityTest>(input.WeighingCalibrationEccentricityTestDto);
                wmCalibrationEccentricityTestToInsert.WMCalibrationHeaderId = input.Id;
                wmCalibrationEccentricityTestToInsert.WMCalibrationDetailWeights = new List<WMCalibrationDetailWeight>();

                if (input.WeighingCalibrationEccentricityTestDto.lstCValueStandardWeight?.Count > 0)
                {
                    foreach (var cValueStandardWeight in input.WeighingCalibrationEccentricityTestDto.lstCValueStandardWeight)
                    {
                        WMCalibrationDetailWeight weight = new WMCalibrationDetailWeight
                        {
                            StandardWeightId = cValueStandardWeight.StandardWeightId,
                            KeyTypeId = (int)WeighingCalibrationStepType.EccentricityTest,
                            CapturedWeightKeyTypeId = (int)WeighingCalibrationCapturedWeightKeyType.CapturedWeightEccentricityCValue
                        };
                        wmCalibrationEccentricityTestToInsert.WMCalibrationDetailWeights.Add(weight);
                    }
                }
                if (input.WeighingCalibrationEccentricityTestDto.lstLFValueStandardWeight?.Count > 0)
                {
                    foreach (var lfValueStandardWeight in input.WeighingCalibrationEccentricityTestDto.lstLFValueStandardWeight)
                    {
                        WMCalibrationDetailWeight weight = new WMCalibrationDetailWeight
                        {
                            StandardWeightId = lfValueStandardWeight.StandardWeightId,
                            KeyTypeId = (int)WeighingCalibrationStepType.EccentricityTest,
                            CapturedWeightKeyTypeId = (int)WeighingCalibrationCapturedWeightKeyType.CapturedWeightEccentricityLFValue
                        };
                        wmCalibrationEccentricityTestToInsert.WMCalibrationDetailWeights.Add(weight);
                    }
                }
                if (input.WeighingCalibrationEccentricityTestDto.lstLBValueStandardWeight?.Count > 0)
                {
                    foreach (var lbValueStandardWeight in input.WeighingCalibrationEccentricityTestDto.lstLBValueStandardWeight)
                    {
                        WMCalibrationDetailWeight weight = new WMCalibrationDetailWeight
                        {
                            StandardWeightId = lbValueStandardWeight.StandardWeightId,
                            KeyTypeId = (int)WeighingCalibrationStepType.EccentricityTest,
                            CapturedWeightKeyTypeId = (int)WeighingCalibrationCapturedWeightKeyType.CapturedWeightEccentricityLBValue
                        };
                        wmCalibrationEccentricityTestToInsert.WMCalibrationDetailWeights.Add(weight);
                    }
                }
                if (input.WeighingCalibrationEccentricityTestDto.lstRFValueStandardWeight?.Count > 0)
                {
                    foreach (var rfValueStandardWeight in input.WeighingCalibrationEccentricityTestDto.lstRFValueStandardWeight)
                    {
                        WMCalibrationDetailWeight weight = new WMCalibrationDetailWeight
                        {
                            StandardWeightId = rfValueStandardWeight.StandardWeightId,
                            KeyTypeId = (int)WeighingCalibrationStepType.EccentricityTest,
                            CapturedWeightKeyTypeId = (int)WeighingCalibrationCapturedWeightKeyType.CapturedWeightEccentricityRFValue
                        };
                        wmCalibrationEccentricityTestToInsert.WMCalibrationDetailWeights.Add(weight);
                    }
                }
                if (input.WeighingCalibrationEccentricityTestDto.lstRBValueStandardWeight?.Count > 0)
                {
                    foreach (var rbValueStandardWeight in input.WeighingCalibrationEccentricityTestDto.lstRBValueStandardWeight)
                    {
                        WMCalibrationDetailWeight weight = new WMCalibrationDetailWeight
                        {
                            StandardWeightId = rbValueStandardWeight.StandardWeightId,
                            KeyTypeId = (int)WeighingCalibrationStepType.EccentricityTest,
                            CapturedWeightKeyTypeId = (int)WeighingCalibrationCapturedWeightKeyType.CapturedWeightEccentricityRBValue
                        };
                        wmCalibrationEccentricityTestToInsert.WMCalibrationDetailWeights.Add(weight);
                    }
                }
                input.WeighingCalibrationEccentricityTestDto.Id = await _wmCalibrationEccentricityTestRepository.InsertAndGetIdAsync(wmCalibrationEccentricityTestToInsert);

                if (input.WeighingCalibrationEccentricityTestDto.TestResultId != (int)CalibrationTestStatus.Passed)
                {
                    input.CalibrationStatusId = (int)CalibrationStatus.Not_Calibrated;
                }
            }
            else if (input.WeighingCalibrationSaveType == (int)WeighingCalibrationStepType.LinearityTest)
            {
                //Insert Weighing Calibration Linearity Test data
                var wmCalibrationLinearityTestToInsert = ObjectMapper.Map<WMCalibrationLinearityTest>(input.WeighingCalibrationLinearityTestDto);
                wmCalibrationLinearityTestToInsert.WMCalibrationHeaderId = input.Id;
                wmCalibrationLinearityTestToInsert.WMCalibrationDetailWeights = new List<WMCalibrationDetailWeight>();

                if (input.WeighingCalibrationLinearityTestDto.lstWeightValue1StandardWeight?.Count > 0)
                {
                    foreach (var weight1Value in input.WeighingCalibrationLinearityTestDto.lstWeightValue1StandardWeight)
                    {
                        WMCalibrationDetailWeight weight = new WMCalibrationDetailWeight
                        {
                            StandardWeightId = weight1Value.StandardWeightId,
                            KeyTypeId = (int)WeighingCalibrationStepType.LinearityTest,
                            CapturedWeightKeyTypeId = (int)WeighingCalibrationCapturedWeightKeyType.CapturedWeightLinearityWeight1Value
                        };
                        wmCalibrationLinearityTestToInsert.WMCalibrationDetailWeights.Add(weight);
                    }
                }
                if (input.WeighingCalibrationLinearityTestDto.lstWeightValue2StandardWeight?.Count > 0)
                {
                    foreach (var weight2Value in input.WeighingCalibrationLinearityTestDto.lstWeightValue2StandardWeight)
                    {
                        WMCalibrationDetailWeight weight = new WMCalibrationDetailWeight
                        {
                            StandardWeightId = weight2Value.StandardWeightId,
                            KeyTypeId = (int)WeighingCalibrationStepType.LinearityTest,
                            CapturedWeightKeyTypeId = (int)WeighingCalibrationCapturedWeightKeyType.CapturedWeightLinearityWeight2Value
                        };
                        wmCalibrationLinearityTestToInsert.WMCalibrationDetailWeights.Add(weight);
                    }
                }
                if (input.WeighingCalibrationLinearityTestDto.lstWeightValue3StandardWeight?.Count > 0)
                {
                    foreach (var weight3Value in input.WeighingCalibrationLinearityTestDto.lstWeightValue3StandardWeight)
                    {
                        WMCalibrationDetailWeight weight = new WMCalibrationDetailWeight
                        {
                            StandardWeightId = weight3Value.StandardWeightId,
                            KeyTypeId = (int)WeighingCalibrationStepType.LinearityTest,
                            CapturedWeightKeyTypeId = (int)WeighingCalibrationCapturedWeightKeyType.CapturedWeightLinearityWeight3Value
                        };
                        wmCalibrationLinearityTestToInsert.WMCalibrationDetailWeights.Add(weight);
                    }
                }
                if (input.WeighingCalibrationLinearityTestDto.lstWeightValue4StandardWeight?.Count > 0)
                {
                    foreach (var weight4Value in input.WeighingCalibrationLinearityTestDto.lstWeightValue4StandardWeight)
                    {
                        WMCalibrationDetailWeight weight = new WMCalibrationDetailWeight
                        {
                            StandardWeightId = weight4Value.StandardWeightId,
                            KeyTypeId = (int)WeighingCalibrationStepType.LinearityTest,
                            CapturedWeightKeyTypeId = (int)WeighingCalibrationCapturedWeightKeyType.CapturedWeightLinearityWeight4Value
                        };
                        wmCalibrationLinearityTestToInsert.WMCalibrationDetailWeights.Add(weight);
                    }
                }
                if (input.WeighingCalibrationLinearityTestDto.lstWeightValue5StandardWeight?.Count > 0)
                {
                    foreach (var weight5Value in input.WeighingCalibrationLinearityTestDto.lstWeightValue5StandardWeight)
                    {
                        WMCalibrationDetailWeight weight = new WMCalibrationDetailWeight
                        {
                            StandardWeightId = weight5Value.StandardWeightId,
                            KeyTypeId = (int)WeighingCalibrationStepType.LinearityTest,
                            CapturedWeightKeyTypeId = (int)WeighingCalibrationCapturedWeightKeyType.CapturedWeightLinearityWeight5Value
                        };
                        wmCalibrationLinearityTestToInsert.WMCalibrationDetailWeights.Add(weight);
                    }
                }

                input.WeighingCalibrationLinearityTestDto.Id = await _wmCalibrationLinearityTestRepository.InsertAndGetIdAsync(wmCalibrationLinearityTestToInsert);

                if (input.WeighingCalibrationLinearityTestDto.TestResultId != (int)CalibrationTestStatus.Passed)
                {
                    input.CalibrationStatusId = (int)CalibrationStatus.Not_Calibrated;
                }
            }
            else if (input.WeighingCalibrationSaveType == (int)WeighingCalibrationStepType.RepeatabilityTest)
            {
                //Insert Weighing Calibration Repeatability Test data
                var wmCalibrationRepeatabilityTestToInsert = ObjectMapper.Map<WMCalibrationRepeatabilityTest>(input.WeighingCalibrationRepeatabilityTestDto);
                wmCalibrationRepeatabilityTestToInsert.WMCalibrationHeaderId = input.Id;
                wmCalibrationRepeatabilityTestToInsert.WMCalibrationDetailWeights = new List<WMCalibrationDetailWeight>();

                if (input.WeighingCalibrationRepeatabilityTestDto.lstWeightValue1StandardWeight?.Count > 0)
                {
                    foreach (var weight1Value in input.WeighingCalibrationRepeatabilityTestDto.lstWeightValue1StandardWeight)
                    {
                        WMCalibrationDetailWeight weight = new WMCalibrationDetailWeight
                        {
                            StandardWeightId = weight1Value.StandardWeightId,
                            KeyTypeId = (int)WeighingCalibrationStepType.RepeatabilityTest,
                            CapturedWeightKeyTypeId = (int)WeighingCalibrationCapturedWeightKeyType.CapturedWeightRepeatabilityWeight1Value
                        };
                        wmCalibrationRepeatabilityTestToInsert.WMCalibrationDetailWeights.Add(weight);
                    }
                }
                if (input.WeighingCalibrationRepeatabilityTestDto.lstWeightValue2StandardWeight?.Count > 0)
                {
                    foreach (var weight2Value in input.WeighingCalibrationRepeatabilityTestDto.lstWeightValue2StandardWeight)
                    {
                        WMCalibrationDetailWeight weight = new WMCalibrationDetailWeight
                        {
                            StandardWeightId = weight2Value.StandardWeightId,
                            KeyTypeId = (int)WeighingCalibrationStepType.RepeatabilityTest,
                            CapturedWeightKeyTypeId = (int)WeighingCalibrationCapturedWeightKeyType.CapturedWeightRepeatabilityWeight2Value
                        };
                        wmCalibrationRepeatabilityTestToInsert.WMCalibrationDetailWeights.Add(weight);
                    }
                }
                if (input.WeighingCalibrationRepeatabilityTestDto.lstWeightValue3StandardWeight?.Count > 0)
                {
                    foreach (var weight3Value in input.WeighingCalibrationRepeatabilityTestDto.lstWeightValue3StandardWeight)
                    {
                        WMCalibrationDetailWeight weight = new WMCalibrationDetailWeight
                        {
                            StandardWeightId = weight3Value.StandardWeightId,
                            KeyTypeId = (int)WeighingCalibrationStepType.RepeatabilityTest,
                            CapturedWeightKeyTypeId = (int)WeighingCalibrationCapturedWeightKeyType.CapturedWeightRepeatabilityWeight3Value
                        };
                        wmCalibrationRepeatabilityTestToInsert.WMCalibrationDetailWeights.Add(weight);
                    }
                }
                if (input.WeighingCalibrationRepeatabilityTestDto.lstWeightValue4StandardWeight?.Count > 0)
                {
                    foreach (var weight4Value in input.WeighingCalibrationRepeatabilityTestDto.lstWeightValue4StandardWeight)
                    {
                        WMCalibrationDetailWeight weight = new WMCalibrationDetailWeight
                        {
                            StandardWeightId = weight4Value.StandardWeightId,
                            KeyTypeId = (int)WeighingCalibrationStepType.RepeatabilityTest,
                            CapturedWeightKeyTypeId = (int)WeighingCalibrationCapturedWeightKeyType.CapturedWeightRepeatabilityWeight4Value
                        };
                        wmCalibrationRepeatabilityTestToInsert.WMCalibrationDetailWeights.Add(weight);
                    }
                }
                if (input.WeighingCalibrationRepeatabilityTestDto.lstWeightValue5StandardWeight?.Count > 0)
                {
                    foreach (var weight5Value in input.WeighingCalibrationRepeatabilityTestDto.lstWeightValue5StandardWeight)
                    {
                        WMCalibrationDetailWeight weight = new WMCalibrationDetailWeight
                        {
                            StandardWeightId = weight5Value.StandardWeightId,
                            KeyTypeId = (int)WeighingCalibrationStepType.RepeatabilityTest,
                            CapturedWeightKeyTypeId = (int)WeighingCalibrationCapturedWeightKeyType.CapturedWeightRepeatabilityWeight5Value
                        };
                        wmCalibrationRepeatabilityTestToInsert.WMCalibrationDetailWeights.Add(weight);
                    }
                }
                if (input.WeighingCalibrationRepeatabilityTestDto.lstWeightValue6StandardWeight?.Count > 0)
                {
                    foreach (var weight6Value in input.WeighingCalibrationRepeatabilityTestDto.lstWeightValue6StandardWeight)
                    {
                        WMCalibrationDetailWeight weight = new WMCalibrationDetailWeight
                        {
                            StandardWeightId = weight6Value.StandardWeightId,
                            KeyTypeId = (int)WeighingCalibrationStepType.RepeatabilityTest,
                            CapturedWeightKeyTypeId = (int)WeighingCalibrationCapturedWeightKeyType.CapturedWeightRepeatabilityWeight6Value
                        };
                        wmCalibrationRepeatabilityTestToInsert.WMCalibrationDetailWeights.Add(weight);
                    }
                }
                if (input.WeighingCalibrationRepeatabilityTestDto.lstWeightValue7StandardWeight?.Count > 0)
                {
                    foreach (var weight7Value in input.WeighingCalibrationRepeatabilityTestDto.lstWeightValue7StandardWeight)
                    {
                        WMCalibrationDetailWeight weight = new WMCalibrationDetailWeight
                        {
                            StandardWeightId = weight7Value.StandardWeightId,
                            KeyTypeId = (int)WeighingCalibrationStepType.RepeatabilityTest,
                            CapturedWeightKeyTypeId = (int)WeighingCalibrationCapturedWeightKeyType.CapturedWeightRepeatabilityWeight7Value
                        };
                        wmCalibrationRepeatabilityTestToInsert.WMCalibrationDetailWeights.Add(weight);
                    }
                }
                if (input.WeighingCalibrationRepeatabilityTestDto.lstWeightValue8StandardWeight?.Count > 0)
                {
                    foreach (var weight8Value in input.WeighingCalibrationRepeatabilityTestDto.lstWeightValue8StandardWeight)
                    {
                        WMCalibrationDetailWeight weight = new WMCalibrationDetailWeight
                        {
                            StandardWeightId = weight8Value.StandardWeightId,
                            KeyTypeId = (int)WeighingCalibrationStepType.RepeatabilityTest,
                            CapturedWeightKeyTypeId = (int)WeighingCalibrationCapturedWeightKeyType.CapturedWeightRepeatabilityWeight8Value
                        };
                        wmCalibrationRepeatabilityTestToInsert.WMCalibrationDetailWeights.Add(weight);
                    }
                }
                if (input.WeighingCalibrationRepeatabilityTestDto.lstWeightValue9StandardWeight?.Count > 0)
                {
                    foreach (var weight9Value in input.WeighingCalibrationRepeatabilityTestDto.lstWeightValue9StandardWeight)
                    {
                        WMCalibrationDetailWeight weight = new WMCalibrationDetailWeight
                        {
                            StandardWeightId = weight9Value.StandardWeightId,
                            KeyTypeId = (int)WeighingCalibrationStepType.RepeatabilityTest,
                            CapturedWeightKeyTypeId = (int)WeighingCalibrationCapturedWeightKeyType.CapturedWeightRepeatabilityWeight9Value
                        };
                        wmCalibrationRepeatabilityTestToInsert.WMCalibrationDetailWeights.Add(weight);
                    }
                }
                if (input.WeighingCalibrationRepeatabilityTestDto.lstWeightValue10StandardWeight?.Count > 0)
                {
                    foreach (var weight10Value in input.WeighingCalibrationRepeatabilityTestDto.lstWeightValue10StandardWeight)
                    {
                        WMCalibrationDetailWeight weight = new WMCalibrationDetailWeight
                        {
                            StandardWeightId = weight10Value.StandardWeightId,
                            KeyTypeId = (int)WeighingCalibrationStepType.RepeatabilityTest,
                            CapturedWeightKeyTypeId = (int)WeighingCalibrationCapturedWeightKeyType.CapturedWeightRepeatabilityWeight10Value
                        };
                        wmCalibrationRepeatabilityTestToInsert.WMCalibrationDetailWeights.Add(weight);
                    }
                }

                input.WeighingCalibrationRepeatabilityTestDto.Id = await _wmCalibrationRepeatabilityTestRepository.InsertAndGetIdAsync(wmCalibrationRepeatabilityTestToInsert);

                if (input.WeighingCalibrationRepeatabilityTestDto.TestResultId != (int)CalibrationTestStatus.Passed)
                {
                    input.CalibrationStatusId = (int)CalibrationStatus.Not_Calibrated;
                }
            }
            else if (input.WeighingCalibrationSaveType == (int)WeighingCalibrationStepType.UncertainityTest)
            {
                //Insert Weighing Calibration Uncertainity Test Result
                var uncertainityTestToInsert = new WMCalibrationUncertainityTest
                {
                    WMCalibrationHeaderId = input.Id,
                    UncertainityValue = input.UncertainityValue,
                    TestResultId = input.UncertainityTestResultId
                };
                input.UncertainityTestId = await _wmCalibrationUncertainityTestRepository.InsertAndGetIdAsync(uncertainityTestToInsert);

                if (input.UncertainityTestResultId != (int)CalibrationTestStatus.Passed)
                {
                    input.CalibrationStatusId = (int)CalibrationStatus.Not_Calibrated;
                }
            }
            if (input.CalibrationStatusId == (int)CalibrationStatus.In_Progress)
            {
                input.CalibrationStatusId = (int)IsWeighingMachineCalibrated(input);
                if (input.CalibrationStatusId == (int)CalibrationStatus.Calibrated)
                {
                    await SaveLatestCalibratedWeighingMachine(input);
                }
            }
            header.CalibrationStatusId = input.CalibrationStatusId;
            await _wmCalibrationHeaderRepository.UpdateAsync(header);
            return input;
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.WeighingCalibration_SubModule + "." + PMMSPermissionConst.Edit)]
        public async Task<double> GetWeightByWeighingMachineCode(int weighingMachineId, double? testWeight = null)
        {
            var ipAddressPort = await _weighingMachineRepository.GetAll().Where(x => x.Id == weighingMachineId).Select(x => new { x.IPAddress, x.PortNumber }).FirstOrDefaultAsync();
            if (ipAddressPort != null)
            {
                //if (testWeight != null)
                //{
                //    return testWeight.GetValueOrDefault();
                //}
                //else 
                if (_configuration.GetValue<bool>(PMMSConsts.IsWeighigMachineEnabled))
                {
                    return await _inwardAppService.GetWeightFromWeighingMachine(ipAddressPort.IPAddress, ipAddressPort.PortNumber);// Get value from Weighing Machine
                }
                //return 0.2787878676;
            }
            return default;
        }

        /// <summary>
        /// This method will check the weighing machine is calibrated or not.
        /// </summary>
        /// <param name="input">WeighingMachineId.</param>
        public async Task<bool> IsWeighingMachineCalibrated(int? WeighingMachineId)
        {
            if (WeighingMachineId != null)
            {
                return await _wmcalibratedMachineRepository.GetAll().AnyAsync(x => x.WeighingMachineId == WeighingMachineId && x.LastCalibrationTestDate.Date == DateTime.UtcNow.Date);
            }
            return false;
        }

        private async Task UpdateCalibrationDetailToCalibrationDto(WeighingCalibrationDto calibrationDto)
        {
            var weighingCalibrationStandardWeightBoxes = await _standardWeightBoxRepository.GetAll().Select(x => new { x.Id, x.StandardWeightBoxId }).AsNoTracking().ToListAsync();
            var WMCalibrationDetails = await _wmCalibrationDetailsRepository.GetAll().Where(x => x.WMCalibrationHeaderId == calibrationDto.Id).ToListAsync();
            var commonCalibrationTestDto = await GetCommonTestDto(calibrationDto.WeighingMachineId.GetValueOrDefault(), calibrationDto.FrequencyModeld);
            if (WMCalibrationDetails?.Count > 0)
            {
                var lstDetailId = WMCalibrationDetails.Select(x => x.Id);
                var lstWeights = await (from calibrationStandardWeight in _calibrationStandardWeightRepository.GetAll()
                                        join standardWeight in _standardWeightRepository.GetAll()
                                        on calibrationStandardWeight.StandardWeightId equals standardWeight.Id
                                        join unitOfMeasurement in _unitOfMeasurementRepository.GetAll()
                                        on standardWeight.UnitOfMeasurementId equals unitOfMeasurement.Id
                                        where lstDetailId.Contains(calibrationStandardWeight.WMCalibrationDetailId.GetValueOrDefault())
                                        select new WMCalibrationInternalStandardWeightDto
                                        {
                                            WMCalibrationDetailTestId = calibrationStandardWeight.WMCalibrationDetailId,
                                            StandardWeightId = calibrationStandardWeight.StandardWeightId,
                                            CapturedWeightKeyTypeId = calibrationStandardWeight.CapturedWeightKeyTypeId,
                                            UserEnteredStandardWeight = standardWeight.StandardWeightId + " - " + standardWeight.Capacity + " " + unitOfMeasurement.UnitOfMeasurement
                                        }).AsNoTracking().ToListAsync() ?? default;

                calibrationDto.lstWeighingCalibrationDetailDto = new List<WeighingCalibrationDetailDto>();
                foreach (var calibrationDetail in WMCalibrationDetails)
                {
                    WeighingCalibrationDetailDto detail = ObjectMapper.Map<WeighingCalibrationDetailDto>(calibrationDetail);
                    var currentCalibration = commonCalibrationTestDto.lstCalibrations.First(x => x.CalibrationLevelId == detail.CalibrationLevelId);
                    detail.lstWeightId = lstWeights.Where(x => x.WMCalibrationDetailTestId == detail.Id)
                        .Select(x => x.StandardWeightId.GetValueOrDefault())?.ToList();
                    detail.UserEnteredWeightId = String.Join(",", lstWeights.Where(x => x.WMCalibrationDetailTestId == detail.Id)
                      .Select(x => x.UserEnteredStandardWeight)?.ToList());
                    detail.WeighingMachineId = calibrationDto.WeighingMachineId;
                    ObjectMapper.Map(currentCalibration, detail);
                    var standardWeightBox = weighingCalibrationStandardWeightBoxes.FirstOrDefault(x => x.Id == detail.StandardWeightBoxId);
                    if (standardWeightBox != null)
                    {
                        detail.UserEnteredWeightBox = standardWeightBox.StandardWeightBoxId;
                    }
                    calibrationDto.lstWeighingCalibrationDetailDto.Add(detail);
                }
            }
            calibrationDto.WeighingCalibrationDetailCurrentDto = new WeighingCalibrationDetailDto();
            var calibrationsNotDone = commonCalibrationTestDto.lstCalibrations.Select(x => x.CalibrationLevelId);
            if (calibrationDto.lstWeighingCalibrationDetailDto != null && calibrationDto.lstWeighingCalibrationDetailDto.Count > 0)
            {
                calibrationsNotDone = commonCalibrationTestDto.lstCalibrations.Select(x => x.CalibrationLevelId)
              .Except(calibrationDto.lstWeighingCalibrationDetailDto.Select(x => x.CalibrationLevelId.GetValueOrDefault()));
            }

            if (calibrationsNotDone?.Count() > 0)
            {
                //Make Calibration First
                var currentCalibration = commonCalibrationTestDto.lstCalibrations.First(x => x.CalibrationLevelId == calibrationsNotDone.First());
                ObjectMapper.Map(currentCalibration, calibrationDto.WeighingCalibrationDetailCurrentDto);
            }
            else
            {
                //Mark IsCalibrationFinished
                calibrationDto.IsAllCalibrationLevelFinished = true;
            }
        }

        private async Task<WeighingCalibrationDto> GetWeighingCalibrationDtoByWeighingMachineId(int weighingMachineId)
        {
            var calibrationDto = await (from weighingMachine in _weighingMachineRepository.GetAllIncluding(x => x.Calibrations)
                                        join unitOfMeasurement in _unitOfMeasurementRepository.GetAll()
                                        on weighingMachine.UnitOfMeasurementId equals unitOfMeasurement.Id
                                        where weighingMachine.Id == weighingMachineId
                                        select new WeighingCalibrationDto
                                        {
                                            WeighingMachineId = weighingMachine.Id,
                                            WeighingMachineCode = weighingMachine.WeighingMachineCode,
                                            Make = weighingMachine.Make,
                                            Model = weighingMachine.Modal,

                                            UnitOfMeasurement = weighingMachine.UnitOfMeasurementId,
                                            UserEnteredUnitOfMeasurement = unitOfMeasurement.UnitOfMeasurement,
                                            Capacity = weighingMachine.Capacity,
                                            LeastCount = weighingMachine.LeastCount,
                                            LeastCountDigitAfterDecimal = weighingMachine.LeastCountDigitAfterDecimal,
                                            FormatNo = weighingMachine.FormatNo,
                                            RefrenceSOPNo = weighingMachine.RefrenceSOPNo,
                                            Version = weighingMachine.Version,
                                            LinearityAcceptanceMaxValueWg1 = weighingMachine.LinearityAcceptanceMaxValueWg1,
                                            LinearityAcceptanceMaxValueWg2 = weighingMachine.LinearityAcceptanceMaxValueWg2,
                                            LinearityAcceptanceMaxValueWg3 = weighingMachine.LinearityAcceptanceMaxValueWg3,
                                            LinearityAcceptanceMaxValueWg4 = weighingMachine.LinearityAcceptanceMaxValueWg4,
                                            LinearityAcceptanceMaxValueWg5 = weighingMachine.LinearityAcceptanceMaxValueWg5,

                                            LinearityAcceptanceMinValueWg1 = weighingMachine.LinearityAcceptanceMinValueWg1,
                                            LinearityAcceptanceMinValueWg2 = weighingMachine.LinearityAcceptanceMinValueWg2,
                                            LinearityAcceptanceMinValueWg3 = weighingMachine.LinearityAcceptanceMinValueWg3,
                                            LinearityAcceptanceMinValueWg4 = weighingMachine.LinearityAcceptanceMinValueWg4,
                                            LinearityAcceptanceMinValueWg5 = weighingMachine.LinearityAcceptanceMinValueWg5,

                                            LinearityAcceptanceValueWg1 = weighingMachine.LinearityAcceptanceValueWg1,
                                            LinearityAcceptanceValueWg2 = weighingMachine.LinearityAcceptanceValueWg2,
                                            LinearityAcceptanceValueWg3 = weighingMachine.LinearityAcceptanceValueWg3,
                                            LinearityAcceptanceValueWg4 = weighingMachine.LinearityAcceptanceValueWg4,
                                            LinearityAcceptanceValueWg5 = weighingMachine.LinearityAcceptanceValueWg5,

                                            EccentricityAcceptanceMinValue = weighingMachine.EccentricityAcceptanceMinValue,
                                            EccentricityAcceptanceMaxValue = weighingMachine.EccentricityAcceptanceMaxValue,

                                            RepeatabilityAcceptanceMinValue = weighingMachine.RepeatabilityAcceptanceMinValue,
                                            RepeatabilityAcceptanceMaxValue = weighingMachine.RepeatabilityAcceptanceMaxValue,

                                            UncertainityInstruction = weighingMachine.UncertaintyInstruction

                                        }).AsNoTracking().FirstOrDefaultAsync();
            calibrationDto.WeighingMachineTestConfigurations = await _testConfigurationRepository.GetAll().Where(x => x.WeighingMachineId == weighingMachineId)
                .Select(x => new WeighingMachineTestconfigurationDto
                {
                    WeighingMachineId = x.WeighingMachineId,
                    FrequencyTypeId = x.FrequencyTypeId,
                    Id = x.Id,
                    IsEccentricityTestRequired = x.IsEccentricityTestRequired,
                    IsLinearityTestRequired = x.IsLinearityTestRequired,
                    IsRepeatabilityTestRequired = x.IsRepeatabilityTestRequired,
                    IsUncertainityTestRequired = x.IsUncertainityTestRequired
                })?.ToListAsync() ?? default;
            return calibrationDto;
        }

        private async Task<WeighingCalibrationCommonTestDto> GetCommonTestDto(int weighingMachineId, int frequencyTypeId)
        {
            var weighingMachineWithCalibrations = await _weighingMachineRepository.GetAllIncluding(x => x.Calibrations)
             .Where(x => x.Id == weighingMachineId)
             .AsNoTracking()
             .FirstOrDefaultAsync();
            var unitOfMeasurement = await _unitOfMeasurementRepository.GetAll()
                .Where(x => x.Id == weighingMachineWithCalibrations.UnitOfMeasurementId)
                .Select(x => new { x.Id, x.UnitOfMeasurement }).AsNoTracking().FirstOrDefaultAsync();
            WeighingCalibrationCommonTestDto calibrationTestDto = new WeighingCalibrationCommonTestDto
            {
                UserEnteredUnitOfMeasurement = unitOfMeasurement.UnitOfMeasurement,
                EccentricityInstruction = weighingMachineWithCalibrations.EccentricityInstruction,
                LinearityInstruction = weighingMachineWithCalibrations.LinearityInstruction,
                RepeatabilityInstruction = weighingMachineWithCalibrations.RepeatabilityInstruction,
                WeighingMachineId = weighingMachineWithCalibrations.Id,
                WeighingMachineCode = weighingMachineWithCalibrations.WeighingMachineCode,
                AcceptableMeanValue = weighingMachineWithCalibrations.MeanValue,
                AcceptableStandardDeviationValue = weighingMachineWithCalibrations.StandardDeviationValue,
                AcceptablePRSDValue = weighingMachineWithCalibrations.PercentageRSDValue,
                AcceptableUncertainityValue = weighingMachineWithCalibrations.UncertaintyAcceptanceValue,
                UncertainityInstruction = weighingMachineWithCalibrations.UncertaintyInstruction,
                MeanWeightRange = $"{weighingMachineWithCalibrations.MeanMinimumValue} - {weighingMachineWithCalibrations.MeanMaximumValue}",

            };
            var decimalCount = $"N{weighingMachineWithCalibrations.LeastCountDigitAfterDecimal.GetValueOrDefault()}";
            calibrationTestDto.CalculatedCapacityWeight = $"{(0.3 * weighingMachineWithCalibrations.Capacity.GetValueOrDefault()).ToString(decimalCount)}";
            calibrationTestDto.lstCalibrations = new List<CalibrationDto>();
            foreach (var calibration in weighingMachineWithCalibrations.Calibrations.OrderBy(x => x.Id).Where(x => x.FrequencyTypeId == frequencyTypeId))
            {
                CalibrationDto calibrationDto = new CalibrationDto
                {
                    CalibrationLevelId = calibration.Id,
                    UserEnteredCalibrationLevel = calibration.CalibrationLevel,

                    WeightRange = $"{calibration.MinimumValue} - {calibration.MaximumValue}",
                    StandardWeight = $"{calibration.StandardWeightValue} {unitOfMeasurement.UnitOfMeasurement}"
                };
                calibrationTestDto.lstCalibrations.Add(calibrationDto);
            }
            return calibrationTestDto;
        }

        private bool IsCheckpointsValid(CreateWeighingCalibrationDto input)
        {
            if (input.WeighingCalibrationCheckpoints?.Count > 0)
            {
                foreach (var checkppint in input.WeighingCalibrationCheckpoints)
                {
                    if (!(string.IsNullOrEmpty(checkppint.DiscrepancyRemark) || String.IsNullOrWhiteSpace(checkppint.DiscrepancyRemark)))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private CalibrationStatus IsWeighingMachineCalibrated(WeighingCalibrationDto input)
        {
            var IsEccentricityTestRequired = false;
            var IsLinearityTestRequired = false;
            var IsRepeatabilityTestRequired = false;
            var IsUncertainityTestRequired = false;
            if (input != null)
            {
                var dailyFrequencyModeId = (int)WeighingMachineFrequencyType.Daily;
                var monthlyFrequencyModeId = (int)WeighingMachineFrequencyType.Monthly;
                var weeklyFrequencyModeId = (int)WeighingMachineFrequencyType.Weekly;
                var testConfigurations = new List<WeighingMachineTestconfigurationDto>();
                if (input.FrequencyModeld == dailyFrequencyModeId)
                {
                    testConfigurations = input.WeighingMachineTestConfigurations.Where(x => x.FrequencyTypeId == dailyFrequencyModeId)?.ToList() ?? default;
                }
                else if (input.FrequencyModeld == monthlyFrequencyModeId)
                {
                    testConfigurations = input.WeighingMachineTestConfigurations.Where(x => x.FrequencyTypeId == monthlyFrequencyModeId)?.ToList() ?? default;
                }
                else if (input.FrequencyModeld == weeklyFrequencyModeId)
                {
                    testConfigurations = input.WeighingMachineTestConfigurations.Where(x => x.FrequencyTypeId == weeklyFrequencyModeId)?.ToList() ?? default;
                }
                if (testConfigurations.Count > 0)
                {
                    IsEccentricityTestRequired = testConfigurations[0].IsEccentricityTestRequired.GetValueOrDefault();
                    IsLinearityTestRequired = testConfigurations[0].IsLinearityTestRequired.GetValueOrDefault();
                    IsRepeatabilityTestRequired = testConfigurations[0].IsRepeatabilityTestRequired.GetValueOrDefault();
                    IsUncertainityTestRequired = testConfigurations[0].IsUncertainityTestRequired.GetValueOrDefault();
                }
            }
            if (input.IsAllCalibrationLevelFinished && (input.WeighingCalibrationEccentricityTestDto.Id > 0 || !IsEccentricityTestRequired)
                && (input.WeighingCalibrationLinearityTestDto.Id > 0 || !IsLinearityTestRequired)
                && (input.WeighingCalibrationRepeatabilityTestDto.Id > 0 || !IsRepeatabilityTestRequired)
                && (input.UncertainityTestId > 0 || !IsUncertainityTestRequired))
            {
                foreach (var calibrationDetail in input.lstWeighingCalibrationDetailDto)
                {
                    if (calibrationDetail.CalibrationStatusId == (int)CalibrationStatus.Not_Calibrated)
                    {
                        return CalibrationStatus.Not_Calibrated;
                    }
                }
                if (IsEccentricityTestRequired && input.WeighingCalibrationEccentricityTestDto.TestResultId != (int)CalibrationTestStatus.Passed)
                {
                    return CalibrationStatus.Not_Calibrated;
                }
                if (IsLinearityTestRequired && input.WeighingCalibrationLinearityTestDto.TestResultId != (int)CalibrationTestStatus.Passed)
                {
                    return CalibrationStatus.Not_Calibrated;
                }
                if (IsRepeatabilityTestRequired && input.WeighingCalibrationRepeatabilityTestDto.TestResultId != (int)CalibrationTestStatus.Passed)
                {
                    return CalibrationStatus.Not_Calibrated;
                }
                if (IsUncertainityTestRequired && input.UncertainityTestResultId != (int)CalibrationTestStatus.Passed)
                {
                    return CalibrationStatus.Not_Calibrated;
                }
                return CalibrationStatus.Calibrated;
            }
            return CalibrationStatus.In_Progress;
        }

        private async Task SaveLatestCalibratedWeighingMachine(WeighingCalibrationDto input)
        {
            var savedWMDetail = await _wmcalibratedMachineRepository.GetAll().Where(x => x.WeighingMachineId == input.WeighingMachineId).FirstOrDefaultAsync();
            if (savedWMDetail == null)
            {
                savedWMDetail = new WMCalibratedLatestMachineDetail();
            }
            savedWMDetail.LastCalibrationTestDate = DateTime.UtcNow;
            savedWMDetail.WeighingMachineId = input.WeighingMachineId;
            savedWMDetail.WMCalibrationHeaderId = input.Id;
            _wmcalibratedMachineRepository.InsertOrUpdate(savedWMDetail);
        }

        /// <summary>
        /// Should apply sorting if needed.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="input">The input.</param>
        protected IQueryable<WeighingCalibrationListDto> ApplySorting(IQueryable<WeighingCalibrationListDto> query, PagedWeighingCalibrationResultRequestDto input)
        {
            //Try to sort query if available
            var sortInput = input as ISortedResultRequest;
            if (sortInput != null && !sortInput.Sorting.IsNullOrWhiteSpace())
            {
                return query.OrderBy(sortInput.Sorting);
            }

            //IQueryable.Task requires sorting, so we should sort if Take will be used.
            if (input is ILimitedResultRequest)
            {
                return query.OrderByDescending(e => e.Id);
            }

            //No sorting
            return query;
        }

        /// <summary>
        /// Should apply paging if needed.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="input">The input.</param>
        protected IQueryable<WeighingCalibrationListDto> ApplyPaging(IQueryable<WeighingCalibrationListDto> query, PagedWeighingCalibrationResultRequestDto input)
        {
            //Try to use paging if available
            var pagedInput = input as IPagedResultRequest;
            if (pagedInput != null)
            {
                return query.PageBy(pagedInput);
            }

            //Try to limit query result if available
            var limitedInput = input as ILimitedResultRequest;
            if (limitedInput != null)
            {
                return query.Take(limitedInput.MaxResultCount);
            }

            //No paging
            return query;
        }

        protected IQueryable<WeighingCalibrationListDto> CreateUserListFilteredQuery(PagedWeighingCalibrationResultRequestDto input)
        {
            var plantId = _httpContextAccessor.HttpContext.Request.Headers["PlantId"].FirstOrDefault();
            var weighingCalibrationQuery = from wmCalibrationHeader in _wmCalibrationHeaderRepository.GetAll()
                                           join weighingMachine in _weighingMachineRepository.GetAll()
                                           on wmCalibrationHeader.WeighingMachineId equals weighingMachine.Id
                                           join calibrationStatus in _calibrationStatusRepository.GetAll()
                                           on wmCalibrationHeader.CalibrationStatusId equals calibrationStatus.Id
                                           join calibrationFrequency in _frequencyTypeRepository.GetAll()
                                           on wmCalibrationHeader.CalibrationFrequencyId equals calibrationFrequency.Id
                                           select new WeighingCalibrationListDto
                                           {
                                               Id = wmCalibrationHeader.Id,
                                               CalibrationFrequencyId = wmCalibrationHeader.CalibrationFrequencyId,
                                               CalibrationStatusId = wmCalibrationHeader.CalibrationStatusId,
                                               CalibrationTestDate = wmCalibrationHeader.CalibrationTestDate,
                                               SubPlantId = weighingMachine.SubPlantId,
                                               WeighingMachineId = wmCalibrationHeader.WeighingMachineId,
                                               WeighingMachineCode = weighingMachine.WeighingMachineCode,
                                               UserEnteredCalibrationStatus = calibrationStatus.StatusName,
                                               UserEnteredCalibrationFrequency = calibrationFrequency.FrequencyName
                                           };

            if (!(string.IsNullOrEmpty(plantId) || string.IsNullOrWhiteSpace(plantId)))
            {
                weighingCalibrationQuery = weighingCalibrationQuery.Where(x => x.SubPlantId == Convert.ToInt32(plantId));
            }
            if (!(string.IsNullOrEmpty(input.Keyword) || string.IsNullOrWhiteSpace(input.Keyword)))
            {
                weighingCalibrationQuery = weighingCalibrationQuery.Where(x => x.WeighingMachineCode.Contains(input.Keyword) ||
                x.UserEnteredCalibrationStatus.Contains(input.Keyword) ||
                x.UserEnteredCalibrationFrequency.Contains(input.Keyword));
            }
            if (input.CalibrationDate != null)
            {
                weighingCalibrationQuery = weighingCalibrationQuery.Where(x => x.CalibrationTestDate.Value.Day == input.CalibrationDate.Value.Day &&
                x.CalibrationTestDate.Value.Month == input.CalibrationDate.Value.Month &&
                x.CalibrationTestDate.Value.Year == input.CalibrationDate.Value.Year);
            }
            if (input.CalibrationStatusId != null)
            {
                weighingCalibrationQuery = weighingCalibrationQuery.Where(x => x.CalibrationStatusId == input.CalibrationStatusId);
            }

            return weighingCalibrationQuery;
        }
    }
}