using UniversiteDomain.DataAdapters;
using UniversiteDomain.DataAdapters.DataAdaptersFactory;
using UniversiteDomain.Entities;

namespace UniversiteDomain.UseCases.EtudiantUseCases.Delete;

public class DeleteUniversiteUserUseCase(IRepositoryFactory repositoryFactory)
{
    public async Task ExecuteAsync(Etudiant etudiant)
    {
        await ExecuteAsync(etudiant.Id);
    }
    
    
    public async Task ExecuteAsync(long id)
    {
        IUniversiteUserRepository universiteUserRepository = repositoryFactory.UniversiteUserRepository();
        IUniversiteUser? universiteUser = await universiteUserRepository.FindAsync(id);
        await CheckBusinessRules(universiteUser);
        await universiteUserRepository.DeleteAsync(universiteUser);
        await repositoryFactory.SaveChangesAsync();
    }

    private async Task CheckBusinessRules(IUniversiteUser? universiteUser)
    {
        ArgumentNullException.ThrowIfNull(universiteUser);
    }

    public bool IsAuthorized(string role)
    {
        return role.Equals(Roles.Scolarite) || role.Equals(Roles.Responsable);
    }
}