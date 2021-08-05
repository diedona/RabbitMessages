using RabbitMessages.Producer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMessages.Producer.Data
{
    public class PeopleFactory
    {
        private readonly IEnumerable<Person> _People;

        public PeopleFactory()
        {
            _People = SeedPeople();
        }

        public Person GetRandom()
        {
            int index = new Random(DateTime.Now.Millisecond).Next(_People.Count() - 1);
            return _People.ElementAt(index);
        }

        private IEnumerable<Person> SeedPeople()
        {
            return new List<Person>()
            {
                new Person()
                {
                    Name = "Diego Doná",
                    Birthday = new DateTime(1989, 6, 19),
                    Id = Guid.NewGuid()
                },
                new Person()
                {
                    Name = "Levy Fidelix",
                    Birthday = new DateTime(1951, 12, 27),
                    Id = Guid.NewGuid(),
                    VIP = true
                },
                new Person()
                {
                    Name = "Luciana Genro",
                    Birthday = new DateTime(1971, 1, 17),
                    Id = Guid.NewGuid(),
                    VIP = true
                },
            };
        }
    }
}
