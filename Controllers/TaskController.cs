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
    public class TaskController : ControllerBase {

        private readonly ILogger<TaskController> _logger;
        private readonly TaskdbContext _context;

        private readonly IMapper _mapper;
        public TaskController (ILogger<TaskController> logger, TaskdbContext context, IMapper mapper) {
            this._logger = logger;
            this._context = context;
            this._mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<AssignmentResponse>> GetAll ([FromQuery] int page = 1, [FromQuery] int limit = 15) {

            var query = this._context.Assignment;
            IEnumerable<Assignment> assignment = await query.Skip ((page - 1) * limit).Take (limit).ToListAsync ();
            var assignmentsShow = this._mapper.Map<IEnumerable<AssignmentShowDTO>> (assignment);
            var count = await query.CountAsync ();
            return new AssignmentResponse {
                Page = page,
                    Pages = (int) Math.Ceiling (count / (float) limit),
                    Count = count,
                    Next = $"/api/v1.0/task?limit={limit}&page={page + 1}",
                    Previous = $"/api/v1.0/task?limit={limit}&page={page -1}",
                    Docs = assignmentsShow
            };
        }

        [HttpGet ("{_id}", Name = "getAssignment")]
        public ActionResult<Assignment> getAssignment (int _id) {
            Assignment assignment = this._context.Assignment.FirstOrDefault (a => a.Id == _id);
            if (assignment == null) {
                return NotFound ();
            } else {
                AssignmentShowDTO assignmentShow = this._mapper.Map<AssignmentShowDTO> (assignment);
                return Ok (assignmentShow);
            }
        }

        [HttpPost]
        public ActionResult Create ([FromBody] AssignmentCreatedDTO assignmentCreated) {
            Assignment assignment = this._mapper.Map<Assignment> (assignmentCreated);
            this._context.Add (assignment);
            this._context.SaveChanges ();
            AssignmentShowDTO assignmentsShow = this._mapper.Map<AssignmentShowDTO> (assignment);
            return new CreatedAtRouteResult ("getAssignment", new { _id = assignmentsShow.Id }, assignmentsShow);
        }

        [HttpPut ("{_id}")]
        public ActionResult<Assignment> updated (int _id, [FromBody] AssignmentUpdatedDTO assignmentUpdated) {
            try {
                Assignment assignment = this._mapper.Map<Assignment> (assignmentUpdated);
                assignment.Id = _id;
                this._context.Entry (assignment).State = EntityState.Modified;
                this._context.SaveChanges ();
                return Ok (new { updated = true, error = new { } });
            } catch (System.Exception e) {
                return Ok (new { updated = true, error = new { ErrorData = e } });
            }
        }

        [HttpDelete ("{_id}")]
        public ActionResult<Assignment> Delete (int _id) {
            var assignment = this._context.Assignment.FirstOrDefault (element => element.Id == _id);
            if (assignment == null) {
                return NotFound ();
            }

            this._context.Remove (assignment);
            this._context.SaveChanges ();
            return assignment;
        }

         [HttpGet ("{_id}/comments")]
        public ActionResult<Assignment> getCommentsTask (int _id) {
            
            Assignment assignment = this._context.Assignment.Include(element => element.Comments).FirstOrDefault (a => a.Id == _id);
            if (assignment == null) {
                return NotFound ();
            } else {
                var assignmentShow = this._mapper.Map<AssignmentPopulateDTO> (assignment);
                return Ok (assignmentShow);
            }
        }

    }
}