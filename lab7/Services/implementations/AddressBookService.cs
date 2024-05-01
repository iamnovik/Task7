using lab7.Data;
using lab7.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace lab7.Services.implementations;

public class AddressBookService
{
    private readonly AppDbContext dbContext;

    public AddressBookService(AppDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    // Добавление контакта в адресную книгу пользователя
    public async Task AddContactAsync(Contact user, Contact contactToAdd)
    {
        var contactRelationship = new ContactRelationship
        {
            SourceContactId = user.Id,
            TargetContactId = contactToAdd.Id
        };

        dbContext.ContactRelationships.Add(contactRelationship);
        await dbContext.SaveChangesAsync();
    }

    // Удаление контакта из адресной книги пользователя
    public async Task RemoveContactAsync(Contact user, Contact contactToRemove)
    {
        var contactRelationship = await dbContext.ContactRelationships.FirstOrDefaultAsync(
            cr => cr.SourceContactId == user.Id && cr.TargetContactId == contactToRemove.Id);

        if (contactRelationship != null)
        {
            dbContext.ContactRelationships.Remove(contactRelationship);
            await dbContext.SaveChangesAsync();
        }
    }

    // Получение всех контактов пользователя
    public async Task<List<Contact>> GetAllContactsAsync(Contact user)
    {
        var relatedContacts = await dbContext.ContactRelationships
            .Where(cr => cr.SourceContactId == user.Id)
            .Select(cr => cr.TargetContact)
            .ToListAsync();

        return relatedContacts;
    }
    
    public async Task<bool> IsInAddressBookAsync(Contact user, Contact contact)
    {
        return await dbContext.ContactRelationships.AnyAsync(
            cr => cr.SourceContactId == user.Id && cr.TargetContactId == contact.Id);
    }

    // Получение всех контактов
    public async Task<List<Contact>> GetAllContactsAsync()
    {
        return await dbContext.Users.ToListAsync();
    }
}