using Eto;
using Eto.Forms;
using TsinghuaNet.Eto.Views;

namespace TsinghuaNet.Eto
{
    public class App : Application
    {
        public App(Platform platform) : base(platform)
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            LocalizeString += App_LocalizeString;
#if DEBUG
            UnhandledException += App_UnhandledException;
#endif
        }

        public override void Run()
        {
            using (var form = new MainForm())
            {
                Run(form);
            }
        }

        private void App_LocalizeString(object sender, LocalizeEventArgs e)
        {
            switch (e.Text)
            {
                case "&File":
                    e.LocalizedText = "文件(&F)";
                    break;
                case "&Help":
                    e.LocalizedText = "帮助(&H)";
                    break;
                case "Quit":
                    e.LocalizedText = "退出";
                    break;
            }
        }

#if DEBUG
        private void App_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show($"发生未捕获的异常：\r\n{e.ExceptionObject.ToString()}\r\n程序即将退出。", "发生异常", MessageBoxType.Error);
            Quit();
        }
#endif
    }
}
