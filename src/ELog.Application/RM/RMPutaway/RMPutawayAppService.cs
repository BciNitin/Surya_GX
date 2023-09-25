using Abp.Application.Services;
using ELog.Application.RM.RMPutaway;
using ELog.Core.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MobiVueEVO.BL;
using MobiVueEVO.DAL;
using System;
using System.Data;
using System.Threading.Tasks;

namespace MobiVueEVO.Application
{
    [PMMSAuthorize]
    public class RMPutawayAppService : ApplicationService, IRMPutawayAppService
    {
        private readonly RMPutAwayBL blobj;
        private string Message;
        private string Result;
        public RMPutawayAppService(IConfiguration configuration)
        {
            DBConfig.Configuration = configuration;
            blobj = new RMPutAwayBL();
        }

        [PMMSAuthorize]
        public async Task<IActionResult> ValidateLocation(string Location)
        {
            DataTable dt = new DataTable();
            try
            {
                dt = await blobj.ValidateLocation(Location);
                Result = dt.Rows[0][0].ToString();
                Message = Result.Split('~')[1].ToString();

                if (Result.Contains("SUCCESS"))
                {
                    Message = $"SUCCESS~{Message}";
                }
                else
                {
                    Message = $"ERROR~{Message}";
                }
                //if (dt.Contains("ERROR"))
                //{
                //    CodeContract.Required<MobiVUEException>(dt.Contains("ERROR"), dt.Split('~')[1]);
                //}
                //else if (dt is null)
                //{
                //    dt = "No Suggested Pallet";
                //}
                return new OkObjectResult(Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new BadRequestObjectResult(ex.Message);
            }
        }

        [PMMSAuthorize]
        public async Task<IActionResult> PutawayAndValidateBarcode(string Location, string barCode)
        {
            DataTable dt = new DataTable();
            try
            {
                dt = await blobj.PutawayAndValidateBarcode(Location, barCode);
                Result = dt.Rows[0][0].ToString();
                if (Result.Contains('~'))
                {
                    Message = Result.Split('~')[1].ToString();
                }

                if (dt.Rows.Count == 0 || dt is null)
                {
                    return new OkObjectResult("Error");
                }
                else
                {

                    if (Result.Contains("N"))
                    {
                        return new OkObjectResult($"ERROR~{Message}");
                        // Message = $"ERROR~{Message}";
                    }
                    //else
                    //{
                    // Message = $"ERROR~{Message}";
                    //}
                }

                //if (dt.Contains("ERROR"))
                //{
                //    CodeContract.Required<MobiVUEException>(dt.Contains("ERROR"), dt.Split('~')[1]);
                //}
                //else if (dt is null)
                //{
                //    dt = "No Suggested Pallet";
                //}

                return new OkObjectResult(dt);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new OkObjectResult(ex.Message);
            }
        }
    }
}