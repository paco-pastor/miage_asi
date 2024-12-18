using UniversiteDomain.DataAdapters;
using UniversiteDomain.DataAdapters.DataAdaptersFactory;
using UniversiteDomain.Entities;
using UniversiteDomain.Exceptions.ParcoursExceptions;


namespace UniversiteDomain.UseCases.EtudiantUseCases.Create;

public class CreateParcoursUseCase(IRepositoryFactory repositoryFactory)
{
    public async Task<Parcours> ExecuteAsync(string nomParcours, int anneeFormation)
    {
        var parcours = new Parcours { NomParcours = nomParcours, AnneeFormation = anneeFormation };
        return await ExecuteAsync(parcours);
    }

    public async Task<Parcours> ExecuteAsync(Parcours parcours)
    {
        await CheckBusinessRules(parcours);
        Parcours p = await repositoryFactory.ParcoursRepository().CreateAsync(parcours);
        repositoryFactory.ParcoursRepository().SaveChangesAsync().Wait();
        return p;
    }

    private async Task CheckBusinessRules(Parcours parcours)
    {
        ArgumentNullException.ThrowIfNull(parcours);
        ArgumentNullException.ThrowIfNull(parcours.NomParcours);
        ArgumentNullException.ThrowIfNull(parcours.AnneeFormation);
        
        List<Parcours> existe = await repositoryFactory.ParcoursRepository().FindByConditionAsync(p => p.NomParcours.Equals(parcours.NomParcours) && p.AnneeFormation.Equals(parcours.AnneeFormation));
        if (existe is { Count: > 0}) throw new DuplicateParcoursException("Le parcours " + parcours.NomParcours + " année " + parcours.AnneeFormation + " existe déjà");
        
        if (parcours.AnneeFormation != 1 && parcours.AnneeFormation != 2) throw new InvalidAnneeFormationException("L'année de formation doit être 1 ou 2");
        
        if (parcours.NomParcours.Length < 3) throw new InvalidNomParcoursException("Le nom du parcours doit contenir plus de 3 caractères");
    }
}