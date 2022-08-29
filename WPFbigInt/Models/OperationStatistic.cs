using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ArithmeticParser.Operations;

namespace WPFbigInt.Models;

public class OperationStatistic: INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public OperationStatistic(Operations operationName)
    {
        _operationName = operationName;
        _operationAmount = 0;
        _calcDuration = TimeSpan.Zero;
        _percentByAllTime = 0;
    }

    private Operations _operationName;
    public Operations OperationName
    {
        get => _operationName;
        set
        {
            _operationName = value;
            OnPropertyChanged("OperationName");
        }
    }

    private int _operationAmount;

    public int OperationAmount
    {
        get => _operationAmount;
        set
        {
            _operationAmount = value;
            OnPropertyChanged("OperationAmount");
            OnPropertyChanged("MiddleCalcDuration");
        }
    }
    
    private TimeSpan _calcDuration;
    public TimeSpan CalcDuration
    {
        get => _calcDuration;
        set
        {
            _calcDuration = value;
            OnPropertyChanged("CalcDuration");
        }
    }

    public TimeSpan MiddleCalcDuration
    {
        get
        {
            if (! (_operationAmount == 0))
                return _calcDuration / _operationAmount;
            return _calcDuration;
        }
    }

    private double? _percentByAllTime;

    public double? PercentByAllTime
    {
        get => _percentByAllTime;
        set
        {
            _percentByAllTime = value;
            OnPropertyChanged("PercentByAllTime");
        }
    }

}