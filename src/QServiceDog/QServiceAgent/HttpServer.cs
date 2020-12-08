using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace QServiceAgent
{
    public class HttpServer
    {
        public void Start(string url = "http://+:4777/")
        {
            HttpListener httpListener = new HttpListener();
            httpListener.Prefixes.Add(url);
            httpListener.Start();
            while (true)
            {
                HttpListenerContext ctx = null;
                try
                {
                    ctx = httpListener.GetContext();
                    aa(ctx);
                }
                catch (Exception ex)
                {
                    if (ctx != null)
                    {
                        try
                        {
                            byte[] buff = Encoding.UTF8.GetBytes(ex.ToString());
                            ctx.Response.OutputStream.Write(buff, 0, buff.Length);
                            ctx.Response.Close();
                        }
                        catch { }
                    }
                }
            }
        }
        void aa(HttpListenerContext context)
        {
            //取得请求的对象
            HttpListenerRequest request = context.Request;
            Resp resp = new Resp();
            HttpListenerResponse response = context.Response;
            response.ContentEncoding = Encoding.UTF8;
            response.ContentType = "application/json; charset=utf-8";
            if (request.LocalEndPoint.Address.Equals(IPAddress.Loopback))
            {
                var reader = new StreamReader(request.InputStream);
                var msg = reader.ReadToEnd();
                if (request.HttpMethod.ToUpper() == "GET")
                {
                    resp.Code = 0;
                    resp.Msg = DateTime.Now.ToString("F");
                }
                else
                if (request.HttpMethod.ToUpper() == "POST")
                {
                    var req = De(msg, new { ActionType = "" });
                    switch (req.ActionType)
                    {
                        case "StartProcess":
                            var yy = Serialize(new
                            {
                                ActionType = "StartProcess",
                                ActionData = new
                                {
                                    FileName = @"c:\windows\system32\notepad.exe",
                                    WorkingPath = @"d:\temp",
                                    Para = @"test.txt"
                                }
                            }
                            );
                            var ss = De(msg, new { ActionData = new { FileName = "", WorkingPath = "", Para = "" } }).ActionData;
                            if (ss != null)
                            {
                                //    string[] command = new string[]{
                                //        $" cmd /c  \"{Path.GetFileName(ss.FileName)}\" /D \"{ss.WorkingPath}\" \"{ss.FileName}\" \"{ss.WorkingPath}\" ",
                                //        //$" start  \"{Path.GetFileName(ss.FileName)}\" /D \"{ss.WorkingPath}\" \"{ss.FileName}\" \"{ss.WorkingPath}\" ",
                                //        "exit"
                                //    };
                                string outputResult;
                                resp.Code = exec(ss.FileName, ss.WorkingPath, ss.Para, out outputResult) ? 0 : 1;
                                //resp.Code = exec(command, out outputResult) ? 1 : 0;
                                resp.Msg = outputResult;
                                resp.Data = msg;
                            }
                            else
                            {
                                resp.Code = 1;
                                resp.Msg = "参数无效";
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            else
            {
                //安全性，不支持从外部访问连接，但暂时无法禁止低权限用户使用该接口获得高权限程序运行能力，后续应该增加可使用的程序清单
                resp.Code = 1;
                resp.Msg = $"仅限本机访问{request.LocalEndPoint},{request.RemoteEndPoint}";
            }
            byte[] buff = Encoding.UTF8.GetBytes(Serialize(resp));
            Stream output = response.OutputStream;
            output.Write(buff, 0, buff.Length);
            output.Close();
        }

        public T De<T>(string data, T type)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeAnonymousType(data, type);
        }
        public T De<T>(string data)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(data);
        }
        public string Serialize(object data)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(data, Newtonsoft.Json.Formatting.Indented);
        }
        bool exec(string path, string workPath, string para, out string output)
        {
            try
            {
                output = string.Empty;
                Process process = new Process();//创建进程对象
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = path;
                startInfo.WorkingDirectory = workPath;
                startInfo.Arguments = para;
                process.StartInfo = startInfo;
                process.Start();
                return true;
            }
            catch (Exception ex)
            {
                output = ex.Message;
                return false;
            }
        }
        bool exec(string[] command, out string output)
        {
            output = string.Empty;
            if (command != null && !command.Equals(""))
            {
                Process process = new Process();//创建进程对象
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = "cmd.exe";//设定需要执行的命令
                startInfo.UseShellExecute = false;//不使用系统外壳程序启动
                startInfo.RedirectStandardInput = true;//不重定向输入
                startInfo.RedirectStandardOutput = true; //重定向输出
                startInfo.CreateNoWindow = false;//不创建窗口
                process.StartInfo = startInfo;
                try
                {
                    if (process.Start())//开始进程
                    {
                        process.WaitForExit(5000);
                        foreach (var line in command)
                        {
                            process.StandardInput.WriteLine(line);
                        }
                        output = process.StandardOutput.ReadToEnd();//读取进程的输出
                    }
                }
                catch (Exception ex)
                {
                    output = ex.ToString();
                    return false;
                }
                finally
                {
                    if (process != null)
                        process.Close();
                }
            }

            return true;
        }

        class Resp
        {
            public int Code { get; set; }
            public string Msg { get; set; }
            public object Data { get; set; }
        }

        //[DataContract]
        //class Req
        //{
        //    [DataMember]
        //    public string ActionType { get; set; }
        //    [DataMember]
        //    public object ActionData { get; set; }
        //}

        //[DataContract]
        //class ProcessInfo
        //{
        //    [DataMember]
        //    public string FileName { get; set; }
        //    [DataMember]
        //    public string WorkingPath { get; set; }
        //    [DataMember]
        //    public string Para { get; set; }
        //}
    }
}
