using CsvHelper;
using Entities;
using Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using RepositoryContracts;
using Serilog;
using SerilogTimings;
using ServiceContractsxUnit;
using ServiceContractsxUnit.DTO;
using ServiceContractsxUnit.Enums;
using ServicesxUnit.Helpers;

namespace ServicesxUnit
{
    public class PersonsService : IPersonsService
    {
        readonly List<Person> _persons;
        //readonly PersonsDbContext _db;
        //readonly ApplicationDbContext _db;
        readonly IPersonsRepository _personsRepository;
        readonly ICountriesService _countriesService;
        readonly ILogger<PersonsService> _logger;
        readonly IDiagnosticContext _diagnosticContext;
        [ActivatorUtilitiesConstructor]
        //public PersonsService(PersonsDbContext personsDbContext, ICountriesService countriesService)
        //{
        //    _db = personsDbContext;
        //    _countriesService = countriesService;
        //}
        //public PersonsService(ApplicationDbContext applicationDbContext, ICountriesService countriesService)
        //{
        //    _db = applicationDbContext;
        //    _countriesService = countriesService;
        //}

        /// <summary>
        /// Initializes a new instance of the <see cref="PersonsService"/> class.
        /// </summary>
        /// <param name="personsRepository">The repository for managing persons.</param>
        /// <param name="logger">The logger for logging information.</param>
        /// <param name="diagnosticContext">The diagnostic context for collecting diagnostic information.</param>
        public PersonsService(IPersonsRepository personsRepository, ILogger<PersonsService> logger, IDiagnosticContext diagnosticContext)
        {
            _personsRepository = personsRepository;
            _logger = logger;
            _diagnosticContext = diagnosticContext;
        }
        //public PersonsService(bool initialize = true)
        //{
        //    _persons = [];
        //    _countryService = new CountriesService();

        //    if (initialize) 
        //    {
        //        //{69084F41-DEE6-4A8B-9BD8-C1F6EC53E989}
        //        //{E93906BA-42DC-4A11-B019-35B62C5B69D6}
        //        //{3B19D1B3-776B-49D1-A2CA-F2956AF29313}
        //        //{98685D3C-B4FA-4C1E-B34C-895C29AB6722}
        //        //{1F2DD554-C23F-4765-B0F6-D16A098BD2E3}
        //        //{7C318DF9-F4AC-43FA-B349-2E3D142EFAD3}
        //        //{DE373035-9B92-4B3D-AD8C-3D65A0D978E2}
        //        //{4FA3BF01-7AD7-4444-9CBB-7130DD006005}
        //        //{E40F69DE-C4E5-4E7D-A940-102CE2936F0C}
        //        //{CDCD4CC9-5939-42FC-B416-A6A9EF380304}
        //        //{6C28068A-2207-4783-A826-677AA1D2C3BF}

        //        _persons.Add(new() {
        //            PersonID = Guid.Parse("69084F41-DEE6-4A8B-9BD8-C1F6EC53E989"),
        //            CountryID = Guid.Parse("47F7853F-85A4-4C32-A2EC-DEA7E0E30C94"),
        //            PersonName = "Donny",
        //            Email = "dcorten0@utexas.edu",
        //            DateOfBirth = DateTime.Parse("1997-01-13"),
        //            Gender = "Female",
        //            Address = "81 Aberg Avenue",
        //            ReceiveNewsLetters = true,
        //        });

        //        _persons.Add(new()
        //        {
        //            PersonID = Guid.Parse("E93906BA-42DC-4A11-B019-35B62C5B69D6"),
        //            CountryID = Guid.Parse("BADD5A24-E691-4B86-A56A-43583D37FC4B"),
        //            PersonName = "Conan",
        //            Email = "cmacwilliam1@ning.com",
        //            DateOfBirth = DateTime.Parse("1996-08-30"),
        //            Gender = "Male",
        //            Address = "743 Del Mar Plaza",
        //            ReceiveNewsLetters = true,
        //        });

