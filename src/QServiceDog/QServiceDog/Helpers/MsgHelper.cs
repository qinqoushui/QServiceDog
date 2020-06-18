using Q.DevExtreme.Tpl;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace QServiceDog.Helpers
{
    /// <summary>
    /// 消息推送
    /// </summary>
    /// <remarks>邮件、微信、短信</remarks>
    public class MsgHelper
    {
        public void Send(string title, string msg, string url)
        {
            sendByWechat("ZhangWenXiang", title, msg, url);
        }

        void sendByWechat(string user, string title, string msg, string url)
        {
            var wechatMsg = new
            {
                touser = user,
                toparty = "",
                totag = "",
                msgtype = "text",
                agentid = 1000045,// 1000002,
                safe = 0,
                text = new
                {
                    content = "警告，停电恢复<a href=\"http://www.sina.com.cn\">sina</a>"
                }
            }.SerializeObject();

           // callWebService("http://qywx.bjzycx.net/WechatWebService.asmx", wechatMsg);
            //1000045
            callWebService("http://work.jstayc.com/WechatWebService.asmx", wechatMsg);
        }

        string callWebService(string webWebServiceUrl, string data)
        {
            string soapText = string.Empty;
            using (var ms = new StringWriter( ))
            {
                using (XmlTextWriter Xmltr = new XmlTextWriter(ms))
                {
                    Xmltr.WriteStartDocument();
                    Xmltr.WriteStartElement("soap", "Envelope", "http://schemas.xmlsoap.org/soap/envelope/");
                    Xmltr.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");
                    Xmltr.WriteAttributeString("xmlns", "xsd", null, "http://www.w3.org/2001/XMLSchema");
                    Xmltr.WriteAttributeString("xmlns", "soap", null, "http://schemas.xmlsoap.org/soap/envelope/");
                    Xmltr.WriteStartElement("Body", "http://schemas.xmlsoap.org/soap/envelope/");
                    Xmltr.WriteStartElement(null, "SendWeachtMsg", "http://tempuri.org/");
                    Xmltr.WriteElementString("JsonData", data);
                    Xmltr.WriteEndElement();
                    Xmltr.WriteEndElement();
                    Xmltr.WriteEndDocument();
                    Xmltr.Close();
                }
                soapText = ms.ToString();
                ms.Close();
            }
            
            using (WebClient webClient = new WebClient())
            {
                try
                {
                    webClient.Proxy = null;
                    byte[] postDatabyte = Encoding.GetEncoding("UTF-8").GetBytes(soapText);

                    webClient.Headers.Add("Content-Type", "text/xml;charset=utf-8");
                    webClient.Headers.Add(HttpRequestHeader.ContentEncoding, "charset=utf-8");
                    byte[] responseData = webClient.UploadData(webWebServiceUrl, "POST", postDatabyte);
                    //解码 
                    string responsestring = Encoding.GetEncoding("UTF-8").GetString(responseData);
                    return responsestring;
                }
                catch (Exception e)
                {
                    return e.GetExceptionMsg();
                }
            }

        }


    }
}
