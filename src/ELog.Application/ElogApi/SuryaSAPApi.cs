﻿using Abp.Application.Services;
using ELog.Application.CommomUtility;
using ELog.Application.Sessions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MobiVueEVO.BO.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ELog.Application.ElogApi
{
    public class SuryaSAPApi : ApplicationService, ISuryaSAPApi
    {
        private readonly IConfiguration _configuration;
        private string connection;
        private ISessionAppService _sessionAppService;
        public SuryaSAPApi(IConfiguration configuration)
        {
            _configuration = configuration;
            connection = _configuration["ConnectionStrings:Default"];
        }
        //------------------------------Master created by Abhishek-------------------------------------
        public object PLANTMASTER([FromBody] SAPMaster.PLANTMASTER[]  _objplantmaster)
        {
            MySqlConnection conn = null;
            conn = new MySqlConnection(connection);
            string sReturn = string.Empty;
            try
            {                                           
                    foreach (SAPMaster.PLANTMASTER objclsProductionorder in _objplantmaster)
                    {
                        String Query = String.Empty;
                        Query = "SELECT * FROM mplantbranch where Code='" + objclsProductionorder.PlantCode + "';";
                        MySqlCommand MyCommand2 = new MySqlCommand(Query, conn);
                        MySqlDataAdapter MyAdapter = new MySqlDataAdapter();
                        conn.Open();
                        MyAdapter.SelectCommand = MyCommand2;
                        DataTable dt = new DataTable();
                        MyAdapter.Fill(dt);                       
                        string insertquery = string.Empty;
                        if (dt.Rows.Count == 0)
                        {                           
                            insertquery = "insert into mplantbranch(Code, Description, Type, Address, CreatedOn, CreatedBy) values('"+ objclsProductionorder.PlantCode + "','"+ objclsProductionorder.Description + "','"+ objclsProductionorder.Type + "','"+ objclsProductionorder.Address + "',now(),'SAPAPI');";
                            MySqlCommand MyCommandINSERT = new MySqlCommand(insertquery, conn);
                            MySqlDataReader MyReader2;                          
                            MyReader2 = MyCommandINSERT.ExecuteReader();                           
                        }
                        else {                        
                            insertquery = "UPDATE mplantbranch SET Description='" + objclsProductionorder.Description + "',Type='" + objclsProductionorder.Type + "',Address='" + objclsProductionorder.Address + "',CreatedBy='SAPAPI',CreatedOn=NOW() WHERE Code='" + objclsProductionorder.PlantCode + "';";
                            MySqlCommand MyCommandINSERT = new MySqlCommand(insertquery, conn);
                            MySqlDataReader MyReader2;                           
                            MyReader2 = MyCommandINSERT.ExecuteReader();
                           
                        }
                        conn.Close();
                       sReturn = "Data Save Successfully.";
                    }
                          
            }
            catch (Exception e)
            {
                sReturn = e.Message;
                Console.WriteLine(e.ToString());
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
            return sReturn;

        }
        public object CUSTOMERMASTER([FromBody] SAPMaster.CUSTOMERMASTER[] _objcustomermaster)
        {
            MySqlConnection conn = null;
            conn = new MySqlConnection(connection);
            string sReturn = string.Empty;
            try
            {
                foreach (SAPMaster.CUSTOMERMASTER objcls in _objcustomermaster)
                {
                    String Query = String.Empty;
                    Query = "SELECT * FROM mcustomerdealer where Code='" + objcls.CustomerCode + "';";
                    MySqlCommand MyCommand2 = new MySqlCommand(Query, conn);
                    MySqlDataAdapter MyAdapter = new MySqlDataAdapter();
                    conn.Open();
                    MyAdapter.SelectCommand = MyCommand2;
                    DataTable dt = new DataTable();
                    MyAdapter.Fill(dt);
                    string insertquery = string.Empty;
                    if (dt.Rows.Count == 0)
                    {
                        insertquery = "insert into mcustomerdealer(Code, Name, Address, City, District, Createdon, CreatedBy) values('" + objcls.CustomerCode + "','" + objcls.CustomerName + "','" + objcls.Address + "','" + objcls.City + "','" + objcls.District + "',now(),'SAPAPI');";
                        MySqlCommand MyCommandINSERT = new MySqlCommand(insertquery, conn);
                        MySqlDataReader MyReader2;
                        MyReader2 = MyCommandINSERT.ExecuteReader();
                    }
                    else
                    {
                        insertquery = "UPDATE mcustomerdealer SET Name='" + objcls.CustomerName + "',Address='" + objcls.Address + "',City='" + objcls.City + "',District='" + objcls.District + "',CreatedBy='SAPAPI',Createdon=NOW() WHERE Code='" + objcls.CustomerName + "';";
                        MySqlCommand MyCommandINSERT = new MySqlCommand(insertquery, conn);
                        MySqlDataReader MyReader2;
                        MyReader2 = MyCommandINSERT.ExecuteReader();

                    }
                    conn.Close();
                    sReturn = "Data Save Successfully.";
                }

            }
            catch (Exception e)
            {
                sReturn = e.Message;
                Console.WriteLine(e.ToString());
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
            return sReturn;

        }
        public object WORKLINEMASTER([FromBody] SAPMaster.WORKLINEMASTER[] _objworklinemaster)
        {
            MySqlConnection conn = null;
            conn = new MySqlConnection(connection);
            string sReturn = string.Empty;
            try
            {
                foreach (SAPMaster.WORKLINEMASTER objcls in _objworklinemaster)
                {
                    String Query = String.Empty;
                    Query = "SELECT * FROM mworkcenterorline where WorkCenterCode='" + objcls.WorkCenterCode + "';";
                    MySqlCommand MyCommand2 = new MySqlCommand(Query, conn);
                    MySqlDataAdapter MyAdapter = new MySqlDataAdapter();
                    conn.Open();
                    MyAdapter.SelectCommand = MyCommand2;
                    DataTable dt = new DataTable();
                    MyAdapter.Fill(dt);
                    string insertquery = string.Empty;
                    if (dt.Rows.Count == 0)
                    {
                        insertquery = "insert into mworkcenterorline(PlantCode, WorkCenterCode, Work_Center_Discription, Active, CreatedOn, CreatedBy) values('" + objcls.PlantCode + "','" + objcls.WorkCenterCode + "','" + objcls.Work_Center_Discription + "','" + objcls.Status + "',now(),'SAPAPI');";
                        MySqlCommand MyCommandINSERT = new MySqlCommand(insertquery, conn);
                        MySqlDataReader MyReader2;
                        MyReader2 = MyCommandINSERT.ExecuteReader();
                    }
                    else
                    {
                        insertquery = "UPDATE mworkcenterorline SET PlantCode='" + objcls.PlantCode + "',Work_Center_Discription='" + objcls.Work_Center_Discription + "',Active='" + objcls.Status + "',CreatedBy='SAPAPI',Createdon=NOW() WHERE WorkCenterCode='" + objcls.WorkCenterCode + "';";
                        MySqlCommand MyCommandINSERT = new MySqlCommand(insertquery, conn);
                        MySqlDataReader MyReader2;
                        MyReader2 = MyCommandINSERT.ExecuteReader();

                    }
                    conn.Close();
                    sReturn = "Data Save Successfully.";
                }

            }
            catch (Exception e)
            {
                sReturn = e.Message;
                Console.WriteLine(e.ToString());
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
            return sReturn;

        }
        public object STORAGELOCATION([FromBody] SAPMaster.STORAGELOCATION[] _objstoragelocationmaster)
        {
            MySqlConnection conn = null;
            conn = new MySqlConnection(connection);
            string sReturn = string.Empty;
            try
            {
                foreach (SAPMaster.STORAGELOCATION objcls in _objstoragelocationmaster)
                {
                    String Query = String.Empty;
                    Query = "SELECT * FROM mstrloc where Code='" + objcls.Code + "';";
                    MySqlCommand MyCommand2 = new MySqlCommand(Query, conn);
                    MySqlDataAdapter MyAdapter = new MySqlDataAdapter();
                    conn.Open();
                    MyAdapter.SelectCommand = MyCommand2;
                    DataTable dt = new DataTable();
                    MyAdapter.Fill(dt);
                    string insertquery = string.Empty;
                    if (dt.Rows.Count == 0)
                    {
                        insertquery = "insert into mstrloc(Code, StrLocCode, Description, CreatedOn, CreatedBy) values('" + objcls.Code + "','" + objcls.StrLocCode + "','" + objcls.Description + "',now(),'SAPAPI');";
                        MySqlCommand MyCommandINSERT = new MySqlCommand(insertquery, conn);
                        MySqlDataReader MyReader2;
                        MyReader2 = MyCommandINSERT.ExecuteReader();
                    }
                    else
                    {
                        insertquery = "UPDATE mworkcenterorline SET StrLocCode='" + objcls.StrLocCode + "',Description='" + objcls.Description + "',CreatedBy='SAPAPI',Createdon=NOW() WHERE StrLocCode='" + objcls.Code + "';";
                        MySqlCommand MyCommandINSERT = new MySqlCommand(insertquery, conn);
                        MySqlDataReader MyReader2;
                        MyReader2 = MyCommandINSERT.ExecuteReader();

                    }
                    conn.Close();
                    sReturn = "Data Save Successfully.";
                }

            }
            catch (Exception e)
            {
                sReturn = e.Message;
                Console.WriteLine(e.ToString());
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
            return sReturn;

        }
        //------------------------------END(Master created by Abhishek)---------------------------------

        //------------------------------Transaction Master Start(created by Abhishek)--------------------
        public object PACKINGORDER([FromBody] SAPMaster.PACKINGORDER[] _objpackingorder)
        {
            MySqlConnection conn = null;
            conn = new MySqlConnection(connection);
            string sReturn = string.Empty;
            try
            {
                foreach (SAPMaster.PACKINGORDER objcls in _objpackingorder)
                {
                    String Query = String.Empty;
                    Query = "SELECT * FROM tpackingorder where PackingOrderNo='" + objcls.PackingOrderNumber + "' and MaterialCode='" + objcls.MaterialCode + "';";
                    MySqlCommand MyCommand2 = new MySqlCommand(Query, conn);
                    MySqlDataAdapter MyAdapter = new MySqlDataAdapter();
                    conn.Open();
                    MyAdapter.SelectCommand = MyCommand2;
                    DataTable dt = new DataTable();
                    MyAdapter.Fill(dt);
                    string insertquery = string.Empty;
                    if (dt.Rows.Count == 0)
                    {
                        insertquery = "insert into tpackingorder(PackingOrderNo, PlantCode, MaterialCode, Quantity,  StrLocCode, Packing_Date, Bom, Work_Center,CreatedOn, CreatedBy ) values('" + objcls.PackingOrderNumber + "','" + objcls.PlantCode + "','" + objcls.MaterialCode + "','" + objcls.Quantity + "','" + objcls.StorageLocation + "','" + objcls.Packing_Date + "','" + objcls.BOM + "','" + objcls.WorkCenter + "',now(),'SAPAPI');";
                        MySqlCommand MyCommandINSERT = new MySqlCommand(insertquery, conn);
                        MySqlDataReader MyReader2;
                        MyReader2 = MyCommandINSERT.ExecuteReader();
                    }
                    else
                    {
                        insertquery = "UPDATE tpackingorder SET PlantCode='" + objcls.PlantCode + "',Quantity='" + objcls.Quantity + "',StrLocCode='" + objcls.StorageLocation + "',Packing_Date='" + objcls.Packing_Date + "',Bom='" + objcls.BOM + "',Work_Center='" + objcls.WorkCenter + "',CreatedBy='SAPAPI',Createdon=NOW() WHERE PackingOrderNo='" + objcls.PackingOrderNumber + "' and MaterialCode='" + objcls.MaterialCode + "';";
                        MySqlCommand MyCommandINSERT = new MySqlCommand(insertquery, conn);
                        MySqlDataReader MyReader2;
                        MyReader2 = MyCommandINSERT.ExecuteReader();

                    }
                    conn.Close();
                    sReturn = "Data Save Successfully.";
                }

            }
            catch (Exception e)
            {
                sReturn = e.Message;
                Console.WriteLine(e.ToString());
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
            return sReturn;

        }
        //------------------------------Transaction Master End()--------------------

    }
}