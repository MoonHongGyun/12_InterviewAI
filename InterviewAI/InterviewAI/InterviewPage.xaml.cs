﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
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
using System.Windows.Shapes;

//추가한 헤더
using OpenCvSharp;
using System.Windows.Threading;

namespace InterviewAI
{
    /// <summary>
    /// InterviewPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class InterviewPage : Page
    {
        VideoCapture webCam;
        Mat camMat = new Mat();
        DispatcherTimer dtTimer;
        bool bCam, bTimer;

        public InterviewPage()
        {
            InitializeComponent();
            bCam = SetCam(); // 카메라 초기화
            bTimer = SetTimer(0.01); // 타이머 간격(0.01ms) 초기화

            if (bCam && bTimer) dtTimer.Start();
        }

        private bool SetCam()
        {
            try
            {
                webCam = new VideoCapture(0); // 카메라 장치 번호는 0
                webCam.FrameHeight = 1000; // 카메라 프레임 높이
                webCam.FrameWidth = (int)Img_cam.Width; // 찍힌 것 너비가 카메라 프레임 너비

                return true;
            }

            catch
            {
                return false;
            } // 예외, 초기화 실패 반환
        }

        private bool SetTimer(double msec)
        {
            try
            {
                dtTimer = new DispatcherTimer();
                //inter_timer = new DispatcherTimer(); //영상

                dtTimer.Interval = TimeSpan.FromMilliseconds(msec);
                // 타이머 시간 조정 = TimeSpan(간격으로).FromMilliseconds(지정된 밀리초를)
                // 타이머를 0.01밀리초 간격으로 시간 조정

                dtTimer.Tick += new EventHandler(timer_tick);
                // 타이머 간격 경과 = timer_tick이라는 이벤트 추가
                //inter_timer.Tick += new EventHandler(intertimer_tick);  //영상

                return true;
            }

            catch
            {
                return false;
            }
        }

        private void timer_tick(object sender, EventArgs e)
        {
            webCam.Read(camMat); // 0번 장비로 만든 VideoCapture 객체에서 cam_mat를 읽음
            Img_cam.Source = OpenCvSharp.WpfExtensions.WriteableBitmapConverter
                .ToWriteableBitmap(camMat);
            // 읽어온 Mat 데이터를 Bitmap 데이터로 변경 후 Img_cam에 그려줌
        } // private void timer_tick

        //private void intertimer_tick(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        intervideo.Write(camMat); // 저장할 매트 지정 | VideoWriter.Write(Mat);
        //    }
        //    catch
        //    {
        //        inter_timer.IsEnabled = false; // 타이머 종료
        //    }
        //}

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            Countdown_lb.Visibility = Visibility.Visible;
            
            for (int i = 0; i < 3; i++)
            {
                Countdown_lb.Content = $"{3-i}";
                await Task.Delay(1000);
            }


            Countdown_lb.Visibility = Visibility.Hidden;
            CountdownBack_lb.Visibility = Visibility.Hidden;
            Record_btn.Visibility = Visibility.Hidden;
            Questgroup.Visibility = Visibility.Visible;
        }
    }
}
