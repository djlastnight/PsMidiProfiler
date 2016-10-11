namespace PsMidiProfiler.Controls
{
    using System;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for MonitorButton.xaml
    /// </summary>
    public partial class MonitorButton : UserControl, INotifyPropertyChanged
    {
        private PsProfileButton profileButton;

        public MonitorButton()
        {
            this.InitializeComponent();
        }

        public MonitorButton(PsProfileButton profileButton)
            : this()
        {
            this.ProfileButton = profileButton;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler Cleared;

        public PsProfileButton ProfileButton
        {
            get
            {
                return this.profileButton;
            }

            set
            {
                this.profileButton = value;
                this.OnPropertyChanged("ProfileButton");
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void DetectNoteClicked(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow;
            if (mainWindow == null)
            {
                return;
            }

            var detector = new NoteDetector(mainWindow.DataContext as MidiViewModel, this.profileButton.Name);
            detector.NoteDetected += (ss, ee) =>
                {
                    this.profileButton.Note = ee.DetectedNote;
                    this.profileButton.Channel = ee.Channel;
                    this.profileButton.NoteOffValue = ee.NoteOffValue;
                    this.ProfileButton = profileButton;
                };

            detector.Owner = mainWindow;
            detector.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            detector.ShowDialog();
        }

        private void ClearClicked(object sender, RoutedEventArgs e)
        {
            this.profileButton.Note = 0;
            this.profileButton.Channel = 0;
            this.profileButton.NoteOffValue = 0;
            this.ProfileButton = this.profileButton;

            if (this.Cleared != null)
            {
                this.Cleared(this, EventArgs.Empty);
            }
        }
    }
}
