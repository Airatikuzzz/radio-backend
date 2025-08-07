namespace radio_backend.Auth;

public class PersonRepository
{ 
    List<Person> People = new List<Person>
    {
        new Person("euro_server", "12345"),
        new Person("bob@gmail.com", "55555")
    };

    public List<Person> GetPeople()
    {
        return People;
    }
}