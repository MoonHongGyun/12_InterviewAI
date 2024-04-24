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

// * * * 프로젝트 - NuGet 패키지 관리 - NAudio 설치 * * *

// NAudio 사용
using NAudio.CoreAudioApi; // NAudio 라이브러리 이용
using NAudio.Wave;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace VRecorder
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        // 음성 파일 경로 | 부모 디렉터리 반환(현재 경로).부모(상위).부모(상위).경로까지
        string saveWavPath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "\\";

        // WaveInEvent micIn_low = new WaveInEvent(); // 음성을 잡는 객체 | 저음질, 128bkps, 8khz

        WaveFileWriter wavWriter = null; // 음성 파일을 기록하는 객체(경로, 포맷)

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Rec_btn_Click(object sender, RoutedEventArgs e)
        {
            WasapiCapture micIn = new WasapiCapture(); // 음성을 잡는 객체 | 512kbps, 16khz

            // 음성 파일 기록 경로 | 경로 결합.음성 파일 경로 + 현재 시각_rec_voice.wav
            string wavFilePath = System.IO.Path.Combine(saveWavPath, DateTime.Now.ToString("yyyy-MM-dd-hh시mm분ss초") + "_voice.wav");

            wavWriter = new WaveFileWriter(wavFilePath, micIn.WaveFormat); // 음성 파일 기록(경로, 이벤트 객체.포맷)
            micIn.StartRecording();

            micIn.DataAvailable += (s, voice) =>
            {
                wavWriter.Write(voice.Buffer, 0, voice.BytesRecorded); // 음성 파일 저장(버퍼, 버퍼 시작, 버퍼 바이트 수)
                if (wavWriter.Position > micIn.WaveFormat.AverageBytesPerSecond * 10) // 10초가 지나면
                {
                    micIn.StopRecording();
                    wavWriter?.Dispose();
                    wavWriter = null;
                    micIn.Dispose();
                    // 녹음 중단 및 객체 해제
                }
            }; // 마이크 정보(소리)가 있으면
        }
    }
}
