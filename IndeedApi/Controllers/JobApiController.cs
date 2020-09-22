using HtmlAgilityPack;
using IndeedApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace IndeedApi.Controllers
{
    public class JobApiController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "" };
        }

        // GET api/values/5
        public async System.Threading.Tasks.Task<IEnumerable<JobModels>> GetAsync(string name)
        {
            JobModels jobModels = new JobModels();
            var listJob = new List<JobModels>();

            var url = "https://id.indeed.com/lowongan-kerja?q=" + name;

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
                Console.ForegroundColor = (ConsoleColor)10;

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
                catch (Exception e)
                {

                }

                var date = JobListItem.Descendants("span")
                    .Where(node => node.GetAttributeValue("class", "")
                    .Contains("date")).FirstOrDefault().InnerText;

                
                listJob.Add( new JobModels {
                    JobName = jobTitle,
                    JobSalary = salary,
                    JobDate = date,
                    JobLink = urlDetail,
                    JobCompany = companyName,
                    JobLocation = location,
                    JobDesc = jobDesc,
            });
            }
            return listJob;
        }

        public async System.Threading.Tasks.Task<IEnumerable<JobModels>> GetAsync(string name, string jobloc)
        {
            JobModels jobModels = new JobModels();
            var listJob = new List<JobModels>();

            var url = "https://id.indeed.com/lowongan-kerja?q=" + name + "&l=" + jobloc;

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
                Console.ForegroundColor = (ConsoleColor)10;

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
                catch (Exception e)
                {

                }

                var date = JobListItem.Descendants("span")
                    .Where(node => node.GetAttributeValue("class", "")
                    .Contains("date")).FirstOrDefault().InnerText;


                listJob.Add(new JobModels
                {
                    JobName = jobTitle,
                    JobSalary = salary,
                    JobDate = date,
                    JobLink = urlDetail,
                    JobCompany = companyName,
                    JobLocation = location,
                    JobDesc = jobDesc,
                });
            }
            return listJob;
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
