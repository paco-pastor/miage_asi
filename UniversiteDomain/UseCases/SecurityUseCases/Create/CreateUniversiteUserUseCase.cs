using UniversiteDomain.DataAdapters;
using UniversiteDomain.DataAdapters.DataAdaptersFactory;
using UniversiteDomain.Entities;
using UniversiteDomain.Exceptions.EtudiantExceptions;

namespace UniversiteDomain.UseCases.SecurityUseCases.Create;

public class CreateUniversiteUserUseCase(IRepositoryFactory repositoryFactory)
{
    public async Task<IUniversiteUser?> ExecuteAsync(string email, string userName, string password, string role, Etudiant ? etudiant)
    {
        await CheckBusinessRules(userName, password, role, etudiant);
        IUniversiteUser? userCree = await repositoryFactory.UniversiteUserRepository().AddUserAsync(email, userName, password, role,etudiant);
        await repositoryFactory.SaveChangesAsync();
        return userCree;
    }
    
    private async Task CheckBusinessRules(string userName, string password, string role, Etudiant ? etudiant)
    {
        ArgumentNullException.ThrowIfNull(userName);
        ArgumentNullException.ThrowIfNull(password);
        ArgumentNullException.ThrowIfNull(role);
        ArgumentNullException.ThrowIfNull(repositoryFactory);
        ArgumentOutOfRangeException.ThrowIfEqual(
            role.Equals(Roles.Scolarite) || role.Equals(Roles.Responsable) || role.Equals(Roles.Etudiant), false);
        // On vérifie que l'étudiant existe
        if (etudiant != null)
        {
            IEtudiantRepository etudiantRepository = repositoryFactory.EtudiantRepository();
            ArgumentNullException.ThrowIfNull(etudiantRepository);
            List<Etudiant> existe = await etudiantRepository.FindByConditionAsync(e => e.NumEtud.Equals(etudiant.NumEtud));
            if (existe is { Count: 0 }) throw new EtudiantNotFoundException(etudiant.NumEtud+" - non trouvé");
        }
    }
    
    public bool IsAuthorized(string role)
    {
        return role.Equals(Roles.Responsable) || role.Equals(Roles.Scolarite) || role.Equals(Roles.Etudiant);
    }
}