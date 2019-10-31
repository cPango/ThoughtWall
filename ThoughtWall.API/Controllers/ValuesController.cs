﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThoughtWall.API.Data;
using ThoughtWall.API.Dtos;
using ThoughtWall.API.Models;

namespace ThoughtWall.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly DataContext _context;

        public ValuesController(DataContext context)
        {
            _context = context;
        }

        // GET api/values
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetThreads()
        {
            // Orders by most recent (using TimeStamp)
            var threads = await _context.Threads
                .Include(x => x.Comments)
                .OrderByDescending(x => x.TimeStamp)
                .Take(5)
                .ToListAsync();
            return Ok(threads);
        }

        // GET Older Threads
        [AllowAnonymous]
        [HttpGet("archives")]
        public async Task<IActionResult> GetOldThreads(int skip)
        {
            var oldThreads = await _context.Threads
                .Include(x => x.Comments)
                .OrderByDescending(x => x.TimeStamp)
                .Skip(skip)
                .Take(5)
                .ToListAsync();
            return Ok(oldThreads);
        }

        [AllowAnonymous]
        [HttpGet("search")]
        public async Task<IActionResult> GetSearchedThreads(string keyword)
        {
            var matches = await _context.Threads
                .Include(x => x.Comments)
                .Where(x => x.Title.ToLower().Contains(keyword.ToLower()))
                .OrderByDescending(x => x.TimeStamp)
                .ToListAsync();
            return Ok(matches);
        }

        // GET api/values/5
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSpecificThread(string id)
        {
            var thread = await _context.Threads
                .Where(x => x.Id == Int32.Parse(id))
                .OrderByDescending(x => x.TimeStamp)
                .ToListAsync();
            return Ok(thread);
        }

        // GET api/values/5/comments
        [AllowAnonymous]
        [HttpGet("{id}/comments")]
        public async Task<IActionResult> GetComments(string id)
        {
            var comments = await _context.Comments
                .Where(x => x.ThreadId == Int32.Parse(id))
                .OrderByDescending(x => x.TimeStamp)
                .ToListAsync();
            return Ok(comments);
        }

        // POST api/values/submit
        [HttpPost("submit")]
        public async Task<IActionResult> PostThread(ThreadPostDto threadPostDto)
        {
            DateTime timeStamp = DateTime.Now;
            var thread = new Thread 
            {
                Username = User.FindFirst(ClaimTypes.Name).Value,
                UserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value),
                Title = threadPostDto.Title,
                Body = threadPostDto.Body,
                TimeStamp = timeStamp
            };

            await _context.Threads.AddAsync(thread);
            await _context.SaveChangesAsync();
            return StatusCode(201);
        }

        // Is called after posting a new thread
        [AllowAnonymous]
        [HttpGet("redirect")]
        public async Task<IActionResult> Redirects(string title) 
        {
            var id = await _context.Threads
                .OrderByDescending(x => x.TimeStamp)
                .Where(x => x.Title == title)
                .FirstAsync();
                        
            return Ok(id);
        }

        // POST api/values/comment
        [HttpPost("comment")]
        public async Task<IActionResult> PostComment(CommentPostDto commentPostDto)
        {
            DateTime timeStamp = DateTime.Now;
            var comment = new Comment
            {
                Username = User.FindFirst(ClaimTypes.Name).Value,
                UserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value),
                ThreadId = Int32.Parse(commentPostDto.ThreadId),
                Body = commentPostDto.Body,
                TimeStamp = timeStamp
            };
            
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
            return StatusCode(201);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
