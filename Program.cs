using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IndeedScraper
{
    class Program
    {
        static void Main(string[] args)
        {
            GetHtmlAsync();

            Console.ReadKey();
        }

        private static async void GetHtmlAsync()
        {
            var url = "https://id.indeed.com/lowongan-kerja?q=web+developer&l=Semarang";

            var httpClient = new HttpClient();
            var html = await httpClient.GetStringAsync(url);
            var salary = "";

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            var JobsHtml = htmlDocument.DocumentNode.Descendants("td")
                .Where(node => node.GetAttributeValue("id", "")
                .Equals("resultsCol")).ToList();
            var JobListItems = JobsHtml[0].Descendants("div")
                .Where(node => node.GetAttributeValue("id", "")
                .Contains("p_")).ToList();

            foreach (var JobListItem in JobListItems)
            {
                Console.ForegroundColor =  (ConsoleColor)10;

                var jobId = JobListItem.GetAttributeValue("id", "");

                var jobTitle = JobListItem.Descendants("a")
                    .Where(node => node.GetAttributeValue("data-tn-element", "")
                    .Equals("jobTitle")).FirstOrDefault().InnerText;

                var companyName = JobListItem.Descendants("span")
                    .Where(node => node.GetAttributeValue("class", "")
                    .Equals("company")).FirstOrDefault().InnerText;

                var location = JobListItem.Descendants("span")
                    .Where(node => node.GetAttributeValue("class", "")
                    .Contains("location")).FirstOrDefault().InnerText;

                var linkDetail = JobListItem.Descendants("a")
                    .FirstOrDefault().GetAttributeValue("href", "");


                var urlDetail = "https://id.indeed.com" + linkDetail;

                var html2 = await httpClient.GetStringAsync(urlDetail);
                var htmlDocument2 = new HtmlDocument();
                htmlDocument2.LoadHtml(html2);

                var jobDesc = htmlDocument2.DocumentNode.Descendants("div")
                    .Where(node => node.GetAttributeValue("id", "")
                    .Equals("jobDescriptionText")).FirstOrDefault().InnerText;

                try
                {
                    salary = JobListItem.Descendants("span")
                                 .Where(node => node.GetAttributeValue("class", "")
                                 .Equals("salaryText")).FirstOrDefault().InnerText;
                }
                catch(Exception e)
                {

                }

                var date = JobListItem.Descendants("span")
                    .Where(node => node.GetAttributeValue("class", "")
                    .Contains("date")).FirstOrDefault().InnerText;
                try
                {
                    Console.WriteLine(jobId + "\n" + jobTitle + "\n" + companyName + " - " + location + "\n" + linkDetail + "\n" + salary + "\n" + date + "\n" + jobDesc);
                }
                catch(Exception e)
                {
                   Console.WriteLine(jobId + "\n" + jobTitle + "\n" + companyName + " - " + location + "\n" + linkDetail + "\n"  +  date + "\n" + jobDesc);
                }

            }

        }
    }
}
