using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TPOS.Api.Dtos.Request;
using TPOS.Api.Dtos.Response;
using TPOS.Core.Entities.Generated;
using TPOS.Core.Interfaces;
using TPOS.Core.Utilities;

namespace TPOS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // [Authorize]
    public class EmployeeController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public EmployeeController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEmployees()
        {
            var employees = await _unitOfWork.EmployeeRepository.GetAsync(
                                    include: emp => emp.Include(x => x.Contact)
                                                    .Include(x => x.Branch)
                                                    .Include(x => x.Department)
                                                    .Include(x => x.Position));
            var empDtos = _mapper.Map<IEnumerable<EmployeeResponseDto>>(employees);
            foreach (var emp in employees)
            {
                var empDto = empDtos.FirstOrDefault(x => x.EmployeeID == emp.EmployeeID);
                if (empDto != null)
                {
                    _mapper.Map(emp.Contact, empDto);
                    _mapper.Map(emp.Branch, empDto);
                }
            }
            return Ok(empDtos);
        }

        [HttpGet("Active")]
        public async Task<IActionResult> GetActiveEmployees()
        {
            var employees = await _unitOfWork.EmployeeRepository.GetAsync(
                                filter: emp => emp.Active,
                                include: emp => emp.Include(x => x.Contact)
                                                    .Include(x => x.Branch)
                                                    .Include(x => x.Department)
                                                    .Include(x => x.Position));
            var empDtos = _mapper.Map<IEnumerable<EmployeeResponseDto>>(employees);
            foreach (var emp in employees)
            {
                var empDto = empDtos.FirstOrDefault(x => x.EmployeeID == emp.EmployeeID);
                if (empDto != null)
                {
                    _mapper.Map(emp.Contact, empDto);
                    _mapper.Map(emp.Branch, empDto);
                }
            }
            return Ok(empDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            var employee = await _unitOfWork.EmployeeRepository.GetSingleAsync(
                                filter: emp => emp.EmployeeID == id,
                                include: emp => emp.Include(x => x.Contact)
                                                    .Include(x => x.Branch)
                                                    .Include(x => x.Department)
                                                    .Include(x => x.Position));
            if (employee == null)
            {
                return NotFound();
            }

            var empDto = _mapper.Map<EmployeeResponseDto>(employee);
            _mapper.Map(employee.Contact, empDto);
            _mapper.Map(employee.Branch, empDto);

            return Ok(empDto);
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployee(EmployeeRequestDto empReqDto)
        {
            try
            {
                var contactInfo = _mapper.Map<ContactInfo>(empReqDto);
                contactInfo.CreatedOn = contactInfo.UpdatedOn = DateTime.UtcNow;
                contactInfo.CreatedBy = contactInfo.UpdatedBy = ClaimsAccessor.UserID;
                contactInfo.Active = true;

                await _unitOfWork.ContactInfoRepository.AddAsync(contactInfo);
                await _unitOfWork.CompleteAsync();

                var employee = _mapper.Map<Employee>(empReqDto);
                employee.ContactID = contactInfo.ContactID;
                employee.CreatedOn = employee.UpdatedOn = DateTime.UtcNow;
                employee.CreatedBy = employee.UpdatedBy = ClaimsAccessor.UserID;
                employee.Active = true;

                await _unitOfWork.EmployeeRepository.AddAsync(employee);
                await _unitOfWork.CompleteAsync();

                return CreatedAtAction(nameof(GetEmployeeById), new { id = employee.EmployeeID }, employee);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, EmployeeRequestDto empReqDto)
        {
            if (id != empReqDto.EmployeeID || empReqDto.ContactID == 0 || empReqDto.BranchID == 0)
            {
                return BadRequest();
            }

            try
            {
                var exisitingEmployee = await _unitOfWork.EmployeeRepository.GetByIdAsync(id);
                if (exisitingEmployee == null)
                {
                    return NotFound();
                }

                var existingContactInfo = await _unitOfWork.ContactInfoRepository.GetByIdAsync(exisitingEmployee.ContactID);
                if (existingContactInfo == null)
                {
                    return NotFound("ContactInfo not found.");
                }

                // Update ContactInfo
                // _mapper.Map(empReqDto, existingContactInfo); // Using Automapper update all of the properties of contactInfo (except Ignore properties)
                // Manually update properties
                // existingContactInfo.Name = empReqDto.Name; // do not update Name
                existingContactInfo.Email = empReqDto.Email;
                existingContactInfo.Phone = empReqDto.Phone;
                existingContactInfo.Address = empReqDto.Address;
                existingContactInfo.Active = empReqDto.Active;

                existingContactInfo.UpdatedOn = DateTime.UtcNow;
                existingContactInfo.UpdatedBy = ClaimsAccessor.UserID;
                _unitOfWork.ContactInfoRepository.Update(existingContactInfo);

                // Update employee
                // _mapper.Map(empReqDto, exisitingEmployee); // Using Automapper update all of the properties of employee (except Ignore properties)   
                // Manually update properties
                exisitingEmployee.BranchID = empReqDto.BranchID;
                exisitingEmployee.PositionID = empReqDto.PositionID;
                exisitingEmployee.DepartmentID = empReqDto.DepartmentID;
                exisitingEmployee.HireDate = empReqDto.HireDate;
                exisitingEmployee.Active = empReqDto.Active;

                exisitingEmployee.UpdatedOn = DateTime.UtcNow;
                exisitingEmployee.UpdatedBy = ClaimsAccessor.UserID;
                _unitOfWork.EmployeeRepository.Update(exisitingEmployee);

                await _unitOfWork.CompleteAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                // Inactive Employee
                var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(id);
                if (employee == null)
                {
                    return NotFound();
                }
                employee.Active = false;
                employee.UpdatedOn = DateTime.UtcNow;
                employee.UpdatedBy = ClaimsAccessor.UserID;

                //await _unitOfWork.EmployeeRepository.DeleteAsync(id);
                _unitOfWork.EmployeeRepository.Update(employee);

                // Inactive related Contact 
                var contactInfo = await _unitOfWork.ContactInfoRepository.GetByIdAsync(employee.ContactID);
                if (contactInfo != null)
                {
                    contactInfo.Active = false;
                    contactInfo.UpdatedOn = DateTime.UtcNow;
                    contactInfo.UpdatedBy = ClaimsAccessor.UserID;

                    //await _unitOfWork.ContactInfoRepository.DeleteAsync(id);
                    _unitOfWork.ContactInfoRepository.Update(contactInfo);
                }

                await _unitOfWork.CompleteAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }

    }
}