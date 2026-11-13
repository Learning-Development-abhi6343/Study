namespace CRUDTests
{
    using System.Linq.Expressions;
    using AutoFixture;
    using Entities;
    using EntityFrameworkCoreMock;
    using FluentAssertions;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using RepositoryContracts;
    using Serilog;
    using ServiceContractsxUnit;
    using ServiceContractsxUnit.DTO;
    using ServiceContractsxUnit.Enums;
    using ServicesxUnit;

    /// <summary>
    /// This class contains unit tests for the PersonsService class, which is responsible for managing person-related operations.
    /// </summary>
    public class PersonsServiceTest
    {
        /// <summary>
        /// The instance of the IPersonsService being tested.
        /// </summary>
        readonly IPersonsService _personService;

        /// <summary>
        /// The instance of the ICountriesService used for country-related operations.
        /// </summary>
        readonly ICountriesService _countriesService;

        /// <summary>
        /// The test output helper used for logging test output.
        /// </summary>
        readonly ITestOutputHelper _testOutputHelper;

        /// <summary>
        /// The mock instance of the IPersonsRepository used for testing.
        /// </summary>
        readonly Mock<IPersonsRepository> _personsRepositoryMock;

        /// <summary>
        /// The instance of the IPersonsRepository used for testing.
        /// </summary>
        readonly IPersonsRepository _personsRepository;

        /// <summary>
        /// The fixture used for generating test data.
        /// </summary>
        readonly IFixture _fixture;

        /// <summary>
        /// Initializes a new instance of the <see cref="PersonsServiceTest"/> class.
        /// </summary>
        /// <param name="testOutputHelper">The test output helper.</param>
        public PersonsServiceTest(ITestOutputHelper testOutputHelper)
        {
            /*_countryService = new CountriesService(new PersonsDbContext(new DbContextOptionsBuilder<PersonsDbContext>().Options));
            _personService = new PersonsService();
            _personService = new PersonsService(new PersonsDbContext(new DbContextOptionsBuilder<PersonsDbContext>().Options), _countriesService);
            _countriesService = new CountriesService(false);*/
            this._testOutputHelper = testOutputHelper;

            var countriesInitialData = new List<Country>() { };
            var personsInitialData = new List<Person>() { };
            var dbContextMock = new DbContextMock<ApplicationDbContext>(new DbContextOptionsBuilder<ApplicationDbContext>().Options);
            var dbContext = dbContextMock.Object;
            dbContextMock.CreateDbSetMock(temp => temp.Countries, countriesInitialData);
            dbContextMock.CreateDbSetMock(temp => temp.Persons, personsInitialData);

            /*this._countriesService = new CountriesService(dbContext);
            this._countriesService = new CountriesService(null);
            this._personService = new PersonsService(dbContext, _countriesService);
            this._personService = new PersonsService(null);*/

            var diagnosticContextMock = new Mock<IDiagnosticContext>();
            var loggerMock = new Mock<Microsoft.Extensions.Logging.ILogger<PersonsService>>();
            this._fixture = new Fixture();
            this._personsRepositoryMock = new Mock<IPersonsRepository>();
            this._personsRepository = this._personsRepositoryMock.Object;
            this._personService = new PersonsService(this._personsRepository, loggerMock.Object, diagnosticContextMock.Object);
        }

        #region AddPerson
        //When we supply null value as PersonAddRequest, it should throw ArgumentNullException
        [Fact]
        //public void AddPerson_NullPerson()
        public async Task AddPerson_NullPerson()
        {
            //Arrange
            PersonAddRequest personAddRequest = null;

            //Act
            //Assert.Throws<ArgumentNullException>(() =>
            //await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            //{
            //    await _personService.AddPerson(personAddRequest);
            //});
            Func<Task> action = async () => await _personService.AddPerson(personAddRequest);
            //action.Invoke();
            await action.Should().ThrowAsync<ArgumentNullException>();
        }
        [Fact]
        public async Task AddPerson_NullPerson_ToBeArgumentNullException()
        {
            //Arrange
            PersonAddRequest personAddRequest = null;

            //Act
            //Assert.Throws<ArgumentNullException>(() =>
            //await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            //{
            //    await _personService.AddPerson(personAddRequest);
            //});
            Func<Task> action = async () => await _personService.AddPerson(personAddRequest);
            //action.Invoke();
            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        //When we supply null value as PersonName, it should throw ArgumentException
        [Fact]
        //public void AddPerson_PersonNameIsNull()
        public async Task AddPerson_PersonNameIsNull()
        {
            //Arrange
            //var personAddRequest = new PersonAddRequest() { PersonName = null };
            var personAddRequest = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.PersonName, null as string)
                .Create();

            //Act
            //await Assert.ThrowsAsync<ArgumentException>(async () =>
            //{
            //    await _personService.AddPerson(personAddRequest);
            //});

            Func<Task> action = async () => await _personService.AddPerson(personAddRequest);
            await action.Should().ThrowAsync<ArgumentException>();
        }
        [Fact]
        //public void AddPerson_PersonNameIsNull()
        public async Task AddPerson_PersonNameIsNull_ToBeArgumentException()
        {
            //Arrange
            //var personAddRequest = new PersonAddRequest() { PersonName = null };
            var personAddRequest = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.PersonName, null as string)
                .Create();

            var person = personAddRequest.ToPerson();

            //When PersonRepository.AddPerson is called, it has to reyurn the same "person" object
            _personsRepositoryMock.Setup(temp => temp.AddPerson(It.IsAny<Person>())).ReturnsAsync(person);

            //Act
            //await Assert.ThrowsAsync<ArgumentException>(async () =>
            //{
            //    await _personService.AddPerson(personAddRequest);
            //});

            Func<Task> action = async () => await _personService.AddPerson(personAddRequest);
            await action.Should().ThrowAsync<ArgumentException>();
        }
        //When we supply proper person details, it should insert the person into the persons list and it should return an object of PersonResponse, which includes with the newly generated person id
        [Fact]
        //public void AddPerson_ProperPersonDetails()
        public async Task AddPerson_ProperPersonDetails()
        {
            //Arrange
            //var personAddRequest = new PersonAddRequest()
            //{
            //    PersonName = "Person name...",
            //    Email = "person@example.com",
            //    Address = "sample address",
            //    CountryID = Guid.NewGuid(),
            //    Gender = GenderOptions.Male,
            //    DateOfBirth = DateTime.Parse("2000-01-01"),
            //    ReceiveNewsLetters = true,
            //};

            //var personAddRequest = _fixture.Create<PersonAddRequest>();
            var personAddRequest = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "someone@example.com").Create();

            //Act            
            var person_response_from_add = await _personService.AddPerson(personAddRequest);
            var persons_list = await _personService.GetAllPersons();

            //Assert
            //Assert.True(person_response_from_add.PersonID != Guid.Empty);
            person_response_from_add.PersonID.Should().NotBe(Guid.Empty);
            //Assert.Contains(person_response_from_add, persons_list);
            persons_list.Should().Contain(person_response_from_add);
        }

        [Fact]
        public async Task AddPerson_ProperPersonDetails_ToBeSuccessful()
        {
            //Arrange
            //var personAddRequest = new PersonAddRequest()
            //{
            //    PersonName = "Person name...",
            //    Email = "person@example.com",
            //    Address = "sample address",
            //    CountryID = Guid.NewGuid(),
            //    Gender = GenderOptions.Male,
            //    DateOfBirth = DateTime.Parse("2000-01-01"),
            //    ReceiveNewsLetters = true,
            //};

            //var personAddRequest = _fixture.Create<PersonAddRequest>();
            var personAddRequest = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "someone@example.com").Create();

            Person person = personAddRequest.ToPerson();
            var person_response_expected = person.ToPersonResponse();

            //if we supply any argument value to the AddPerson method it should return the same return value
            _personsRepositoryMock.Setup(temp => temp.AddPerson(It.IsAny<Person>())).ReturnsAsync(person);

            //Act            
            var person_response_from_add = await _personService.AddPerson(personAddRequest);
            person_response_expected.PersonID = person_response_from_add.PersonID;
            //var persons_list = await _personService.GetAllPersons();

            //Assert
            //Assert.True(person_response_from_add.PersonID != Guid.Empty);
            person_response_from_add.PersonID.Should().NotBe(Guid.Empty);
            //Assert.Contains(person_response_from_add, persons_list);
            //persons_list.Should().Contain(person_response_from_add);
            person_response_from_add.Should().Be(person_response_expected);
        }
        #endregion


        #region GetPersonByPersonID
        //If we supply null as PersonID, it should return null as PersonResponse
        [Fact]
        //public void GetPersonByPersonID_NullPersonID()
        public async Task GetPersonByPersonID_NullPersonID()
        {
            //Arrange
            Guid? personID = null;

            //Act
            var person_response_from_get = await _personService.GetPersonByPersonID(personID);

            //Assert
            //Assert.Null(person_response_from_get);
            person_response_from_get.Should().BeNull();
        }
        [Fact]
        //public void GetPersonByPersonID_NullPersonID()
        public async Task GetPersonByPersonID_NullPersonID_ToBeNull()
        {
            //Arrange
            Guid? personID = null;

            //Act
            var person_response_from_get = await _personService.GetPersonByPersonID(personID);

            //Assert
            //Assert.Null(person_response_from_get);
            person_response_from_get.Should().BeNull();
        }
        //If we supply a valid PersonID, it should return the valid personDetails as PersonReponse object
        [Fact]
        //public void GetPersonByPersonID_WithPersonID()
        public async Task GetPersonByPersonID_WithPersonID()
        {
            //Arrange
            //var country_request = new CountryAddRequest() { CountryName = "Canada" };
            var country_request = _fixture.Create<CountryAddRequest>();

            var country_response = await _countriesService.AddCountry(country_request);

            //Act
            //var person_request = new PersonAddRequest()
            //{
            //    PersonName = "person name...",
            //    Email = "email@Sample.com",
            //    Address = "address",
            //    CountryID = country_response.CountryID,
            //    DateOfBirth = DateTime.Parse("2000-01-01"),
            //    Gender = GenderOptions.Male,
            //    ReceiveNewsLetters = false
            //};
            var person_request = _fixture.Build<PersonAddRequest>()
                //.With(temp => temp.CountryID, country_response.CountryID)
                .With(temp => temp.Email, "person@example.com")
                .Create();
            var person_response_from_add = await _personService.AddPerson(person_request);

            var person_response_from_get = await _personService.GetPersonByPersonID(person_response_from_add.PersonID);

            //Assert
            //Assert.Equal(person_response_from_add, person_response_from_get);
            person_response_from_get.Should().Be(person_response_from_add);
        }
        [Fact]
        //public void GetPersonByPersonID_WithPersonID()
        public async Task GetPersonByPersonID_WithPersonID_ToBeSuccessful()
        {
            //Arrange            
            var person = _fixture.Build<Person>()
                //.With(temp => temp.CountryID, country_response.CountryID)
                .With(temp => temp.Email, "person@example.com")
                .With(temp => temp.Country, null as Country)
                .Create();
            var person_reponse_expected = person.ToPersonResponse();

            _personsRepositoryMock.Setup(temp => temp.GetPersonByPersonID(It.IsAny<Guid>())).ReturnsAsync(person);
            //Act
            var person_response_from_get = await _personService.GetPersonByPersonID(person.PersonID);

            //Assert
            //Assert.Equal(person_response_from_add, person_response_from_get);
            person_response_from_get.Should().Be(person_reponse_expected);
        }
        #endregion


        #region GetAllPersons
        // The GetAllPersons should return an empty list by default
        //[Fact]
        ////public void GetAllPersons_EmptyList()
        //public async Task GetAllPersons_EmptyList()
        //{
        //    //Act
        //    var persons_from_get = await _personService.GetAllPersons();

        //    //Assert
        //    //Assert.Empty(persons_from_get);
        //    persons_from_get.Should().BeEmpty();
        //}
        [Fact]
        //public void GetAllPersons_EmptyList()
        public async Task GetAllPersons_EmptyList()
        {
            //Arragne
            var persons = new List<Person>();
            _personsRepositoryMock.Setup(temp => temp.GetAllPersons()).ReturnsAsync(persons);

            //Act
            var persons_from_get = await _personService.GetAllPersons();

            //Assert
            //Assert.Empty(persons_from_get);
            persons_from_get.Should().BeEmpty();
        }
        // First, we will add few persons; and then when we call GetAllPersons(), it should rteurn the same persons that were added
        [Fact]
        //public void GetAllPersons_AddFewPersons()
        public async Task GetAllPersons_AddFewPersons()
        {
            //Arrange
            //var country_request_1 = new CountryAddRequest() { CountryName = "USA" };
            //var country_request_2 = new CountryAddRequest() { CountryName = "India" };
            var country_request_1 = _fixture.Create<CountryAddRequest>();
            var country_request_2 = _fixture.Create<CountryAddRequest>();

            var country_response_1 = await _countriesService.AddCountry(country_request_1);
            var country_response_2 = await _countriesService.AddCountry(country_request_2);

            //var person_request_1 = new PersonAddRequest() 
            //{ 
            //    PersonName = "Smith",
            //    Email = "smith@example.com",
            //    Gender = GenderOptions.Male,
            //    Address = "address of smith",
            //    CountryID = country_response_1.CountryID,
            //    DateOfBirth = DateTime.Parse("2002-05-06"),
            //    ReceiveNewsLetters = true,
            //};

            var person_request_1= _fixture.Build<PersonAddRequest>()
                .With(temp => temp.PersonName, "Smith")
                .With(temp => temp.Email, "smith@example.com")
                .With(temp => temp.CountryID, country_response_1.CountryID)
                .Create();
            var person_request_2 = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.PersonName, "Mary")
                .With(temp => temp.Email, "mary@example.com")
                .With(temp => temp.CountryID, country_response_2.CountryID)
                .Create();

            //var person_request_2 = new PersonAddRequest() 
            //{
            //    PersonName = "Mary",
            //    Email = "mary@example.com",
            //    Gender = GenderOptions.Female,
            //    Address = "address of mary",
            //    CountryID = country_response_2.CountryID,
            //    DateOfBirth = DateTime.Parse("2000-02-02"),
            //    ReceiveNewsLetters = false,
            //};

            //var person_request_3 = new PersonAddRequest() 
            //{
            //    PersonName = "Rahman",
            //    Email = "rahman@example.com",
            //    Gender = GenderOptions.Female,
            //    Address = "address of rahman",
            //    CountryID = country_response_2.CountryID,
            //    DateOfBirth = DateTime.Parse("1999-03-03"),
            //    ReceiveNewsLetters = true,
            //};

            var person_request_3 = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.PersonName, "Rahman")
                .With(temp => temp.Email, "rahman@example.com")
                .With(temp => temp.CountryID, country_response_2.CountryID)
                .Create();

            var person_requests = new List<PersonAddRequest>() { person_request_1, person_request_2, person_request_3 };
            var person_response_list_from_add = new List<PersonResponse>();
            foreach (var person_request in person_requests)
            {
                var person_response = await _personService.AddPerson(person_request);
                person_response_list_from_add.Add(person_response);
            }

            //Print person_response_list_from_add
            _testOutputHelper.WriteLine("Expected:");
            foreach(var person_response_from_add in person_response_list_from_add)
            {
                _testOutputHelper.WriteLine(person_response_from_add.ToString());
            }

            //Act
            var persons_list_from_get = await _personService.GetAllPersons();

            //Print persons_list_from_get
            _testOutputHelper.WriteLine("Actual:");
            foreach (var person_response_from_get in persons_list_from_get)
            {
                _testOutputHelper.WriteLine(person_response_from_get.ToString());
            }

            //Assert
            //foreach (var person_response_from_add in person_response_list_from_add)
            //{
            //    Assert.Contains(person_response_from_add, persons_list_from_get);
            //}
            persons_list_from_get.Should().BeEquivalentTo(person_response_list_from_add);
        }
        [Fact]
        //public void GetAllPersons_AddFewPersons()
        public async Task GetAllPersons_WithFewPersons_ToBeSuccessful()
        {
            //Arrange
            var persons = new List<Person>()
            {
                _fixture.Build<Person>().With(temp => temp.Email, "someone_1@example.com").With(temp => temp.Country, null as Country).Create(),
                _fixture.Build<Person>().With(temp => temp.Email, "someone_2@example.com").With(temp => temp.Country, null as Country).Create(),
                _fixture.Build<Person>().With(temp => temp.Email, "someone_3@example.com").With(temp => temp.Country, null as Country).Create(),
            };
            
            var person_response_list_expected = persons.Select(temp => temp.ToPersonResponse());
           
            //Print person_response_list_from_add
            _testOutputHelper.WriteLine("Expected:");
            foreach (var person_response_from_add in person_response_list_expected)
            {
                _testOutputHelper.WriteLine(person_response_from_add.ToString());
            }
            _personsRepositoryMock.Setup(temp => temp.GetAllPersons()).ReturnsAsync(persons);

            //Act
            var persons_list_from_get = await _personService.GetAllPersons();

            //Print persons_list_from_get
            _testOutputHelper.WriteLine("Actual:");
            foreach (var person_response_from_get in persons_list_from_get)
            {
                _testOutputHelper.WriteLine(person_response_from_get.ToString());
            }
            
            //Assert
            //foreach (var person_response_from_add in person_response_list_from_add)
            //{
            //    Assert.Contains(person_response_from_add, persons_list_from_get);
            //}
            persons_list_from_get.Should().BeEquivalentTo(person_response_list_expected);
        }
        #endregion


        #region GetFilteredPersons
        // If the search text is empty and search by is "PersonName", it should return all persons
        [Fact]
        //public void GetFilteredPersons_EmptySearchText()
        public async Task GetFilteredPersons_EmptySearchText()
        {
            //Arrange
            //var country_request_1 = new CountryAddRequest() { CountryName = "USA" };
            //var country_request_2 = new CountryAddRequest() { CountryName = "India" };

            var country_request_1 = _fixture.Create<CountryAddRequest>();
            var country_request_2 = _fixture.Create<CountryAddRequest>();

            var country_response_1 = await _countriesService.AddCountry(country_request_1);
            var country_response_2 = await _countriesService.AddCountry(country_request_2);

            //var person_request_1 = new PersonAddRequest()
            //{
            //    PersonName = "Smith",
            //    Email = "smith@example.com",
            //    Gender = GenderOptions.Male,
            //    Address = "address of smith",
            //    CountryID = country_response_1.CountryID,
            //    DateOfBirth = DateTime.Parse("2002-05-06"),
            //    ReceiveNewsLetters = true,
            //};

            //var person_request_2 = new PersonAddRequest()
            //{
            //    PersonName = "Mary",
            //    Email = "mary@example.com",
            //    Gender = GenderOptions.Female,
            //    Address = "address of mary",
            //    CountryID = country_response_2.CountryID,
            //    DateOfBirth = DateTime.Parse("2000-02-02"),
            //    ReceiveNewsLetters = false,
            //};

            //var person_request_3 = new PersonAddRequest()
            //{
            //    PersonName = "Rahman",
            //    Email = "rahman@example.com",
            //    Gender = GenderOptions.Female,
            //    Address = "address of rahman",
            //    CountryID = country_response_2.CountryID,
            //    DateOfBirth = DateTime.Parse("1999-03-03"),
            //    ReceiveNewsLetters = true,
            //};

            var person_request_1 = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.PersonName, "Smith")
                .With(temp => temp.Email, "smith@example.com")
                .With(temp => temp.CountryID, country_response_1.CountryID)
                .Create();

            var person_request_2 = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.PersonName, "Mary")
                .With(temp => temp.Email, "mary@example.com")
                .With(temp => temp.CountryID, country_response_2.CountryID)
                .Create();

            var person_request_3 = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.PersonName, "Rahman")
                .With(temp => temp.Email, "rahman@example.com")
                .With(temp => temp.CountryID, country_response_2.CountryID)
                .Create();

            var person_requests = new List<PersonAddRequest>() { person_request_1, person_request_2, person_request_3 };
            var person_response_list_from_add = new List<PersonResponse>();
            foreach (var person_request in person_requests)
            {
                var person_response = await _personService.AddPerson(person_request);
                person_response_list_from_add.Add(person_response);
            }

            //Print person_response_list_from_add
            _testOutputHelper.WriteLine("Expected:");
            foreach (var person_response_from_add in person_response_list_from_add)
            {
                _testOutputHelper.WriteLine(person_response_from_add.ToString());
            }

            //Act
            var persons_list_from_search = await _personService.GetFilteredPersons(nameof(Person.PersonName), string.Empty);

            //Print persons_list_from_search
            _testOutputHelper.WriteLine("Actual:");
            foreach (var person_response_from_get in persons_list_from_search)
            {
                _testOutputHelper.WriteLine(person_response_from_get.ToString());
            }

            //Assert
            //foreach (var person_response_from_add in person_response_list_from_add)
            //{
            //    Assert.Contains(person_response_from_add, persons_list_from_search);
            //}
            persons_list_from_search.Should().BeEquivalentTo(person_response_list_from_add);
        }
        [Fact]
        public async Task GetFilteredPersons_EmptySearchText_ToBeSuccessful()
        {
            //Arrange           
            var persons = new List<Person>()
            {
                _fixture.Build<Person>().With(temp => temp.Email, "someone_1@example.com").With(temp => temp.Country, null as Country).Create(),
                _fixture.Build<Person>().With(temp => temp.Email, "someone_2@example.com").With(temp => temp.Country, null as Country).Create(),
                _fixture.Build<Person>().With(temp => temp.Email, "someone_3@example.com").With(temp => temp.Country, null as Country).Create(),
            };

            var person_response_list_expected = persons.Select(temp => temp.ToPersonResponse());

            //Print person_response_list_from_add
            _testOutputHelper.WriteLine("Expected:");
            foreach (var person_response_from_add in person_response_list_expected)
            {
                _testOutputHelper.WriteLine(person_response_from_add.ToString());
            }
            _personsRepositoryMock.Setup(temp => temp.GetFilteredPersons(It.IsAny<Expression<Func<Person, bool>>>())).ReturnsAsync(persons);
            //Act
            var persons_list_from_search = await _personService.GetFilteredPersons(nameof(Person.PersonName), string.Empty);

            //Print persons_list_from_search
            _testOutputHelper.WriteLine("Actual:");
            foreach (var person_response_from_get in persons_list_from_search)
            {
                _testOutputHelper.WriteLine(person_response_from_get.ToString());
            }

            //Assert
            //foreach (var person_response_from_add in person_response_list_from_add)
            //{
            //    Assert.Contains(person_response_from_add, persons_list_from_search);
            //}
            persons_list_from_search.Should().BeEquivalentTo(person_response_list_expected);
        }
        
        // First we will add a few persons; and then we will search based on PersonName with some search string. It should return the matching persons
        [Fact]
        //public void GetFilteredPersons_SearchByPersonName()
        public async Task GetFilteredPersons_SearchByPersonName()
        {
            //Arrange
            //var country_request_1 = new CountryAddRequest() { CountryName = "USA" };
            //var country_request_2 = new CountryAddRequest() { CountryName = "India" };

            var country_request_1 = _fixture.Create<CountryAddRequest>();
            var country_request_2 = _fixture.Create<CountryAddRequest>();

            var country_response_1 = await _countriesService.AddCountry(country_request_1);
            var country_response_2 = await _countriesService.AddCountry(country_request_2);

            //var person_request_1 = new PersonAddRequest()
            //{
            //    PersonName = "Smith",
            //    Email = "smith@example.com",
            //    Gender = GenderOptions.Male,
            //    Address = "address of smith",
            //    CountryID = country_response_1.CountryID,
            //    DateOfBirth = DateTime.Parse("2002-05-06"),
            //    ReceiveNewsLetters = true,
            //};

            //var person_request_2 = new PersonAddRequest()
            //{
            //    PersonName = "Mary",
            //    Email = "mary@example.com",
            //    Gender = GenderOptions.Female,
            //    Address = "address of mary",
            //    CountryID = country_response_2.CountryID,
            //    DateOfBirth = DateTime.Parse("2000-02-02"),
            //    ReceiveNewsLetters = false,
            //};

            //var person_request_3 = new PersonAddRequest()
            //{
            //    PersonName = "Rahman",
            //    Email = "rahman@example.com",
            //    Gender = GenderOptions.Female,
            //    Address = "address of rahman",
            //    CountryID = country_response_2.CountryID,
            //    DateOfBirth = DateTime.Parse("1999-03-03"),
            //    ReceiveNewsLetters = true,
            //};

            var person_request_1 = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.PersonName, "Smith")
                .With(temp => temp.Email, "smith@example.com")
                .With(temp => temp.CountryID, country_response_1.CountryID)
                .Create();

            var person_request_2 = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.PersonName, "Mary")
                .With(temp => temp.Email, "mary@example.com")
                .With(temp => temp.CountryID, country_response_2.CountryID)
                .Create();

            var person_request_3 = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.PersonName, "Rahman")
                .With(temp => temp.Email, "rahman@example.com")
                .With(temp => temp.CountryID, country_response_2.CountryID)
                .Create();

            var person_requests = new List<PersonAddRequest>() { person_request_1, person_request_2, person_request_3 };
            var person_response_list_from_add = new List<PersonResponse>();
            foreach (var person_request in person_requests)
            {
                var person_response = await _personService.AddPerson(person_request);
                person_response_list_from_add.Add(person_response);
            }

            //Print person_response_list_from_add
            _testOutputHelper.WriteLine("All Persons:");
            foreach (var person_response_from_add in person_response_list_from_add)
            {
                _testOutputHelper.WriteLine(person_response_from_add.ToString());
            }

            //Act
            var persons_list_from_search = await _personService.GetFilteredPersons(nameof(Person.PersonName), "ma");

            //Print persons_list_from_search
            _testOutputHelper.WriteLine($"Matched Persons with SearchBy {nameof(Person.PersonName)} and searchString ma:");
            foreach (var person_response_from_get in persons_list_from_search)
            {
                _testOutputHelper.WriteLine(person_response_from_get.ToString());
            }

            //Assert
            //foreach (var person_response_from_add in person_response_list_from_add)
            //{
            //    if (person_response_from_add.PersonName != null)
            //    {
            //        if (person_response_from_add.PersonName.Contains("ma", StringComparison.OrdinalIgnoreCase))
            //        {
            //            Assert.Contains(person_response_from_add, persons_list_from_search);
            //        }
            //    }
            //}
            persons_list_from_search.Should().OnlyContain(temp => temp.PersonName.Contains("ma", StringComparison.OrdinalIgnoreCase));
        }
        [Fact]
        public async Task GetFilteredPersons_SearchByPersonName_ToBeSuccessful()
        {
            //Arrange           
            var persons = new List<Person>()
            {
                _fixture.Build<Person>().With(temp => temp.Email, "someone_1@example.com").With(temp => temp.Country, null as Country).Create(),
                _fixture.Build<Person>().With(temp => temp.Email, "someone_2@example.com").With(temp => temp.Country, null as Country).Create(),
                _fixture.Build<Person>().With(temp => temp.Email, "someone_3@example.com").With(temp => temp.Country, null as Country).Create(),
            };

            var person_response_list_expected = persons.Select(temp => temp.ToPersonResponse());

            //Print person_response_list_from_add
            _testOutputHelper.WriteLine("Expected:");
            foreach (var person_response_from_add in person_response_list_expected)
            {
                _testOutputHelper.WriteLine(person_response_from_add.ToString());
            }
            _personsRepositoryMock.Setup(temp => temp.GetFilteredPersons(It.IsAny<Expression<Func<Person, bool>>>())).ReturnsAsync(persons);
            //Act
            var persons_list_from_search = await _personService.GetFilteredPersons(nameof(Person.PersonName), "sa");

            //Print persons_list_from_search
            _testOutputHelper.WriteLine("Actual:");
            foreach (var person_response_from_get in persons_list_from_search)
            {
                _testOutputHelper.WriteLine(person_response_from_get.ToString());
            }

            //Assert
            //foreach (var person_response_from_add in person_response_list_from_add)
            //{
            //    Assert.Contains(person_response_from_add, persons_list_from_search);
            //}
            persons_list_from_search.Should().BeEquivalentTo(person_response_list_expected);
        }
        #endregion


        #region GetSortedPersons

        /// <summary>
        /// Verifies that persons are sorted by name in descending order.
        /// </summary>
        /// <returns>A task representing the asynchronous test operation.</returns>
        // When we sort based on PersonName in DESC, it should return persons list in descending on PersonName
        [Fact]

        // public void GetSortedPersons_()
        public async Task GetSortedPersons()
        {
            // Arrange
            /*var country_request_1 = new CountryAddRequest() { CountryName = "USA" };
            /var country_request_2 = new CountryAddRequest() { CountryName = "India" };*/

            var country_request_1 = this._fixture.Create<CountryAddRequest>();
            var country_request_2 = this._fixture.Create<CountryAddRequest>();

            var country_response_1 = await this._countriesService.AddCountry(country_request_1);
            var country_response_2 = await this._countriesService.AddCountry(country_request_2);

            /*var person_request_1 = new PersonAddRequest()
            {
                PersonName = "Smith",
                Email = "smith@example.com",
                Gender = GenderOptions.Male,
                Address = "address of smith",
                CountryID = country_response_1.CountryID,
                DateOfBirth = DateTime.Parse("2002-05-06"),
                ReceiveNewsLetters = true,
            };*/

            /*var person_request_2 = new PersonAddRequest()
            {
                PersonName = "Mary",
                Email = "mary@example.com",
                Gender = GenderOptions.Female,
                Address = "address of mary",
                CountryID = country_response_2.CountryID,
                DateOfBirth = DateTime.Parse("2000-02-02"),
                ReceiveNewsLetters = false,
            };*/

            /*var person_request_3 = new PersonAddRequest()
            {
                PersonName = "Rahman",
                Email = "rahman@example.com",
                Gender = GenderOptions.Female,
                Address = "address of rahman",
                CountryID = country_response_2.CountryID,
                DateOfBirth = DateTime.Parse("1999-03-03"),
                ReceiveNewsLetters = true,
            };*/

            var person_request_1 = this._fixture.Build<PersonAddRequest>()
                .With(temp => temp.PersonName, "Smith")
                .With(temp => temp.Email, "smith@example.com")
                .With(temp => temp.CountryID, country_response_1.CountryID)
                .Create();

            var person_request_2 = this._fixture.Build<PersonAddRequest>()
                .With(temp => temp.PersonName, "Mary")
                .With(temp => temp.Email, "mary@example.com")
                .With(temp => temp.CountryID, country_response_2.CountryID)
                .Create();

            var person_request_3 = this._fixture.Build<PersonAddRequest>()
                .With(temp => temp.PersonName, "Rahman")
                .With(temp => temp.Email, "rahman@example.com")
                .With(temp => temp.CountryID, country_response_2.CountryID)
                .Create();

            var person_requests = new List<PersonAddRequest>() { person_request_1, person_request_2, person_request_3 };
            var person_response_list_from_add = new List<PersonResponse>();
            foreach (var person_request in person_requests)
            {
                var person_response = await this._personService.AddPerson(person_request);
                person_response_list_from_add.Add(person_response);
            }

            // Print person_response_list_from_add
            this._testOutputHelper.WriteLine("All Persons:");
            foreach (var person_response_from_add in person_response_list_from_add)
            {
                this._testOutputHelper.WriteLine(person_response_from_add.ToString());
            }

            var allPersons = await this._personService.GetAllPersons();

            // Act
            var persons_list_from_sort = await this._personService.GetSortedPersons(allPersons, nameof(Person.PersonName), SortOrderOptions.DESC);

            // Print persons_list_from_sort
            this._testOutputHelper.WriteLine($"Sorted Persons with SortBy {nameof(Person.PersonName)} in DESC order:");
            foreach (var person_response_from_get in persons_list_from_sort)
            {
                this._testOutputHelper.WriteLine(person_response_from_get.ToString());
            }

            // person_response_list_from_add = [..person_response_list_from_add.OrderByDescending(temp => temp.PersonName)];

            // Assert
            /*for (var i = 0; i < person_response_list_from_add.Count; i++)
            {
                Assert.Equal(person_response_list_from_add[i], persons_list_from_sort[i]);
            }
            persons_list_from_sort.Should().BeEquivalentTo(person_response_list_from_add);*/

            persons_list_from_sort.Should().BeInDescendingOrder(temp => temp.PersonName);
        }

        /// <summary>
        /// Verifies that persons are sorted in descending order by person name.
        /// </summary>
        /// <returns>A task representing the asynchronous test operation.</returns>
        [Fact]
        public async Task GetSortedPersons_ToBeSuccessful()
        {
            // Arrange
            var persons = new List<Person>()
            {
                this._fixture.Build<Person>().With(temp => temp.Email, "someone_1@example.com").With(temp => temp.Country, null as Country).Create(),
                this._fixture.Build<Person>().With(temp => temp.Email, "someone_2@example.com").With(temp => temp.Country, null as Country).Create(),
                this._fixture.Build<Person>().With(temp => temp.Email, "someone_3@example.com").With(temp => temp.Country, null as Country).Create(),
            };

            var person_response_list_expected = persons.Select(temp => temp.ToPersonResponse());
            this._personsRepositoryMock.Setup(temp => temp.GetAllPersons()).ReturnsAsync(persons);

            // Print person_response_list_expected
            this._testOutputHelper.WriteLine("All Persons:");
            foreach (var person_response_from_add in person_response_list_expected)
            {
                this._testOutputHelper.WriteLine(person_response_from_add.ToString());
            }

            var allPersons = await this._personService.GetAllPersons();

            // Act
            var persons_list_from_sort = await this._personService.GetSortedPersons(allPersons, nameof(Person.PersonName), SortOrderOptions.DESC);

            // Print persons_list_from_search
            this._testOutputHelper.WriteLine($"Sorted Persons with SortBy {nameof(Person.PersonName)} in DESC order:");
            foreach (var person_response_from_get in persons_list_from_sort)
            {
                this._testOutputHelper.WriteLine(person_response_from_get.ToString());
            }

            // Assert
            persons_list_from_sort.Should().BeInDescendingOrder(temp => temp.PersonName);
        }
        #endregion


        #region UpdatePerson
        //When we supply null as PersonUpdateRequest, it should throw ArgumentNullException
        [Fact]
        //public void UpdatePerson_NullPerson()
        public async Task UpdatePerson_NullPerson()
        {
            //Arrange
            PersonUpdateRequest? person_update_request = null;

            //Assert
            //await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            //{
            //    //Act
            //    await _personService.UpdatePerson(person_update_request);
            //});
            Func<Task> action = async () => await _personService.UpdatePerson(person_update_request);
            await action.Should().ThrowAsync<ArgumentNullException>();
        }
        [Fact]
        public async Task UpdatePerson_NullPerson_ToBEArgumentNullException()
        {
            //Arrange
            PersonUpdateRequest? person_update_request = null;

            //Assert
            //await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            //{
            //    //Act
            //    await _personService.UpdatePerson(person_update_request);
            //});
            Func<Task> action = async () => await _personService.UpdatePerson(person_update_request);
            await action.Should().ThrowAsync<ArgumentNullException>();
        }
        //When we supply invalid Person ID, it should throw ArgumentException
        [Fact]
        //public void UpdatePerson_InValiPersonID()
        public async Task UpdatePerson_InValiPersonID()
        {
            //Arrange
            //var person_update_request = new PersonUpdateRequest() { PersonID = Guid.NewGuid() };
            var person_update_request = _fixture.Build<PersonUpdateRequest>()
                //.With(temp => temp.PersonID, Guid.NewGuid())
                .Create();

            //Assert
            //await Assert.ThrowsAsync<ArgumentException>(async () =>
            //{
            //    //Act
            //    await _personService.UpdatePerson(person_update_request);
            //});

            Func<Task> action = async () => await _personService.UpdatePerson(person_update_request);
            await action.Should().ThrowAsync<ArgumentException>();
        }

        //When Person Name is null, it should throw ArgumentException
        [Fact]
        //public void UpdatePerson_PersonNameIsNull()
        public async Task UpdatePerson_PersonNameIsNull()
        {
            //Arrange
            //var country_add_request = new CountryAddRequest() { CountryName = "UK" };
            var country_add_request = _fixture.Create<CountryAddRequest>();
            var country_response_from_add = await _countriesService.AddCountry(country_add_request);

            //var person_add_request = new PersonAddRequest()
            //{
            //    PersonName = "John",
            //    Email ="john@example.com",
            //    CountryID = country_response_from_add.CountryID,
            //    Address = "address...",
            //};

            var person_add_request = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.PersonName, "John")
                .With(temp => temp.Email, "john@example.com")
                .With(temp => temp.CountryID, country_response_from_add.CountryID)
                .Create();

            var person_response_from_add = await _personService.AddPerson(person_add_request);

            var person_update_request = person_response_from_add.ToPersonUpdateRequest();
            person_update_request.PersonName = null;


            //Assert
            //await Assert.ThrowsAsync<ArgumentException>(async () =>
            //{
            //    //Act
            //    await _personService.UpdatePerson(person_update_request);
            //});

            var action = async () => await _personService.UpdatePerson(person_update_request);
            await action.Should().ThrowAsync<ArgumentException>();
        }
        [Fact]
        //public void UpdatePerson_PersonNameIsNull()
        public async Task UpdatePerson_PersonNameIsNull_ToBeArgumentException()
        {
            //Arrange
            var person = _fixture.Build<Person>()
                .With(temp => temp.PersonName, null as string)
                .With(temp => temp.Email, "john@example.com")
                .With(temp => temp.Country, null as Country)
                .With(temp => temp.Gender, "Male")
                .Create();

            var person_response_from_add = person.ToPersonResponse();

            var person_update_request = person_response_from_add.ToPersonUpdateRequest();            

            //Assert
            //await Assert.ThrowsAsync<ArgumentException>(async () =>
            //{
            //    //Act
            //    await _personService.UpdatePerson(person_update_request);
            //});

            var action = async () => await _personService.UpdatePerson(person_update_request);
            await action.Should().ThrowAsync<ArgumentException>();
        }
        //First, add a new person and try to update the person name and email
        [Fact]
        //public void UpdatePerson_PersonFullDetailsUpdation()
        public async Task UpdatePerson_PersonFullDetailsUpdation()
        {
            //Arrange
            //var country_add_request = new CountryAddRequest() { CountryName = "UK" };
            var country_add_request = _fixture.Create<CountryAddRequest>();
            var country_response_from_add = await _countriesService.AddCountry(country_add_request);

            //var person_add_request = new PersonAddRequest()
            //{
            //    PersonName = "John",
            //    CountryID = country_response_from_add.CountryID,
            //    Address = "Abc road",
            //    DateOfBirth = DateTime.Parse("2000-01-01"),
            //    Email = "abc@example.com",
            //    Gender = GenderOptions.Male,
            //    ReceiveNewsLetters = true,
            //};

            var person_add_request = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.PersonName, "John")
                .With(temp => temp.Email, "john@example.com")
                .With(temp => temp.CountryID, country_response_from_add.CountryID)
                .With(temp => temp.Address, "Abc road")
                .Create();

            var person_response_from_add = await _personService.AddPerson(person_add_request);

            var person_update_request = person_response_from_add.ToPersonUpdateRequest();
            person_update_request.PersonName = "William";
            person_update_request.Email = "william@example.com";


            //Act
            var person_response_from_update = await _personService.UpdatePerson(person_update_request);
            var person_response_from_get = await _personService.GetPersonByPersonID(person_response_from_update.PersonID);

            //Assert
            //Assert.Equal(person_response_from_get, person_response_from_update);
            person_response_from_update.Should().Be(person_response_from_get);
        }
        [Fact]
        //public void UpdatePerson_PersonFullDetailsUpdation()
        public async Task UpdatePerson_PersonFullDetailsUpdation_ToBeSuccessful()
        {
            //Arrange
            var person = _fixture.Build<Person>()
                .With(temp => temp.Email, "someone@example.com")
                .With(temp => temp.Country, null as Country)
                .With(temp => temp.Gender, "Male")
                .Create();

            var person_response_expected = person.ToPersonResponse();

            var person_update_request = person_response_expected.ToPersonUpdateRequest();
            _personsRepositoryMock.Setup(temp => temp.UpdatePerson(It.IsAny<Person>())).ReturnsAsync(person);
            _personsRepositoryMock.Setup(temp => temp.GetPersonByPersonID(It.IsAny<Guid>())).ReturnsAsync(person);

            //Act
            var person_response_from_update = await _personService.UpdatePerson(person_update_request);

            //Assert
            //Assert.Equal(person_response_from_get, person_response_from_update);
            person_response_from_update.Should().Be(person_response_expected);
        }
        #endregion


        #region DeletePerson
        //If we supply a valid PersonID, it should return true
        [Fact]
        //public void DeletePerson_ValidPersonID()
        public async Task DeletePerson_ValidPersonID()
        {
            //Arrange
            //var country_add_request = new CountryAddRequest() { CountryName = "USA" };
            var country_add_request = _fixture.Create<CountryAddRequest>();
            var country_response_from_add = await _countriesService.AddCountry(country_add_request);

            //var person_add_request = new PersonAddRequest()
            //{
            //    PersonName = "Jones",
            //    Email = "jones@example.com",
            //    Address = "address",
            //    CountryID = country_response_from_add.CountryID,
            //    DateOfBirth = Convert.ToDateTime("2010-01-01"),
            //    Gender = GenderOptions.Male,
            //    ReceiveNewsLetters = true,
            //};

            var person_add_request = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.PersonName, "Jones")
                .With(temp => temp.Email, "jones@example.com")
                .With(temp => temp.CountryID, country_response_from_add.CountryID)
                .With(temp => temp.Address, "address")
                .Create();
            var person_response_from_add = await _personService.AddPerson(person_add_request);

            //Act
            bool isDeleted = await _personService.DeletePerson(person_response_from_add.PersonID);

            //Assert
            //Assert.True(isDeleted);
            isDeleted.Should().BeTrue();
        }
        [Fact]
        //public void DeletePerson_ValidPersonID()
        public async Task DeletePerson_ValidPersonID_ToBeSuccessful()
        {
            //Arrange
            var person = _fixture.Build<Person>()
                .With(temp => temp.PersonName, "Jones")
                .With(temp => temp.Email, "jones@example.com")
                .With(temp => temp.Country, null as Country)
                .With(temp => temp.Gender, "Male")
                .With(temp => temp.Address, "address")
                .Create();
            
            _personsRepositoryMock.Setup(temp => temp.DeletePersonByPersonID(It.IsAny<Guid>())).ReturnsAsync(true);
            _personsRepositoryMock.Setup(temp => temp.GetPersonByPersonID(It.IsAny<Guid>())).ReturnsAsync(person);

            //Act
            bool isDeleted = await _personService.DeletePerson(person.PersonID);

            //Assert
            //Assert.True(isDeleted);
            isDeleted.Should().BeTrue();
        }
        //If we supply an invalid PersonID, it should return false
        [Fact]
        //public void DeletePerson_InvalidPersonID()
        public async Task DeletePerson_InvalidPersonID()
        {
            //Arrange            

            //Act
            bool isDeleted = await _personService.DeletePerson(Guid.NewGuid());

            //Assert
            //Assert.False(isDeleted);
            isDeleted.Should().BeFalse();
        }
        #endregion
    }
}
