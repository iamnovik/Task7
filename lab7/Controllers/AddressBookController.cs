using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using lab7.Models;
using lab7.Services.implementations;
using Microsoft.AspNetCore.Identity;

namespace lab7.Controllers
{
    [Authorize]
    public class AddressBookController : Controller
    {
        private readonly AddressBookService _addressBookService;
        private readonly UserManager<Contact> _userManager;

        public AddressBookController(AddressBookService addressBookService, UserManager<Contact> userManager)
        {
            _addressBookService = addressBookService;
            _userManager = userManager;
        }

        // Действие для отображения всех контактов пользователя
        public async Task<IActionResult> Index(string searchName)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var contacts = new List<ContactViewModel>();

            if (!string.IsNullOrEmpty(searchName))
            {
                ViewBag.IsSearch = true;
                ViewBag.Title = "Search Results";
                ViewBag.SearchName = searchName;
                if (searchName.Equals(currentUser.FullName))
                {
                    return View(contacts);
                } ;
                var allContacts = await _addressBookService.GetAllContactsAsync();
                var filteredContacts = allContacts.Where(c => c.FullName.Contains(searchName)).ToList();

                foreach (var contact in filteredContacts)
                {
                    var isInAddressBook = await _addressBookService.IsInAddressBookAsync(currentUser, contact);
                    contacts.Add(new ContactViewModel
                    {
                        Contact = contact,
                        IsInAddressBook = isInAddressBook
                    });
                }

                ViewBag.SearchName = searchName;
            }
            else
            {
                ViewBag.IsSearch = false;
                ViewBag.Title = "Address Book";
                ViewBag.SearchName = "";
                var userContacts = await _addressBookService.GetAllContactsAsync(currentUser);

                foreach (var contact in userContacts)
                {
                    contacts.Add(new ContactViewModel
                    {
                        Contact = contact,
                        IsInAddressBook = true
                    });
                }

                ViewBag.SearchName = "";
            }

            return View(contacts);
        }



        // Действие для добавления нового контакта
        [HttpPost]
        public async Task<IActionResult> AddContact(string contactId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var contactToAdd = await _userManager.FindByIdAsync(contactId);
            if (contactToAdd != null)
            {

                await _addressBookService.AddContactAsync(currentUser, contactToAdd);
            }
            return RedirectToAction(nameof(Index));
        }

        // Действие для удаления контакта
        [HttpPost]
        public async Task<IActionResult> RemoveContact(string contactId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var contactToRemove = await _userManager.FindByIdAsync(contactId);
            if (contactToRemove != null)
            {
                await _addressBookService.RemoveContactAsync(currentUser, contactToRemove);
            }
            return RedirectToAction(nameof(Index));
        }

        // Другие действия, если нужно...
    }
}
