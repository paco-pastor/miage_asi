using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UniversiteDomain.DataAdapters.DataAdaptersFactory;
using UniversiteDomain.Entities;
using UniversiteDomain.UseCases.EtudiantUseCases.Create;
using UniversiteDomain.Dtos;
using UniversiteDomain.UseCases.EtudiantUseCases.Delete;
using UniversiteDomain.UseCases.EtudiantUseCases.Get;
using UniversiteDomain.UseCases.EtudiantUseCases.Update;
using UniversiteDomain.UseCases.SecurityUseCases.Create;
using UniversiteDomain.UseCases.SecurityUseCases.Get;
using UniversiteEFDataProvider.Entities;

namespace UniversiteRestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EtudiantController(IRepositoryFactory repositoryFactory) : ControllerBase
    {
        // GET: api/<EtudiantController>
        [HttpGet]
        public async Task<ActionResult<List<EtudiantDto>>> GetAllEtudiants()
        {
            GetAllEtudiantsUseCase getAllEtudiantsUc = new GetAllEtudiantsUseCase(repositoryFactory);
            List<Etudiant> etudiants = null;
            
            try
            {
                etudiants = await getAllEtudiantsUc.ExecuteAsync();
            }
            catch (Exception e)
            {
                ModelState.AddModelError(nameof(e), e.Message);
                return ValidationProblem();
            }
                
            List<EtudiantDto> dtos = new List<EtudiantDto>();
            foreach (Etudiant etudiant in etudiants)
            {
                dtos.Add(new EtudiantDto().ToDto(etudiant));
            }
            
            return Ok(dtos);
        }

        // GET api/<EtudiantController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EtudiantDto>> GetEtudiant(long id)
        {
            GetEtudiantUseCase getEtudiantUc = new GetEtudiantUseCase(repositoryFactory);
            Etudiant etudiant = new Etudiant();
            try
            {
                etudiant = await getEtudiantUc.ExecuteAsync(id);
            }
            catch (Exception e)
            {
                ModelState.AddModelError(nameof(e), e.Message);
                return ValidationProblem();
            }
            EtudiantDto dto = new EtudiantDto().ToDto(etudiant);
            return Ok(dto);
        }
        
        // Définissons une nouvelle route pour ne pas créer de conflit avec le get de base
// GET api/<EtudiantController>/complet/5
        [HttpGet("complet/{id}")]
        public async Task<ActionResult<EtudiantCompletDto>> GetUnEtudiantCompletAsync(long id)
        {
            // Identification et authentification
            string role="";
            string email="";
            IUniversiteUser user = null;
            try
            {
                CheckSecu(out role, out email, out user);
            }
            catch (Exception e)
            {
                return Unauthorized();
            }
    
            GetEtudiantCompletUseCase uc = new GetEtudiantCompletUseCase(repositoryFactory);
            // Autorisation
            // On vérifie si l'utilisateur connecté a le droit d'accéder à la ressource
            if (!uc.IsAuthorized(role, user, id)) return Unauthorized();
            Etudiant? etud;
            try
            {
                etud = await uc.ExecuteAsync(id);
            }
            catch (Exception e)
            {
                ModelState.AddModelError(nameof(e), e.Message);
                return ValidationProblem();
            }
            if (etud == null) return NotFound();
            return new EtudiantCompletDto().ToDto(etud);
        }

        // Crée un nouvel étudiant sans parcours
        // POST api/<EtudiantController>
        [HttpPost]
        public async Task<ActionResult<EtudiantDto>> PostAsync([FromBody] EtudiantDto etudiantDto)
        {
            CreateEtudiantUseCase createEtudiantUc = new CreateEtudiantUseCase(repositoryFactory);          
            CreateUniversiteUserUseCase createUserUc = new CreateUniversiteUserUseCase(repositoryFactory);
            
            string role="";
            string email="";
            IUniversiteUser user = null;
            if(!createUserUc.IsAuthorized(role) || !createEtudiantUc.IsAuthorized(role)) return Unauthorized();
            CheckSecu(out role, out email, out user);
            
            Etudiant etud = etudiantDto.ToEntity();
            try
            {
                etud = await createEtudiantUc.ExecuteAsync(etud);
            }
            catch (Exception e)
            {
                // On récupère ici les exceptions personnalisées définies dans la couche domain
                // Et on les envoie avec le code d'erreur 400 et l'intitulé "erreurs de validation"
                ModelState.AddModelError(nameof(e), e.Message);
                return ValidationProblem();
            }
            
            try
            {
                // Création du user associé
                user = new UniversiteUser { UserName = etudiantDto.Email, Email = etudiantDto.Email, Etudiant = etud };
                // Un créé l'utilisateur avec un mot de passe par défaut et un rôle étudiant
                await createUserUc.ExecuteAsync(etud.Email, etud.Email, "Miage2025#", Roles.Etudiant, etud); 
            }
            catch (Exception e)
            {
                // On supprime l'étudiant que l'on vient de créer. Sinon on a un étudiant mais pas de user associé
                await new DeleteEtudiantUseCase(repositoryFactory).ExecuteAsync(etud.Id);
                ModelState.AddModelError(nameof(e), e.Message);
                return ValidationProblem();
            }
            
            EtudiantDto dto = new EtudiantDto().ToDto(etud);
            // On revoie la route vers le get qu'on n'a pas encore écrit!
            return CreatedAtAction(nameof(GetEtudiant), new { id = dto.Id }, dto);
        }
        
        // PUT api/<EtudiantController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<EtudiantDto>> PutAsync(long id, [FromBody] EtudiantDto etudiantDto)
        {
            UpdateEtudiantUseCase updateEtudiantUc = new UpdateEtudiantUseCase(repositoryFactory);
            UpdateUniversiteUserUseCase updateUserUc = new UpdateUniversiteUserUseCase(repositoryFactory);

            if (id != etudiantDto.Id)
            {
                return BadRequest();
            }
            string role="";
            string email="";
            IUniversiteUser user = null;
            try
            {
                CheckSecu(out role, out email, out user);
            }
            catch (Exception e)
            {
                return Unauthorized();
            }
            if (!updateEtudiantUc.IsAuthorized(role)|| !updateUserUc.IsAuthorized(role)) return Unauthorized();
            // Mise à jour de l'étudiant
            try
            {
                await updateUserUc.ExecuteAsync(etudiantDto.ToEntity());
                await updateEtudiantUc.ExecuteAsync(etudiantDto.ToEntity());
            }
            catch (Exception e)
            {
                ModelState.AddModelError(nameof(e), e.Message);
                return ValidationProblem();
            }
            
            return NoContent();
        }


        // DELETE api/<EtudiantController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Parcours>> DeleteAsync(long id)
        {
            DeleteEtudiantUseCase etudiantUc = new DeleteEtudiantUseCase(repositoryFactory);
            DeleteUniversiteUserUseCase userUc = new DeleteUniversiteUserUseCase(repositoryFactory);
            string role = "";
            string email = "";
            IUniversiteUser user = null;
            try
            {
                CheckSecu(out role, out email, out user);
            }
            catch (Exception e)
            {
                return Unauthorized();
            }

            if (!etudiantUc.IsAuthorized(role) || !userUc.IsAuthorized(role)) return Unauthorized();
            // On supprime l'étudiant et le user avec l'Id id
            try
            {
                await userUc.ExecuteAsync(id);
                await etudiantUc.ExecuteAsync(id);
            }
            catch (Exception e)
            {
                ModelState.AddModelError(nameof(e), e.Message);
                return ValidationProblem();
            }

            return NoContent();
        }
        
        private void CheckSecu(out string role, out string email, out IUniversiteUser user)
        {
            role = "";
            // Récupération des informations de connexion dans la requête http entrante
            ClaimsPrincipal claims = HttpContext.User;
            // Faisons nos tests pour savoir si la personne est bien connectée
            if (claims.Identity?.IsAuthenticated != true) throw new UnauthorizedAccessException();
            // Récupérons le email de la personne connectée
            if (claims.FindFirst(ClaimTypes.Email)==null) throw new UnauthorizedAccessException();
            email = claims.FindFirst(ClaimTypes.Email).Value;
            if (email==null) throw new UnauthorizedAccessException();
            // Vérifions qu'il est bien associé à un utilisateur référencé
            user = new FindUniversiteUserByEmailUseCase(repositoryFactory).ExecuteAsync(email).Result;
            if (user==null) throw new UnauthorizedAccessException();
            // Vérifions qu'un rôle a bien été défini
            if (claims.FindFirst(ClaimTypes.Role)==null) throw new UnauthorizedAccessException();
            // Récupérons le rôle de l'utilisateur
            var ident = claims.Identities.FirstOrDefault();
            if (ident == null)throw new UnauthorizedAccessException();
            role = ident.FindFirst(ClaimTypes.Role).Value;
            if (role == null) throw new UnauthorizedAccessException();
            // Vérifions que le user a bien le role envoyé via http
            bool isInRole = new IsInRoleUseCase(repositoryFactory).ExecuteAsync(email, role).Result; 
            if (!isInRole) throw new UnauthorizedAccessException();
            // Si tout est passé sans renvoyer d'exception, le user est authentifié et conncté
        }
    }
}
