namespace PsMidiProfiler.Controls
{
    using System;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using PsMidiProfiler.Enums;
    using PsMidiProfiler.Models;
    using radio42.Multimedia.Midi;

    public partial class TwoOctaveKeysMonitor : UserControl, IControllerMonitor, INoteHighlighter, INotifyPropertyChanged
    {
        private PsDevice device;

        private Visibility c3;
        private Visibility csharp3;
        private Visibility d3;
        private Visibility dsharp3;
        private Visibility e3;
        private Visibility f3;
        private Visibility fsharp3;
        private Visibility g3;
        private Visibility gsharp3;
        private Visibility a3;
        private Visibility asharp3;
        private Visibility b3;

        private Visibility c4;
        private Visibility csharp4;
        private Visibility d4;
        private Visibility dsharp4;
        private Visibility e4;
        private Visibility f4;
        private Visibility fsharp4;
        private Visibility g4;
        private Visibility gsharp4;
        private Visibility a4;
        private Visibility asharp4;
        private Visibility b4;

        private Visibility c5;

        public TwoOctaveKeysMonitor()
        {
            this.InitializeComponent();
            this.device = new PsDevice("Two Octave Keys Midi Profile", DeviceType.Piano);

            this.C3 = Visibility.Hidden;
            this.CSharp3 = Visibility.Hidden;
            this.D3 = Visibility.Hidden;
            this.DSharp3 = Visibility.Hidden;
            this.E3 = Visibility.Hidden;
            this.F3 = Visibility.Hidden;
            this.FSharp3 = Visibility.Hidden;
            this.G3 = Visibility.Hidden;
            this.GSharp3 = Visibility.Hidden;
            this.A3 = Visibility.Hidden;
            this.ASharp3 = Visibility.Hidden;
            this.B3 = Visibility.Hidden;

            this.C4 = Visibility.Hidden;
            this.CSharp4 = Visibility.Hidden;
            this.D4 = Visibility.Hidden;
            this.DSharp4 = Visibility.Hidden;
            this.E4 = Visibility.Hidden;
            this.F4 = Visibility.Hidden;
            this.FSharp4 = Visibility.Hidden;
            this.G4 = Visibility.Hidden;
            this.GSharp4 = Visibility.Hidden;
            this.A4 = Visibility.Hidden;
            this.ASharp4 = Visibility.Hidden;
            this.B4 = Visibility.Hidden;

            this.C5 = Visibility.Hidden;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Controller Controller
        {
            get { throw new NotImplementedException(); }
        }

        public PsDevice Device
        {
            get { return this.device; }
        }

        public Visibility C3
        {
            get
            {
                return this.c3;
            }

            set
            {
                this.c3 = value;
                this.OnPropertyChanged("C3");
            }
        }

        public Visibility CSharp3
        {
            get
            {
                return this.csharp3;
            }

            set
            {
                this.csharp3 = value;
                this.OnPropertyChanged("CSharp3");
            }
        }

        public Visibility D3
        {
            get
            {
                return this.d3;
            }

            set
            {
                this.d3 = value;
                this.OnPropertyChanged("D3");
            }
        }

        public Visibility DSharp3
        {
            get
            {
                return this.dsharp3;
            }

            set
            {
                this.dsharp3 = value;
                this.OnPropertyChanged("DSharp3");
            }
        }

        public Visibility E3
        {
            get
            {
                return this.e3;
            }

            set
            {
                this.e3 = value;
                this.OnPropertyChanged("E3");
            }
        }

        public Visibility F3
        {
            get
            {
                return this.f3;
            }

            set
            {
                this.f3 = value;
                this.OnPropertyChanged("F3");
            }
        }

        public Visibility FSharp3
        {
            get
            {
                return this.fsharp3;
            }

            set
            {
                this.fsharp3 = value;
                this.OnPropertyChanged("FSharp3");
            }
        }

        public Visibility G3
        {
            get
            {
                return this.g3;
            }

            set
            {
                this.g3 = value;
                this.OnPropertyChanged("G3");
            }
        }

        public Visibility GSharp3
        {
            get
            {
                return this.gsharp3;
            }

            set
            {
                this.gsharp3 = value;
                this.OnPropertyChanged("GSharp3");
            }
        }

        public Visibility A3
        {
            get
            {
                return this.a3;
            }

            set
            {
                this.a3 = value;
                this.OnPropertyChanged("A3");
            }
        }

        public Visibility ASharp3
        {
            get
            {
                return this.asharp3;
            }

            set
            {
                this.asharp3 = value;
                this.OnPropertyChanged("ASharp3");
            }
        }

        public Visibility B3
        {
            get
            {
                return this.b3;
            }

            set
            {
                this.b3 = value;
                this.OnPropertyChanged("B3");
            }
        }

        public Visibility C4
        {
            get
            {
                return this.c4;
            }

            set
            {
                this.c4 = value;
                this.OnPropertyChanged("C4");
            }
        }

        public Visibility CSharp4
        {
            get
            {
                return this.csharp4;
            }

            set
            {
                this.csharp4 = value;
                this.OnPropertyChanged("CSharp4");
            }
        }

        public Visibility D4
        {
            get
            {
                return this.d4;
            }

            set
            {
                this.d4 = value;
                this.OnPropertyChanged("D4");
            }
        }

        public Visibility DSharp4
        {
            get
            {
                return this.dsharp4;
            }

            set
            {
                this.dsharp4 = value;
                this.OnPropertyChanged("DSharp4");
            }
        }

        public Visibility E4
        {
            get
            {
                return this.e4;
            }

            set
            {
                this.e4 = value;
                this.OnPropertyChanged("E4");
            }
        }

        public Visibility F4
        {
            get
            {
                return this.f4;
            }

            set
            {
                this.f4 = value;
                this.OnPropertyChanged("F4");
            }
        }

        public Visibility FSharp4
        {
            get
            {
                return this.fsharp4;
            }

            set
            {
                this.fsharp4 = value;
                this.OnPropertyChanged("FSharp4");
            }
        }

        public Visibility G4
        {
            get
            {
                return this.g4;
            }

            set
            {
                this.g4 = value;
                this.OnPropertyChanged("G4");
            }
        }

        public Visibility GSharp4
        {
            get
            {
                return this.gsharp4;
            }

            set
            {
                this.gsharp4 = value;
                this.OnPropertyChanged("GSharp4");
            }
        }

        public Visibility A4
        {
            get
            {
                return this.a4;
            }

            set
            {
                this.a4 = value;
                this.OnPropertyChanged("A4");
            }
        }

        public Visibility ASharp4
        {
            get
            {
                return this.asharp4;
            }

            set
            {
                this.asharp4 = value;
                this.OnPropertyChanged("ASharp4");
            }
        }

        public Visibility B4
        {
            get
            {
                return this.b4;
            }

            set
            {
                this.b4 = value;
                this.OnPropertyChanged("B4");
            }
        }

        public Visibility C5
        {
            get
            {
                return this.c5;
            }

            set
            {
                this.c5 = value;
                this.OnPropertyChanged("C5");
            }
        }

        public void HighlightNote(byte note, bool isNoteOn, byte velocity)
        {
            if (note < 48 || note > 72)
            {
                return;
            }

            var result = Helpers.Convert.ToVisibility(isNoteOn);
            switch (note)
            {
                case 48:
                    this.C3 = result;
                    break;
                case 49:
                    this.CSharp3 = result;
                    break;
                case 50:
                    this.D3 = result;
                    break;
                case 51:
                    this.DSharp3 = result;
                    break;
                case 52:
                    this.E3 = result;
                    break;
                case 53:
                    this.F3 = result;
                    break;
                case 54:
                    this.FSharp3 = result;
                    break;
                case 55:
                    this.G3 = result;
                    break;
                case 56:
                    this.GSharp3 = result;
                    break;
                case 57:
                    this.A3 = result;
                    break;
                case 58:
                    this.ASharp3 = result;
                    break;
                case 59:
                    this.B3 = result;
                    break;
                case 60:
                    this.C4 = result;
                    break;
                case 61:
                    this.CSharp4 = result;
                    break;
                case 62:
                    this.D4 = result;
                    break;
                case 63:
                    this.DSharp4 = result;
                    break;
                case 64:
                    this.E4 = result;
                    break;
                case 65:
                    this.F4 = result;
                    break;
                case 66:
                    this.FSharp4 = result;
                    break;
                case 67:
                    this.G4 = result;
                    break;
                case 68:
                    this.GSharp4 = result;
                    break;
                case 69:
                    this.A4 = result;
                    break;
                case 70:
                    this.ASharp4 = result;
                    break;
                case 71:
                    this.B4 = result;
                    break;
                case 72:
                    this.C5 = result;
                    break;
                default:
                    throw new NotImplementedException("Unexpected note to highlight: " + note);
            }

            var status = isNoteOn ? MIDIStatus.NoteOn : MIDIStatus.NoteOff;
            MidiModel.Send(new radio42.Multimedia.Midi.MidiShortMessage(status, 1, note, velocity, 0));
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
