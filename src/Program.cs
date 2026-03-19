using System;
using System.Diagnostics;
using APP.Sudoku;


class Program
{
    
    static void Main()
    {
        var sw = Stopwatch.StartNew();

        Sudoku s = new Sudoku();

        sw.Stop();
        Console.WriteLine(sw.ElapsedMilliseconds);

        s.show_boards();
    }
}