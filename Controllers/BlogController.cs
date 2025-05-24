using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http; 
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using noticias.Models;

namespace blog.Controllers
{
    public class BlogController : Controller
    {
        private readonly ILogger<BlogController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public BlogController(ILogger<BlogController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com");
            List<PostPlaceholder>? posts = null;

            try
            {
                posts = await client.GetFromJsonAsync<List<PostPlaceholder>>("/posts");
            }
            catch (HttpRequestException e)
            {
                _logger.LogError(e, "Error al obtener posts de JSONPlaceholder");
                ViewBag.ErrorMessage = "No se pudieron cargar las noticias en este momento. Inténtalo más tarde.";
            }
            
            return View(posts ?? new List<PostPlaceholder>());
        }


        [HttpGet]
        public IActionResult Listar()
        {
            return View();
        }


        [HttpGet("api/blog/postdetails/{postId}")]
        public async Task<IActionResult> GetPostDetailsFromPlaceholder(int postId)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com");
            
            try
            {
                var postTask = client.GetFromJsonAsync<PostPlaceholder>($"/posts/{postId}");

                var post = await postTask;
                if (post == null) return NotFound("Post no encontrado en JSONPlaceholder.");

                return Ok(post);
            }
            catch (HttpRequestException e)
            {
                _logger.LogError(e, "Error al obtener detalles del post {PostId} de JSONPlaceholder", postId);
                return StatusCode(500, "Error al contactar el servicio externo.");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}