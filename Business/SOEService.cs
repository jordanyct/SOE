using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Business
{
    public class SOEService
    {
        public string RemoveWords(List<string> stopWordsList, string inputText)
        {
            string outputText = inputText;
            foreach (string word in stopWordsList)
            {
                string regexp = @"(?i)\s?\b" + word + @"\b\s?";                
                outputText = Regex.Replace(outputText, regexp, "");
            }
            return outputText;
        }

        public int ExternalLink(string inputText)
        {
            var linkParser = new Regex(@"\b(?:http[s]?://)\S+\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);            
            return linkParser.Matches(inputText).Count;
        }

        public Dictionary<string, int> CountOccurrences(string inputText)
        {
            var wordList = Regex.Split(inputText, @"\W+");
            Dictionary<string, int> Occurrences = new Dictionary<string, int>();
            foreach (string s in wordList.Distinct().ToList())
            {
                if(!string.IsNullOrWhiteSpace(s))
                Occurrences.Add(s, wordList.Count(x => x.Equals(s)));
            }
            return Occurrences;
        }

        public Dictionary<string, int> CountMetaTagOccurrences(string inputText)
        {
            Dictionary<string, int> Occurrences = null;
            var wordList = Regex.Split(inputText, @"\W+");
            Match KeywordMatch = Regex.Match(inputText, "<meta name=\"keywords\" content=\"([^<]*)\">", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            if (KeywordMatch.Length > 0)
            {
                Occurrences = new Dictionary<string, int>();
                var metaKeywords = KeywordMatch.Groups[1].Value;
                if (!string.IsNullOrEmpty(metaKeywords))
                {
                    var metaList = metaKeywords.Split(',').Distinct();                    
                    foreach (string s in metaList)
                    {
                        Occurrences.Add(s, wordList.Count(x => x.Equals(s)));
                    }
                }
            }
            return Occurrences;
        }
    }
}
