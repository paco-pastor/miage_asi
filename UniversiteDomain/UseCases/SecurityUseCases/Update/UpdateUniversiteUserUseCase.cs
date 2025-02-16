using UniversiteDomain.DataAdapters;
using UniversiteDomain.DataAdapters.DataAdaptersFactory;
using UniversiteDomain.Entities;
using UniversiteDomain.Exceptions.EtudiantExceptions;

namespace UniversiteDomain.UseCases.EtudiantUseCases.Update;

public class UpdateUniversiteUserUseCase(IRepositoryFactory repositoryFactory)
{
    public async Task ExecuteAsync(Etudiant? etudiant)
    {
        await CheckBusinessRules(etudiant);
        await repositoryFactory.UniversiteUserRepository().UpdateUserAsync(etudiant);
        await repositoryFactory.SaveChangesAsync();
    }

    private async Task CheckBusinessRules(Etudiant? etudiant)
    {
        ArgumentNullException.ThrowIfNull(etudiant);
        ArgumentNullException.ThrowIfNull(etudiant.Nom);
        ArgumentNullException.ThrowIfNull(etudiant.Prenom);
        ArgumentNullException.ThrowIfNull(etudiant.NumEtud);
        ArgumentNullException.ThrowIfNull(repositoryFactory);

        // On vérifie que l'étudiant existe
        if (etudiant == null)
        {
            throw new EtudiantNotFoundException("Etudiant non trouvé");
        }
        
        IEtudiantRepository etudiantRepository = repositoryFactory.EtudiantRepository();
        ArgumentNullException.ThrowIfNull(etudiantRepository);
        List<Etudiant> existe = await etudiantRepository.FindByConditionAsync(e => e.NumEtud.Equals(etudiant.NumEtud));
        if (existe is { Count: 0 }) throw new EtudiantNotFoundException(etudiant.NumEtud+" - non trouvé");
        
        // On vérifie qu'un user est lier à l'étudiant
        IUniversiteUserRepository universiteUserRepository = repositoryFactory.UniversiteUserRepository();
        ArgumentNullException.ThrowIfNull(universiteUserRepository);
        IUniversiteUser? user = await universiteUserRepository.FindByEmailAsync(existe[0].Email);
        if (user == null) throw new EtudiantNotFoundException("User non trouvé");
        
    }

    public bool IsAuthorized(string role)
    {
        return role.Equals(Roles.Scolarite) || role.Equals(Roles.Responsable);
    }
    
}