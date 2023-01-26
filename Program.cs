using System;
using System.Globalization;
using System.Reflection.Metadata;

class Program
{
    private static void Main()
    {
        string[] text = File.ReadAllLines("file.txt");
        int startSum = int.Parse(text[0]);
        string[] sumOutStart = GetSumWithoutStart(text);
        string[][] timeSumOp = GetTimeSumOperation(sumOutStart);
        long[] time = FillArrayTime(timeSumOp);
        string[][] sorted = Sort(time, timeSumOp);
        DoIfCorrect(sorted, startSum);
    }

    private static long[] FillArrayTime(string[][] timeSumOp)
    {
        long[] time = new long[timeSumOp.Length];
        for (int i = 0; i < timeSumOp.Length; i++)
        {
            time[i] = GetTime(timeSumOp[i][0]);
        }
        return time;
    }

    private static void DoIfCorrect(string[][] sorted, int startSum)
    {
        bool correct = CheckCorrect(sorted, startSum);
        switch(correct)
        {
            case true:
                DoUserParameters(sorted, startSum);
                break;
            case false:
                Console.WriteLine("Файл некорректен");
                break;
        }
    }

    private static string[] GetSumWithoutStart(string[] sum)
    {
        string[] sumOutStart = new string[sum.Length - 1];
        for (int i = 0; i < sumOutStart.Length; i++)
        {
            sumOutStart[i] = sum[i + 1];
        }
        return sumOutStart;
    }

    private static string[][] GetTimeSumOperation(string[] sumOutStart)
    {

        string[][] timeSumOp = new string[sumOutStart.Length][];
        for (int i = 0; i < sumOutStart.Length; i++)
        {
            timeSumOp[i] = new string[3];
            string[] operation = sumOutStart[i].Replace(" | ", "|").Split('|');
            for (int j = 0; j < operation.Length; j++)
                timeSumOp[i][j] = operation[j];
        }
        return timeSumOp;
    }

    private static long GetTime(string time)
    {
        time = time.Replace("-", "").Replace(" ", "").Replace(":", "");
        long allTime = long.Parse(time);
        return allTime;
    }

    private static string[][] Sort(long[] time, string[][] sort)
    {
        int[] array = new int[time.Length];
        for (int i = 0; i < time.Length; i++)
        {
            int index = Array.IndexOf(time, time.Min());
            array[i] = index;
            time[index] = 999999999999;
        }
        string[][] sorted = new string[sort.Length][];
        for (int i = 0; i < array.Length; i++)
        {
            int k = array[i];
            sorted[i] = new string[sort[k].Length];
            sorted[i] = sort[k];
        }
        return sorted;
    }

    private static int FindEndSum(string[][] sum, int startSum)
    {
        for (int i = 0; i < sum.Length; i++)
        {
            switch (sum[i][1])
            {
                case "revert":
                    startSum = DoOperationWithRevert(sum[i - 1][2], sum[i - 1][1],startSum);
                    break;
                default:
                    startSum = DoOperation(sum[i][2], sum[i][1], startSum);
                    break;
            }
        }
        return startSum;
    }

    private static int DoOperationWithRevert(string first,string second, int startSum)
    {
        switch (first)
        {
            case "in":
                startSum -= int.Parse(second);
                break;
            case "out":
                startSum += int.Parse(second);
                break;
        }
        return startSum;
    }
    private static int DoOperation(string first,string second, int startSum)
    {
        switch (first)
        {
            case "in":
                startSum += int.Parse(second);
                break;
            case "out":
                startSum -= int.Parse(second);
                break;
        }
        return startSum;
    }

    private static int FindSumInTime(string[][] sum, long time, int startSum)
    {
        for (int i = 0; i < sum.Length; i++)
        {
            long allTime = GetTime(sum[i][0]);
            if (allTime <= time)
            {
                switch (sum[i][1])
                {
                    case "revert":
                        startSum = DoOperationWithRevert(sum[i - 1][2], sum[i - 1][1], startSum);
                        break;
                    default:
                        startSum = DoOperation(sum[i][2], sum[i][1], startSum);
                        break;
                }
            }
            else
            {
                return startSum;
            }
        }
        return startSum;
    }

    private static bool CheckCorrect(string[][] sum, int startSum)
    {
        for (int i = 0; i < sum.Length; i++)
        {
            switch (sum[i][1])
            {
                case "revert":
                    if (i == 0) return false;
                    else if (sum[i - 1][1] == "revert") return false;
                    else startSum = DoInOut(sum[i - 1][2], sum[i - 1][1], startSum);
                    break;
            
                default:
                    startSum = DoInOutSecond(sum[i][2], sum[i][1], startSum);
                    if (startSum < 0) return false;
                    break;

            }
        }
        return true;
    }

    private static int DoInOutSecond(string first, string second, int startSum)
    {
        switch (first)
        {
            case "in":
                startSum += Convert.ToInt32(second);
                break;
            default:
                startSum -= Convert.ToInt32(second);
                break;
        }
        return startSum;
    }

    private static int DoInOut(string first, string second, int startSum)
    {
        switch (first)
        {
            case "in":
                startSum -= Convert.ToInt32(second);
                break;
            default:
                startSum += Convert.ToInt32(second);
                break;
        }
        return startSum;
    }
    private static void DoUserParameters(string[][] sum, int startSum)
    {
        string firstTime = sum[0][0];
        long first = GetTime(firstTime);
        Console.WriteLine("Введите дату и время в формате: YYYY-MM-DD HH:MM");
        Console.WriteLine("Нажмите Enter, чтобы узнать итоговый остаток средств");
        string date = Console.ReadLine();
        if (date == "")
        {
            Console.WriteLine(FindEndSum(sum, startSum));
        }
        else
        {
            long userDate = GetTime(date);
            Console.WriteLine(FindSumInTime(sum, GetTime(date), startSum));
        }
    }
}