

[STAThread]
static void Main()
{
    Application.EnableVisualStyles();
    Application.SetCompatibleTextRenderingDefault(false);
    //请在启动函数中添加
	QCommon.UI.Common.Setting.Default();
    Application.Run(new  App.FormX());
}


版本升级时，已修改的文件在移除NUGET包里会自动跳过 ，升级时会提示覆盖，请谨慎选择