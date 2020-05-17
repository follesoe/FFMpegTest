using FFmpegInterop;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Playback;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace FFMpegTest
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class MainPage : Page
	{
		private FFmpegInteropMSS FFmpegMSS;
		private MediaPlaybackItem playbackItem;

		public FFmpegInteropConfig Config { get; set; }

		public MainPage()
		{
			Config = new FFmpegInteropConfig();
			Config.DefaultBufferTimeUri = TimeSpan.FromMilliseconds(50);
			Config.StreamBufferSize = 0;
			Config.FFmpegOptions.Add("stimeout", 100000);
			Config.FFmpegOptions.Add("rtsp_transport", "udp");
			Config.FFmpegOptions.Add("reorder_queue_size", 64);
			//Config.FFmpegOptions.Add("packet-buffering", 0);
			//Config.FFmpegOptions.Add("fflags", "nobuffer");
			//Config.FFmpegOptions.Add("probesize", 32);
			//Config.FFmpegOptions.Add("sync", "ext");

			this.InitializeComponent();
		}

		private void MediaFailed(object sender, ExceptionRoutedEventArgs e)
		{
			Debug.WriteLine($"Media Failed: {e.ErrorMessage}");
		}

		private void MediaOpened(object sender, RoutedEventArgs e)
		{
			Debug.WriteLine("Media Opened");
		}

		private async void StartPlaybackClick(object sender, RoutedEventArgs e)
		{
			Debug.WriteLine("Start Playback");

			try
			{
				mediaElement.Stop();
				FFmpegMSS = await FFmpegInteropMSS.CreateFromUriAsync(url.Text, Config);
				var source = FFmpegMSS.CreateMediaPlaybackItem();

				// Pass MediaStreamSource to Media Element
				mediaElement.SetPlaybackSource(source);

				// Close control panel after opening media
				Splitter.IsPaneOpen = false;
			}
			catch(Exception ex)
			{
				Debug.WriteLine($"Error starting playback : {ex.Message}");
			}
		}

		private void BufferingProgressChanged(object sender, RoutedEventArgs e)
		{
			Debug.WriteLine($"Buffering progress: {mediaElement.BufferingProgress*100}%");
		}
	}
}
