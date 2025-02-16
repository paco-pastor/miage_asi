using UniversiteDomain.DataAdapters.DataAdaptersFactory;
using UniversiteDomain.Entities;

namespace UniversiteDomain.UseCases.ParcoursUseCases.Get;

public class GetParcoursUseCase(IRepositoryFactory repositoryFactory)
{
    public async Task<Parcours> ExecuteAsync(long id)
    {
        Parcours? parcours = await repositoryFactory.ParcoursRepository().FindAsync(id);
        await CheckBusinessRules(parcours);
        return parcours;
    }

    private async Task CheckBusinessRules(Parcours? parcours)
    {
        ArgumentNullException.ThrowIfNull(parcours);
    }
    
    public bool IsAuthorized(string role)
    {
        return role.Equals(Roles.Responsable) || role.Equals(Roles.Scolarite) || role.Equals(Roles.Etudiant);
    }
}
