namespace PsMidiProfiler.Controls
{
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;
    using PsMidiProfiler.Commands;

    /// <summary>
    /// Interaction logic for ProfilePreviewer.xaml
    /// </summary>
    public partial class ProfilePreviewer : Window, INotifyPropertyChanged
    {
        private string profileText;

        private ICommand copyToClipboardCommand;

        public ProfilePreviewer(string profileText)
            : this()
        {
            this.ProfileText = profileText;
        }

        private ProfilePreviewer()
        {
            this.InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string ProfileText
        {
            get
            {
                return this.profileText;
            }

            set
            {
                this.profileText = value;
                this.OnPropertyChanged("ProfileText");
            }
        }

        public ICommand CopyToClipboardCommand
        {
            get
            {
                if (this.copyToClipboardCommand == null)
                {
                    this.copyToClipboardCommand = new RelayCommand(this.OnCopyToClipboardRequested);
                }

                return this.copyToClipboardCommand;
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void OnCopyToClipboardRequested(object obj)
        {
            if (this.profileText == null)
            {
                System.Windows.MessageBox.Show(
                    "Copy to clipboard failed!",
                    "Phase Shift Midi Profiler",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            Clipboard.SetText(this.profileText);
        }
    }
}
