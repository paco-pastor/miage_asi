using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UniversiteDomain.DataAdapters.DataAdaptersFactory;
using UniversiteDomain.Entities;
using UniversiteDomain.UseCases.UeUseCases.Create;
using UniversiteDomain.Dtos;
using UniversiteDomain.UseCases.UeUseCases.Get;

namespace UniversiteRestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UeController(IRepositoryFactory repositoryFactory) : ControllerBase
    {
        // GET: api/<UeController>
        [HttpGet]
        public async Task<ActionResult<List<UeDto>>> GetAllUes()
        {
            GetAllUesUseCase getAllUesUc = new GetAllUesUseCase(repositoryFactory);
            List<Ue> ues = null;
            try
            {
                ues = await getAllUesUc.ExecuteAsync();
            }
            catch (Exception e)
            {
                ModelState.AddModelError(nameof(e), e.Message);
                return ValidationProblem();
            }
                
            List<UeDto> dtos = new List<UeDto>();
            foreach (Ue ue in ues)
            {
                dtos.Add(new UeDto().ToDto(ue));
            }
            return Ok(dtos);
        }
        
        // GET api/<UeController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UeDto>> GetUe(long id)
        {
            GetUeUseCase getUeUc = new GetUeUseCase(repositoryFactory);
            Ue ue = new Ue();
            try
            {
                ue = await getUeUc.ExecuteAsync(id);
            }
            catch (Exception e)
            {
                ModelState.AddModelError(nameof(e), e.Message);
                return ValidationProblem();
            }
            
            UeDto dto = new UeDto().ToDto(ue);
            return Ok(dto);
        }

        // POST api/<UeController>
        [HttpPost]
        public async Task<ActionResult<UeDto>> PostAsync([FromBody] UeDto ueDto)
        {
            CreateUeUseCase createUeUc = new CreateUeUseCase(repositoryFactory);
            Ue ue = ueDto.ToEntity();
            try
            {
                ue = await createUeUc.ExecuteAsync(ue);
            }
            catch (Exception e)
            {
                ModelState.AddModelError(nameof(e), e.Message);
                return ValidationProblem();
            }
            UeDto dto = new UeDto().ToDto(ue);
            return CreatedAtAction(nameof(GetUe), new { id = dto.Id }, dto);
        }

        // PUT api/<UeController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UeController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
