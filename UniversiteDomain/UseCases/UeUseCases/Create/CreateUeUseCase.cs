using UniversiteDomain.DataAdapters.DataAdaptersFactory;
using UniversiteDomain.Entities;
using UniversiteDomain.Exceptions.UeExceptions;

namespace UniversiteDomain.UseCases.UeUseCases.Create;

public class CreateUeUseCase(IRepositoryFactory repositoryFactory)
{
    
    public async Task<Ue> ExecuteAsync(string numeroUe, string intitule)
    {
        var ue = new Ue
        {
            NumeroUe = numeroUe,
            Intitule = intitule
        };
        return await ExecuteAsync(ue);
    }
    
    public async Task<Ue> ExecuteAsync(Ue ue)
    {
        await CheckBusinessRules(ue);
        Ue newUe = await repositoryFactory.UeRepository().CreateAsync(ue);
        repositoryFactory.UeRepository().SaveChangesAsync().Wait();
        return newUe;
    }
    
    private async Task CheckBusinessRules(Ue ue)
    {
        //Un use case pour la création d'une Ue. Comme règle de gestion vous devez vérifier que deux Ues ne portent pas le même numéro et qu’une UE a un intitulé >3 caractères.
        ArgumentNullException.ThrowIfNull(ue);
        ArgumentNullException.ThrowIfNull(ue.NumeroUe);
        ArgumentNullException.ThrowIfNull(ue.Intitule);
        ArgumentNullException.ThrowIfNull(repositoryFactory);
        ArgumentNullException.ThrowIfNull(repositoryFactory.UeRepository());
        
        if(ue.Intitule.Length < 3) throw new ArgumentOutOfRangeException("L'intitulé de l'UE doit contenir plus de 3 caractères");
        
        List<Ue> ues = await repositoryFactory.UeRepository().FindByConditionAsync(u => u.NumeroUe.Equals(ue.NumeroUe));
        
        if (ues is {Count:>0})
        {
            throw new DuplicateUeException("Une UE avec ce numéro existe déjà");
        }
        
    }
    
    public bool IsAuthorized(string role)
    {
        return role.Equals(Roles.Responsable) || role.Equals(Roles.Scolarite);
    }
}