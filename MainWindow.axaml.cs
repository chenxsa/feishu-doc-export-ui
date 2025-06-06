using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using System.IO;
using System.Text;
using Avalonia.Threading;
using System.Threading.Tasks;

namespace feishu_doc_export
{
    public partial class MainWindow : Window
    { 
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            // this.AttachDevTools(); // Avalonia 11 默认已支持调试工具，可移除
#endif
            var exportBtn = this.FindControl<Button>("ExportBtn");
            exportBtn.Click += ExportBtn_Click;
            var selectExportPathBtn = this.FindControl<Button>("SelectExportPathBtn");
            selectExportPathBtn.Click += async (s, e) =>
            {
                var storageProvider = this.StorageProvider;
                if (storageProvider != null && storageProvider.CanPickFolder)
                {
                    var folder = await storageProvider.OpenFolderPickerAsync(new Avalonia.Platform.Storage.FolderPickerOpenOptions
                    {
                        Title = "选择导出目录",
                        AllowMultiple = false
                    });
                    if (folder != null && folder.Count > 0)
                    {
                        this.FindControl<TextBox>("ExportPathBox").Text = folder[0].Path.LocalPath;
                    }
                }
            };
            var clearLogBtn = this.FindControl<Button>("ClearLogBtn");
            clearLogBtn.Click += (s, e) =>
            {
                this.FindControl<TextBox>("LogBox").Text = string.Empty;
            };
            // 日志重定向
            Console.SetOut(new LogBoxWriter(this));
        }

        private async void ExportBtn_Click(object sender, RoutedEventArgs e)
        {
            var appId = this.FindControl<TextBox>("AppIdBox").Text ?? "";
            var appSecret = this.FindControl<TextBox>("AppSecretBox").Text ?? "";
            var exportType = (this.FindControl<ComboBox>("ExportTypeBox").SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "docx";
            var docType = (this.FindControl<ComboBox>("DocTypeBox").SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "wiki";
            var wikiSpaceId = this.FindControl<TextBox>("WikiSpaceIdBox").Text ?? "";
            var exportPath = this.FindControl<TextBox>("ExportPathBox").Text ?? "";
          
            Console.WriteLine("正在导出...");
            ExportBtnEnable(false);
            try
            {
                await Task.Run(() =>
                {
                    // 这里调用原有的导出逻辑，可通过 GlobalConfig.Init 等方式传参
                    GlobalConfig.AppId = appId;
                    GlobalConfig.AppSecret = appSecret;
                    GlobalConfig.DocSaveType = exportType;
                    GlobalConfig.Type = docType;
                    GlobalConfig.WikiSpaceId = wikiSpaceId;
                    GlobalConfig.ExportPath = exportPath;
                    GlobalConfig.Quit = false;
                    GlobalConfig.InitAsposeLicense();
                    Program.RunExportFromGui();
                });
               Console.WriteLine("导出完成！");
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"导出失败：{ex.Message}");
            }
            ExportBtnEnable(true);
        }

        private void ExportBtnEnable(bool enable)
        {
            var exportBtn = this.FindControl<Button>("ExportBtn");
            exportBtn.IsEnabled = enable;
        }

        // 日志重定向到LogBox
        private class LogBoxWriter : TextWriter
        {
            private readonly MainWindow _window;
            private readonly StringBuilder _buffer = new StringBuilder();
            public LogBoxWriter(MainWindow window)
            {
                _window = window;
            }
            public override Encoding Encoding => Encoding.UTF8;
            public override void Write(char value)
            {
                if (value == '\n')
                {
                    var text = _buffer.ToString();
                    _buffer.Clear();
                    Dispatcher.UIThread.Post(() =>
                    {
                        var logBox = _window.FindControl<TextBox>("LogBox");
                        logBox.Text += text + "\n";
                        logBox.CaretIndex = logBox.Text.Length;
                    });
                }
                else if (value != '\r')
                {
                    _buffer.Append(value);
                }
            }
            public override void Write(string value)
            {
                Dispatcher.UIThread.Post(() =>
                {
                    var logBox = _window.FindControl<TextBox>("LogBox");
                    logBox.Text += value;
                    logBox.CaretIndex = logBox.Text.Length;
                });
            }
        }
    }
}
