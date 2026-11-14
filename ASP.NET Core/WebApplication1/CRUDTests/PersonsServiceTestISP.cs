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
    public class PersonsServiceTestISP
    {
        /// <summary>
        /// Service for retrieving people.
        /// </summary>
        private readonly IPersonsGetterService _personsGetterService;

        /// <summary>
        /// Provides access to the service used to add persons.
        /// </summary>
        private readonly IPersonsAdderService _personsAdderService;

        /// <summary>
        /// Provides access to the service that updates person records.
        /// </summary>
        private readonly IPersonsUpdaterService _personsUpdaterService;

        /// <summary>
        /// Provides the service for deleting persons.
        /// </summary>
        private readonly IPersonsDeleterService _personsDeleterService;

        /// <summary>
        /// Provides the service used to sort persons.
        /// </summary>
        private readonly IPersonsSorterService _personsSorterService;

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
        /// Initializes a new instance of the <see cref="PersonsServiceTestISP"/> class.
        /// </summary>
        /// <param name="testOutputHelper">The test output helper.</param>
        public PersonsServiceTestISP(ITestOutputHelper testOutputHelper)
        {
            this._testOutputHelper = testOutputHelper;

            var countriesInitialData = new List<Country>() { };
            var personsInitialData = new List<Person>() { };
            var dbContextMock = new DbContextMock<ApplicationDbContext>(new DbContextOptionsBuilder<ApplicationDbContext>().Options);
            var dbContext = dbContextMock.Object;
            dbContextMock.CreateDbSetMock(temp => temp.Countries, countriesInitialData);
            dbContextMock.CreateDbSetMock(temp => temp.Persons, personsInitialData);

            var diagnosticContextMock = new Mock<IDiagnosticContext>();
            var loggerMock = new Mock<Microsoft.Extensions.Logging.ILogger<PersonsService>>();
            this._fixture = new Fixture();
            this._personsRepositoryMock = new Mock<IPersonsRepository>();
            this._personsRepository = this._personsRepositoryMock.Object;
            this._personsGetterService = new PersonsGetterService(this._personsRepository, loggerMock.Object, diagnosticContextMock.Object);
            this._personsAdderService = new PersonsAdderService(this._personsRepository);
            this._personsUpdaterService = new PersonsUpdaterService(this._personsRepository);
            this._personsDeleterService = new PersonsDeleterService(this._personsRepository);
            this._personsSorterService = new PersonsSorterService(loggerMock.Object);
        }

        #region AddPerson
        [Fact]
        public async Task AddPerson_NullPerson_ToBeArgumentNullException()
        {
            // Arrange
            PersonAddRequest personAddRequest = null;

            // Act
            Func<Task> action = async () => await this._personsAdderService.AddPerson(personAddRequest);
            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task AddPerson_PersonNameIsNull_ToBeArgumentException()
        {
            // Arrange
            var personAddRequest = this._fixture.Build<PersonAddRequest>()
                .With(temp => temp.PersonName, null as string)
                .Create();

            var person = personAddRequest.ToPerson();

            // When PersonRepository.AddPerson is called, it has to reyurn the same "person" object
            this._personsRepositoryMock.Setup(temp => temp.AddPerson(It.IsAny<Person>())).ReturnsAsync(person);

            // Act
            Func<Task> action = async () => await this._personsAdderService.AddPerson(personAddRequest);
            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task AddPerson_ProperPersonDetails_ToBeSuccessful()
        {
            // Arrange
            var personAddRequest = this._fixture.Build<PersonAddRequest>().With(temp => temp.Email, "someone@example.com").Create();

            Person person = personAddRequest.ToPerson();
            var person_response_expected = person.ToPersonResponse();

            // if we supply any argument value to the AddPerson method it should return the same return value
            this._personsRepositoryMock.Setup(temp => temp.AddPerson(It.IsAny<Person>())).ReturnsAsync(person);

            // Act
            var person_response_from_add = await this._personsAdderService.AddPerson(personAddRequest);
            person_response_expected.PersonID = person_response_from_add.PersonID;

            // Assert
            person_response_from_add.PersonID.Should().NotBe(Guid.Empty);
            person_response_from_add.Should().Be(person_response_expected);
        }
        #endregion


        #region GetPersonByPersonID
        [Fact]
        public async Task GetPersonByPersonID_NullPersonID_ToBeNull()
        {
            // Arrange
            Guid? personID = null;

            // Act
            var person_response_from_get = await this._personsGetterService.GetPersonByPersonID(personID);

            // Assert
            person_response_from_get.Should().BeNull();
        }

        [Fact]
        public async Task GetPersonByPersonID_WithPersonID_ToBeSuccessful()
        {
            // Arrange
            var person = this._fixture.Build<Person>()
                .With(temp => temp.Email, "person@example.com")
                .With(temp => temp.Country, null as Country)
                .Create();
            var person_reponse_expected = person.ToPersonResponse();

            this._personsRepositoryMock.Setup(temp => temp.GetPersonByPersonID(It.IsAny<Guid>())).ReturnsAsync(person);

            // Act
            var person_response_from_get = await this._personsGetterService.GetPersonByPersonID(person.PersonID);

            // Assert
            person_response_from_get.Should().Be(person_reponse_expected);
        }
        #endregion


        #region GetAllPersons
        [Fact]
        public async Task GetAllPersons_EmptyList()
        {
            // Arrange
            var persons = new List<Person>();
            this._personsRepositoryMock.Setup(temp => temp.GetAllPersons()).ReturnsAsync(persons);

            // Act
            var persons_from_get = await this._personsGetterService.GetAllPersons();

            // Assert
            persons_from_get.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllPersons_WithFewPersons_ToBeSuccessful()
        {
            // Arrange
            var persons = new List<Person>()
            {
                this._fixture.Build<Person>().With(temp => temp.Email, "someone_1@example.com").With(temp => temp.Country, null as Country).Create(),
                this._fixture.Build<Person>().With(temp => temp.Email, "someone_2@example.com").With(temp => temp.Country, null as Country).Create(),
                this._fixture.Build<Person>().With(temp => temp.Email, "someone_3@example.com").With(temp => temp.Country, null as Country).Create(),
            };

            var person_response_list_expected = persons.Select(temp => temp.ToPersonResponse());

            // Print person_response_list_from_add
            this._testOutputHelper.WriteLine("Expected:");
            foreach (var person_response_from_add in person_response_list_expected)
            {
                this._testOutputHelper.WriteLine(person_response_from_add.ToString());
            }

            this._personsRepositoryMock.Setup(temp => temp.GetAllPersons()).ReturnsAsync(persons);

            // Act
            var persons_list_from_get = await this._personsGetterService.GetAllPersons();

            // Print persons_list_from_get
            this._testOutputHelper.WriteLine("Actual:");
            foreach (var person_response_from_get in persons_list_from_get)
            {
                this._testOutputHelper.WriteLine(person_response_from_get.ToString());
            }

            // Assert
            persons_list_from_get.Should().BeEquivalentTo(person_response_list_expected);
        }
        #endregion


        #region GetFilteredPersons
        [Fact]
        public async Task GetFilteredPersons_EmptySearchText_ToBeSuccessful()
        {
            // Arrange
            var persons = new List<Person>()
            {
                this._fixture.Build<Person>().With(temp => temp.Email, "someone_1@example.com").With(temp => temp.Country, null as Country).Create(),
                this._fixture.Build<Person>().With(temp => temp.Email, "someone_2@example.com").With(temp => temp.Country, null as Country).Create(),
                this._fixture.Build<Person>().With(temp => temp.Email, "someone_3@example.com").With(temp => temp.Country, null as Country).Create(),
            };

            var person_response_list_expected = persons.Select(temp => temp.ToPersonResponse());

            // Print person_response_list_from_add
            this._testOutputHelper.WriteLine("Expected:");
            foreach (var person_response_from_add in person_response_list_expected)
            {
                this._testOutputHelper.WriteLine(person_response_from_add.ToString());
            }

            this._personsRepositoryMock.Setup(temp => temp.GetFilteredPersons(It.IsAny<Expression<Func<Person, bool>>>())).ReturnsAsync(persons);
            // Act
            var persons_list_from_search = await this._personsGetterService.GetFilteredPersons(nameof(Person.PersonName), string.Empty);

            // Print persons_list_from_search
            this._testOutputHelper.WriteLine("Actual:");
            foreach (var person_response_from_get in persons_list_from_search)
            {
                this._testOutputHelper.WriteLine(person_response_from_get.ToString());
            }

            // Assert
            persons_list_from_search.Should().BeEquivalentTo(person_response_list_expected);
        }

        [Fact]
        public async Task GetFilteredPersons_SearchByPersonName_ToBeSuccessful()
        {
            // Arrange
            var persons = new List<Person>()
            {
                this._fixture.Build<Person>().With(temp => temp.Email, "someone_1@example.com").With(temp => temp.Country, null as Country).Create(),
                this._fixture.Build<Person>().With(temp => temp.Email, "someone_2@example.com").With(temp => temp.Country, null as Country).Create(),
                this._fixture.Build<Person>().With(temp => temp.Email, "someone_3@example.com").With(temp => temp.Country, null as Country).Create(),
            };

            var person_response_list_expected = persons.Select(temp => temp.ToPersonResponse());

            // Print person_response_list_from_add
            this._testOutputHelper.WriteLine("Expected:");
            foreach (var person_response_from_add in person_response_list_expected)
            {
                this._testOutputHelper.WriteLine(person_response_from_add.ToString());
            }

            this._personsRepositoryMock.Setup(temp => temp.GetFilteredPersons(It.IsAny<Expression<Func<Person, bool>>>())).ReturnsAsync(persons);

            // Act
            var persons_list_from_search = await this._personsGetterService.GetFilteredPersons(nameof(Person.PersonName), "sa");

            // Print persons_list_from_search
            this._testOutputHelper.WriteLine("Actual:");
            foreach (var person_response_from_get in persons_list_from_search)
            {
                this._testOutputHelper.WriteLine(person_response_from_get.ToString());
            }

            // Assert
            persons_list_from_search.Should().BeEquivalentTo(person_response_list_expected);
        }
        #endregion


        #region GetSortedPersons
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

            var allPersons = await this._personsGetterService.GetAllPersons();

            // Act
            var persons_list_from_sort = await this._personsSorterService.GetSortedPersons(allPersons, nameof(Person.PersonName), SortOrderOptions.DESC);

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
        [Fact]
        public async Task UpdatePerson_NullPerson_ToBEArgumentNullException()
        {
            // Arrange
            PersonUpdateRequest? person_update_request = null;

            // Assert
            Func<Task> action = async () => await this._personsUpdaterService.UpdatePerson(person_update_request);
            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task UpdatePerson_InValiPersonID()
        {
            // Arrange
            var person_update_request = this._fixture.Build<PersonUpdateRequest>().Create();

            // Assert

            Func<Task> action = async () => await this._personsUpdaterService.UpdatePerson(person_update_request);
            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task UpdatePerson_PersonNameIsNull_ToBeArgumentException()
        {
            // Arrange
            var person = this._fixture.Build<Person>()
                .With(temp => temp.PersonName, null as string)
                .With(temp => temp.Email, "john@example.com")
                .With(temp => temp.Country, null as Country)
                .With(temp => temp.Gender, "Male")
                .Create();

            var person_response_from_add = person.ToPersonResponse();

            var person_update_request = person_response_from_add.ToPersonUpdateRequest();

            // Assert
            var action = async () => await this._personsUpdaterService.UpdatePerson(person_update_request);
            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task UpdatePerson_PersonFullDetailsUpdation_ToBeSuccessful()
        {
            // Arrange
            var person = this._fixture.Build<Person>()
                .With(temp => temp.Email, "someone@example.com")
                .With(temp => temp.Country, null as Country)
                .With(temp => temp.Gender, "Male")
                .Create();

            var person_response_expected = person.ToPersonResponse();

            var person_update_request = person_response_expected.ToPersonUpdateRequest();
            this._personsRepositoryMock.Setup(temp => temp.UpdatePerson(It.IsAny<Person>())).ReturnsAsync(person);
            this._personsRepositoryMock.Setup(temp => temp.GetPersonByPersonID(It.IsAny<Guid>())).ReturnsAsync(person);

            // Act
            var person_response_from_update = await this._personsUpdaterService.UpdatePerson(person_update_request);

            // Assert
            person_response_from_update.Should().Be(person_response_expected);
        }
        #endregion


        #region DeletePerson
        [Fact]
        public async Task DeletePerson_ValidPersonID_ToBeSuccessful()
        {
            // Arrange
            var person = this._fixture.Build<Person>()
                .With(temp => temp.PersonName, "Jones")
                .With(temp => temp.Email, "jones@example.com")
                .With(temp => temp.Country, null as Country)
                .With(temp => temp.Gender, "Male")
                .With(temp => temp.Address, "address")
                .Create();

            this._personsRepositoryMock.Setup(temp => temp.DeletePersonByPersonID(It.IsAny<Guid>())).ReturnsAsync(true);
            this._personsRepositoryMock.Setup(temp => temp.GetPersonByPersonID(It.IsAny<Guid>())).ReturnsAsync(person);

            // Act
            bool isDeleted = await this._personsDeleterService.DeletePerson(person.PersonID);

            // Assert
            isDeleted.Should().BeTrue();
        }

        [Fact]
        public async Task DeletePerson_InvalidPersonID()
        {
            // Arrange

            // Act
            bool isDeleted = await this._personsDeleterService.DeletePerson(Guid.NewGuid());

            // Assert
            isDeleted.Should().BeFalse();
        }
        #endregion
    }
}
