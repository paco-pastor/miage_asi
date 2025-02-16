using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UniversiteDomain.DataAdapters.DataAdaptersFactory;
using UniversiteDomain.Dtos;
using UniversiteDomain.Entities;
using UniversiteDomain.UseCases.EtudiantUseCases.Get;
using UniversiteDomain.UseCases.ParcoursUseCases.Create;
using UniversiteDomain.UseCases.ParcoursUseCases.EtudiantDansParcours;
using UniversiteDomain.UseCases.ParcoursUseCases.Get;
using UniversiteDomain.UseCases.ParcoursUseCases.UeDansParcours;

namespace UniversiteRestApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ParcoursController(IRepositoryFactory repositoryFactory) : ControllerBase
    {
        // GET: api/<ParcoursController>
        [HttpGet]
        public async Task<ActionResult<List<ParcoursDto>>> GetAllParcours()
        {
            GetAllParcoursUseCase getAllParcoursUc = new GetAllParcoursUseCase(repositoryFactory);
            List<Parcours> parcours = null;
            try
            {
                parcours = await getAllParcoursUc.ExecuteAsync();
            }
            catch (Exception e)
            {
                ModelState.AddModelError(nameof(e), e.Message);
                return ValidationProblem();
            }
                
            List<ParcoursDto> dtos = new List<ParcoursDto>();
            foreach (Parcours parcour in parcours)
            {
                dtos.Add(new ParcoursDto().ToDto(parcour));
            }
            return Ok(dtos);
        }

        // GET api/<ParcoursController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ParcoursDto>> GetParcours(long id)
        {
            GetParcoursUseCase getParcoursUc = new GetParcoursUseCase(repositoryFactory);
            Parcours parcours = new Parcours();
            try
            {
                parcours = await getParcoursUc.ExecuteAsync(id);
            }
            catch (Exception e)
            {
                ModelState.AddModelError(nameof(e), e.Message);
                return ValidationProblem();
            }
            ParcoursDto dto = new ParcoursDto().ToDto(parcours);
            return Ok(dto);
        }

        // POST api/<ParcoursController>
        [HttpPost]
        public async Task<ActionResult<ParcoursDto>> PostAsync([FromBody] ParcoursDto parcoursDto)
        {
            CreateParcoursUseCase createParcoursUc = new CreateParcoursUseCase(repositoryFactory);
            Parcours parcours = parcoursDto.ToEntity();
            try
            {
                parcours = await createParcoursUc.ExecuteAsync(parcours);
            }
            catch (Exception e)
            {
                ModelState.AddModelError(nameof(e), e.Message);
                return ValidationProblem();
            }
            
            ParcoursDto dto = new ParcoursDto().ToDto(parcours);
            return CreatedAtAction(nameof(GetParcours), new { id = dto.Id }, dto);
        }

        // PUT api/<ParcoursController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ParcoursController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        
        //Route api AddEtudiant
        [HttpPut("{parcoursId}/addEtudiant/{etudiantId}")]
        public async Task<ActionResult<ParcoursCompletDto>> AddEtudiantAsync( long parcoursId, long etudiantId)
        {
            AddEtudiantDansParcoursUseCase addEtudiantDansParcoursUc = new AddEtudiantDansParcoursUseCase(repositoryFactory);
            
            Parcours parcours = null;
            try
            {
                parcours = await addEtudiantDansParcoursUc.ExecuteAsync(parcoursId, etudiantId);
            }
            catch (Exception e)
            {
                ModelState.AddModelError(nameof(e), e.Message);
                return ValidationProblem();
            }
            
            ParcoursCompletDto dto = new ParcoursCompletDto().ToDto(parcours);
            return CreatedAtAction(nameof(GetParcours), new { id = dto.Id }, dto);
        }
        
        [HttpPut("{parcoursId}/addUe/{ueId}")]
        public async Task<ActionResult<ParcoursCompletDto>> AddUeAsync( long parcoursId, long ueId)
        {
            AddUeDansParcoursUseCase addUeDansParcoursUseCase = new AddUeDansParcoursUseCase(repositoryFactory);
            
            Parcours parcours = null;
            try
            {
                parcours = await addUeDansParcoursUseCase.ExecuteAsync(parcoursId, ueId);
            }
            catch (Exception e)
            {
                ModelState.AddModelError(nameof(e), e.Message);
                return ValidationProblem();
            }
            ParcoursCompletDto dto = new ParcoursCompletDto().ToDto(parcours);
            return Ok(dto);
        }
    }
}
