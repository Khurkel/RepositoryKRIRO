using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using HtmlAgilityPack;
using RepositoryKRIRO.Models;

namespace RepositoryKRIRO
{
    public class HttpGet
    {
        string _url;

       

        public HttpGet(string url)
        {
            _url = url;
        }
        public async Task<int> GetNumAsync()
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            HttpClient client = new HttpClient(clientHandler);
            HttpResponseMessage response = await client.GetAsync(_url);
            List<Practice> practiceList = new List<Practice>();
            if (response.IsSuccessStatusCode)
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                string responseBody = await response.Content.ReadAsStringAsync();
                HtmlDocument htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(responseBody);
                HtmlNode numNode = htmlDoc.DocumentNode.SelectSingleNode("//*[@class='navigation-pages']").Elements("a").Last<HtmlNode>();
                return int.Parse(numNode.InnerText);
            }
            else
            {
                return 5;
            }
        }
        public async Task<List<Practice>> GetPraticeAsync()
        {
            try
            {
                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
                HttpClient client = new HttpClient(clientHandler);
                // Send a GET request to the desired URL
                HttpResponseMessage response = await client.GetAsync(_url);
                List<Practice> practiceList = new List<Practice>();
                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    // Read the response content as a string
                    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                    string responseBody = await response.Content.ReadAsStringAsync();
                    HtmlDocument htmlDoc = new HtmlDocument();
                    htmlDoc.LoadHtml(responseBody);
                    HtmlNodeCollection practiceNodes = htmlDoc.DocumentNode.SelectNodes("//*[@class='news-item']");
                    foreach (HtmlNode practiceNode in practiceNodes)
                    {
                        Practice practice = new Practice()
                        {
                            Name = practiceNode.Element("a").InnerText,
                            Author = practiceNode.Element("span").InnerText.Replace("&copy;:&nbsp", ""),
                            Place = practiceNode.Elements("span").Last<HtmlNode>().InnerText.Replace("&nbsp;", "").Replace("&quot;","\""),
                            Link = "https://komiedu.ru/" + practiceNode.Element("a").GetAttributeValue("href", "").ToString(),

                        };
                        practiceList.Add(practice);
                    }
                    return practiceList;
                }
                else
                {
                    // Request was not successful, handle the
                    Practice practice = new Practice()
                    {
                        Name = "Error: " + response.StatusCode,
                    };
                    practiceList.Add(practice);
                    return practiceList;
                }

            }
            catch (Exception ex)
            {
                // Exception occurred, handle it 
                List<Practice> practiceList = new List<Practice>();
                Practice practice = new Practice()
                {
                    Name = "Exception: " + ex.Message,
                };
                practiceList.Add(practice);
                return practiceList;
            }
        }
                /*         Не реализованно, так как очень трудно запарсить
                     public async Task<List<Lessons>> GetLessonAsync() {
                         try
                         {
                             HttpClientHandler clientHandler = new HttpClientHandler();
                             clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
                             HttpClient client = new HttpClient(clientHandler);
                             // Send a GET request to the desired URL
                             HttpResponseMessage response = await client.GetAsync(_url);
                             List<Lessons> lessonsList = new List<Lessons>();
                             // Check if the request was successful
                             if (response.IsSuccessStatusCode)
                             {
                                 // Read the response content as a string
                                 string responseBody = await response.Content.ReadAsStringAsync();
                                 HtmlDocument htmlDoc = new HtmlDocument();
                                 htmlDoc.LoadHtml(responseBody);
                                 HtmlNodeCollection lessonsNodes = htmlDoc.DocumentNode.SelectNodes("//*[@data-type='0']");
                                 foreach (HtmlNode lessonsNode in lessonsNodes)
                                 {
                                     Lessons lessons = new Lessons()
                                     {
                                         Name = "",
                                         Link = "",
                                     };
                                     lessonsList.Add(lessons);
                                 }
                                 return lessonsList;
                             }
                             else
                             {
                                 // Request was not successful, handle the
                                 Lessons lessons = new Lessons()
                                 {
                                     Name = "Error: " + response.StatusCode,
                                 };
                                 lessonsList.Add(lessons);
                                 return lessonsList;
                             }

                         }
                         catch (Exception ex)
                         {
                             // Exception occurred, handle it 
                             List<Lessons> lessonsList = new List<Lessons>();
                             Lessons lessons = new Lessons()
                             {
                                 Name = "Exception: " + ex.Message,
                             };
                             lessonsList.Add(lessons);
                             return lessonsList;
                         }
                     }*/
                public async Task<List<News>> GetNewsAsync()
                {
                try
                {
                    HttpClientHandler clientHandler = new HttpClientHandler();
                    clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
                    HttpClient client = new HttpClient(clientHandler);
                    // Send a GET request to the desired URL
                    HttpResponseMessage response = await client.GetAsync(_url);
                    List<News> newsList = new List<News>();
                    // Check if the request was successful
                    if (response.IsSuccessStatusCode)
                    {
                        // Read the response content as a string
                        string responseBody = await response.Content.ReadAsStringAsync();
                        HtmlDocument htmlDoc = new HtmlDocument();
                        htmlDoc.LoadHtml(responseBody);
                        HtmlNodeCollection newsNodes = htmlDoc.DocumentNode.SelectNodes("//*[@class='news-item']");
                        foreach (HtmlNode newsNode in newsNodes)
                        {
                            News news = new News()
                            {
                                Name = newsNode.Element("a").InnerText,
                                Description = "",
                                Date = newsNode.Element("span").InnerText,
                                Link = "https://kriro.ru/" + newsNode.Element("a").GetAttributeValue("href", "").ToString()
                            };
                            newsList.Add(news);
                        }
                        return newsList;
                    }
                    else
                    {
                        // Request was not successful, handle the
                        News news = new News()
                        {
                            Name = "Error: " + response.StatusCode,
                            Description = "",
                            Date = "",
                            Link = "https://kriro.ru/"
                        };
                        newsList.Add(news);
                        return newsList;
                    }

                }
                catch (Exception ex)
                {
                    // Exception occurred, handle it 
                    List<News> newsList = new List<News>();
                    News news = new News()
                    {
                        Name = "Exception: " + ex.Message,
                        Description = "",
                        Date = "",
                        Link = "https://kriro.ru/"
                    };
                    newsList.Add(news);
                    return newsList;
                }
            }
        }

    }

