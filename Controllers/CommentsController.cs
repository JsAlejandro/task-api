using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using taskmanager_api.Models;
using taskmanager_api.ModelsResponse;
namespace taskmanager_api.Controllers {
    [ApiController]
    [Route ("/api/v1.0/[controller]")]
    public class CommentsController : ControllerBase {

        private readonly ILogger<CommentsController> _logger;
        private readonly TaskdbContext _context;

        private readonly IMapper _mapper;
        public CommentsController (ILogger<CommentsController> logger, TaskdbContext context, IMapper mapper) {
            this._logger = logger;
            this._context = context;
            this._mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<CommentsResponse>> GetAll ([FromQuery] int page = 1, [FromQuery] int limit = 15) {
            var query = this._context.Comments;
            IEnumerable<Comments> comments = await query.Skip ((page - 1) * limit).Take (limit).ToListAsync ();
            var commentsDTO = this._mapper.Map<IEnumerable<CommentsShowDTO>> (comments);
            int count = await query.CountAsync ();
            return new CommentsResponse {
                Page = page,
                    Pages = (int) Math.Ceiling (count / (float) limit),
                    Count = count,
                    Next = $"/api/v1.0/comments?limit={limit}&page={page + 1}",
                    Previous = $"/api/v1.0/comments?limit={limit}&page={page -1}",
                    Docs = commentsDTO
            };
        }

        [HttpGet ("{_id}", Name = "getComments")]
        public ActionResult<Comments> getComments (int _id) {

            var comment = this._context.Comments.FirstOrDefault (a => a.Id == _id);
            if (comment == null) {
                return NotFound ();
            } else {
                CommentsShowDTO commentShow = this._mapper.Map<CommentsShowDTO> (comment);
                return Ok (commentShow);
            }

        }

        [HttpPost]
        public ActionResult Create ([FromBody] CommentsCreatedDTO commentsC) {
            Comments comment = this._mapper.Map<Comments> (commentsC);
            this._context.Add (comment);
            this._context.SaveChanges ();
            CommentsShowDTO commentShow = this._mapper.Map<CommentsShowDTO> (comment);
            return new CreatedAtRouteResult ("getComments", new { _id = commentShow.Id }, commentShow);
        }

        [HttpPut ("{_id}")]
        public ActionResult<Comments> updated (int _id, [FromBody] CommentsUpdatedDTO commentDTO) {
            try {
                Comments comment = this._mapper.Map<Comments> (commentDTO);
                comment.Id = _id;
                this._context.Entry (comment).State = EntityState.Modified;
                this._context.SaveChanges ();
                return Ok (new { updated = true, error = new { } });
            } catch (System.Exception e) {
                return Ok (new { updated = true, error = new { ErrorData = e } });
            }
        }

        [HttpDelete ("{_id}")]
        public ActionResult<Comments> Delete (int _id) {
            var comments = this._context.Comments.FirstOrDefault (element => element.Id == _id);
            if (comments == null) {
                return NotFound ();
            }

            this._context.Remove (comments);
            this._context.SaveChanges ();
            return comments;
        }

    }
}