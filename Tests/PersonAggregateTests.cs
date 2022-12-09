using Domain;

namespace Tests
{
    [TestClass]
    public class PersonAggregateTests
    {
        [TestMethod]
        public void DeletePersonTest_CantDeletePersonWithChildren()
        {
            // arrange
            var personAggregate = new PersonAggregate(new Person
            {
                Children = new List<Child>
                {
                    new Child()
                }
            });

            // act
            personAggregate.DeletePerson();

            // assert
            Assert.IsTrue(personAggregate.HasErrors());
            Assert.AreEqual(1, personAggregate.Errors().Count());
        }

        [TestMethod]
        public void DeletePersonTest_CanDeletePerson()
        {
            // arrange
            var personAggregate = new PersonAggregate(new Person
            {
                Children = new List<Child>()
            });

            // act
            personAggregate.DeletePerson();

            // assert
            Assert.IsFalse(personAggregate.HasErrors());
            var events = personAggregate.Events().ToList();
            Assert.AreEqual(1, events.Count());
            Assert.IsInstanceOfType(events[0], typeof(PersonDeletedEvent));
        }

        [TestMethod]
        public void AddPerson_PersonIsNegativeYearsOld()
        {
            // arrange
            var personAggregate = new PersonAggregate(new Person());
            var personToAdd = new Person
            {
                FirstName = "Super",
                LastName = "Man",
                Age = -1
            };

            // act
            personAggregate.AddPerson(personToAdd);

            // assert
            Assert.IsTrue(personAggregate.HasErrors());
            Assert.AreEqual(1, personAggregate.Errors().Count());
        }

        [TestMethod]
        public void AddPerson_Success_NoAddress()
        {
            // arrange
            var personAggregate = new PersonAggregate(new Person());
            var personToAdd = new Person
            {
                FirstName = "Super",
                LastName = "Man",
                Age = 21
            };

            // act
            personAggregate.AddPerson(personToAdd);

            // assert
            Assert.IsFalse(personAggregate.HasErrors());
            var events = personAggregate.Events().ToList();
            Assert.AreEqual(1, events.Count());
            Assert.IsInstanceOfType(events[0], typeof(PersonAddedEvent));

            var personAddedEvent = (PersonAddedEvent)events[0];
            Assert.AreEqual("Super", personAddedEvent.PersonAddData.FirstName);
            Assert.AreEqual("Man", personAddedEvent.PersonAddData.LastName);
            Assert.AreEqual(21, personAddedEvent.PersonAddData.Age);
            Assert.IsNull(personAddedEvent.PersonAddData.Address);
            Assert.IsNull(personAddedEvent.PersonAddData.Children);
        }
    }
}