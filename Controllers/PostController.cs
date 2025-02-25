using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class PostController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public PostController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public async Task<IActionResult> GetAsync()
    {
        var posts = await _unitOfWork.PostRepository.GetAsync();
        if (posts == null || !posts.Any())
        {
            return NotFound();
        }
        return Ok(posts);
    }

    // [HttpGet("{id}")]
    // public IActionResult Get(Guid id)
    // {
    //     app.MapGet("/posts", async (PostDb db) => await db.Posts.ToListAsync());
    //     app.MapGet(
    //         "/posts/{id}",
    //         async (Guid id, PostDb db) =>
    //             await db.Posts.FindAsync(id) is Post post ? Results.Ok(post) : Results.NotFound()
    //     );
    // }

    [HttpPost]
    public async Task<IActionResult> PostAsync([FromBody] Post post)
    {
        if (post == null)
        {
            return BadRequest("Post is null.");
        }

        await _unitOfWork.PostRepository.AddAsync(post);
        await _unitOfWork.SaveAsync();
        return CreatedAtAction(nameof(PostAsync), new { id = post.Id }, post);
    }
}

// app.MapPost(
//     "/posts",
//     async (Post post, PostDb db) =>
//     {
//         db.Posts.Add(post);
//         await db.SaveChangesAsync();
//         return Results.Created($"/posts/{post.Id}", post);
//     }
// );


// import React, { useState } from 'react';

// const CreatePost = () => {
//     const [title, setTitle] = useState('');
//     const [content, setContent] = useState('');

//     const handleSubmit = async (event) => {
//         event.preventDefault();

//         const post = {
//             title,
//             content,
//         };

//         const response = await fetch('/Post', {
//             method: 'POST',
//             headers: {
//                 'Content-Type': 'application/json',
//             },
//             body: JSON.stringify(post),
//         });

//         if (response.ok) {
//             console.log('Post created successfully!');
//         } else {
//             console.log('Failed to create post');
//         }
//     };

//     return (
//         <form onSubmit={handleSubmit}>
//             <input
//                 type="text"
//                 placeholder="Title"
//                 value={title}
//                 onChange={(e) => setTitle(e.target.value)}
//             />
//             <textarea
//                 placeholder="Content"
//                 value={content}
//                 onChange={(e) => setContent(e.target.value)}
//             />
//             <button type="submit">Create Post</button>
//         </form>
//     );
// };

// export default CreatePost;
