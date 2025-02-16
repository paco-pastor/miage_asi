using UniversiteDomain.DataAdapters.DataAdaptersFactory;
using UniversiteDomain.Entities;

namespace UniversiteDomain.UseCases.UeUseCases.Get;

public class GetUeUseCase(IRepositoryFactory repositoryFactory)
{
    public async Task<Ue> ExecuteAsync(long id)
    {
        Ue? ue = await repositoryFactory.UeRepository().FindAsync(id);
        await CheckBusinessRules(ue);
        return ue;
    }

    private async Task CheckBusinessRules(Ue? ue)
    {
    }
    
    public bool IsAuthorized(string role)
    {
        return role.Equals(Roles.Responsable) || role.Equals(Roles.Scolarite) || role.Equals(Roles.Etudiant);
    }
}