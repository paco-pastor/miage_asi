﻿using UniversiteDomain.DataAdapters;
using UniversiteDomain.DataAdapters.DataAdaptersFactory;
using UniversiteDomain.Entities;
using UniversiteDomain.Exceptions.EtudiantExceptions;
using UniversiteDomain.Util;

namespace UniversiteDomain.UseCases.EtudiantUseCases.Update;

public class UpdateEtudiantUseCase(IRepositoryFactory repositoryFactory)
{
    public async Task ExecuteAsync(Etudiant etudiant)
    {
        IEtudiantRepository etudiantRepository = repositoryFactory.EtudiantRepository();
        await CheckBusinessRules(etudiant);
        await etudiantRepository.UpdateAsync(etudiant);
        await repositoryFactory.SaveChangesAsync();
    }

    private async Task CheckBusinessRules(Etudiant etudiant)
    {
        ArgumentNullException.ThrowIfNull(etudiant);
        ArgumentNullException.ThrowIfNull(etudiant.NumEtud);
        ArgumentNullException.ThrowIfNull(etudiant.Email);
        ArgumentNullException.ThrowIfNull(repositoryFactory);
        ArgumentNullException.ThrowIfNull(repositoryFactory.EtudiantRepository());
        
        // On recherche un étudiant avec le même numéro étudiant
        List<Etudiant> existe = await repositoryFactory.EtudiantRepository().FindByConditionAsync(e=>e.NumEtud.Equals(etudiant.NumEtud));

        // Si un étudiant avec le même numéro étudiant existe déjà, on lève une exception personnalisée
        if (existe is {Count:>0})throw new DuplicateNumEtudException(etudiant.NumEtud + " - ce numéro d'étudiant est déjà affecté à un étudiant");
        
        // Vérification du format du mail
        if (!CheckEmail.IsValidEmail(etudiant.Email)) throw new InvalidEmailException(etudiant.Email + " - Email mal formé");
        
        // On vérifie si l'email est déjà utilisé
        existe = await repositoryFactory.EtudiantRepository().FindByConditionAsync(e=>e.Email.Equals(etudiant.Email));
        // Une autre façon de tester la vacuité de la liste
        if (existe is {Count:>0}) throw new DuplicateEmailException(etudiant.Email +" est déjà affecté à un étudiant");
        // Le métier définit que les nom doite contenir plus de 3 lettres
        if (etudiant.Nom.Length < 3) throw new InvalidNomEtudiantException(etudiant.Nom +" incorrect - Le nom d'un étudiant doit contenir plus de 3 caractères");

    }

    public bool IsAuthorized(string role)
    {
        return role.Equals(Roles.Scolarite) || role.Equals(Roles.Responsable);
    }
}