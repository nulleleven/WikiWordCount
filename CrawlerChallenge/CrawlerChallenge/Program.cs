using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace CrawlerChallenge
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Enter words to exclude, seperated by a single space: ");
            var excludes = Console.ReadLine().Split(' ');

            var stringCounts = new Dictionary<string, int>();
            var client = new WebClient();

            var pageString = client.DownloadString("https://en.wikipedia.org/wiki/Microsoft");
            pageString = pageString.Substring(pageString.IndexOf("id=\"History\">History</span>"));
            pageString = pageString.Substring(0, pageString.IndexOf("Corporate affairs</span>"));
            pageString = StripHtml(pageString);
            var pageStringList = pageString.Split(' ').ToList();

            foreach (var subString in pageStringList)
            {
                if (!string.IsNullOrWhiteSpace(subString) && !excludes.Contains(subString))
                {
                    if (stringCounts.ContainsKey(subString))
                    {
                        stringCounts[subString]++;
                    }
                    else
                    {
                        stringCounts.Add(subString, 1);
                    }
                }
            }

            var orderedStringCounts = stringCounts.OrderBy(key => key.Value).Reverse().ToList();

            Console.WriteLine("Top occurring words...");
            for (var i = 0; i < 10; i++)
            {
                Console.WriteLine($"{i + 1}: {orderedStringCounts[i].Key} {orderedStringCounts[i].Value}");
            }

            Console.ReadLine();
        }

        public static string StripHtml(string page)
        {
            char[] array = new char[page.Length];
            int arrayIndex = 0;
            bool inside = false;

            for (int i = 0; i < page.Length; i++)
            {
                char let = page[i];
                if (let == '<')
                {
                    inside = true;
                    continue;
                }
                if (let == '>')
                {
                    inside = false;
                    continue;
                }
                if (!inside)
                {
                    array[arrayIndex] = let;
                    arrayIndex++;
                }
            }
            return new string(array, 0, arrayIndex);
        }
    }
}