        //        _persons.Add(new()
        //        {
        //            PersonID = Guid.Parse("3B19D1B3-776B-49D1-A2CA-F2956AF29313"),
        //            CountryID = Guid.Parse("ACA7FED4-04CF-48F7-8A1D-044F618E06E4"),
        //            PersonName = "Brandtr",
        //            Email = "brosnau2@npr.org",
        //            DateOfBirth = DateTime.Parse("1996-01-28"),
        //            Gender = "Male",
        //            Address = "74 Russell Drive",
        //            ReceiveNewsLetters = true,
        //        });

        //        _persons.Add(new()
        //        {
        //            PersonID = Guid.Parse("98685D3C-B4FA-4C1E-B34C-895C29AB6722"),
        //            CountryID = Guid.Parse("60ED85A5-73BB-4838-A5F0-ECE7AC373DDD"),
        //            PersonName = "Melisse",
        //            Email = "mlightwing3@webeden.co.uk",
        //            DateOfBirth = DateTime.Parse("1997-03-21"),
        //            Gender = "Female",
        //            Address = "4380 Sherman Alley",
        //            ReceiveNewsLetters = true,
        //        });

        //        _persons.Add(new()
        //        {
        //            PersonID = Guid.Parse("1F2DD554-C23F-4765-B0F6-D16A098BD2E3"),
        //            CountryID = Guid.Parse("C4B876D6-72C4-4FC6-A7B9-46F38ABF10DC"),
        //            PersonName = "Ignaz",
        //            Email = "ibiddy4@smugmug.com",
        //            DateOfBirth = DateTime.Parse("1996-05-14"),
        //            Gender = "Male",
        //            Address = "184 Melody Alley",
        //            ReceiveNewsLetters = true,
        //        });

        //        _persons.Add(new()
        //        {
        //            PersonID = Guid.Parse("7C318DF9-F4AC-43FA-B349-2E3D142EFAD3"),
        //            CountryID = Guid.Parse("47F7853F-85A4-4C32-A2EC-DEA7E0E30C94"),
        //            PersonName = "Carine",
        //            Email = "cboughtwood5@google.co.uk",
        //            DateOfBirth = DateTime.Parse("1993-03-04"),
        //            Gender = "Female",
        //            Address = "75156 Vernon Center",
        //            ReceiveNewsLetters = false,
        //        });

        //        _persons.Add(new()
        //        {
        //            PersonID = Guid.Parse("DE373035-9B92-4B3D-AD8C-3D65A0D978E2"),
        //            CountryID = Guid.Parse("BADD5A24-E691-4B86-A56A-43583D37FC4B"),
        //            PersonName = "Horatio",
        //            Email = "hciccotto6@microsoft.com",
        //            DateOfBirth = DateTime.Parse("1994-11-04"),
        //            Gender = "Male",
        //            Address = "3265 Golf Alley",
        //            ReceiveNewsLetters = true,
        //        });

        //        _persons.Add(new()
        //        {
        //            PersonID = Guid.Parse("4FA3BF01-7AD7-4444-9CBB-7130DD006005"),
        //            CountryID = Guid.Parse("ACA7FED4-04CF-48F7-8A1D-044F618E06E4"),
        //            PersonName = "Flossy",
        //            Email = "fkivelle7@sakura.ne.jp",
        //            DateOfBirth = DateTime.Parse("1993-03-11"),
        //            Gender = "Female",
        //            Address = "5036 Blaine Lane",
        //            ReceiveNewsLetters = true,
        //        });

