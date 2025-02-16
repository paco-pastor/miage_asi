using UniversiteDomain.DataAdapters;
using UniversiteDomain.DataAdapters.DataAdaptersFactory;
using UniversiteDomain.Entities;

namespace UniversiteDomain.UseCases.EtudiantUseCases.Delete;

public class DeleteEtudiantUseCase(IRepositoryFactory repositoryFactory)
{
    public async Task ExecuteAsync(Etudiant etudiant)
    {
        await ExecuteAsync(etudiant.Id);
    }
    
    
    public async Task ExecuteAsync(long id)
    {
        IEtudiantRepository etudiantRepository = repositoryFactory.EtudiantRepository();
        Etudiant? etudiant = await etudiantRepository.FindAsync(id);
        await CheckBusinessRules(etudiant);
        await etudiantRepository.DeleteAsync(etudiant);
        await repositoryFactory.SaveChangesAsync();
    }

    private async Task CheckBusinessRules(Etudiant? etudiant)
    {
        ArgumentNullException.ThrowIfNull(etudiant);
    }

    public bool IsAuthorized(string role)
    {
        return role.Equals(Roles.Scolarite) || role.Equals(Roles.Responsable);
    }
}