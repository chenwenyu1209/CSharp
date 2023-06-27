using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Drawing;

namespace AutoClick
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();//窗口初始化
        }

        //启动暂停
        ManualResetEvent OnOff = new ManualResetEvent(true);

        // 定义静态类NativeMethods 将DllImport特性应用于mouse_event方法声明上 注：DllImport特性只能用于方法声明上不可以在类中直接使用该特性。
        public static class NativeMethods
        {
            [DllImport("user32.dll")]
            public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);
        }

        //窗口初始化及自动获取系统当前时间功能
        public void Form1_Load(object sender, EventArgs e)
        {

        }


        //点击确定后 获取设定的小时、分钟、秒
        public void button1_Click(object sender, EventArgs e)
        {


            int Tickle = 1;
            if (Tickle == 1 && isPaused == false)
            {
                label11.Text = "功能启用";
                label11.ForeColor = Color.Red;
            }

            //打断While
            int Interrupt = 1;

            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(100);

                // 鼠标常数
                const int MOUSEEVENTF_LEFTDOWN = 0x0002;
                const int MOUSEEVENTF_LEFTUP = 0x0004;
                //将小时、分钟、秒设置为int格式
                int Settime_hour =Convert.ToInt32(0+ textBox1.Text);
                int Settime_minute =Convert.ToInt32(0 + textBox2.Text);
                int Settime_second = Convert.ToInt32(0 + textBox3.Text);
                int Settime_delay = Convert.ToInt32(0 + textBox4.Text);

                Console.WriteLine("设定小时：" + Settime_hour);
                Console.WriteLine("设定分钟：" + Settime_minute);
                Console.WriteLine("设定秒数：" + Settime_second);
                Console.WriteLine("设定秒数：" + Settime_delay);
                while (Interrupt == 1 && !isPaused)
                {
                    TimeZoneInfo chinaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("China Standard Time");
                    DateTime currentChinaTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, chinaTimeZone);
                    Console.WriteLine(currentChinaTime);
                    int currentHour = currentChinaTime.Hour; // 获取当前小时数
                    int currentMinute = currentChinaTime.Minute; // 获取当前分钟数
                    int currentSecond = currentChinaTime.Second; // 获取当前秒钟数


                    //开始判断
                    if (Settime_hour == currentHour && Settime_minute == currentMinute && Settime_second <= currentSecond)
                    {
                        //添加延迟毫秒数
                        Thread.Sleep(Settime_delay);
                        Console.WriteLine("开始点击");
                        for (int i = 0; i < 5; i++)
                        {
                            // 使用 NativeMethods 类的 mouse_event 方法触发鼠标点击
                            // 模拟鼠标左键按下
                            NativeMethods.mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
                            Thread.Sleep(10); // 延时一段时间，确保鼠标点击生效
                            // 模拟鼠标左键释放
                            NativeMethods.mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
                            Interrupt = 0;
                        }
                    }
                }
            });
        }

         

        // 暂停键
        public bool isPaused = false;
        public void button2_Click(object sender, EventArgs e)
        {
            if (isPaused)
            {
                isPaused = false;
                button2.Text = "暂停";
                label11.Text = "请再次点击确定";
                label11.ForeColor = Color.Blue;
            }
            else
            {
                isPaused = true;
                button2.Text = "继续";
                label11.Text = "功能停止";
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.jx3box.com/tool/63302");
        }
    }
}