﻿using UniversiteDomain.DataAdapters.DataAdaptersFactory;
using UniversiteDomain.Entities;

namespace UniversiteDomain.UseCases.SecurityUseCases.Get;

public class FindUniversiteUserByEmailUseCase(IRepositoryFactory repositoryFactory)
{
    public async Task<IUniversiteUser?> ExecuteAsync(string email)
    {
        await CheckBusinessRules(email);
        IUniversiteUser? user = await repositoryFactory.UniversiteUserRepository().FindByEmailAsync(email);
        return user;
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