        //        _persons.Add(new()
        //        {
        //            PersonID = Guid.Parse("E40F69DE-C4E5-4E7D-A940-102CE2936F0C"),
        //            CountryID = Guid.Parse("60ED85A5-73BB-4838-A5F0-ECE7AC373DDD"),
        //            PersonName = "Gwenette",
        //            Email = "gdiamant8@youtube.com",
        //            DateOfBirth = DateTime.Parse("1992-07-30"),
        //            Gender = "Female",
        //            Address = "4267 Drewry Drive",
        //            ReceiveNewsLetters = true,
        //        });

        //        _persons.Add(new()
        //        {
        //            PersonID = Guid.Parse("CDCD4CC9-5939-42FC-B416-A6A9EF380304"),
        //            CountryID = Guid.Parse("C4B876D6-72C4-4FC6-A7B9-46F38ABF10DC"),
        //            PersonName = "Reggis",
        //            Email = "rschall9@hatena.ne.jp",
        //            DateOfBirth = DateTime.Parse("1990-01-02"),
        //            Gender = "Male",
        //            Address = "743 Chive Drive",
        //            ReceiveNewsLetters = true,
        //        });

        //        /*
        //            Donny,dcorten0@utexas.edu,1997-01-13,Female,81 Aberg Avenue,true
        //            Conan,cmacwilliam1@ning.com,1996-08-30,Male,743 Del Mar Plaza,true
        //            Brandtr,brosnau2@npr.org,1996-01-28,Male,74 Russell Drive,true
        //            Melisse,mlightwing3@webeden.co.uk,1997-03-21,Female,4380 Sherman Alley,true
        //            Ignaz,ibiddy4@smugmug.com,1996-05-14,Male,184 Melody Alley,true
        //            Carine,cboughtwood5@google.co.uk,1993-03-04,Female,75156 Vernon Center,false
        //            Horatio,hciccotto6@microsoft.com,1994-11-04,Male,3265 Golf Alley,true
        //            Flossy,fkivelle7@sakura.ne.jp,1993-03-11,Female,5036 Blaine Lane,true
        //            Gwenette,gdiamant8@youtube.com,1992-07-30,Female,4267 Drewry Drive,true
        //            Reggis,rschall9@hatena.ne.jp,1990-01-02,Male,743 Chive Drive,true
        //        */
        //    }
        //}
        //PersonResponse ConvertPersonToPersonResponse(Person person)
        //{
        //    var personResponse = person.ToPersonResponse();

        //    //personResponse.Country = _countryService.GetCountryByCopuntryID(person.CountryID)?.CountryName;
        //    personResponse.Country = person.Country?.CountryName;
        //    return personResponse;
        //}

        /// <summary>
        /// Adds a new person based on the provided PersonAddRequest.
        /// </summary>
        /// <param name="personAddRequest">The request containing the details for the new person.</param>
        /// <returns>The response containing the details of the added person.</returns>
        // public PersonResponse AddPerson(PersonAddRequest? personAddRequest)
        public async Task<PersonResponse> AddPerson(PersonAddRequest? personAddRequest)
        {
            // check if PersonAddRequest is not null
            ArgumentNullException.ThrowIfNull(personAddRequest);

            //Validation: PersonName
            //if (string.IsNullOrEmpty(personAddRequest.PersonName))
            //{
            //    throw new ArgumentException("PersonName can't be blank");
            //}

            //Model validations
            ValidationHelper.ModelValidation(personAddRequest);

            //convert personAddRequest into Person type
            var person = personAddRequest.ToPerson();

            //generate PersonID
            person.PersonID = Guid.NewGuid();

            //add person object to persons list
            //_persons.Add(person);
            //_db.Persons.Add(person);
            //_db.SaveChanges();
            //await _db.SaveChangesAsync();
            await _personsRepository.AddPerson(person);

            //_db.sp_InsertPerson(person);

            //convert the Person object to PersonResponse type
            //return ConvertPersonToPersonResponse(person);
            return person.ToPersonResponse();
        }

