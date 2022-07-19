using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.Events.DTOs;
using Application.Users.DTOs;
using AutoMapper;
using Domain;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Repositories;

namespace Application.Services
{
	public class UserImageService
	{
		// private EventRepository _eventRepository;
		private readonly UserImageRepository _imageRepository;
		private readonly IMapper mapper;

		public UserImageService(
			UserImageRepository imageRepository,
			IMapper _mapper)
		{
			_mapper = mapper;
			this._imageRepository = imageRepository;
		}

		public async Task<List<UserImage>> Get()
		{
			return await _imageRepository.GetAll(); ;
		}

		public async Task<List<UserImage>> GetAllFromUser(Guid userId)
		{
			return await _imageRepository.GetQuery().Where(i => i.UserId == userId).ToListAsync();
		}

		public async Task<UserImage> GetByID(Guid id) => await _imageRepository.GetByID(id);

		public async Task<bool> Insert(UserImage e) { _imageRepository.Insert(e); return await _imageRepository.Save(); }
		public async Task<bool> Update(UserImage e) { _imageRepository.Update(e); return await _imageRepository.Save(); }
		public async Task<bool> Delete(UserImage e)
		{
			_imageRepository.Delete(e); return await _imageRepository.Save();
		}
		public async Task<bool> Save() { return await _imageRepository.Save(); }
	}
}