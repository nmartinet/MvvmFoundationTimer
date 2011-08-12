using MvvmFoundation.Wpf;
using System.Timers;
using System.Windows.Input;

namespace MvvmFoundationTimer.ViewModels
{
    
    class TimerViewModel : ObservableObject
    {
        #region Observable Properties
        
        public string TimeString {
            get {
                int m = _time / 60;
                int s = _time % 60;
                return string.Format("{0:00}:{1:00}", m, s ); 
            }
        }
        
        public int InputMinutes {
            get { return this._inputMinutes; }
            set {
                this._inputMinutes = value;
                base.RaisePropertyChanged("InputMinutes");
            }
        }

        public int InputSeconds {
            get { return this._inputSeconds; }
            set {
                this._inputSeconds = value;
                base.RaisePropertyChanged("InputSeconds");
            }
        }

        #endregion

        #region Commands

        public ICommand StartCommand {
            get {return _startTimer ?? (_startTimer = new RelayCommand(() => _startTimerMethod())); }        
        }
        public ICommand StopCommand
        {
            get { return _stopTimer ?? (_stopTimer = new RelayCommand(() => _stopTimerMethod())); }
        }


        #endregion

        #region Fields
        
        RelayCommand _startTimer;
        RelayCommand _stopTimer;
        int _inputMinutes = 1;
        int _inputSeconds = 0;
        int _time = 60;
        private Timer _timer = new Timer();
        
        #endregion

        #region Methods

        private void _startTimerMethod() {
            _time = _inputMinutes * 60 + _inputSeconds;
            _timer.Interval = 1000;
            _timer.Elapsed += new ElapsedEventHandler(_tick);
            _timer.Start();
            base.RaisePropertyChanged("TimeString");
        }

        private void _stopTimerMethod() {
            _stopping();
        }

        private void _tick(object sender, ElapsedEventArgs e) {
            if (_time > 0) {
                _time -= 1;
                base.RaisePropertyChanged("TimeString");
            } else {

                _timerEnd();
                _stopTimerMethod();
                
            }
        }

        private void _stopping() {
            _timer.Stop();
            _timer.Elapsed -= new ElapsedEventHandler(_tick);
        }

        private void _timerEnd() {
            System.Media.SystemSounds.Beep.Play();
        }

        #endregion 

    }

}