        /// <summary>
        /// Retrieves all persons from the data source and returns them as a list of PersonResponse objects.
        /// </summary>
        /// <returns>A list of PersonResponse objects.</returns>
        //public List<PersonResponse> GetAllPersons()
        public async Task<List<PersonResponse>> GetAllPersons()
        {
            //return [.._persons.Select(temp => temp.ToPersonResponse())];
            //return [.._persons.Select(ConvertPersonToPersonResponse)];
            //return [.._db.Persons.Select(ConvertPersonToPersonResponse)];
            //SELECT * from Persons
            //var persons = _db.Persons.Include(nameof(Person.Country)).ToList();
            //var persons = await _db.Persons.Include(nameof(Person.Country)).ToListAsync();
            //return [..persons.ToList().Select(ConvertPersonToPersonResponse)];
            //return [..persons.ToList().Select(p => p.ToPersonResponse())];
            //return [.._db.Persons.ToList().Select(ConvertPersonToPersonResponse)];
            //return [.._db.sp_GetAllPersons().Select(ConvertPersonToPersonResponse)];

            var persons = await _personsRepository.GetAllPersons();
            _logger.LogInformation("GetAllPersons of PersonsService");
            return [.. persons.Select(temp => temp.ToPersonResponse())];
        }

        /// <summary>
        /// Retrieves a person by their unique identifier (PersonID) and returns the corresponding PersonResponse object.
        /// </summary>
        /// <param name="personID">The unique identifier of the person to retrieve.</param>
        /// <returns>The PersonResponse object if found, otherwise null.</returns>
        // public PersonResponse? GetPersonByPersonID(Guid? personID)
        public async Task<PersonResponse?> GetPersonByPersonID(Guid? personID)
        {
            if (personID == null)
            {
                return null;
            }

            // var person = _persons.FirstOrDefault(temp => temp.PersonID == personID);
            // var person = _db.Persons.Include(nameof(Person.Country)).FirstOrDefault(temp => temp.PersonID == personID);
            // var person = await _db.Persons.Include(nameof(Person.Country)).FirstOrDefaultAsync(temp => temp.PersonID == personID);
            var person = await _personsRepository.GetPersonByPersonID(personID.Value);

            if (person == null)
            {
                return null;
            }

            //return person.ToPersonResponse();
            //return ConvertPersonToPersonResponse(person);
            return person.ToPersonResponse();
        }

