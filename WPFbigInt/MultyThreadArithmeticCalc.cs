using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using WPFbigInt.FileServices;
using ArithmeticParser;
using ArithmeticParser.Operations;
using WPFbigInt.Models;

namespace WPFbigInt;

public class MultyThreadArithmeticCalc : IDisposable
{
    private readonly IFileService _fileService;
    private readonly CancellationTokenSource _cts;
    private const int segmentSize = 1000;
    private Task<string>[] _tasks;
    private int _currentSegmentSize;

    public MultyThreadArithmeticCalc(IFileService fileService)
    {
        _fileService = fileService;
        _cts = new CancellationTokenSource();
        _progresCounter = 0;
        Statistics = new ObservableCollection<OperationStatistic>();

        foreach (var operation in (Operations[])Enum.GetValues(typeof(Operations)))
        {
            Statistics.Add(new OperationStatistic(operation));
        }
    }

    

    #region Propetries

    private int _progresCounter;
    
    public ObservableCollection<OperationStatistic> Statistics { get; set; }

    #endregion

    public event Action<int> UpdateProgress; // обновление прогресса выполнения
    private string HandleLine(string input, CancellationToken token)
    {
        ArithmeticBigIntCalc calculator = new ArithmeticBigIntCalc(input);
        string result;
        if (token.IsCancellationRequested)
            token.ThrowIfCancellationRequested();
        try
        {
            result = calculator.Calc().ToString();
            lock (Statistics)
            {
                
                foreach (var statistic in Statistics)
                {
                    statistic.OperationAmount += calculator.OperaionsCounter[statistic.OperationName];
                    statistic.CalcDuration += calculator.OperaionsDurations[statistic.OperationName];
                }

                
            }
            
        }
        catch (Exception e)
        {
            result = "ОШИБКА";
        }

        lock (Statistics)
        {
            _progresCounter++;
        }
        UpdateProgress?.Invoke(_progresCounter);

        return result;
    }

    private void GenerateTasks(CancellationToken token)
    {
        for (int i = 0; i < _currentSegmentSize; i++)
        {
            string line;
            lock (_fileService)
            {
                line = _fileService.ReadFileLine();
            }

            _tasks[i] = new Task<string>(() => HandleLine(line, token), token);
            _tasks[i].Start();
        }
    }

    private void WriteTasksResults()
    {
        for (int i = 0; i < _currentSegmentSize; i++)
        {
            if (!_tasks[i].IsCanceled)
            {
                lock (_fileService)
                {
                    _fileService.WriteFileLine(_tasks[i].Result);
                }
            }
            
        }
    }

    //обработка возможной остановки потоков
    private void CatchTaskCancel()
    {
        try
        {
            Task.WaitAll(_tasks);
        }
        catch (AggregateException ae)
        {
            //можно добавить обработку отмены, но незачем
        }
    }


    // обработка одного сегмента
    private void HandleSegment(CancellationToken token)
    {
        _tasks = new Task<string>[_currentSegmentSize];

        GenerateTasks(token);
        // нити запустились и работают
        CatchTaskCancel();

        //все отработали, в том же порядке и запишем результат в файл
        WriteTasksResults();
    }

    //разделяем на фиксированные сегменты, чтобы не перегрузить ОЗУ
    private void DivideBySegments(int fileLength, CancellationToken token)
    {
        int segmentRemainig = fileLength;
        while (segmentRemainig > 0)
        {
            if (token.IsCancellationRequested)
                token.ThrowIfCancellationRequested();
            _currentSegmentSize = segmentRemainig > segmentSize ? segmentSize : segmentRemainig;
            HandleSegment(token);
            segmentRemainig -= _currentSegmentSize;
        }
    }

    public async Task Start(int fileLength)
    {
        CancellationToken token = _cts.Token;
        Task task = new Task(() => DivideBySegments(fileLength, token), token);
        task.Start();
        try
        {
            await task; // при остановке работы выбросит ошибку
        }
        catch (OperationCanceledException ae)
        {
            //можно добавить обработку отмены, но незачем
        }
        
    }

    
    public void Stop()
    {
        _cts.Cancel();
    }

    public void Dispose()
    {
        _cts.Dispose();
    }
}