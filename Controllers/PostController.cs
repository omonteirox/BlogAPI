using Blog.Data;
using BlogAPI.ViewModels.Post;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.Controllers
{
    [ApiController]
    public class PostController : ControllerBase
    {
        [HttpGet("v1/posts")]
        public async Task<IActionResult> GetAsync([FromServices] BlogDataContext ctx, [FromQuery] int page = 0, [FromQuery] int pageSize = 25)
        {
            var posts = await ctx.Posts.AsNoTracking().Include(x => x.Category).Include(x => x.Author).Select(x => new ViewPostModel
            {
                Author = x.Author.Name,
                Category = x.Category.Name,
                LastUpdateDate = x.LastUpdateDate,
                Id = x.Id,
                Title = x.Title,
                Slug = x.Slug
            }).Skip(page * pageSize).Take(pageSize).OrderByDescending(x => x.LastUpdateDate).ToListAsync();
            return Ok();
        }
    }
}