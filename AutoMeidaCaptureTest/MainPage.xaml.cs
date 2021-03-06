﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace AutoMeidaCaptureTest
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private LowLagMediaRecording _mediaRecording;
        private MediaCapture _mediaCapture;

        public MainPage()
        {
            this.InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter.ToString() == "rec")
            {
                _mediaCapture = new MediaCapture();
                await _mediaCapture.InitializeAsync();
                _mediaCapture.Failed += _mediaCapture_Failed;
                var myVideo = await Windows.Storage.StorageLibrary.GetLibraryAsync(Windows.Storage.KnownLibraryId.Videos);
                StorageFile file = await myVideo.SaveFolder.CreateFileAsync("video.mp4", CreationCollisionOption.GenerateUniqueName);
                _mediaRecording = await _mediaCapture.PrepareLowLagRecordToStorageFileAsync(
            MediaEncodingProfile.CreateMp4(VideoEncodingQuality.Auto), file);
                await _mediaRecording.StartAsync();
            }
        }

        protected async override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            _mediaCapture.Failed -= _mediaCapture_Failed;
            await _mediaRecording.StopAsync();
        }

        private void _mediaCapture_Failed(MediaCapture sender, MediaCaptureFailedEventArgs errorEventArgs)
        {
            System.Diagnostics.Debug.Write(errorEventArgs.Message);
        }

        private async void BtnClick(object sender, RoutedEventArgs e)
        {
            await _mediaRecording.StopAsync();
        }
    }
}