using System;
using System.Linq;
using System.Windows;

namespace PsMidiProfiler
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var viewModel = this.DataContext as MidiViewModel;
            if (viewModel.MidiInDevices == null || viewModel.MidiInDevices.Count() == 0)
            {
                System.Windows.MessageBox.Show(
                    "No MIDI In Devices found! Terminating application.",
                    "Phase Shift MIDI Profiler by djlastnight",
                    MessageBoxButton.OK,
                    MessageBoxImage.Stop);

                Environment.Exit(0);
            }

            viewModel.ProfileGenerated += this.OnMidiProfileGenerated;
        }

        void OnMidiProfileGenerated(object sender, ProfileGeneratedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(
                    e.Error,
                    "Phase Shift MIDI Profile®",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);

                return;
            }

            var previewer = new ProfilePreviewer(e.ProfileText);
            previewer.ShowDialog();
        }
    }
}