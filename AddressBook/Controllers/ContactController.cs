using AddressBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AddressBook.Controllers
{
    //[Authorize]
    public class ContactController : Controller
    {
        

        public ActionResult Index()
        {
            ViewBag.btnSaveTitle = true;
            ViewBag.btnCancelEnable = false;
            using (AddressBookDBEntities ABEntity = new AddressBookDBEntities())
            {
                return View(ABEntity.Contacts.ToList());
            }
        }

        [HttpGet]
        public ActionResult Create(int id=0)
        {
            ViewBag.btnSaveTitle = true;
            ViewBag.btnCancelEnable = false;
            if (id == 0)
                return View();
            using (AddressBookDBEntities ABEntity = new AddressBookDBEntities())
            {
                ViewBag.btnSaveTitle = false;
                ViewBag.btnCancelEnable = true;
                return View(ABEntity.Contacts.Find(id));
            }
        }

        [HttpPost]
        public ActionResult Create(Contact contact, string btnCancel,int id=0)
        {
            if (btnCancel != null)
                return RedirectToAction("Index");
            if (ModelState.IsValid)
            {
                using (AddressBookDBEntities ABEntity = new AddressBookDBEntities())
                {
                    if (id == 0)
                        ABEntity.Contacts.Add(contact);
                    else
                    {
                        var selContact = ABEntity.Contacts.First(c => c.ContactID == id);
                        selContact.FirstName = contact.FirstName;
                        selContact.LastName = contact.LastName;
                        selContact.Phone = contact.Phone;
                        selContact.StreetName = contact.StreetName;
                        selContact.Province = contact.Province;
                        selContact.PostalCode = contact.PostalCode;
                        selContact.Country = contact.Country;
                    }
                    ABEntity.SaveChanges();
                    ModelState.Clear();
                    ViewBag.Message = "Successfuly Registration Done.";
                }
            }

            return RedirectToAction("Index"); 

        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            using (AddressBookDBEntities ABEntity = new AddressBookDBEntities())
            {
                ViewBag.Message = "";

                var selContact = ABEntity.Contacts.First(c => c.ContactID == id);
                ABEntity.Contacts.Remove(selContact);
                ABEntity.SaveChanges();

                ModelState.Clear();
                selContact = null;

                return RedirectToAction("Index");
            }
        }

        public ActionResult GetContactsList(string searchInput)
        {
            using (AddressBookDBEntities ABEntity = new AddressBookDBEntities())
            {
                //if (searchInput=="")
                    //return PartialView(ABEntity.Contacts.ToList());
                return PartialView("ContactsList",ABEntity.Contacts.Where(c => c.FirstName.Contains(searchInput) || c.LastName.Contains(searchInput)).ToList());
            }
        }

        
        public ActionResult Search(string searchInput)
        {
            using (AddressBookDBEntities ABEntity = new AddressBookDBEntities())
            {
                var contact = ABEntity.Contacts.Where(c => c.FirstName.Contains(searchInput) || c.LastName.Contains(searchInput)).ToList();
                return View("Index",ABEntity.Contacts.Where(c => c.FirstName.Contains(searchInput) || c.LastName.Contains(searchInput)).ToList());
            }

        }
    
        public ActionResult newform()
        {
            ViewBag.btnSaveTitle = true;
            ViewBag.btnCancelEnable = false;
            return View();
        }
    }
}
