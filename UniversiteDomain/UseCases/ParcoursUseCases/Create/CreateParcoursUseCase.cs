using UniversiteDomain.DataAdapters;
using UniversiteDomain.DataAdapters.DataAdaptersFactory;
using UniversiteDomain.Entities;
using UniversiteDomain.Exceptions.ParcoursExceptions;

namespace UniversiteDomain.UseCases.ParcoursUseCases.Create;

public class CreateParcoursUseCase(IRepositoryFactory repositoryFactory)
{
    
    public async Task<Parcours> ExecuteAsync(string nomParcours, int anneeFormation)
    {
        var parcours = new Parcours
        {
            NomParcours = nomParcours,
            AnneeFormation = anneeFormation
        };
        return await ExecuteAsync(parcours);
    }
    
    public async Task<Parcours> ExecuteAsync(Parcours parcours)
    {
        await CheckBusinessRules(parcours);
        Parcours newParcours = await repositoryFactory.ParcoursRepository().CreateAsync(parcours);
        repositoryFactory.ParcoursRepository().SaveChangesAsync().Wait();
        return newParcours;
    }
    
    private async Task CheckBusinessRules(Parcours parcours)
    {
        ArgumentNullException.ThrowIfNull(parcours);
        ArgumentNullException.ThrowIfNull(parcours.AnneeFormation);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(parcours.AnneeFormation);
        ArgumentNullException.ThrowIfNull(parcours.NomParcours);
        ArgumentNullException.ThrowIfNull(repositoryFactory.ParcoursRepository());
        
        if (parcours.AnneeFormation != 1 && parcours.AnneeFormation != 2)
        {
            throw new ArgumentOutOfRangeException("L'année de formation doit être 1 ou 2");
        }
        
        List<Parcours> parcoursList = await repositoryFactory.ParcoursRepository().FindByConditionAsync(p => p.NomParcours.Equals(parcours.NomParcours));
        if (parcoursList is {Count:>0})
        {
            throw new DuplicateParcoursException("Un parcours avec ce nom existe déjà");
        }
    }
    
    public bool IsAuthorized(string role)
    {
        return role.Equals(Roles.Responsable) || role.Equals(Roles.Scolarite);
    }
    
}