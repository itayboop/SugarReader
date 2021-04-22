using System;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using System.Threading;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace SugarReader
{
	public partial class MainForm : Form
	{
		private bool abortUpdater;
		private WebClient client;
		private int currentSugar;
		private object locker;

		public MainForm()
		{
			this.client = new WebClient() { Encoding = Encoding.UTF8 };
			this.FormClosed += (o, e) => { abortUpdater = true; };
			this.locker = new object();
			this.abortUpdater = false;

			InitializeComponent();
			GetCurrentSugarRead();
			Task.Run(MsgBoxAlert);
			Task.Run(AutoUpdate);
		}

		public void MsgBoxAlert()
		{
			while (!this.abortUpdater)
			{
				Monitor.Enter(locker); 
				if (this.currentSugar > Properties.Settings.Default.MaxSugar)
				{
					Monitor.Exit(locker);
					MessageBox.Show("Your sugar is too HIGH! treat it!", "High sugar level alert");
				}
				else if (this.currentSugar < Properties.Settings.Default.MinSugar)
				{
					Monitor.Exit(locker);
					MessageBox.Show("Your sugar is too LOW! treat it!", "Low sugar level alert");
				}
				Thread.Sleep(TimeSpan.FromMinutes(20));
			}
		}

		private void AutoUpdate()
		{
			while (!this.abortUpdater)
			{
				GetCurrentSugarRead();
				Thread.Sleep(TimeSpan.FromMinutes(1));
			}
		}

		private void GetCurrentSugarRead()
		{
			lock (this.locker)
			{
				string jsonData = this.client.DownloadString(Properties.Settings.Default.UserAPIurl);
				dynamic responseObject = JsonConvert.DeserializeObject(jsonData);
				this.currentSugar = 60;
				this.infoLabel.Text = responseObject.full;
			}

			if (this.currentSugar > Properties.Settings.Default.MaxSugar)
			{
				this.infoLabel.ForeColor = System.Drawing.Color.Orange;
			}
			else if (this.currentSugar < Properties.Settings.Default.MinSugar)
			{
				this.infoLabel.ForeColor = System.Drawing.Color.Red;
			}
			else
			{
				this.infoLabel.ForeColor = System.Drawing.Color.Green;
			}
		}
	}
}
