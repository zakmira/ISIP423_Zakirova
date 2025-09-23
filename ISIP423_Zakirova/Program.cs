using System;
using System.Collections.Generic;
using System.Text;

class TextStatistics
{
    public string Text { get; set; }
    public int WordCount { get; set; }
    public string ShortestWord { get; set; }
    public int SentenceCount { get; set; }
    public int ABcount { get; set; } //гласные и согласные
    public string LongestWord { get; set; }
    public Dictionary<char, int> LetterFreq { get; set; } = new Dictionary<char, int>(); //почитать про char!

    public 

}