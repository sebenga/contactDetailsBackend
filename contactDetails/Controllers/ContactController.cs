using contactDetails.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System.Linq;

namespace contactDetails.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
       
        private readonly IConfiguration configuration;
        public ContactController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        [HttpGet]
        public JsonResult Get()
        {
            MongoClient dbClient = new MongoClient(this.configuration.GetConnectionString("ContactCon"));
            var dbList = dbClient.GetDatabase("contactsdb").GetCollection<Contact>("ContactDetails").AsQueryable();
            return new JsonResult(dbList);

        }

        [HttpPost]
        public JsonResult Post(Contact contact)
        {
            MongoClient dbClient = new MongoClient(this.configuration.GetConnectionString("ContactCon"));
            int LastContactId = dbClient.GetDatabase("contactsdb").GetCollection<Contact>("ContactDetails").AsQueryable().Count();
            contact.ContactId = LastContactId + 1;  
            dbClient.GetDatabase("contactsdb").GetCollection<Contact>("ContactDetails").InsertOne(contact);
            return new JsonResult("Added Successfully");

        }

        [HttpPut]
        public JsonResult Put(Contact contact)
        {
            MongoClient dbClient = new MongoClient(this.configuration.GetConnectionString("ContactCon"));
            var filter = Builders<Contact>.Filter.Eq("ContactId", contact.ContactId);
            var update = Builders<Contact>.Update.Set("Name", contact.Name)
                                               .Set("Email", contact.Email)
                                               .Set("Phone", contact.Phone)
                                               .Set("Company", contact.Company)
                                               .Set("Notes", contact.Notes);
            dbClient.GetDatabase("contactsdb").GetCollection<Contact>("ContactDetails").UpdateOne(filter, update);
            return new JsonResult("Update Successfully");

        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            MongoClient dbClient = new MongoClient(this.configuration.GetConnectionString("ContactCon"));
            var filter = Builders<Contact>.Filter.Eq("ContactId", id);
          
            dbClient.GetDatabase("contactsdb").GetCollection<Contact>("ContactDetails").DeleteOne(filter);
            return new JsonResult("Delete Successfully");

        }
    }
}
