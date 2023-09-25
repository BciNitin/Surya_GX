using Common;
using MobiVueEVO.BO;
using MobiVueEVO.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace MobiVueEVO.BL
{
    public class BL_MobCommon
    {
        DL_Common dlboj = null;

        public bool charCheck(char input_char)
        {
            bool bValue = true;
            // Checking for Alphabet 
            if ((input_char >= 65 && input_char <= 90) || (input_char >= 97 && input_char <= 122))
            {
                bValue = true;
            }
            // Checking for Digits 
            else
            {
                bValue = false;
            }
            return bValue;
        }
        public string sFinalBarcodeValue(string sValue)
        {
            string sFinalValue = string.Empty;
            try
            {
                char[] sarr = sValue.ToCharArray();
                string sNumber = "!099";
                string sAlpha = "!100";
                string sStartNumber = "!105";
                string sStartAlpha = "!104";
                bool bLastChar = false;
                for (int i = 0; i < sarr.Length; i++)
                {
                    if (i == 0)
                    {
                        if (charCheck(sarr[i]))
                        {
                            sFinalValue = sStartAlpha + sarr[i];
                            bLastChar = true;
                        }
                        else
                        {
                            sFinalValue = sStartNumber + sarr[i];
                            bLastChar = false;
                        }
                    }
                    else
                    {
                        if (charCheck(sarr[i]))
                        {
                            if (bLastChar == false)
                            {
                                sFinalValue = sFinalValue + sAlpha + sarr[i];
                            }
                            else
                            {
                                sFinalValue = sFinalValue + sarr[i];
                            }
                            bLastChar = true;

                        }
                        else
                        {
                            if (bLastChar == true)
                            {
                                sFinalValue = sFinalValue + sNumber + sarr[i];
                            }
                            else
                            {
                                sFinalValue = sFinalValue + sarr[i];
                            }
                            bLastChar = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
               // PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtData, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
            return sFinalValue;
        }
    }
}
