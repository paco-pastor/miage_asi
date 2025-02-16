using UniversiteDomain.DataAdapters.DataAdaptersFactory;
using UniversiteDomain.Entities;

namespace UniversiteDomain.UseCases.EtudiantUseCases.Get;

public class GetEtudiantUseCase(IRepositoryFactory repositoryFactory)
{
    public async Task<Etudiant> ExecuteAsync(long id)
    {
        Etudiant? etudiant = await repositoryFactory.EtudiantRepository().FindAsync(id);
        await CheckBusinessRules(etudiant);
        return etudiant;
    }

    private async Task CheckBusinessRules(Etudiant? etudiant)
    {
        ArgumentNullException.ThrowIfNull(etudiant);
    }
    
    public bool IsAuthorized(string role)
    {
        return role.Equals(Roles.Responsable) || role.Equals(Roles.Scolarite) || role.Equals(Roles.Etudiant);
    }
}