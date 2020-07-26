using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using taskmanager_api.Models;
using taskmanager_api.ModelsResponse;
namespace taskmanager_api.Controllers {
    [ApiController]
    [Route ("/api/v1.0/[controller]")]

    public class UsersController : ControllerBase {

        private readonly ILogger<UsersController> _logger;
        private readonly TaskdbContext _context;
        private readonly IMapper _mapper;
        public UsersController (ILogger<UsersController> logger, TaskdbContext context, IMapper mapper) {
            this._logger = logger;
            this._context = context;
            this._mapper = mapper;

        }

        [HttpGet]
        public async Task<ActionResult<UsersResponse>> GetAll ([FromQuery] int page = 1, [FromQuery] int limit = 15) {
            var query = this._context.Users;
            IEnumerable<Users> users = await query.Skip ((page - 1) * limit).Take (limit).ToListAsync ();
            var usersShow = this._mapper.Map<IEnumerable<UsersShowDTO>> (users);
            int count = await query.CountAsync ();
            return new UsersResponse {
                Page = page,
                    Pages = (int) Math.Ceiling (count / (float) limit),
                    Count = count,
                    Next = $"/api/v1.0/users?limit={limit}&page={page + 1}",
                    Previous = $"/api/v1.0/users?limit={limit}&page={page -1}",
                    Docs = usersShow
            };
        }

        [HttpGet ("{_id}", Name = "getUser")]
        public ActionResult<UsersShowDTO> getUser (int _id) {

            var user = this._context.Users.FirstOrDefault (a => a.Id == _id);
            if (user == null) {
                return NotFound ();
            } else {
                UsersShowDTO userShow = this._mapper.Map<UsersShowDTO> (user);
                return Ok (userShow);
            }

        }

        [HttpPost]
        public ActionResult Create ([FromBody] UsersCreatedDTO userCreated) {
            Users user = this._mapper.Map<Users> (userCreated);
            this._context.Add (user);
            this._context.SaveChanges ();
            UsersShowDTO usersShow = this._mapper.Map<UsersShowDTO> (user);
            return new CreatedAtRouteResult ("getUser", new { _id = usersShow.Id }, usersShow);
        }

        [HttpPut ("{_id}")]
        public ActionResult<Users> updated (int _id, [FromBody] UsersUpdatedDTO userUpdate) {
            try {
                Users user = this._mapper.Map<Users> (userUpdate);
                user.Id = _id;
                this._context.Entry (user).State = EntityState.Modified;
                this._context.SaveChanges ();
                return Ok (new { updated = true, error = new { } });
            } catch (System.Exception e) {
                return Ok (new { updated = true, error = new { ErrorData = e } });
            }

        }

        [HttpPatch ("{_id}")]
        public async Task<ActionResult> updatedPatch (int _id, [FromBody] JsonPatchDocument<Users> patchDocument) {
            try {
                if (patchDocument == null) {
                    return BadRequest (new { title = "One or more validation errors occurred ", status = 400, errors = ModelState.Values.SelectMany (x => x.Errors) });
                }
                var user = await this._context.Users.FirstOrDefaultAsync (a => a.Id == _id);
                if (user == null) {
                    return NotFound ();
                }

                patchDocument.ApplyTo (user, ModelState);

                var isValid = TryValidateModel (user);

                if (!isValid) {
                    return BadRequest (new { title = "One or more validation errors occurred ", status = 400, errors = ModelState.Values.SelectMany (x => x.Errors) });
                }
                await this._context.SaveChangesAsync ();
                return Ok (new { updated = true, error = new { } });
            } catch (System.Exception e) {
                return Ok (new { updated = false, error = new { } });
            }

        }

        [HttpDelete ("{_id}")]
        public ActionResult<Users> Delete (int _id) {
            var user = this._context.Users.FirstOrDefault (element => element.Id == _id);
            if (user == null) {
                return NotFound ();
            }

            this._context.Remove (user);
            this._context.SaveChanges ();
            return user;
        }

    }
}