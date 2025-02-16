using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UniversiteDomain.DataAdapters.DataAdaptersFactory;
using UniversiteDomain.Dtos;
using UniversiteDomain.Entities;
using UniversiteDomain.UseCases.NoteUseCases.Create;
using UniversiteDomain.UseCases.NoteUseCases.Get;

namespace UniversiteRestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NoteController(IRepositoryFactory repositoryFactory) : ControllerBase
    {
        // GET: api/<NoteController>
        [HttpGet]
        public async Task<ActionResult<List<NoteDto>>> GetAllNotes()
        {
            GetAllNotesUseCase getAllNotesUc = new GetAllNotesUseCase(repositoryFactory);
            List<Note> notes = null;
            
            try
            {
                notes = await getAllNotesUc.ExecuteAsync();
            }
            catch (Exception e)
            {
                ModelState.AddModelError(nameof(e), e.Message);
                return ValidationProblem();
            }
            
            List<NoteDto> dtos = new List<NoteDto>();
            foreach (Note note in notes)
            {
                dtos.Add(new NoteDto().ToDto(note));
            }
            
            return Ok(dtos);
        }

        // GET api/<NoteController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<NoteDto>> GetNote(int id)
        {
            GetNoteUseCase getNoteUc = new GetNoteUseCase(repositoryFactory);
            Note note = new Note();
            try
            {
                note = await getNoteUc.ExecuteAsync(id);
            }
            catch (Exception e)
            {
                ModelState.AddModelError(nameof(e), e.Message);
                return ValidationProblem();
            }
            NoteDto dto = new NoteDto().ToDto(note);
            return Ok(dto);
        }

        // Crée une nouvelle note
        // POST api/<NoteController>
        [HttpPost]
        public async Task<ActionResult<NoteDto>> Post([FromBody] NoteBodyDto noteDto)
        {
            CreateNoteUseCase createNoteUc = new CreateNoteUseCase(repositoryFactory);
            Note note = noteDto.ToEntity();
            try
            {
                note = await createNoteUc.ExecuteAsync(note);
            }
            catch (Exception e)
            {
                ModelState.AddModelError(nameof(e), e.Message);
                return ValidationProblem();
            }
            return Ok(new NoteDto().ToDto(note));
        }

        // PUT api/<NoteController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<NoteController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