        /// <summary>
        /// Retrieves a list of persons filtered based on the specified search criteria (searchBy and searchString) and returns them as a list of PersonResponse objects.
        /// </summary>
        /// <param name="searchBy">The property to search by.</param>
        /// <param name="searchString">The string to search for.</param>
        /// <returns>A list of PersonResponse objects that match the search criteria.</returns>
        // public List<PersonResponse> GetFilteredPersons(string searchBy, string? searchString)
        public async Task<List<PersonResponse>> GetFilteredPersons(string searchBy, string? searchString)
        {
            this._logger.LogInformation("GetFilteredPersons of PersonsService");
            /*var allPersons = await GetAllPersons();
            var matchingPersons = allPersons;*/

            /*if (string.IsNullOrEmpty(searchBy) || string.IsNullOrEmpty(searchString))
            {
                return matchingPersons;
            }*/

            /*var matchingPersons = searchBy switch
            {
                //case nameof(Person.PersonName):
                nameof(PersonResponse.PersonName) => [.. allPersons.Where(temp => string.IsNullOrEmpty(temp.PersonName) || temp.PersonName.Contains(searchString, StringComparison.OrdinalIgnoreCase))],
                //case nameof(Person.Email):
                nameof(PersonResponse.Email) => [.. allPersons.Where(temp => string.IsNullOrEmpty(temp.Email) || temp.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase))],
                nameof(PersonResponse.DateOfBirth) => [.. allPersons.Where(temp => temp.DateOfBirth == null || temp.DateOfBirth.Value.ToString("dd MMM yyyy").Contains(searchString, StringComparison.OrdinalIgnoreCase))],
                nameof(PersonResponse.Gender) => [.. allPersons.Where(temp => string.IsNullOrEmpty(temp.Gender) || temp.Gender.Equals(searchString, StringComparison.OrdinalIgnoreCase))],
                nameof(PersonResponse.Address) => [.. allPersons.Where(temp => string.IsNullOrEmpty(temp.Address) || temp.Address.Contains(searchString, StringComparison.OrdinalIgnoreCase))],
                _ => allPersons,
            };*/

            var matchingPersons = new List<Person>();
            using (Operation.Time("Time taken to get filtered persons from database"))
            {
                matchingPersons = searchBy switch
                {
                    // case nameof(Person.PersonName):
                    nameof(PersonResponse.PersonName) => [.. await this._personsRepository.GetFilteredPersons(temp => temp.PersonName.Contains(searchString))],

                    // case nameof(Person.Email):
                    nameof(PersonResponse.Email) => [.. await this._personsRepository.GetFilteredPersons(temp => string.IsNullOrEmpty(temp.Email) || temp.Email.Contains(searchString))],
                    nameof(PersonResponse.DateOfBirth) => [.. await this._personsRepository.GetFilteredPersons(temp => temp.DateOfBirth == null || temp.DateOfBirth.Value.ToString("dd MMM yyyy").Contains(searchString))],
                    nameof(PersonResponse.Gender) => [.. await this._personsRepository.GetFilteredPersons(temp => string.IsNullOrEmpty(temp.Gender) || temp.Gender.Equals(searchString))],
                    nameof(PersonResponse.Address) => [.. await this._personsRepository.GetFilteredPersons(temp => string.IsNullOrEmpty(temp.Address) || temp.Address.Contains(searchString))],
                    _ => await this._personsRepository.GetAllPersons(),
                };
            } // end of "using block" of serilog timings

            this._diagnosticContext.Set("MatchingPersons", matchingPersons);
            return [.. matchingPersons.Select(temp => temp.ToPersonResponse())];
        }

        /// <summary>
        /// Retrieves a list of persons sorted based on the specified property (sortBy) and sort order (sortOrder) and returns them as a list of PersonResponse objects.
        /// </summary>
        /// <param name="allPersons">The list of all persons.</param>
        /// <param name="sortBy">The property to sort by.</param>
        /// <param name="sortOrder">The sort order.</param>
        /// <returns>A list of sorted person responses.</returns>
        // public List<PersonResponse> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrder)
        public async Task<List<PersonResponse>> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrder)
        {
            this._logger.LogInformation("GetSortedPersons of PersonsService");
            if (string.IsNullOrEmpty(sortBy))
            {
                return allPersons;
            }

            IEnumerable<PersonResponse> sortedPersons = 
            (sortBy, sortOrder) switch
            {
                (nameof(PersonResponse.PersonName), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase),
                (nameof(PersonResponse.PersonName), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase),

                (nameof(PersonResponse.Email), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Email, StringComparer.OrdinalIgnoreCase),
                (nameof(PersonResponse.Email), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Email, StringComparer.OrdinalIgnoreCase),

                (nameof(PersonResponse.DateOfBirth), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.DateOfBirth),
                (nameof(PersonResponse.DateOfBirth), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.DateOfBirth),

                (nameof(PersonResponse.Age), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Age),
                (nameof(PersonResponse.Age), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Age),

                (nameof(PersonResponse.Gender), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Gender, StringComparer.OrdinalIgnoreCase),
                (nameof(PersonResponse.Gender), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Gender, StringComparer.OrdinalIgnoreCase),

                (nameof(PersonResponse.Country), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Country, StringComparer.OrdinalIgnoreCase),
                (nameof(PersonResponse.Country), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Country, StringComparer.OrdinalIgnoreCase),

                (nameof(PersonResponse.Address), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Address, StringComparer.OrdinalIgnoreCase),
                (nameof(PersonResponse.Address), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Address, StringComparer.OrdinalIgnoreCase),

                (nameof(PersonResponse.ReceiveNewsLetters), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.ReceiveNewsLetters),
                (nameof(PersonResponse.ReceiveNewsLetters), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.ReceiveNewsLetters),

                _ => allPersons
            };

            return [..sortedPersons];
        }

