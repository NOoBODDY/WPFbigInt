using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using WPFbigInt.FileServices;
using WPFbigInt.Models;
using WPFbigInt.Services;

namespace WPFbigInt.ViewModels;

public class MainViewModel: INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private readonly IFileService _fileService;
    private readonly SynchronizationContext _contextUI;
    private readonly IUserNotyfication _userNotyfication;
    private bool isRunnig;
    
    public MainViewModel(IFileService fileService, IUserNotyfication userNotyfication)
    {
        StartButtonName = "Запустить расчет";
        isRunnig = false;
        _contextUI = new SynchronizationContext();
        SynchronizationContext.SetSynchronizationContext(_contextUI);
        _fileService = fileService;
        _userNotyfication = userNotyfication;
        _fileLength = 1;
        
        FileButtonsActive = true;
        
    }

    private void CreateNewCalculator()
    {
        _calculator = new MultyThreadArithmeticCalc(_fileService);
        _calculator.UpdateProgress += UpdateStats;
    }

    private void DisposeCalculator()
    {
        _calculator.Dispose();
        _calculator = null;
    }
    
    private void UpdateStats(int progressValue)
    {
        lock (_contextUI)
        {
            _contextUI.Send((object? state) =>
            {
                ExpressionsDone = progressValue;
                ProgressBarValue = progressValue * 100 / _fileLength;
            }, null);
        }
        
        
    }
    
    private MultyThreadArithmeticCalc _calculator;
    
    private int _fileLength;

    private ObservableCollection<OperationStatistic> _statisticTable;
    public ObservableCollection<OperationStatistic> StatisticTable
    {
        get => _statisticTable;
        set
        {
            _statisticTable = value;
            OnPropertyChanged("StatisticTable");
        }
    }

    private int _expressionsDone;
    public int ExpressionsDone
    {
        get => _expressionsDone;
        set
        {
            _expressionsDone = value;
            OnPropertyChanged();
        }
    }
    
    private double _progressBarValue;
    public double ProgressBarValue
    {
        get => _progressBarValue;
        set
        {
            _progressBarValue = value;
            OnPropertyChanged("ProgressBarValue");
        }
    }

    private string _startButtonName;

    public string StartButtonName
    {
        get => _startButtonName;
        set
        {
            _startButtonName = value;
            OnPropertyChanged();
        }
    }

    private bool _fileButtonsActive;

    public bool FileButtonsActive
    {
        get => _fileButtonsActive;
        set
        {
            _fileButtonsActive = value;
            OnPropertyChanged();
        }
    }

 
    
    public RelayCommand StartStopCalculation
    {
        get
        {
            return new RelayCommand((object obj)=> StartStop());
        }
    }

    private void StartStop()
    {
        if (isRunnig)
        {
            Stop();
        }
        else
        {
            Start();
        }
    }

    private async void Start()
    {
        if (_fileService.IsReadingFileChosen() && _fileService.IsWritingFileChosen())
        {
            CreateNewCalculator();
            StartButtonName = "Остановить расчет";
            FileButtonsActive = false;
            isRunnig = true;
            StartTimer();
            _fileLength = Convert.ToInt32(_fileService.ReadFileLine());
            await _calculator.Start(_fileLength);
            _fileService.CloseReadingFile(); // освобождаем файл
            StatisticTable = _calculator.Statistics; // подгружаем стистику в основном потоке
            foreach (var statistic in StatisticTable)
            {
                statistic.PercentByAllTime = statistic.CalcDuration / CurrentTime * 100;
            }
            isRunnig = false;
            StartButtonName = "Запустить расчет";
            FileButtonsActive = true;
            _userNotyfication.SendMessage("Рассчет окончен");
            DisposeCalculator();
        }
        else
        {
            _userNotyfication.SendMessage("Файлы не выбраны");
        }
    }

    private async void Stop()
    {
        _calculator.Stop();
    }
    
    
    private RelayCommand? _chooseInputFile;
    public RelayCommand ChooseInputFile
    {
        get
        {
            return _chooseInputFile ??= new RelayCommand(
                (obj =>
                {
                    try
                    {
                        _fileService.ChooseOpenFilePath();
                    }
                    catch (Exception e)
                    {
                        _userNotyfication.SendMessage(e.Message);
                    }
                    
                })
            );
        }
    }
    private RelayCommand? _chooseOutputFile;
    public RelayCommand ChooseOutputFile
    {
        get
        {
            return _chooseOutputFile ??= new RelayCommand(
                (obj =>
                {
                    _fileService.ChooseSaveFilePath();
                })
            );
        }
    }

    #region Timer

    private DateTime? _startTime;
    private  TimeSpan _interval = TimeSpan.FromMilliseconds(100);

    private TimeSpan? _currentTime;
    public TimeSpan? CurrentTime
    {
        get => _currentTime;
        set
        {
            _currentTime = value;
            OnPropertyChanged();
        }
    }
    private TimeSpan? _remainigTime;
    public TimeSpan? RemainigTime
    {
        get => _remainigTime;
        set
        {
            _remainigTime = value;
            OnPropertyChanged();
        }
    }


    private async void StartTimer()
    {
        CurrentTime = TimeSpan.Zero;
        RemainigTime = TimeSpan.Zero;
        _startTime = DateTime.Now;
        try
        {
            while (isRunnig)
            {
                CurrentTime = DateTime.Now - _startTime;
                RemainigTime = ExpressionsDone !=0? CurrentTime / ExpressionsDone *( _fileLength - ExpressionsDone ): TimeSpan.Zero ;
            
                await Task.Delay(_interval);
            }
        }
        catch (Exception e)
        {
            isRunnig = false;
            throw;
        }
        
        RemainigTime = TimeSpan.Zero;
    }



    #endregion


}