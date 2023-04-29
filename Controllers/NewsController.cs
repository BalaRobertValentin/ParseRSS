using Microsoft.AspNetCore.Mvc;
using ParseRSS.Data;
using ParseRSS.Models;
using System.ServiceModel.Syndication;
using System.Xml;


namespace ParseRSS.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NewsController : Controller
    {
        private readonly NewsDbContext _dbContext;

        public NewsController(NewsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult GetNews(string sort_by = "pub_date", string sort_order = "desc", string? keyword = null)
        {
            var feed = SyndicationFeed.Load(XmlReader.Create("https://rss.nytimes.com/services/xml/rss/nyt/World.xml"));
            var news = new List<News>();
            foreach (var item in feed.Items)
            {
                var newsItem = new News
                {
                    Title = item.Title.Text,
                    Link = item.Links.First().Uri.AbsoluteUri,
                    Description = item.Summary.Text,
                    PubDate = item.PublishDate,
                    Publisher = feed.Title.Text, // set Publisher to the feed's Title
                    NewsGroup = item.Categories.FirstOrDefault()?.Name,
                };

                news.Add(newsItem);
            }

            // Filter news based on keyword
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                news = news.Where(n => n.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase) || n.Description.Contains(keyword, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            switch (sort_by)
            {
                case "title":
                    news = sort_order == "desc" ? news.OrderByDescending(n => n.Title).ToList() : news.OrderBy(n => n.Title).ToList();
                    break;
                case "pub_date":
                default:
                    news = sort_order == "desc" ? news.OrderByDescending(n => n.PubDate).ToList() : news.OrderBy(n => n.PubDate).ToList();
                    break;
            }

            // Set ViewBag values for sorting and keyword
            ViewBag.SortBy = sort_by;
            ViewBag.SortOrder = sort_order;
            ViewBag.Keyword = keyword;

            return View(news);
        }

        [HttpPost]
        public async Task<IActionResult> SaveNews(List<News> newsList)
        {
            await _dbContext.News.AddRangeAsync(newsList);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}
