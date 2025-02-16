using UniversiteDomain.DataAdapters.DataAdaptersFactory;
using UniversiteDomain.Entities;

namespace UniversiteDomain.UseCases.SecurityUseCases.Get;

public class IsInRoleUseCase(IRepositoryFactory repositoryFactory)
{
    public async Task<bool> ExecuteAsync(string email, string role)
    {
        await CheckBusinessRules(email);
        return await repositoryFactory.UniversiteUserRepository().IsInRoleAsync(email, role);
    }

    private async Task CheckBusinessRules(string email)
    {
        ArgumentNullException.ThrowIfNull(email);
        ArgumentNullException.ThrowIfNull(repositoryFactory);
        ArgumentNullException.ThrowIfNull(repositoryFactory.UniversiteUserRepository());
    }
  
    public bool IsAuthorized(string role)
    {
        return role.Equals(Roles.Responsable) || role.Equals(Roles.Scolarite) || role.Equals(Roles.Etudiant);
    }
}