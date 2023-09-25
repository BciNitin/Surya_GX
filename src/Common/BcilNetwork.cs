using System;
using System.Net;
using System.Net.Sockets;
/*========================================================================================
Procedure/Module :  PRINTING USING SOCKET
Purpose          :  PRINTING USING SOCKET 
Created By       :  Nishant
Created on       :  21-JAN-2011
Modified By      :  Nishant
Modified on      :  ------------------
Copyright (c) Bar Code India Ltd. All rights reserved.
========================================================================================*/
namespace Common
{
    public class BcilNetwork : IDisposable
    {
        #region "Construcgtor"        


        #endregion

        #region "Distructor"               
        ~BcilNetwork()
        {
            Dispose(true);
        }
        #endregion

        #region Private Variables
        private string _PrinterIp = "";
        private int _PrinterPort = 9100;
        private string _Prn = "";
        private string _LogPath = @"C:\";
        private bool _LogWriter = false;
        private Socket _Sock = null;
        private IPEndPoint serverEndPoint;
        private IPAddress IPAddressServer;
        private bool _IsDisposed = false;
        #endregion

        #region Public variables
        public string PrinterIP
        {
            get { return _PrinterIp; }
            set { _PrinterIp = value; }
        }
        public int PrinterPort
        {
            get { return _PrinterPort; }
            set { _PrinterPort = value; }
        }
        public string Prn
        {
            get { return _Prn; }
            set { _Prn = value; }
        }
        public bool LogWriter
        {
            get { return _LogWriter; }
            set { _LogWriter = value; }
        }
        public string LogPath
        {
            get { return _LogPath; }
            set { _LogPath = value; }
        }
        #endregion

