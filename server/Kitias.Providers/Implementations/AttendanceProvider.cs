﻿using AutoMapper;
using Kitias.Persistence.DTOs;
using Kitias.Persistence.Entities.Scheduler;
using Kitias.Persistence.Entities.Scheduler.Attendence;
using Kitias.Persistence.Enums;
using Kitias.Providers.Interfaces;
using Kitias.Providers.Models;
using Kitias.Providers.Models.Attendances;
using Kitias.Repository.Interfaces.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kitias.Providers.Implementations
{
	/// <summary>
	/// Provider to work with attendances
	/// </summary>
	public class AttendanceProvider : Provider, IAttendanceProvider
	{
		/// <summary>
		/// Constructor to get services
		/// </summary>
		/// <param name="mapper">Mapping</param>
		/// <param name="logger">Logging</param>
		/// <param name="unitOfWork">Class to work with different dbs</param>
		public AttendanceProvider(IMapper mapper, ILogger<Provider> logger, IUnitOfWork unitOfWork) : base(mapper, logger, unitOfWork)
		{ }

		public async Task<Result<IEnumerable<ShedulersListResult>>> TakeTeacherShedulersAsync(string email)
		{
			var teacher = await _unitOfWork.Teacher
				.FindByAndInclude(t => t.Person.Email == email, p => p.Person)
				.SingleOrDefaultAsync();

			if (teacher == null)
				return ReturnFailureResult<IEnumerable<ShedulersListResult>>($"Couldn't find teacher with email {email}", "Couldn't find teacher");
			var shedulers = _unitOfWork.ShedulerAttendace
				.FindByAndInclude(s => s.TeacherId == teacher.Id, s => s.Group);
			var result = _mapper.Map<IEnumerable<ShedulersListResult>>(shedulers);

			_logger.LogInformation($"Take all shedulers of the teacher {email}");
			return ResultHandler.OnSuccess(result);
		}

		public async Task<Result<IEnumerable<AttendanceDto>>> TakeShedulerAttendancesAsync(Guid id)
		{
			var sheduler = await _unitOfWork.ShedulerAttendace
				.FindBy(s => s.Id == id)
				.Include(s => s.Group)
				.Include(s => s.Attendances)
				.ThenInclude(s => s.Student)
				.ThenInclude(s => s.Person)
				.Include(s => s.Attendances)
				.ThenInclude(s => s.Subject)
				.SingleOrDefaultAsync();

			if (sheduler == null)
				return ReturnFailureResult<IEnumerable<AttendanceDto>>($"Couldn't find sheduler with id {id}", "Couldn't find sheduler");
			var result = _mapper.Map<IEnumerable<AttendanceDto>>(sheduler.Attendances.OrderBy(r => r.Date));

			_logger.LogInformation($"Take all attendances of the sheduer {id}");
			return ResultHandler.OnSuccess(result);
		}

		public async Task<Result<IEnumerable<StudentAttendanceDto>>> TakeShedulerStudentAttendancesAsync(Guid id)
		{
			var sheduler = await _unitOfWork.ShedulerAttendace
				.FindBy(s => s.Id == id)
				.Include(s => s.Group)
				.Include(s => s.StudentAttendances)
				.ThenInclude(s => s.Student)
				.ThenInclude(s => s.Person)
				.SingleOrDefaultAsync();

			if (sheduler == null)
				return ReturnFailureResult<IEnumerable<StudentAttendanceDto>>($"Couldn't find sheduler with id {id}", "Couldn't find sheduler");
			var result = _mapper.Map<IEnumerable<StudentAttendanceDto>>(sheduler.StudentAttendances);

			_logger.LogInformation($"Take all student attendances of the scheduler {id}");
			return ResultHandler.OnSuccess(result);
		}

		public async Task<Result<AttendanceShedulerDto>> CreateShedulerAsync(ShedulerProviderRequestModel model)
		{
			var teacher = await _unitOfWork.Teacher
				.FindByAndInclude(
					t => t.Person.Email == model.TeacherEmail,
					p => p.Person
				).SingleOrDefaultAsync();

			if (teacher == null)
				return ReturnFailureResult<AttendanceShedulerDto>($"Couldn't find teacher with email {model.TeacherEmail}", "Couldn't find teacher");
			_logger.LogInformation($"Found teacher with id {teacher.Id}");
			var group = await _unitOfWork.Group
				.FindByAndInclude(g => g.Number == model.GroupNumber)
				.SingleOrDefaultAsync();

			if (group != null)
				_logger.LogInformation($"Found group with id {group.Id}");
			try
			{
				var newSheduler = _unitOfWork.ShedulerAttendace.Create(new()
				{
					TeacherId = teacher.Id,
					Name = model.Name,
					GroupId = group?.Id ?? null
				});
				var isSave = await _unitOfWork.SaveChangesAsync();

				if (isSave <= 0)
					throw new ApplicationException("Couldn't save new sheduler");
				newSheduler.Teacher = teacher;
				newSheduler.Group = group;
				var result = _mapper.Map<AttendanceShedulerDto>(newSheduler);

				_logger.LogInformation($"Sheduler with id {newSheduler.Id} was successfully created");
				return ResultHandler.OnSuccess(result);
			}
			catch (ApplicationException)
			{
				throw;
			}
			catch (Exception ex)
			{
				return ReturnFailureResult<AttendanceShedulerDto>(ex.Message, "Error sheduler data");
			}
		}

		public async Task<Result<AttendanceShedulerDto>> UpdateShedulerAsync(Guid id, ShedulerRequestModel model)
		{
			var sheduler = await _unitOfWork.ShedulerAttendace
				.FindBy(s => s.Id == id)
				.Include(s => s.Teacher)
				.ThenInclude(s => s.Person)
				.Include(s => s.Group)
				.SingleOrDefaultAsync();

			if (sheduler == null)
				return ReturnFailureResult<AttendanceShedulerDto>($"Couldn't find sheduler with id {id}", "Couldn't find sheduler");
			try
			{
				Group group = null;

				if (model.GroupNumber != null)
				{
					group = await _unitOfWork.Group
						.FindByAndInclude(g => g.Number == model.GroupNumber)
						.SingleOrDefaultAsync();

					if (group != null)
						_logger.LogInformation($"Found group with id {group.Id}");
					sheduler.GroupId = group.Id;
				}
				if (model.Name != null)
					sheduler.Name = model.Name;
				var newSheduler = _unitOfWork.ShedulerAttendace.Update(sheduler);
				var isSave = await _unitOfWork.SaveChangesAsync();

				if (isSave <= 0)
					throw new ApplicationException("Couldn't update sheduler");
				if (group != null)
					newSheduler.Group = group;
				var result = _mapper.Map<AttendanceShedulerDto>(newSheduler);

				_logger.LogInformation($"Sheduler with id {newSheduler.Id} was successfully created");
				return ResultHandler.OnSuccess(result);
			}
			catch (ApplicationException)
			{
				throw;
			}
			catch (Exception ex)
			{
				return ReturnFailureResult<AttendanceShedulerDto>(ex.Message, "Error sheduler data");
			}
		}

		public async Task<Result<string>> DeleteShedulerAsync(Guid id)
		{
			var sheduler = await _unitOfWork.ShedulerAttendace
				.FindBy(s => s.Id == id)
				.SingleOrDefaultAsync();

			if (sheduler == null)
				return ReturnFailureResult<string>($"Couldn't find sheduler with id {id}", "Couldn't find sheduler");
			try
			{
				_unitOfWork.ShedulerAttendace.Delete(sheduler);
				var isSave = await _unitOfWork.SaveChangesAsync();

				if (isSave <= 0)
					throw new ApplicationException("Couldn't delete sheduler");
				_logger.LogInformation($"Sheduler with id {id} is successfully deleted");
				return ResultHandler.OnSuccess("Sheduler successfully deleted");
			}
			catch (ApplicationException)
			{
				throw;
			}
			catch (Exception ex)
			{
				return ReturnFailureResult<string>(ex.Message, "Error sheduler data");
			}
		}

		public async Task<Result<IEnumerable<StudentAttendanceDto>>> CreateStudentAttendanceAsync(IEnumerable<StudentAttendanceRequestModel> models)
		{
			try
			{
				ICollection<StudentAttendance> attendances = new List<StudentAttendance>();

				foreach (var model in models)
				{
					var newStudentAttendance = new StudentAttendance
					{
						Grade = Helpers.GetEnumMemberFromString<Grade>(model.Grade),
						Raiting = byte.Parse(model.Raiting)
					};

					if (model.StudentId != null)
					{
						var student = await _unitOfWork.Student
							.FindBy(s => s.Id == model.StudentId)
							.Include(s => s.Person)
							.SingleOrDefaultAsync();

						if (student == null)
							return ReturnFailureResult<IEnumerable<StudentAttendanceDto>>($"Couldn't find student with id {model.StudentId}", "Couldn't find student");
						newStudentAttendance.StudentId = model.StudentId;
						_unitOfWork.StudentAttendace.Create(newStudentAttendance);
						newStudentAttendance.Student = student;
						attendances.Add(newStudentAttendance);
					}
					else if (model.StudentName != null)
					{
						newStudentAttendance.StudentName = model.StudentName;
						_unitOfWork.StudentAttendace.Create(newStudentAttendance);
						attendances.Add(newStudentAttendance);
					}
					else
						return ReturnFailureResult<IEnumerable<StudentAttendanceDto>>("Enter students");
					_logger.LogInformation($"Created new studenAttendance with id {newStudentAttendance.Id}");
				}
				var isSave = await _unitOfWork.SaveChangesAsync();

				if (isSave <= 0)
					throw new ApplicationException("Couldn't save new student attendances");
				var result = _mapper.Map<IEnumerable<StudentAttendanceDto>>(attendances);

				_logger.LogInformation("Student attendance was successfully created");
				return ResultHandler.OnSuccess(result);
			}
			catch (ApplicationException)
			{
				throw;
			}
			catch (Exception ex)
			{
				return ReturnFailureResult<IEnumerable<StudentAttendanceDto>>(ex.Message, "Error student attendances data");
			}
		}

		private Result<T> ReturnFailureResult<T>(string loggerMessage, string errorMessage = null)
			where T : class
		{
			_logger.LogError(loggerMessage);
			return ResultHandler.OnFailure<T>(errorMessage ?? loggerMessage);
		}
	}
}
