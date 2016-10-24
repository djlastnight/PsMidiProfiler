namespace PsMidiProfiler
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Windows;
    using System.Windows.Input;
    using Microsoft.Win32;
    using PsMidiProfiler.Commands;
    using PsMidiProfiler.ViewModels;

    public partial class ProfileActivator : Window, INotifyPropertyChanged
    {
        private readonly int drumLaneCount;

        private string generatedProfile;

        private string currentPsUserProfile;

        private ICommand activateCommand;

        private string debugText;

        public ProfileActivator(IControllerMonitor monitor, string generatedProfile) 
            : this()
        {
            if (monitor == null)
            {
                throw new ArgumentNullException("monitor");
            }

            if (generatedProfile == null)
            {
                throw new ArgumentNullException("generatedProfile");
            }

            this.generatedProfile = generatedProfile;

            if (monitor is Controls.FourLaneDrumsMonitor)
            {
                this.drumLaneCount = 4;
            }
            else if (monitor is Controls.FiveLaneDrumsMonitor)
            {
                this.drumLaneCount = 5;
            }
            else
            {
                this.drumLaneCount = 0;
            }
        }

        private ProfileActivator()
        {
            this.InitializeComponent();
            var defaultProfile = this.DefaultPsUserProfile;
            this.CurrentPsUserProfile = this.PsUserProfiles.Contains(defaultProfile) ? defaultProfile : null;
            this.DebugText = string.Empty;
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

        public string DebugText
        {
            get
            {
                return this.debugText;
            }

            set
            {
                this.debugText = value + Environment.NewLine;
                this.OnPropertyChanged("DebugText");
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
                MessageBox.Show(
                    "Failed to prepare the needed information! Please activate your midi profile manually!",
                    "Phase Shift MIDI Profiler",
                    MessageBoxButton.OK,
                    MessageBoxImage.Stop);

                return;
            }

            if (!this.ConfigureProfileForMidiLink())
            {
                return;
            }

            this.CreateBackup(Path.Combine(this.GamePath, "settings", "config.ini"));

            if (!this.MakeCurrentProfileDefault())
            {
                this.UndoProfileChanges();
                return;
            }

            if (!this.ChangeDrumLaneCount())
            {
                this.UndoProfileChanges();
                this.UndoConfigChanges();
                return;
            }

            if (!this.AddMidiProfile())
            {
                this.UndoProfileChanges();
                this.UndoConfigChanges();
                return;
            }

            MessageBox.Show(
                "MIDI Profile Activated.\r\nYou may now start Phase Shift and hit F1 to start the midi link.\r\n" +
                "After this use can your instrument. Have fun!");
            this.Close();
        }

        private bool ConfigureProfileForMidiLink()
        {
            var profileFileLocation = Path.Combine(this.GamePath, "profiles", this.CurrentPsUserProfile + ".dat");

            if (!this.CreateBackup(profileFileLocation))
            {
                this.DebugText += "Failed to create backup of selected profile!";
                return false;
            }

            // Selecting the first midi profile (index 0)
            bool profileSet = this.ChangeIniValue(profileFileLocation, "MIDILINKPROFILE", "0", Encoding.Unicode);

            // Using the keyboard, which is device type 3 to create the link
            bool typeSet = this.ChangeIniValue(profileFileLocation, "MIDILINKTYPE", "3", Encoding.Unicode);

            // Enabling Auto Midi Link option
            bool autoSet = this.ChangeIniValue(profileFileLocation, "AUTOMIDILINK", "1", Encoding.Unicode);

            // Setting the MIDI In device name
            bool deviceSet = this.ChangeIniValue(profileFileLocation, "MIDILINKNAME", this.CurrentMidiInDevice, Encoding.Unicode);

            return profileSet && typeSet && autoSet && deviceSet;
        }

        private bool MakeCurrentProfileDefault()
        {
            if (this.CurrentPsUserProfile == this.DefaultPsUserProfile)
            {
                this.DebugText += string.Format("{0}'s profile is already the default profile.", this.currentPsUserProfile);
                return true;
            }

            string configIniPath = Path.Combine(this.GamePath, "settings", "config.ini");

            if (!this.CreateBackup(configIniPath))
            {
                this.DebugText += "Failed to create backup of config.ini";
                return false;
            }

            return this.ChangeIniValue(
                configIniPath,
                "DEFAULTPROFILE",
                string.Format("Profiles\\{0}.dat", this.CurrentPsUserProfile),
                Encoding.Unicode);
        }

        private bool ChangeDrumLaneCount()
        {
            string configPath = Path.Combine(this.GamePath, "settings", "config.ini");
            if (this.drumLaneCount == 4 || this.drumLaneCount == 5)
            {
                return this.ChangeIniValue(
                    configPath,
                    "KEYBOARDDRUMLANECOUNT",
                    this.drumLaneCount.ToString(),
                    Encoding.Unicode);
            }

            return true;
        }

        private bool AddMidiProfile()
        {
            if (!this.CreateBackup(this.MidiProfilesIniLocation))
            {
                this.DebugText += "Failed to create backup of midi_profiles.ini";
                return false;
            }

            // midi_profiles.ini uses ASCII encoding
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
                this.DebugText += "Failed to write the requested profile to midi_profiles.ini!";
                return false;
            }

            this.DebugText += "Successfully added the requested profile to midi_profiles.ini";
            return true;
        }

        private void UndoProfileChanges()
        {
            var originalPath = Path.Combine(this.GamePath, "profiles", this.currentPsUserProfile + ".dat");
            var backupPath = Path.Combine(this.GamePath, "profiles", this.currentPsUserProfile + ".backup");

            try
            {
                File.Copy(backupPath, originalPath, true);
                File.Delete(backupPath);
            }
            catch (Exception)
            {
            }
        }

        private void UndoConfigChanges()
        {
            string originalPath = Path.Combine(this.GamePath, "settings", "config.ini");
            string backupPath = Path.Combine(this.GamePath, "settings", "config.backup");

            try
            {
                File.Copy(backupPath, originalPath, true);
                File.Delete(backupPath);
            }
            catch (Exception)
            {
            }
        }

        private bool ChangeIniValue(string iniPath, string tag, string value, Encoding encoding)
        {
            if (!File.Exists(iniPath))
            {
                this.DebugText += string.Format("Fatal Error: File not found: {0}", iniPath);
                return false;
            }

            string fileName = Path.GetFileName(iniPath);
            string extension = Path.GetExtension(iniPath);

            var lines = File.ReadAllLines(iniPath, encoding);
            var line = lines.LastOrDefault(x => x.StartsWith(tag, StringComparison.OrdinalIgnoreCase));
            if (line == null || !line.Contains("="))
            {
                this.DebugText += string.Format("Failed to find {0} tag at {1}", tag, fileName);
                return false;
            }

            int index = lines.ToList().IndexOf(line);
            lines[index] = string.Format("{0} = \"{1}\"", tag, value);

            try
            {
                // set new profile
                File.WriteAllLines(iniPath, lines, encoding);
            }
            catch (Exception ex)
            {
                this.DebugText += string.Format("Failed to change {0} value. File {1}. Error: {2}", tag, fileName, ex.Message);
                return false;
            }

            this.DebugText += string.Format("Successfully changed {0} to {1}. [{2}].", tag, value, fileName);
            return true;
        }

        private bool CreateBackup(string filePath)
        {
            try
            {
                string extension = Path.GetExtension(filePath);
                File.Copy(filePath, filePath.Replace(extension, ".backup"), true);
            }
            catch (Exception ex)
            {
                this.DebugText += string.Format("Failed to create file backup: \"{0}\"", ex.Message);
                return false;
            }

            this.DebugText += string.Format("Successfully created backup of \"{0}\"", Path.GetFileName(filePath));
            return true;
        }
    }
}