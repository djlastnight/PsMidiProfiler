namespace PsMidiProfiler.Controls
{
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;
    using PsMidiProfiler.Commands;
    using PsMidiProfiler.ViewModels;

    /// <summary>
    /// Interaction logic for ProfilePreviewer.xaml
    /// </summary>
    public partial class ProfilePreviewer : Window, INotifyPropertyChanged
    {
        private string profileText;

        private ICommand tryActivateProfileCommand;

        private ICommand showTutorialCommand;

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

        public ICommand TryActivateProfileCommand
        {
            get
            {
                if (this.tryActivateProfileCommand == null)
                {
                    this.tryActivateProfileCommand = new RelayCommand(this.OnTryActivateProfileRequested);
                }

                return this.tryActivateProfileCommand;
            }
        }

        public ICommand ShowTutorialCommand
        {
            get
            {
                if (this.showTutorialCommand == null)
                {
                    this.showTutorialCommand = new RelayCommand(this.OnShowTutorialRequested);
                }

                return this.showTutorialCommand;
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void OnTryActivateProfileRequested(object obj)
        {
            var activator = new ProfileActivator(this.profileText);
            if (activator.GamePath == null)
            {
                MessageBox.Show(
                    "Failed to retrieve game folder!",
                    "Phase Shift Profiler",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);

                return;
            }

            activator.ShowDialog();
        }

        private void OnShowTutorialRequested(object obj)
        {
            var tutorialWindow = new TutorialWindow();
            tutorialWindow.ShowDialog();
        }
    }
}
