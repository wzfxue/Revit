﻿using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;

namespace VL.Library
{
    /// <summary>
    /// VL WPF 窗体基类,附加了ESC退出功能
    /// </summary>
    public class VLWindow : Window
    {
        protected VLViewModel ViewModel;

        public VLWindow() { }//页面需要无参数构造函数
        public VLWindow(VLViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = ViewModel;
            //ESC退出
            Loaded += VLWindowBase_Loaded;
            Closing += VLWindowBase_Closing;
            //窗体定位
            Relocate();

            Timer = new System.Windows.Forms.Timer();
            Timer.Interval = 100;
            Timer.Tick += (sender, e) =>
            {
                if (NeedClose)
                {
                    NeedClose = false;
                    ViewModel.Close();
                }
            };
            Timer.Start();
        }

        #region 窗体定位
        public void Relocate()
        {
            WindowStartupLocation = WindowStartupLocation.Manual;
            IntPtr intPtr = Process.GetCurrentProcess().MainWindowHandle;
            var location = new FormMethod().GetPointX(intPtr);
            Left = location.X;
            Top = location.Y;
        } 
        #endregion

        #region ESC退出
        private void VLWindowBase_Loaded(object sender, RoutedEventArgs e)
        {
            Hook();
        }

        private void VLWindowBase_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Timer.Stop();
            Unhook();
        }

        KeyBoardHook KeyBoarHook;

        void Unhook()
        {
            if (KeyBoarHook != null)
            {
                KeyBoarHook.UnHook();
                KeyBoarHook.OnKeyDownEvent -= KeyBoarHook_OnKeyDownEvent;
                GC.Collect();//增加GC防止窗体关闭后,钩子未被卸载.
            }
        }

        void Hook()
        {
            KeyBoarHook = new KeyBoardHook();
            KeyBoarHook.SetHook();
            KeyBoarHook.OnKeyDownEvent += KeyBoarHook_OnKeyDownEvent;
        }

        /// <summary>
        /// 钩子事件,监听ESC关闭窗体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        int KeyBoarHook_OnKeyDownEvent(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (ViewModel != null && ViewModel.IsIdling && e.KeyData == System.Windows.Forms.Keys.Escape)
            {
                //ViewModel.Close();//如果窗体需要阻塞 比如关闭前需要确认 那么即需要改为非阻塞的方式实现
                NeedClose = true;
                return 1;
            }
            return 0;
        }

        bool NeedClose = false;
        System.Windows.Forms.Timer Timer;
        #endregion

        #region 窗体常规处理(关闭)
        protected override void OnClosing(CancelEventArgs e)
        {
            if (ViewModel.IsIdling)
                ViewModel.Closing();
            base.OnClosing(e);
        }
        #endregion
    }

    #region 窗体定位
    //主窗体位置结构类
    public struct LocationRect
    {
        public int Left; //最左坐标
        public int Top; //最上坐标
        public int Right; //最右坐标
        public int Bottom; //最下坐标
    }

    /// <summary>
    /// 界面公共方法
    /// </summary>
    public class FormMethod
    {
        [DllImport("user32.dll")]
        static extern bool GetWindowRect(IntPtr hWnd, ref LocationRect lpRect);

        /// <summary>
        /// 弹窗位置随revit
        /// </summary>
        public System.Drawing.Point GetPointX(IntPtr intPtr)
        {
            LocationRect rect = new LocationRect();
            // IntPtr ptr = comboxHandle.Mng.SearchDialog(intPtr, "MDIClient", 0);
            var mdi = MethodForWin32.FindWindowEx(intPtr, IntPtr.Zero, "MDIClient", null);
            GetWindowRect(mdi, ref rect);
            int width = rect.Right - rect.Left;
            int height = rect.Bottom - rect.Top;
            int x = rect.Left;
            int y = rect.Top;
            return new System.Drawing.Point(x, y);
        }
    }
    #endregion
}
