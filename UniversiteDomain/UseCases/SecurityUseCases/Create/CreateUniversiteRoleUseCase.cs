using UniversiteDomain.DataAdapters.DataAdaptersFactory;
using UniversiteDomain.Entities;

namespace UniversiteDomain.UseCases.SecurityUseCases.Create;

public class CreateUniversiteRoleUseCase(IRepositoryFactory repositoryFactory)
{
    public async Task ExecuteAsync(string role)
    {
        await CheckBusinessRules(role);
        await repositoryFactory.UniversiteRoleRepository().AddRoleAsync(role);
        await repositoryFactory.SaveChangesAsync();
    }

    private async Task CheckBusinessRules(string role)
    {
        ArgumentNullException.ThrowIfNull(role);
        ArgumentNullException.ThrowIfNull(repositoryFactory);
    }
    
    public bool IsAuthorized(string role)
    {
        return role.Equals(Roles.Responsable) || role.Equals(Roles.Scolarite);
    }
}