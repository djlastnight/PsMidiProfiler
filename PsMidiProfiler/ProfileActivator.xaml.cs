namespace PsMidiProfiler
{
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Windows;
    using Microsoft.Win32;
    using PsMidiProfiler.ViewModels;
    using System.Windows.Input;
    using PsMidiProfiler.Commands;
    using System.Text;
    using System.Collections.Generic;

    /// <summary>
    /// Interaction logic for ProfileActivator.xaml
    /// </summary>
    public partial class ProfileActivator : Window, INotifyPropertyChanged
    {
        private string generatedProfile;

        private string currentPsUserProfile;

        private ICommand activateCommand;

        private ProfileActivator()
        {
            this.InitializeComponent();
            this.CurrentPsUserProfile = this.DefaultPsUserProfile;
        }

        public ProfileActivator(string generatedProfile) 
            : this()
        {
            if (generatedProfile == null)
            {
                throw new ArgumentNullException("generatedProfile");
            }

            this.generatedProfile = generatedProfile;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand ActivateCommand
        {
            get
            {
                if (this.activateCommand == null)
                {
                    this.activateCommand = new RelayCommand(this.OnActivateRequested);
                }

                return this.activateCommand;
            }
        }

        public string CurrentMidiInDevice
        {
            get
            {
                var mainWindow = App.Current.MainWindow;
                if (mainWindow != null)
                {
                    var viewModel = mainWindow.DataContext as MidiViewModel;
                    if (viewModel != null)
                    {
                        return viewModel.CurrentMidiInDevice;
                    }
                }

                return "Your Midi In Device";
            }
        }

        public string GamePath
        {
            get
            {
                RegistryKey key;
                if (Environment.Is64BitOperatingSystem)
                {
                    key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Wow6432Node\Phase Shift");
                }
                else
                {
                    key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Phase Shift");
                }

                if (key != null)
                {
                    return key.GetValue(null).ToString();
                }

                return null;
            }
        }

        public string MidiProfilesIniLocation
        {
            get
            {
                string gamePath = this.GamePath;
                if (gamePath == null)
                {
                    return null;
                }

                string iniLocation = Path.Combine(gamePath, "settings", "midi_profiles.ini");
                if (File.Exists(iniLocation))
                {
                    return iniLocation;
                }

                return null;
            }
        }

        public string[] PsUserProfiles
        {
            get
            {
                string gamePath = this.GamePath;

                if (gamePath == null)
                {
                    return null;
                }

                string profilesFolderLocation = Path.Combine(gamePath, "profiles");
                if (Directory.Exists(profilesFolderLocation))
                {
                    string[] datFiles = Directory.GetFiles(profilesFolderLocation, "*.dat");
                    if (datFiles.Length == 0)
                    {
                        return null;
                    }

                    string[] users = new string[datFiles.Length];

                    for (int i = 0; i < datFiles.Length; i++)
                    {
                        users[i] = Path.GetFileNameWithoutExtension(datFiles[i]);
                    }

                    return users;
                }

                return null;
            }
        }

        public string CurrentPsUserProfile
        {
            get
            {
                return this.currentPsUserProfile;
            }

            set
            {
                this.currentPsUserProfile = value;
                this.OnPropertyChanged("CurrentPsUserProfile");
            }
        }

        public string DefaultPsUserProfile
        {
            get
            {
                string gamePath = this.GamePath;
                if (gamePath == null)
                {
                    return null;
                }

                string configFileLocation = Path.Combine(gamePath, "settings", "config.ini");
                if (!File.Exists(configFileLocation))
                {
                    return null;
                }

                string[] lines = File.ReadAllLines(configFileLocation, Encoding.Unicode);
                string line = lines.LastOrDefault(x => x.StartsWith("defaultprofile", StringComparison.OrdinalIgnoreCase));
                if (line == null)
                {
                    return null;
                }

                if (!line.Contains("="))
                {
                    return null;
                }

                string value = line.Substring(line.IndexOf("\"")).Replace("\"", string.Empty);

                if (value.StartsWith("Profiles\\", StringComparison.OrdinalIgnoreCase))
                {
                    value = value.Substring(9);
                }

                if (value.EndsWith(".dat", StringComparison.OrdinalIgnoreCase))
                {
                    value = value.Substring(0, value.Length - 4);
                }

                return value;
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void OnActivateRequested(object obj)
        {
            if (this.CurrentMidiInDevice == null ||
                this.GamePath == null ||
                this.MidiProfilesIniLocation == null ||
                this.PsUserProfiles == null ||
                this.CurrentPsUserProfile == null)
            {
                MessageBox.Show("Failed to prepare the needed information!");
                return;
            }

            // adding the profile to midi_profies.ini (ASCII encoding)
            var newLines = this.generatedProfile.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            var oldLines = File.ReadAllLines(this.MidiProfilesIniLocation, Encoding.ASCII);

            List<string> lines = new List<string>();
            lines.AddRange(newLines);
            lines.AddRange(oldLines);
            try
            {
                File.WriteAllLines(this.MidiProfilesIniLocation, lines, Encoding.ASCII);
            }
            catch (Exception)
            {
                MessageBox.Show("Failed to write the requested profile!");
                return;
            }

            if (!this.MakeCurrentProfileDefault())
            {
                MessageBox.Show("Failed to change the default profile!");
                return;
            }

            if (!this.ConfigureProfileForMidiLink())
            {
                MessageBox.Show("Failed to configure the choosen profile!");
                return;
            }

            MessageBox.Show("Successfully activated your midi profile.\r\nYou may now start Phase Shift and hit F1.");
        }

        private bool MakeCurrentProfileDefault()
        {
            if (this.CurrentPsUserProfile == this.DefaultPsUserProfile)
            {
                return true;
            }

            string configIniPath = Path.Combine(this.GamePath, "settings", "config.ini");

            return this.IniChangeValue(
                configIniPath,
                "DEFAULTPROFILE",
                string.Format("Profiles\\{0}.dat", this.CurrentPsUserProfile),
                Encoding.Unicode);
        }

        private bool ConfigureProfileForMidiLink()
        {
            var datFileLocation = Path.Combine(this.GamePath, "profiles", this.CurrentPsUserProfile + ".dat");

            // Selecting the first midi profile (index 0)
            bool profileSet = this.IniChangeValue(datFileLocation, "MIDILINKPROFILE", "0", Encoding.Unicode);

            // Using the keyboard, which is device type 3 to create the link
            bool typeSet = this.IniChangeValue(datFileLocation, "MIDILINKTYPE", "3", Encoding.Unicode);

            // Enabling Auto Midi Link option
            bool autoSet = this.IniChangeValue(datFileLocation, "AUTOMIDILINK", "1", Encoding.Unicode);

            // Setting the MIDI In device name
            bool deviceSet = this.IniChangeValue(datFileLocation, "MIDILINKNAME", this.CurrentMidiInDevice, Encoding.Unicode);

            return profileSet && typeSet && autoSet && deviceSet;
        }

        private bool IniChangeValue(string iniPath, string tag, string value, Encoding encoding)
        {
            System.Windows.MessageBox.Show("TODO: Insert the new value, if it does not exsist! Change the drum lane count, when profiling four or five lane drums!");

            if (!File.Exists(iniPath))
            {
                return false;
            }

            string extension = Path.GetExtension(iniPath);

            var lines = File.ReadAllLines(iniPath, encoding);
            var line = lines.LastOrDefault(x => x.StartsWith(tag, StringComparison.OrdinalIgnoreCase));
            if (line == null || !line.Contains("="))
            {
                return false;
            }

            try
            {
                // create backup
                File.WriteAllLines(
                    iniPath.Replace(extension, ".backup"),
                    lines,
                    encoding);
            }
            catch (Exception)
            {
                return false;
            }


            int index = lines.ToList().IndexOf(line);
            lines[index] = string.Format("{0} = \"{1}\"", tag, value);

            try
            {
                // set new profile
                File.WriteAllLines(iniPath, lines, encoding);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}
