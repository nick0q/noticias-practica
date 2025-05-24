using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using noticias.Data;
using noticias.Models;
using noticias.Service;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace noticias.Rest
{
    [ApiController]
    [Route("api/feedback")]
    public class FeedbackApiController : Controller
    {

        private readonly FeedbackService _feedbackService;

        public FeedbackApiController(FeedbackService service)
        {
            _feedbackService = service;
        }



        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<Feedback>>> List()
        {
            var feedbacks = await _feedbackService.GetAll();

            if (feedbacks == null || !feedbacks.Any())
                return NotFound("No posts found.");

            return Ok(feedbacks);
        }



        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Feedback>> Get(int id)
        {
            var feedback = await _feedbackService.GetById(id);

            if (feedback == null)
                return NotFound($"Post with ID {id} not found.");

            return Ok(feedback);
        }



        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize]
        public async Task<ActionResult<Feedback>> Create([FromBody] Feedback feedback)
        {
            if (feedback == null)
            {
                return BadRequest("Invalid post data. Request body is null.");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                 return Unauthorized("User ID claim not found.");
            }

            feedback.Username = userId;

            try
            {
                var success = await _feedbackService.Add(feedback);

                if (!success)
                {
                    return BadRequest("Failed to add post due to a service error.");
                }

                return CreatedAtAction(
                    nameof(Get),
                    new { id = feedback.Id },
                    feedback
                );
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while adding the post.");
            }
        }


    }
}