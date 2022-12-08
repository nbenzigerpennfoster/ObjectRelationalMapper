namespace Domain
{
    public class PersonAggregate : Aggregate
    {
        private readonly Person person;

        public PersonAggregate(Person person)
            : base(person.PersonId.ToString())
        {
            this.person = person;
        }

        public void AddPerson(Person personToAdd)
        {
            ValidatePerson(personToAdd);

            if (HasErrors())
            {
                return;
            }

            person.FirstName = personToAdd.FirstName;
            person.LastName = personToAdd.LastName;
            person.Age = personToAdd.Age;
            person.Address = personToAdd.Address;

            personToAdd.PersonId = person.PersonId;

            StageEvent(new PersonAddedEvent
            {
                PersonAddData = personToAdd
            });
        }

        public void UpdatePerson(Person personUpdateRequest)
        {
            ValidatePerson(personUpdateRequest);

            if (HasErrors())
            {
                return;
            }

            person.FirstName = personUpdateRequest.FirstName;
            person.LastName = personUpdateRequest.LastName;
            person.Age = personUpdateRequest.Age;
            person.Address = personUpdateRequest.Address;

            personUpdateRequest.PersonId = person.PersonId;

            StageEvent(new PersonUpdatedEvent
            {
                PersonUpdateData = personUpdateRequest
            });
        }

        public void DeletePerson()
        {
            if (person.Children.Count > 0)
            {
                RecordError("Person can't be deleted if they have children");
                return;
            }

            StageEvent(new PersonDeletedEvent
            {
                DeletedEntity = person
            });
        }

        private void ValidatePerson(Person personToValidate)
        {
            if (personToValidate.FirstName == null || personToValidate.LastName == null)
            {
                RecordError("Person doesn't have a complete name");
            }

            if (personToValidate.Age > 150)
            {
                RecordError("Age range is invalid");
            }

            if (personToValidate.Address != null)
            {
                if (personToValidate.Address.Address1 == null
                    || personToValidate.Address.City == null
                    || personToValidate.Address.State == null
                    || personToValidate.Address.Zip == null)
                {
                    RecordError("Invalid address");
                }
            }
        }
    }
}
