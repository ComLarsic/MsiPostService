using MsiPostOrmUtility;
using MsiPosts.DTO;
using MsiPostUtility;
using MsiPostOrm.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using MsiPostOrm;

namespace MsiPosts;

/// <summary>
/// The service that handles msi posts
/// </summary>
public class MsiPostService : IMsiPostService
{
    private readonly IMsiPostOrmService _ormService;

    public MsiPostService(IMsiPostOrmService ormService)
        => _ormService = ormService;

    public async Task<Guid> CreatePostAsync(Guid profile, string text)
    {
        Guid id = Guid.NewGuid();
        await _ormService.Context(async db =>
        {
            var entity = new PostEntity
            {
                Id = id,
                ProfileId = profile,
                Text = text,
                CreatedAt = DateTime.UtcNow,
            };
            await db.AddAsync(entity);
            await db.SaveChangesAsync();
        });
        return id;
    }


    public async Task DeletePostAsync(Guid id)
        => await _ormService.Context(async db =>
        {
            db.Remove(new PostEntity { Id = id });

            // Remove all likes on the post
            db.RemoveRange(db.Likes.Where(l => l.PostId == id));
            await db.SaveChangesAsync();
        });

    public async Task EditPostAsync(Guid id, string text)
        => await _ormService.Context(async db =>
        {
            var post = await db.Posts.FirstOrDefaultAsync(p => p.Id == id) ?? throw new Exception("Invalid post id");
            post.Text = text;
            await db.SaveChangesAsync();
        });

    public async Task<PostDTO?> GetPostAsync(Guid id)
        => await _ormService.Context(async db =>
        {
            var entity = await db.Posts.FirstOrDefaultAsync(p => p.Id == id) ?? throw new Exception("Post not found");
            return new PostDTO
            {
                Id = entity.Id,
                ProfileId = entity.ProfileId,
                Text = entity.Text,
                CreatedAt = entity.CreatedAt,
                Likes = await db.Likes.Where(l => l.PostId == id).Select(l => new LikeDTO
                {
                    Id = l.Id,
                    ProfileId = l.ProfileId,
                    PostId = l.PostId,
                }).ToListAsync(),
            };
        });

    public async Task<IEnumerable<PostDTO>> GetPostsOfProfileAsync(Guid profile)
        => await _ormService.Context(async db =>
             await db.Posts.Where(p => p.ProfileId == profile).Select(p => new PostDTO
             {
                 Id = p.Id,
                 ProfileId = p.ProfileId,
                 Text = p.Text,
                 CreatedAt = p.CreatedAt,
                 Likes = db.Likes.Where(l => l.PostId == p.Id).Select(l => new LikeDTO
                 {
                     Id = l.Id,
                     ProfileId = l.ProfileId,
                     PostId = l.PostId,
                 }).ToList(),
             }).ToListAsync());

    public async Task<PagedResponse<PostDTO>> GetPostsOfProfileAsync(Guid profile, int page, int pageSize)
        => await _ormService.Context(async db =>
        {
            int offset = (page - 1) * pageSize;
            var posts = await db.Posts.Where(p => p.ProfileId == profile)
                .Skip(offset)
                .Take(pageSize)
                .ToListAsync();
            var count = await db.Posts.Where(p => p.ProfileId == profile).CountAsync();
            var totalPages = (int)Math.Ceiling((double)count / pageSize);
            var postsDto = posts.Select(p => new PostDTO
            {
                Id = p.Id,
                ProfileId = p.ProfileId,
                Text = p.Text,
                CreatedAt = p.CreatedAt,
                Likes = db.Likes.Where(l => l.PostId == p.Id).Select(l => new LikeDTO
                {
                    Id = l.Id,
                    ProfileId = l.ProfileId,
                    PostId = l.PostId,
                }).ToList(),
            }).ToList();


            return new PagedResponse<PostDTO>
            {
                Values = postsDto,
                Page = page,
                TotalPages = totalPages,
            };
        });
}
