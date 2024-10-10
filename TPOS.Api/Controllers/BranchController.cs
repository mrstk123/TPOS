using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TPOS.Api.Dtos.Request;
using TPOS.Api.Dtos.Response;
using TPOS.Application.Constants;
using TPOS.Application.Interfaces;
using TPOS.Domain.Entities.Generated;
using TPOS.Infrastructure.Security;

namespace TPOS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BranchController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public BranchController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [Permission(Permissions.Branch.View)]
        public async Task<IActionResult> GetAllBranches()
        {
            var branches = await _unitOfWork.BranchRepository.GetAsync(
                                    include: b => b.Include(x => x.Company)
                                                   .Include(x => x.Contact));
            var branchRespDtos = _mapper.Map<IEnumerable<BranchResponseDto>>(branches);
            foreach (var branch in branches)
            {
                var branchRespDto = branchRespDtos.FirstOrDefault(x => x.BranchID == branch.BranchID);
                if (branchRespDto != null)
                {
                    _mapper.Map(branch.Company, branchRespDto);
                    _mapper.Map(branch.Contact, branchRespDto);
                }
            }
            return Ok(branchRespDtos);
        }

        [HttpGet("Active")]
        [Permission(Permissions.Branch.View)]
        public async Task<IActionResult> GetActiveBranches()
        {
            var branches = await _unitOfWork.BranchRepository.GetAsync(
                                filter: b => b.Active,
                                include: b => b.Include(x => x.Company)
                                               .Include(x => x.Contact));
            var branchRespDtos = _mapper.Map<IEnumerable<BranchResponseDto>>(branches);
            foreach (var branch in branches)
            {
                var branchRespDto = branchRespDtos.FirstOrDefault(x => x.BranchID == branch.BranchID);
                if (branchRespDto != null)
                {
                    _mapper.Map(branch.Company, branchRespDto);
                    _mapper.Map(branch.Contact, branchRespDto);
                }
            }
            return Ok(branchRespDtos);
        }

        [HttpGet("{id}")]
        [Permission(Permissions.Branch.View)]
        public async Task<IActionResult> GetBranchById(int id)
        {
            var branch = await _unitOfWork.BranchRepository.GetSingleAsync(
                                filter: branch => branch.BranchID == id,
                                include: branch => branch.Include(x => x.Company)
                                                    .Include(x => x.Contact));
            if (branch == null)
            {
                return NotFound();
            }

            var branchRespDto = _mapper.Map<BranchResponseDto>(branch);
            _mapper.Map(branch.Company, branchRespDto);
            _mapper.Map(branch.Contact, branchRespDto);

            return Ok(branchRespDto);
        }

        [HttpPost]
        [Permission(Permissions.Branch.Create)]
        public async Task<IActionResult> AddBranch(BranchRequestDto branchReqDto)
        {
            try
            {
                int? contactID = null;
                // Check Contact Person Name
                if (!string.IsNullOrEmpty(branchReqDto.Name))
                {
                    var contactInfo = _mapper.Map<ContactInfo>(branchReqDto);
                    contactInfo.CreatedOn = contactInfo.UpdatedOn = DateTime.UtcNow;
                    contactInfo.CreatedBy = contactInfo.UpdatedBy = ClaimsAccessor.UserID;
                    contactInfo.Active = true;

                    await _unitOfWork.ContactInfoRepository.AddAsync(contactInfo);
                    await _unitOfWork.CompleteAsync();
                    contactID = contactInfo.ContactID;
                }

                var branch = _mapper.Map<Branch>(branchReqDto);
                branch.ContactID = contactID;
                branch.CreatedOn = branch.UpdatedOn = DateTime.UtcNow;
                branch.CreatedBy = branch.UpdatedBy = ClaimsAccessor.UserID;
                branch.Active = true;

                await _unitOfWork.BranchRepository.AddAsync(branch);
                await _unitOfWork.CompleteAsync();

                return CreatedAtAction(nameof(GetBranchById), new { id = branch.BranchID }, branch);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }

        [HttpPut("{id}")]
        [Permission(Permissions.Branch.Modify)]
        public async Task<IActionResult> UpdateBranch(int id, BranchRequestDto branchReqDto)
        {
            if (id != branchReqDto.BranchID || branchReqDto.CompanyID == 0)
            {
                return BadRequest();
            }

            try
            {
                var exisitingBranch = await _unitOfWork.BranchRepository.GetByIdAsync(id);
                if (exisitingBranch == null)
                {
                    return NotFound();
                }

                if (exisitingBranch.ContactID.HasValue) // Already Link to Contact
                {
                    var existingContactInfo = await _unitOfWork.ContactInfoRepository.GetByIdAsync(exisitingBranch.ContactID.Value);
                    if (existingContactInfo == null)
                    {
                        return NotFound("ContactInfo not found.");
                    }

                    // Manually update properties of Contact
                    //existingContactInfo.Name = branchReqDto.Name;   // Branch Contact Person Name
                    //existingContactInfo.Email = branchReqDto.Email;
                    //existingContactInfo.Phone = branchReqDto.Phone;
                    //existingContactInfo.Address = branchReqDto.Address;
                    //existingContactInfo.Active = branchReqDto.Active;

                    // Update properties of Contact
                    _mapper.Map(branchReqDto, existingContactInfo);

                    existingContactInfo.UpdatedOn = DateTime.UtcNow;
                    existingContactInfo.UpdatedBy = ClaimsAccessor.UserID;
                    _unitOfWork.ContactInfoRepository.Update(existingContactInfo);
                }
                else
                {
                    // New ContactInfo
                    if (!string.IsNullOrEmpty(branchReqDto.Name))
                    {
                        var newContactInfo = _mapper.Map<ContactInfo>(branchReqDto);
                        newContactInfo.CreatedOn = newContactInfo.UpdatedOn = DateTime.UtcNow;
                        newContactInfo.CreatedBy = newContactInfo.UpdatedBy = ClaimsAccessor.UserID;
                        newContactInfo.Active = true;

                        await _unitOfWork.ContactInfoRepository.AddAsync(newContactInfo);
                        await _unitOfWork.CompleteAsync();
                        exisitingBranch.ContactID = newContactInfo.ContactID;
                    }
                }


                // Manually update properties of Branch
                //exisitingBranch.BranchName = branchReqDto.BranchName;
                exisitingBranch.Location = branchReqDto.Location;
                exisitingBranch.Active = branchReqDto.Active;

                exisitingBranch.UpdatedOn = DateTime.UtcNow;
                exisitingBranch.UpdatedBy = ClaimsAccessor.UserID;
                _unitOfWork.BranchRepository.Update(exisitingBranch);

                await _unitOfWork.CompleteAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }

        [HttpDelete("{id}")]
        [Permission(Permissions.Branch.Delete)]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                // Inactive Branch
                var branch = await _unitOfWork.BranchRepository.GetByIdAsync(id);
                if (branch == null)
                {
                    return NotFound();
                }
                branch.Active = false;
                branch.UpdatedOn = DateTime.UtcNow;
                branch.UpdatedBy = ClaimsAccessor.UserID;

                //await _unitOfWork.BranchRepository.DeleteAsync(id);
                _unitOfWork.BranchRepository.Update(branch);

                // Inactive related Contact 
                if (branch.ContactID.HasValue)
                {
                    var contactInfo = await _unitOfWork.ContactInfoRepository.GetByIdAsync(branch.ContactID.Value);
                    if (contactInfo != null)
                    {
                        contactInfo.Active = false;
                        contactInfo.UpdatedOn = DateTime.UtcNow;
                        contactInfo.UpdatedBy = ClaimsAccessor.UserID;

                        //await _unitOfWork.ContactInfoRepository.DeleteAsync(id);
                        _unitOfWork.ContactInfoRepository.Update(contactInfo);
                    }
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
