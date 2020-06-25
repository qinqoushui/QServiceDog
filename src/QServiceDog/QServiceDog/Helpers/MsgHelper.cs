using MailKit.Net.Smtp;
using MimeKit;
using Q.DevExtreme.Tpl;
using QServiceDog.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
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
        public void Send(List<EventPushRecord> records, List<Sender> senders)
        {
            foreach (var sender in senders)
            {
                if (Enum.TryParse<Enums.EnumSender>("e" + sender.TypeName, out var e))
                {
                    switch (e)
                    {
                        case Enums.EnumSender.e企业微信:
                            foreach (var record in records)
                            {
                                if (string.IsNullOrEmpty(record.EventSubscriber.WXName))
                                    continue;
                                sendByWechat(record.EventSubscriber.WXName, $"{record.EventInfo.Client} {record.EventInfo.Time.ToString("yyyy-MM-dd HH:mm:ss")} {record.EventInfo.Msg}", sender.Para);
                                record.Pushed = true;
                                record.PushTime = DateTime.Now;
                            }
                            break;
                        case Enums.EnumSender.e邮箱:
                            //QQ邮箱反垃圾群发，阿里云禁止25端口，用不了126邮箱
                            //sendByEmail(records.Select(r => r.EventSubscriber).ToArray(), $"{records.First().EventInfo.Msg}", $"{records.First().EventInfo.Client} {records.First().EventInfo.Time.ToString("yyyy-MM-dd HH:mm:ss")} {records.First().EventInfo.Msg}", sender.Para);
                            //records.ForEach(r =>
                            //{
                            //    r.Pushed = true;
                            //    r.PushTime = DateTime.Now;
                            //});
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        void sendByWechat(string user, string msg, string para)
        {
            var p = para.DeserializeAnonymousType(new { agentid = 0, url = "" });
            var wechatMsg = new
            {
                touser = user,
                toparty = "",
                totag = "",
                msgtype = "text",
                agentid = p.agentid,// 1000002,
                safe = 0,
                text = new
                {
                    content = msg
                }
            }.SerializeObject();

            // callWebService("", wechatMsg);
            //1000045
            callWebService(p.url, wechatMsg);
        }

        string callWebService(string webWebServiceUrl, string data)
        {
            string soapText = string.Empty;
            using (var ms = new StringWriter())
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


        void sendByEmail(EventSubscriber[] reciver, string title, string content, string para)
        {
            if (reciver.Count(r => !string.IsNullOrEmpty(r.EMail)) == 0)
                return;
            var p = para.DeserializeAnonymousType(new { account = "", password = "", smtp = "", port = 25, cc = new string[0] });
            MimeMessage message = new MimeMessage();
            message.Subject = title;
            message.From.Add(new MailboxAddress(p.account, p.account));
            message.To.AddRange(reciver.Select(r => new MailboxAddress(r.Name, r.EMail)).ToList());
            TextPart textPart = new TextPart("html");
            textPart.SetText(Encoding.UTF8, content);
            var mimeparts = new Multipart("mixed");
            mimeparts.Add(textPart);
            message.Body = mimeparts;
            using (SmtpClient smtpClient = new SmtpClient())
            {
                smtpClient.Connect(p.smtp, p.port, MailKit.Security.SecureSocketOptions.Auto, default(CancellationToken));
                smtpClient.ServerCertificateValidationCallback = (s, c, h, e) => true;
                //smtpClient.Authenticate(p.account,p.password, default(CancellationToken));
                NetworkCredential nc = new NetworkCredential(p.account, p.password);
                smtpClient.Authenticate(nc, default(CancellationToken));
                smtpClient.Send(message, default(CancellationToken), null);
                smtpClient.Disconnect(true, default(CancellationToken));
            }
        }


    }
}
