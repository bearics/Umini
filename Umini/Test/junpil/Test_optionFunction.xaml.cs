﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Runtime.InteropServices;
using System.Windows.Shapes;
using System.Windows.Interop;
using System.Windows.Forms;
using System.Drawing;
using WpfApp1;
using System.Windows.Threading; // 타이머 사용
using System.Management; // 자동종료 사용
using System.Diagnostics;
using System.IO;
using System.Timers;
using System.ComponentModel;
using System.Runtime.Serialization;



using Umini.Test;

namespace Umini.Test.junpil
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Test_optionFunction : Window
    {
        public System.Windows.Forms.NotifyIcon notify;

        [DllImport("user32.dll")]

        private static extern int RegisterHotKey(int hwnd, int id, int fsModifiers, int vk); // 핫키 등록

        [DllImport("user32.dll")]

        private static extern int UnregisterHotKey(int hwnd, int id); // 핫키 제거
        [DllImport("user32.dll")]
        public static extern IntPtr SendMessageW(IntPtr hWnd, int Msg,IntPtr wParam, IntPtr lParam); //시스템 볼륨 조절

        System.Timers.Timer timer = new System.Timers.Timer();

        private const int APPCOMMAND_VOLUME_MUTE = 0x80000;
        private const int APPCOMMAND_VOLUME_UP = 0xA0000;
        private const int APPCOMMAND_VOLUME_DOWN = 0x90000;
        private const int WM_APPCOMMAND = 0x319;

        private HotKey _hotkey;

        //public System.Windows.Forms.NotifyIcon notify;

        Page1 p1 = new Page1();

        public Test_optionFunction()
        {
            InitializeComponent();
            Window a = new Window();


            Loaded += (s, e) =>   //핫키등록
            {
                _hotkey = new HotKey(ModifierKeys.Control | ModifierKeys.Alt, WpfApp1.Keys.F5, this);
                _hotkey.HotKeyPressed += (k) => Console.Beep();
                _hotkey = new HotKey(ModifierKeys.Control | ModifierKeys.Alt, WpfApp1.Keys.F1, this);
                _hotkey.HotKeyPressed += (k) => VolDown();
                _hotkey = new HotKey(ModifierKeys.Control | ModifierKeys.Alt, WpfApp1.Keys.F2, this);
                _hotkey.HotKeyPressed += (k) => VolUp();
                _hotkey = new HotKey(ModifierKeys.Control | ModifierKeys.Alt, WpfApp1.Keys.F3, this);
                _hotkey.HotKeyPressed += (k) => Mute();
            };
        }

      

        private void Mute()
        {
            SendMessageW(new WindowInteropHelper(this).Handle, WM_APPCOMMAND, new WindowInteropHelper(this).Handle,
                (IntPtr)APPCOMMAND_VOLUME_MUTE);
        }

        private void VolDown()
        {
            SendMessageW(new WindowInteropHelper(this).Handle, WM_APPCOMMAND, new WindowInteropHelper(this).Handle,
                (IntPtr)APPCOMMAND_VOLUME_DOWN);
        }

        private void VolUp()
        {
            SendMessageW(new WindowInteropHelper(this).Handle, WM_APPCOMMAND, new WindowInteropHelper(this).Handle,
                (IntPtr)APPCOMMAND_VOLUME_UP);
        }

        private void NavigationWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e) // 트레이
        {
            if (notify.Visible == true) { }
            else
            {
                string messageBoxText = "트레이로 최소화 하시려면 Yes , 종료는 No입니다.";
                string caption = "트레이";
                MessageBoxButton button = MessageBoxButton.YesNoCancel;
                

                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show(messageBoxText, caption, button);
                switch (messageBoxResult)
                {
                    case MessageBoxResult.Yes: // 트레이로 보냄
                        e.Cancel = true;
                        notify.Visible = true;
                        this.Visibility = Visibility.Hidden;
                        break;
                    case MessageBoxResult.No:
                        // 윈도우 종료
                        break;
                    case MessageBoxResult.Cancel: // 취소
                        e.Cancel = true;
                        break;
                }
            }


            return;
        }
        private void Notify_DoubleClick(object sender, EventArgs e) // 트레이 버튼 더블클릭
        {
            notify.Visible = false;
            this.Visibility = Visibility.Visible;
        }
        private void Notify_Click(object sender, EventArgs e) // 트레이 버튼 클릭
        {
            string messageBoxText = "최대화는 Yes , 종료는 No입니다.";
            string caption = "트레이";
            MessageBoxButton button = MessageBoxButton.YesNoCancel;
            //MessageBoxImage icon = MessageBoxImage.Warning;

            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show(messageBoxText, caption, button);
            switch (messageBoxResult)
            {
                case MessageBoxResult.Yes: // Save document and exit
                    notify.Visible = false;
                    this.Visibility = Visibility.Visible;
                    break;
                case MessageBoxResult.No: // Exit without saving
                    this.Close();
                    break;
                case MessageBoxResult.Cancel: // Don''''''''t exit
                    break;
            }
        }



        private void NavigationWindow_Loaded(object sender, RoutedEventArgs e)
        {
            notify = new System.Windows.Forms.NotifyIcon();
            notify.Icon = Properties.Resources.Icon1;
            notify.Text = "Umini";
            notify.DoubleClick += Notify_DoubleClick;
            notify.MouseClick += Notify_Click;
        }


       
 



        private new void PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {

            if (!(((Key.D0 <= e.Key) && (e.Key <= Key.D9))
                || ((Key.NumPad0 <= e.Key) && (e.Key <= Key.NumPad9))
                || e.Key == Key.Back || e.Key == Key.Right || e.Key == Key.Left || e.Key == Key.Up || e.Key == Key.Down))
            {
                System.Windows.MessageBox.Show("숫자만 입력 해주세요");
                e.Handled = true;
            }
            else
            {

            }

        }

        private void Button_Click(object sender, RoutedEventArgs e) // 알람 버튼
        {

            int inHour = Int32.Parse(Txtb1.Text);
            int inMin = Int32.Parse(Txtb3.Text);
            DateTime nowTime = DateTime.Now;
            DateTime desTime;
            desTime = nowTime;
            if (inHour != 0 && inMin != 0)
                timer.Interval = 1000 * (3600 * inHour) * (60 * inMin);
            else if (inHour == 0 && inMin != 0)
            {
                timer.Interval = 1000 * (60 * inMin);
            }
            else if (inMin == 0)
            {
                timer.Interval = 1000 * (3600 * inHour);
            }
            timer.Elapsed += new ElapsedEventHandler(timer_Event_Alarm);
            timer.Start();

            



            System.Windows.MessageBox.Show(inHour + "시간" + inMin + "분 후에 알람이 울립니다");


        }

 

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            int inHour = Int32.Parse(Txtb2.Text);
            int inMin = Int32.Parse(Txtb4.Text);
            if (inHour != 0 && inMin != 0)
                timer.Interval = 1000 * (3600 * inHour) * (60 * inMin);
            else if (inHour == 0 && inMin != 0)
            {
                timer.Interval = 1000 * (60 * inMin);
            }
            else if (inMin == 0)
            {
                timer.Interval = 1000 * (3600 * inHour);
            }
            timer.Elapsed += new ElapsedEventHandler(timer_Event_Shut);
            timer.Start();
        }


        void timer_Event_Alarm(object sender, ElapsedEventArgs e)
        {
            System.Windows.MessageBox.Show("알람 시간");
            timer.Stop();
        }
        void timer_Event_Shut(object sender, ElapsedEventArgs e)
        {
            timer.Stop();
            Process.Start("shutdown.exe", "-s -t -f 00"); // api 찾기
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e) // Enable TOP MOST
        {
           // App.Current.MainWindow.Topmost = true; -> 메인 윈도우 최상위시
            Window.GetWindow(this).Topmost = true; // 현재 체크박스가 있는 윈도우 최상위시

        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e) //Disable TOP MOST
        {
            //App.Current.MainWindow.Topmost = false;-> 메인 윈도우 최상위 해제시
            Window.GetWindow(this).Topmost = false;// 현재 체크박스가 있는 윈도우 최상위해제시

        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }
    }
}