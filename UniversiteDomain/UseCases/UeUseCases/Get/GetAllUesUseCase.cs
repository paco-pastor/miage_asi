using UniversiteDomain.DataAdapters.DataAdaptersFactory;
using UniversiteDomain.Entities;

namespace UniversiteDomain.UseCases.UeUseCases.Get;

public class GetAllUesUseCase(IRepositoryFactory repositoryFactory)
{
    public async Task<List<Ue>> ExecuteAsync()
    {
        List<Ue> ues = await repositoryFactory.UeRepository().FindAllAsync();
        await CheckBusinessRules(ues);
        return ues;
    }

    private async Task CheckBusinessRules(List<Ue> ues)
    {
    }
    
    public bool IsAuthorized(string role)
    {
        return role.Equals(Roles.Responsable) || role.Equals(Roles.Scolarite) || role.Equals(Roles.Etudiant);
    }
    
}