        #region "NetworkPrinting"
        /// <summary>
        /// INITIALISE SOCKET
        /// </summary>
        /// <returns></returns>
        bool _InitializeSockClient()
        {
            try
            {
                _Sock = null;
                IPAddressServer = IPAddress.Parse(_PrinterIp);
                serverEndPoint = new IPEndPoint(IPAddressServer, _PrinterPort);
                _Sock = new Socket(serverEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                _Sock.Connect(serverEndPoint);
                if (_Sock.Connected)
                    return true;
                else
                {
                    // _Logger.LogMessage(EventNotice.EventTypes.evtInfo,"_InitializeSockClient", "Initialze Socket Connection Failed........."  );                            
                    return false;
                }
            }
            catch (Exception ex)
            {
                PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtData, System.Reflection.Assembly.GetExecutingAssembly().GetName() + "::" + System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                //_Logger.LogMessage(EventNotice.EventTypes.evtInfo, "_InitializeSockClient", ex.Message);                                                    
                throw ex;
            }
        }
        /// <summary>
        /// CHECK FOR SOCKET CONNECTED OR NOT
        /// </summary>
        /// <returns></returns>
        bool _IsSockConnected()
        {
            try
            {
                if (_Sock == null)
                    return false;
                if (_Sock.Connected == false)
                    return false;
                return true;
            }
            catch (System.Exception ex)
            {
                PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtData, System.Reflection.Assembly.GetExecutingAssembly().GetName() + "::" + System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                _SocklientTerminate();
                //_Logger.LogMessage(EventNotice.EventTypes.evtError, "IsSockConnected", ex.Message);                                                                            
                throw ex;
            }
        }

        /// <summary>
        /// SOCKET TERMINATE
        /// </summary>
        void _SocklientTerminate()
        {
            try
            {
                if (_Sock != null && _Sock.Connected)
                    _Sock.Close();
                _Sock = null;
            }
            catch (Exception ex)
            {
                PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtData, System.Reflection.Assembly.GetExecutingAssembly().GetName() + "::" + System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                //_Logger.LogMessage(EventNotice.EventTypes.evtError, "SocklientTerminate", ex.StackTrace);                                                                                                    
            }
        }
        /// <summary>
        /// DATA RECEIVE FROM PRINTER VIA SOCKET
        /// </summary>
        /// <param name="_client"></param>
        /// <returns></returns>
        string _Response(Socket _client)
        {
            try
            {
                Byte[] _data = new Byte[8025];
                SocketAsyncEventArgs _ar = new SocketAsyncEventArgs();
                _client.ReceiveTimeout = 5000;
                Int32 byteCount = _client.Receive(_data);
                return System.Text.Encoding.ASCII.GetString(_data, 0, byteCount);
            }
            catch (Exception ex)
            {
                PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtData, System.Reflection.Assembly.GetExecutingAssembly().GetName() + "::" + System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                _SocklientTerminate();
                //_Logger.LogMessage(EventNotice.EventTypes.evtError, "_Response", ex.StackTrace);
                throw ex;
            }
        }
        /// <summary>
        /// SEND DATA FROM SOCKET
        /// </summary>
        /// <param name="_sChkPrinterStatusFlag"></param>
        /// <returns></returns>
        public string NetworkPrint()
        {
            byte[] _dBuffer = System.Text.Encoding.ASCII.GetBytes("");
            try
            {
                if (_IsSockConnected() == false)
                {
                    if (_InitializeSockClient() != false)
                        return "PRINTER NOT INITIALIZE";
                }
                _dBuffer = System.Text.Encoding.ASCII.GetBytes(_Prn);

                _Sock.Send(_dBuffer);
                return "SUCCESS";
            }
            catch (Exception ex)
            {
                PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtData, System.Reflection.Assembly.GetExecutingAssembly().GetName() + "::" + System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                _SocklientTerminate();
                //_Logger.LogMessage(EventNotice.EventTypes.evtError, "NetworkPrint", ex.StackTrace);                        
                return "PRINTER NOT IN NETWORK";
            }
        }
        /// <summary>
        /// Get Network Printer Status
        /// </summary>
        /// <returns></returns>
        public string NetworkPrinterStatus()
        {
            //string[] _Arr = null;
            string _sReturn = "";
            byte[] _dBuffer = System.Text.Encoding.ASCII.GetBytes("");
            try
            {
                if (_IsSockConnected() == false)
                {
                    if (_InitializeSockClient() != true)
                    {
                        _sReturn = "PRINTER NOT INITIALIZE";
                        return _sReturn;
                    }
                }
                //_dBuffer = System.Text.Encoding.ASCII.GetBytes("~HS");
                //_Sock.Send(_dBuffer);
                //_sReturn = _Response(_Sock);
                ////_sReturn = "014,0,0,0411,000,0,0,0,000,0,0,0001,0,0,0,1,0,6,0,00000000,1,0001234,0";
                //_dBuffer = System.Text.Encoding.ASCII.GetBytes("");
                //_Arr = _sReturn.Split(',');
                //if (_Arr.Length > 14)
                //{
                //    if (_Arr[1].Trim() == "1")
                //        _sReturn = "PRINTER PAPER OUT";                                             
                //    else if (_Arr[2].Trim() == "1")
                //        _sReturn = "PRINTER IN PAUSE STATUS";                                                            
                //    else if (_Arr[5].Trim() == "1")
                //        _sReturn = "PRINTER BUFFER FULL";                                                           
                //    else if (Convert.ToInt64(_Arr[4].Trim()) > 50)
                //        _sReturn = "UNUSED BIT GREATER THAN 50";                                                           
                //    else if (_Arr[14].Trim() == "1")
                //        _sReturn = "PRINTER RIBBON OUT";                                                            
                //    else
                //        _sReturn = "PRINTER READY";                            
                //}
                //else
                //    _sReturn = "UNKNOWN ERROR";
                _sReturn = "PRINTER READY";
                return _sReturn;
            }
            catch (Exception ex)
            {
                PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtData, System.Reflection.Assembly.GetExecutingAssembly().GetName() + "::" + System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                _SocklientTerminate();
                // _Logger.LogMessage(EventNotice.EventTypes.evtError, "NetworkPrinterStatus", ex.StackTrace);                        
                _sReturn = "PRINTER NOT IN NETWORK";
            }
            return _sReturn;
        }
        #endregion

        #region "Dispose"
        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="IsDisposing"></param>
        protected virtual void Dispose(bool IsDisposing)
        {
            if (_IsDisposed)
                return;
            if (IsDisposing)
            {
                // Free any managed resources in this section
            }
            _IsDisposed = true;
        }
        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            _SocklientTerminate();
            Dispose(true);
            // Tell the garbage collector not to call the finalizer
            // since all the cleanup will already be done.
            GC.SuppressFinalize(true);
        }
        #endregion
    }
}
