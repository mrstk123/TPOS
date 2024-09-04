using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TPOS.Api.Dtos;
using TPOS.Core.Entities.Generated;
using TPOS.Core.Interfaces;
using TPOS.Core.Utilities;

// First writing Controller

namespace TPOS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class CustomerController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CustomerController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetAllCustomers()
        {
            var customers = await _unitOfWork.CustomerRepository.GetAsync(
                include: cus => cus.Include(x => x.Contact));

            var customersDto = _mapper.Map<IEnumerable<CustomerDto>>(customers);
            foreach (var customer in customers)
            {
                var customerDto = customersDto.FirstOrDefault(dto => dto.CustomerID == customer.CustomerID);
                if (customerDto != null)
                {
                    _mapper.Map(customer.Contact, customerDto); // Map ContactInfo to CustomerDto
                }
            }

            return Ok(customersDto);
        }

        [HttpGet("Active")]
        public async Task<ActionResult<IEnumerable<Customer>>> GetActiveCustomers()
        {
            var customers = await _unitOfWork.CustomerRepository.GetAsync(
                filter: cus => cus.Active,
                include: cus => cus.Include(x => x.Contact));

            var customersDto = _mapper.Map<IEnumerable<CustomerDto>>(customers);
            foreach (var customer in customers)
            {
                var customerDto = customersDto.FirstOrDefault(dto => dto.CustomerID == customer.CustomerID);
                if (customerDto != null)
                {
                    _mapper.Map(customer.Contact, customerDto); // Map ContactInfo to CustomerDto
                }
            }

            return Ok(customersDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomerById(int id)
        {
            var customer = await _unitOfWork.CustomerRepository.GetSingleAsync(
                                    filter: cus => cus.CustomerID == id,
                                    include: cus => cus.Include(x => x.Contact));

            if (customer == null)
            {
                return NotFound();
            }

            var customerDto = _mapper.Map<CustomerDto>(customer);
            _mapper.Map(customer.Contact, customerDto); // Ignore Base Columns of Contact in mapping to customerDto to avoid overriding Base Columns of customerDto (see in AutoMapperProfile)

            return Ok(customerDto);
        }

        [HttpPost]
        public async Task<IActionResult> AddCustomer([FromBody] CustomerDto customerDto)
        {
            try
            {
                // Map DTO to ContactInfo entity
                var contactInfo = _mapper.Map<ContactInfo>(customerDto);
                contactInfo.CreatedOn = contactInfo.UpdatedOn = DateTime.UtcNow;
                contactInfo.CreatedBy = contactInfo.UpdatedBy = ClaimsAccessor.UserID;
                contactInfo.Active = true;

                await _unitOfWork.ContactInfoRepository.AddAsync(contactInfo);
                await _unitOfWork.CompleteAsync();

                // Map DTO to Customer entity
                var customer = _mapper.Map<Customer>(customerDto);
                customer.ContactID = contactInfo.ContactID;
                customer.CreatedOn = customer.UpdatedOn = DateTime.UtcNow;
                customer.CreatedBy = customerDto.UpdatedBy = ClaimsAccessor.UserID;
                customer.Active = true;

                await _unitOfWork.CustomerRepository.AddAsync(customer);
                await _unitOfWork.CompleteAsync();

                return CreatedAtAction(nameof(GetCustomerById), new { id = customer.CustomerID }, customer);  // The newly created customer
            }
            catch (Exception ex)
            {
                // Log exception (you might use a logging framework like Serilog or NLog here)
                // Log.Error(ex, "An error occurred while creating a customer.");

                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(int id, [FromBody] CustomerDto customerDto)
        {
            if (id != customerDto.CustomerID || customerDto.ContactID == 0)
            {
                return BadRequest();
            }

            try
            {
                var customer = await _unitOfWork.CustomerRepository.GetByIdAsync(id);
                if (customer == null)
                {
                    return NotFound();
                }

                var contactInfo = await _unitOfWork.ContactInfoRepository.GetByIdAsync(customer.ContactID);
                if (contactInfo == null)
                {
                    return NotFound("ContactInfo not found.");
                }

                // Update ContactInfo
                _mapper.Map(customerDto, contactInfo);
                contactInfo.UpdatedOn = DateTime.UtcNow;
                contactInfo.UpdatedBy = ClaimsAccessor.UserID;
                _unitOfWork.ContactInfoRepository.Update(contactInfo);

                // Update Customer
                _mapper.Map(customerDto, customer);
                customer.UpdatedOn = DateTime.UtcNow;
                customer.UpdatedBy = ClaimsAccessor.UserID;
                _unitOfWork.CustomerRepository.Update(customer);

                await _unitOfWork.CompleteAsync();
                return NoContent();
            }
            catch(Exception ex)
            {
                // Log exception
                // Log.Error(ex, "An error occurred while updating the customer.");

                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var customer = await _unitOfWork.CustomerRepository.GetByIdAsync(id);
                if (customer == null)
                {
                    return NotFound();
                }
                customer.Active = false;
                customer.UpdatedBy = ClaimsAccessor.UserID;
                customer.UpdatedOn = DateTime.UtcNow;

                //await _unitOfWork.CustomerRepository.DeleteAsync(id);
                _unitOfWork.CustomerRepository.Update(customer);
                await _unitOfWork.CompleteAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                // Log exception
                // Log.Error(ex, "An error occurred while deleting the customer.");

                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }
    }
}
