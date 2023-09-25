using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI.HtmlControls;
namespace Common
{
    public static class CommonHelper
    {
        public static string[] _strRights { get; set; }
        public static string sCompanyName = string.Empty;
        public static string sCustomMessage = "There is some error, Please check on server log";
        public static string sValue = string.Empty;
        public static string sBYPassLogin = "0";
        public static void ShowMessage(string msg, HtmlGenericControl sMsgPlaceHolder, string sType)
        {
            if (sMsgPlaceHolder != null)
            {
                sMsgPlaceHolder.InnerHtml = "";
                StringBuilder sb = new StringBuilder();
                sb.Append("<button type='button' class='close' data-dismiss='alert' aria-hidden='true'> &times;</button>");
                switch (sType)
                {
                    case "Info":
                        sb.Append("<h4> <i class='icon fa fa-info'></i>");
                        sb.Append(sCompanyName);
                        sb.Append("</h4>");
                        sb.AppendLine(msg);
                        break;
                    case "Success":
                        sb.Append("<h4> <i class='icon fa fa-check'></i> ");
                        sb.Append(sCompanyName);
                        sb.Append("</h4>");
                        sb.AppendLine(msg);
                        break;
                    case "Warning":
                        sb.Append("<h4> <i class='icon fa fa-warning'></i>");
                        sb.Append(sCompanyName);
                        sb.Append("</h4>");
                        sb.AppendLine(msg);
                        break;
                    case "Error":
                        //sb.Append("<script type = 'text/javascript'>");
                        //sb.Append("window.onload=function(){");
                        //sb.Append("alert('");
                        //sb.Append(msg);
                        //sb.Append("')};");
                        //sb.Append("</script>");
                        sb.Length = 0;
                        sb.Append("<button type='button' class='close' data-dismiss='alert' aria-hidden='true'> &times;</button>");
                        sb.Append("<h4> <i class='icon fa fa-ban'></i> ");
                        sb.Append(sCompanyName);
                        sb.Append("</h4>");
                        sb.AppendLine(msg);
                        break;
                    case "Notification":
                        sb.Append("<script type = 'text/javascript'>");
                        sb.Append("window.onload=function(){");
                        sb.Append("alert('");
                        sb.Append(msg);
                        sb.Append("')};");
                        sb.Append("</script>");
                        sb.AppendLine(msg);
                        break;
                }
                sMsgPlaceHolder.Style.Add("display", "");
                sMsgPlaceHolder.InnerHtml = sb.ToString();
                sb.Length = 0;
            }
        }
        public static BcilLib.BcilLogger mBcilLogger;

        public enum MessageType
        {
            Info,
            Success,
            Warning,
            Error,
            REWORK,
            Notification
        }
        public static void HideMessage(HtmlGenericControl msginfo, HtmlGenericControl msgsuccess, HtmlGenericControl msgwarning, HtmlGenericControl msgerror)
        {
            msginfo.InnerHtml = "";
            msginfo.Style.Add("display", "none");
            msgsuccess.InnerHtml = "";
            msgsuccess.Style.Add("display", "none");
            msgwarning.InnerHtml = "";
            msgwarning.Style.Add("display", "none");
            msgerror.InnerHtml = "";
            msgerror.Style.Add("display", "none");
        }
        public static void HideSuccessMessage(HtmlGenericControl msginfo, HtmlGenericControl msgwarning, HtmlGenericControl msgerror)
        {
            msginfo.InnerHtml = "";
            msginfo.Style.Add("display", "none");
            msgwarning.InnerHtml = "";
            msgwarning.Style.Add("display", "none");
            msgerror.InnerHtml = "";
            msgerror.Style.Add("display", "none");
        }
        public static string GetRights(string _PageName, DataTable dtRights)
        {
            char _RtVw = '0';

            var vrCountry = (from country in dtRights.AsEnumerable()
                             where country.Field<string>("MODULENAME").ToUpper() == _PageName.ToUpper()
                             select country);
            var rows = vrCountry.ToList();
            if (rows.Count > 0)
            {
                _RtVw = rows[0][1].ToString() == "True" ? '1' : '0';
            }
            return (_RtVw + "^");
        }
        public static void ShowCustomErrorMessage(string sErrorMessage, HtmlGenericControl msgerror)
        {
            if (CommonHelper.sValue == "0")
            {
                CommonHelper.ShowMessage(sErrorMessage, msgerror, CommonHelper.MessageType.Error.ToString());
            }
            else
            {
                CommonHelper.ShowMessage(CommonHelper.sCustomMessage, msgerror, CommonHelper.MessageType.Error.ToString());
            }
        }
    }
}
