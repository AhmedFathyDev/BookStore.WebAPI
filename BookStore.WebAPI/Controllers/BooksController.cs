﻿using BookStore.API.Models;
using BookStore.API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository bookRepository;

        public BooksController(IBookRepository bookRepository)
        {
            this.bookRepository = bookRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBooks()
        {
            var books = await bookRepository.GetAllBooksAsync();
            return Ok(books);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookById([FromRoute] int id)
        {
            var book = await bookRepository.GetBookByIdAsync(id);
            return book == null ? NotFound() : Ok(book);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddBook([FromBody] BookModel bookModel)
        {
            var id = await bookRepository.AddBookAsync(bookModel);
            return CreatedAtAction(nameof(GetBookById), new { controller = "books", id }, id);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateBook([FromRoute] int id, [FromBody] BookModel bookModel)
        {
            await bookRepository.UpdateBookAsync(id, bookModel);
            return Ok();
        }

        [HttpPatch("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateBookByPatch([FromRoute] int id, [FromBody] JsonPatchDocument jsonObject)
        {
            await bookRepository.UpdateBookByPatchAsync(id, jsonObject);
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteBook([FromRoute] int id)
        {
            await bookRepository.DeleteBookAsync(id);
            return Ok();
        }
    }
} 