        /// <summary>
        /// Updates the details of an existing person based on the provided PersonUpdateRequest and returns the updated PersonResponse.
        /// </summary>
        /// <param name="personUpdateRequest">The request containing the updated person details.</param>
        /// <returns>The updated person response.</returns>
        // public PersonResponse UpdatePerson(PersonUpdateRequest? personUpdateRequest)
        public async Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest)
        {
            ArgumentNullException.ThrowIfNull(personUpdateRequest);

            // validation
            ValidationHelper.ModelValidation(personUpdateRequest);

            // get matching person object to update
            // var matchingPerson = _persons.FirstOrDefault(temp => temp.PersonID == personUpdateRequest.PersonID) ?? throw new ArgumentException("Given person id doesn't exist");
            // var matchingPerson = _db.Persons.FirstOrDefault(temp => temp.PersonID == personUpdateRequest.PersonID) ?? throw new ArgumentException("Given person id doesn't exist");
            // var matchingPerson = await _db.Persons.FirstOrDefaultAsync(temp => temp.PersonID == personUpdateRequest.PersonID) ?? throw new ArgumentException("Given person id doesn't exist");
            var matchingPerson = await this._personsRepository.GetPersonByPersonID(personUpdateRequest.PersonID);
            if (matchingPerson == null)
            {
                 // throw new ArgumentException("Given person id doesn't exist");
                 throw new InvalidPersonIDException("Given person id doesn't exist");
            }

            // update all details
            matchingPerson!.PersonName = personUpdateRequest.PersonName;
            matchingPerson!.Email = personUpdateRequest.Email;
            matchingPerson!.DateOfBirth = personUpdateRequest.DateOfBirth;
            matchingPerson!.Gender = personUpdateRequest.Gender.ToString();
            matchingPerson!.CountryID = personUpdateRequest.CountryID;
            matchingPerson!.Address = personUpdateRequest.Address;
            matchingPerson!.ReceiveNewsLetters = personUpdateRequest.ReceiveNewsLetters;

            // _db.SaveChanges(); //Update the database with the changes made to the matchingPerson object
            // await _db.SaveChangesAsync(); //Update the database with the changes made to the matchingPerson object
            await this._personsRepository.UpdatePerson(matchingPerson!);

            // return matchingPerson.ToPersonResponse();
            // return ConvertPersonToPersonResponse(matchingPerson);
            return matchingPerson!.ToPersonResponse();
        }

        /// <summary>
        /// Deletes a person based on the provided PersonID and returns a boolean indicating whether the deletion was successful.
        /// </summary>
        /// <param name="personID">The ID of the person to delete.</param>
        /// <returns>A task that represents the asynchronous operation. The task result indicates whether the deletion was successful.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the personID is null.</exception>
        // public bool DeletePerson(Guid? personID)
        public async Task<bool> DeletePerson(Guid? personID)
        {
            if (personID == null)
            {
                throw new ArgumentNullException(nameof(personID));
            }

            /*var person = _persons.FirstOrDefault(temp => temp.PersonID == personID);
            var person = _db.Persons.FirstOrDefault(temp => temp.PersonID == personID);
            var person = await _db.Persons.FirstOrDefaultAsync(temp => temp.PersonID == personID);*/
            var person = await this._personsRepository.GetPersonByPersonID(personID.Value);
            if (person == null)
            {
                return false;
            }

            /*_persons.RemoveAll(temp => temp.PersonID == personID);
            _db.Persons.Remove(_db.Persons.First(temp => temp.PersonID == personID));
            _db.SaveChanges(); //Save changes to the database after removing the person
            await _db.SaveChangesAsync(); //Save changes to the database after removing the person*/
            await this._personsRepository.DeletePersonByPersonID(personID.Value);
            return true;
        }

        public async Task<MemoryStream> GetPersonsCSV()
        {
            var memoryStream = new MemoryStream();
            using (var streamWriter = new StreamWriter(memoryStream, leaveOpen: true))
            using (var csvWriter = CsvWriter.Create(streamWriter, leaveOpen: true))
            {
                csvWriter.WriteField(nameof(PersonResponse.PersonName));
                csvWriter.WriteField(nameof(PersonResponse.Email));
                csvWriter.WriteField(nameof(PersonResponse.DateOfBirth));
                csvWriter.WriteField(nameof(PersonResponse.Age));
                csvWriter.WriteField(nameof(PersonResponse.Gender));
                csvWriter.WriteField(nameof(PersonResponse.Country));
                csvWriter.WriteField(nameof(PersonResponse.Address));
                csvWriter.WriteField(nameof(PersonResponse.ReceiveNewsLetters));
                csvWriter.EndRecord();

                var persons = await GetAllPersons();
                foreach (var person in persons)
                {
                    csvWriter.WriteField(person.PersonName);
                    csvWriter.WriteField(person.Email);
                    csvWriter.WriteField(person.DateOfBirth?.ToString("dd MMM yyyy"));
                    csvWriter.WriteField(person.Age);
                    csvWriter.WriteField(person.Gender);
                    csvWriter.WriteField(person.Country);
                    csvWriter.WriteField(person.Address);
                    csvWriter.WriteField(person.ReceiveNewsLetters);
                    csvWriter.EndRecord();
                }
            }

            memoryStream.Position = 0;
            return memoryStream;
        }

        public async Task<MemoryStream> GetPersonsExcel()
        {
            var memoryStream = new MemoryStream();            
            using var excelPackage = new ExcelPackage(memoryStream);
            var workSheet = excelPackage.Workbook.Worksheets.Add("PersonsSheet");
            workSheet.Cells["A1"].Value = "Person Name";
            workSheet.Cells["B1"].Value = "Email";
            workSheet.Cells["C1"].Value = "Date of Birth";
            workSheet.Cells["D1"].Value = "Age";
            workSheet.Cells["E1"].Value = "Gender";
            workSheet.Cells["F1"].Value = "Country";
            workSheet.Cells["G1"].Value = "Address";
            workSheet.Cells["H1"].Value = "Receive News Letters";

            using var headerCells = workSheet.Cells["A1:H1"];
            headerCells.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            headerCells.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
            headerCells.Style.Font.Bold = true;

            int row = 2;
            //var persons = _db.Persons.Include(nameof(Person.Country)).Select(temp => temp.ToPersonResponse()).ToList();
            var persons = await GetAllPersons();
            foreach (var person in persons)
            {
                workSheet.Cells[$"A{row}"].Value = person.PersonName; //Cells[row, 1] = person.PersonName;
                workSheet.Cells[$"B{row}"].Value = person.Email;
                workSheet.Cells[$"C{row}"].Value = person.DateOfBirth?.ToString("dd MMM yyyy");
                workSheet.Cells[$"D{row}"].Value = person.Age;
                workSheet.Cells[$"E{row}"].Value = person.Gender;
                workSheet.Cells[$"F{row}"].Value = person.Country;
                workSheet.Cells[$"G{row}"].Value = person.Address;
                workSheet.Cells[$"H{row}"].Value = person.ReceiveNewsLetters;
                row++;
            }
            workSheet.Cells[$"A1:H{row}"].AutoFitColumns();
            //excelPackage.Save();
            await excelPackage.SaveAsync();

            memoryStream.Position = 0;
            return memoryStream;
        }
    }
}
