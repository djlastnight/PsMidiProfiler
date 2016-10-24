namespace PsMidiProfiler
{
    using System;
    using System.Linq;
    using System.Windows;
    using PsMidiProfiler.Controls;
    using PsMidiProfiler.Enums;
    using PsMidiProfiler.ViewModels;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MidiViewModel viewModel;

        public MainWindow()
        {
            this.InitializeComponent();
            this.viewModel = this.DataContext as MidiViewModel;

            if (this.viewModel.MidiInDevices == null || this.viewModel.MidiInDevices.Count() == 0)
            {
                System.Windows.MessageBox.Show(
                    "No MIDI In Devices found! Terminating application.",
                    "Phase Shift MIDI Profiler by djlastnight",
                    MessageBoxButton.OK,
                    MessageBoxImage.Stop);

                Environment.Exit(0);
            }

            this.viewModel.ProfileGenerated += this.OnMidiProfileGenerated;
        }

        private void OnMidiProfileGenerated(object sender, ProfileGeneratedEventArgs e)
        {
            if (e.Profile.ErrorType != MidiProfileErrorType.NoError)
            {
                if (e.Profile.ErrorType == MidiProfileErrorType.NoButtonsDefined &&
                    this.viewModel.ControllerMonitor is CustomControllerMonitor)
                {
                    string message = "Warning: You are about to create MIDI profile,\r\n" +
                        "which contains no button data.\r\n\r\n" +
                        "Please confirm that you know what you are doing.";

                    var prompt = MessageBox.Show(
                        message,
                        "Confirm profile creation",
                        MessageBoxButton.YesNoCancel,
                        MessageBoxImage.Warning);

                    if (prompt == MessageBoxResult.Yes)
                    {
                        this.viewModel.GenerateProfileCommand.Execute(false);
                    }

                    return;
                }

                MessageBox.Show(
                    e.Profile.ErrorText,
                    "Phase Shift MIDI Profile®",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);

                return;
            }

            var previewer = new ProfilePreviewer(this.viewModel.ControllerMonitor, e.Profile.ProfileText);
            previewer.ShowDialog();
        }
    }
}