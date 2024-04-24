using System;
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
using System.Windows.Shapes;

// * * * 프로젝트 - NuGet 패키지 관리 - OpenCvSharp4.Windows 설치 * * *

// OpenCv 사용
using OpenCvSharp; // OpenCv 데이터 형식이나 메소드 사용 | Mat 클래스

// Timer 사용 | Thread 클래스 객체
using System.Windows.Threading;

namespace rtRecorder
{
    public partial class MainWindow : System.Windows.Window // OpenCv에도 Window가 있어 모호해지므로 명시
    {
        // 필요한 변수들
        VideoCapture w_cam; // Video 캡처 클래스 객체 | 캠을 의미함
        Mat cam_mat = new Mat(); // 이미지를 담기 위한 Mat 클래스 객체 | 캠의 이미지를 표현하는 곳을 의미함
        DispatcherTimer dt_timer; // WPF UI 스레드를 사용하는 타이머
        bool b_cam, b_timer; // 캠과 타이머와 초기화되었는지 확인하는 bool 값

        VideoWriter intervideo; // VideoWriter 클래스 객체 | 면접 영상
        DispatcherTimer inter_timer; // 면접 영상 타이머

        public MainWindow()
        {
            InitializeComponent(); // 디자이너 단에 정의된 컴포넌트 정의 호출 -> 정의된 대로 표시하기 위해
        } // public MainWindow()

        private void Windows_loaded(object sender, RoutedEventArgs e)
        {
            b_cam = SetCam(); // 카메라 초기화
            b_timer = SetTimer(0.01); // 타이머 간격(0.01ms) 초기화
            
            if (b_cam && b_timer) dt_timer.Start(); // 위 초기화 완료 시 타이머 실행
        } // private void Windows_loaded

        private bool SetCam()
        {
            try
            {
                w_cam = new VideoCapture(0); // 카메라 장치 번호는 0
                w_cam.FrameHeight = 1000; // 카메라 프레임 높이
                w_cam.FrameWidth = (int)Img_cam.Width; // 찍힌 것 너비가 카메라 프레임 너비

                return true;
            }

            catch
            {
                return false;
            } // 예외, 초기화 실패 반환
        } // private bool SetCam

        private bool SetTimer(double msec)
        {
            try
            {
                dt_timer = new DispatcherTimer();
                inter_timer = new DispatcherTimer();

                dt_timer.Interval = TimeSpan.FromMilliseconds(msec);
                // 타이머 시간 조정 = TimeSpan(간격으로).FromMilliseconds(지정된 밀리초를)
                // 타이머를 0.01밀리초 간격으로 시간 조정

                dt_timer.Tick += new EventHandler(timer_tick);
                // 타이머 간격 경과 = timer_tick이라는 이벤트 추가
                inter_timer.Tick += new EventHandler(intertimer_tick);

                return true;
            }

            catch
            {
                return false;
            }
        } // private bool SetTimer

        private void timer_tick(object sender, EventArgs e)
        {
            w_cam.Read(cam_mat); // 0번 장비로 만든 VideoCapture 객체에서 cam_mat를 읽음
            Img_cam.Source = OpenCvSharp.WpfExtensions.WriteableBitmapConverter
                .ToWriteableBitmap(cam_mat);
            // 읽어온 Mat 데이터를 Bitmap 데이터로 변경 후 Img_cam에 그려줌
        } // private void timer_tick

        private void intertimer_tick(object sender, EventArgs e)
        {
            try
            {
                intervideo.Write(cam_mat); // 저장할 매트 지정 | VideoWriter.Write(Mat);
            }
            catch
            {
                inter_timer.IsEnabled = false; // 타이머 종료
            }
        } // private void intertimer_tick

        private void Btn_Record_Click(object sender, RoutedEventArgs e)
        {
            string videoname = DateTime.Now.ToString("yyyy-MM-dd-hh시mm분ss초"); // 면접 영상 파일명(현재 시각/~06시~)
            intervideo = new VideoWriter("../../../" + videoname + ".mp4", FourCC.DIVX, 30, cam_mat.Size()); // 영상 저장 이름(경로), 코덱, 프레임, 크기 설정 | VideoWriter(이름, 코덱, 프레임 수, 프레임 크기)
            inter_timer.IsEnabled = true; // 타이머 시작
            
        } // private void Btn_Record_Click

        private void Btn_save_Click(object sender, RoutedEventArgs e)
        {
            inter_timer.IsEnabled = false;
            intervideo.Release(); // VideoWriter 객체 해제
        } // private void Btn_save_Click

    } // public partial class MainWindow

} // namespace rtRecorder
